using MissingLinkPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Threading;

using CsvHelper;
using System.IO;
using System.Threading.Tasks;

using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Configuration;
using MissingLinkPro.Helpers;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;

namespace MissingLinkPro.Controllers
{
    [Authorize]
    public class ApplicationController : HttpsBaseController
    {
        /*User Application DB Settings*/
        private ApplicationDbContext db = new ApplicationDbContext();
        public ApplicationController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationController()
        {
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        /*END User Application DB Settings*/

        public async Task<ActionResult> Index()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (!user.EmailConfirmed)               // Email not confirmed
                return View("EmailNotConfirmed", new ApplicationUser { Email = user.Email });

            //if ((ParameterKeeper)Session["Params"] != null)  // If user is accessing Index page, any previous Session should be flushed.
            //    Session["Params"] = null;

            ParameterKeeper pk = new ParameterKeeper
            {
                top = 50,
                skip = 1,
                ExcludeLinkbackResults = true,
                DisplayAllResults = true,
                ResultType = "web",
                MaxResultRange = 1000,
                InitialSkip = 1
            };
            return View(pk);
        }

        public ActionResult ViewResults()
        {
            if ((ParameterKeeper)Session["Params"] == null)
                return View(new ParameterKeeper { BingSearchQuery = "none", ClientWebsite = "none", OmitCount = 0, top = 0, ResultType = "none", ParsedResults = new List<ProcessHub.SearchResult>()});
            else
            {
                ParameterKeeper p = (ParameterKeeper)Session["Params"];
                return View(p);
            }
        }

        public ActionResult EmailNotConfirmed(ApplicationUser a) {
            return View(a);
        }

        /**
         * Creates the ProcessHub that handles the Bing search and scraping. Method checks a non-admin user's account
         * for a valid subscription, and proceeds if one exists. A Freemium account is automatically assigned if a user
         * were to use the tool past a cancelled subscription's date. Also recalculates the form fields to stay within
         * the set limit of 1000 max results.
         **/
        public async Task<ActionResult> Process(ParameterKeeper Parameters, bool NewSession = false)
        {
            ViewBag.StatusMessage = "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            Package userPackage = db.Packages.Find(user.PackageId);

            if (!user.EmailConfirmed)                   // Email not confirmed
                return View("EmailNotConfirmed", new ApplicationUser { Email = user.Email });

            if (NewSession == true)
            {     // NewSession is set to true if coming from Application page; wipe old session.
                Session["Params"] = null;
                Parameters.InitialSkip = Parameters.skip;
            }

            bool isAdmin = false;
            ApplicationRoleManager _roleManager = HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            IList<string> roles = await UserManager.GetRolesAsync(user.Id);
            foreach (string s in roles)
                if (s.Equals("Admin"))
                {
                    isAdmin = true;
                    break;
                }

            if (!isAdmin)                   // If not an Admin, Account checkpoint begins here.
            {
                DateTime CurrentDate = DateTime.Now;                                // Need to perform anniversary check.
                if ((DateTime.Now).CompareTo(user.Anniversary.AddMonths(1)) >= 0)   // Reset number of queries performed for the month if current date is later than user's one-month-later date.
                {
                    bool ok = false;
                    if (!user.IsActive)  // the result of a cancelled subscription past its time; need to reassign Freemium subscription.
                    {  
                        user = StripeHelper.AssignNewSubscription(user, 1);
                        userPackage = db.Packages.Find(user.PackageId);
                        ok = true;
                    }
                    else                // Date is still within user's subscription cycle.
                    {
                        string status = "";
                        try
                        {
                            status = StripeHelper.GetSubscriptionStatus(user);      // Check on user's subscription status.
                        }
                        catch (Stripe.StripeException)      // Could not find the user's subscription; may have fizzled due to cancellation.
                        {
                            status = "active";
                            user = StripeHelper.AssignNewSubscription(user, 1); // Assign new Freemium subscription.
                        }
                        if (status.Equals("active")) ok = true;             // Subscription is fine; proceed.
                        else {                                              // Not okay; redirect to Process page with error message.
                            if (!NewSession)                                // Subscription dies while in the middle of a search.
                                Parameters = (ParameterKeeper)Session["Params"];
                            else
                                Parameters.ParsedResults = new List<ProcessHub.SearchResult>();  // Prepare new List for model
                            ProcessHub model = new ProcessHub(Parameters);
                            model.ParsedResults = Parameters.ParsedResults;
                            model.SearchErrorEncountered = true;
                            //if (status.Equals("past_due")) model.SearchErrorMsg = "Subscription payment status: past due.";
                            //else if (status.Equals("unpaid")) model.SearchErrorMsg = "Subscription payment status: unpaid.";
                            //else if (status.Equals("canceled")) model.SearchErrorMsg = "Subscription payment status: canceled.";
                            //else model.SearchErrorMsg = "Subscription payment status: indeterminate.";
                            if (status.Equals("past_due")) ViewBag.StatusMessage = "Subscription payment status: past due.";
                            else if (status.Equals("unpaid")) ViewBag.StatusMessage = "Subscription payment status: unpaid.";
                            else if (status.Equals("canceled")) ViewBag.StatusMessage = "Subscription payment status: canceled.";
                            else ViewBag.StatusMessage = "Subscription payment status: indeterminate. Requires assistance.";
                            return View(model);
                        }
                    }

                    if (ok)     // If okay, reset queries and update anniversary date.
                    {
                        user.QueriesPerformed = 0;
                        user = StripeHelper.UpdateAnniversary(user);
                    }
                }

                if (user.QueriesPerformed >= userPackage.SearchesPerMonth)      // Used up all available queries.
                {
                    if (!NewSession)
                        Parameters = (ParameterKeeper)Session["Params"];
                    else
                        Parameters.ParsedResults = new List<ProcessHub.SearchResult>();
                    ProcessHub model = new ProcessHub(Parameters);
                    model.ParsedResults = Parameters.ParsedResults;
                    model.SearchErrorEncountered = true;
                    ViewBag.StatusMessage = "You have reached your maximum number of searches for the month. A higher-grade plan may be available to you. Please check your Subscriptions page for more details.";
                    //model.SearchErrorMsg = "You have reached your maximum number of searches for the month. A higher-grade plan may be available to you. Please check your Subscriptions page for more details.";
                    return View(model);
                }
                if (Parameters.top > userPackage.MaxResults) Parameters.top = userPackage.MaxResults;   // User's requested Results/Search is limited to Package settings.
            }             // End of User account checkpoint.

            ProcessHub Engine = null;

            if ((ParameterKeeper)Session["Params"] != null)    // If a session is continuing from the previous (user chooses to retrieve next set of results).
            {
                Parameters = (ParameterKeeper)Session["Params"];
                Engine = new ProcessHub(Parameters);
                Engine.ParsedResults = Parameters.ParsedResults;
            }

            else if (ModelState.IsValid)             // Fresh run; should check validity of inputs.
            {
                if ((Parameters.top + Parameters.skip) > 1001)    // Recalculating results based on skip value.
                {
                    Parameters.top = 1000 - (Parameters.skip - 1);
                    if (Parameters.top <= 0) Parameters.top = 1;
                }

                Engine = new ProcessHub(Parameters);
            }
            else
                return View("Index", Parameters);
            Setting PingTime = db.Settings.SingleOrDefault(setting => setting.SettingName == "PingTime");
            Setting LoadTime = db.Settings.SingleOrDefault(setting => setting.SettingName == "LoadTime");
            if (PingTime == null || LoadTime == null)
            {
                Engine.PingTime = 8000;
                Engine.LoadTime = 15000;
            }
            else
            {
                Engine.PingTime = Convert.ToInt32(PingTime.Value);
                Engine.LoadTime = Convert.ToInt32(LoadTime.Value);
            }
            Engine.run();
            updateResults(Engine, Parameters);      // Update results, save to session on next line.
            Session["Params"] = Parameters;
            if (!Engine.SearchErrorEncountered)     // If no search error returned from Bing API, or user has maxed out results for given search.
            {
                user.QueriesPerformed++;
                user.TotalQueriesPerformed++;
                user.DateTimeStamp = DateTime.Now;
                await UserManager.UpdateAsync(user);
            }
            ViewBag.StatusMessage = Engine.SearchErrorMsg;
            return View(Engine);
        } // Process

        /**
         * Performs updating of search results and potential error messages; saves to ParameterKeeper.
         * Para@    ProcessHub p
         *          ParameterKeeper param
         **/
        private void updateResults(ProcessHub Engine, ParameterKeeper Parameters)
        {
            Parameters.MaxResultRange = 1000 - (Parameters.skip - 1) - Parameters.top;  // Adjust maximum number possible results remaining
            Parameters.skip += Parameters.top;                                          // Adjust offset
            Parameters.ParsedResults = Engine.ParsedResults;                            // Save search results to parameters, which saves to Session
            Parameters.OmitCount = Engine.OmitCount;                                    // Update number of omitted results
            if (Engine.SearchErrorEncountered)
            {
                Parameters.SearchErrorEncountered = Engine.SearchErrorEncountered;
                Parameters.SearchErrorMsg = Engine.SearchErrorMsg;
            }
        } // updateResults

        /**
         * Allows CSV Export of search results; called from webpage.
         **/
        public FileResult ExportResults() {
            ParameterKeeper Parameters;
            if ((ParameterKeeper)Session["Params"] != null)
                Parameters = (ParameterKeeper)Session["Params"];
            else
            {
                Parameters = new ParameterKeeper();
                Parameters.ParsedResults = new List<ProcessHub.SearchResult>();
                Parameters.ResultType = "web";
            }
            FileResult complete_file = WriteFile(Parameters.ParsedResults, Parameters);
            return complete_file;
        } // ExportResults

        /**
         * Writes and returns a CSV file with given search results and user search parameters.
         **/
        private FileResult WriteFile(List<MissingLinkPro.Models.ProcessHub.SearchResult> Results, ParameterKeeper Parameters) {

            var output = new MemoryStream();
            var writer = new StreamWriter(output);
            var csv = new CsvWriter(writer);

            writer.WriteLine("MissingLink Data Export");
            writer.WriteLine(DateTime.Now.ToString() + "\n");

            csv.WriteField("Search Query");
            csv.WriteField(Parameters.BingSearchQuery);
            //csv.NextRecord();
            //csv.WriteField("Phrase Search");
            //csv.WriteField(param.PhraseSearchString);
            csv.NextRecord();
            csv.WriteField("Target Website To Link To");
            csv.WriteField(Parameters.ClientWebsite);
            csv.NextRecord();
            csv.WriteField("Result Type");
            csv.WriteField(Parameters.ResultType);
            csv.NextRecord();
            csv.WriteField("Number of Results Found");
            csv.WriteField(Parameters.ParsedResults.Count);
            csv.NextRecord();
            csv.WriteField("Results Omitted");
            csv.WriteField(Parameters.OmitCount);
            csv.NextRecord();

            if (Parameters.SearchErrorEncountered)
            {
                csv.WriteField("Search Error Encountered");
                csv.WriteField(Parameters.SearchErrorMsg);
                csv.NextRecord();
            }

            csv.NextRecord();

            csv.WriteField("Rank ID");
            csv.WriteField("Title");
            csv.WriteField("URL");
            csv.WriteField("Links To Target");
            //csv.WriteField("Contains Phrase");
            if (Parameters.ResultType.Equals("news")){
                csv.WriteField("Source");
                csv.WriteField("Indexed On");
            }
            csv.WriteField("Errors");
            csv.NextRecord();

            foreach (MissingLinkPro.Models.ProcessHub.SearchResult result in Results) {

                if (result.SkipThisResult) continue;

                csv.WriteField(result.Id);
                csv.WriteField(result.Title);
                csv.WriteField(result.Url);
                if (result.LinksToClientWebsite) csv.WriteField("yes");
                else if (result.ExceptionFound) csv.WriteField("n/a");
                else csv.WriteField("no");

                //if (sr.ExceptionFound || param.PhraseSearchString == null || param.PhraseSearchString.Equals("")) csv.WriteField("n/a");
                //else if (sr.ContainsSearchPhrase) csv.WriteField("yes");
                //else csv.WriteField("no");

                if (Parameters.ResultType.Equals("news")) {     // Additional columns for news results
                    csv.WriteField(result.Source);
                    csv.WriteField(result.Date);
                }

                if (result.ExceptionFound) csv.WriteField(result.ErrorMsg);
                else csv.WriteField("none");
                csv.NextRecord();
            }

            writer.Flush();
            output.Position = 0;
            return File(output, "application/CSV", "ExportResults.csv");
        } // WriteFile

    } // class MainController
} // EOF
