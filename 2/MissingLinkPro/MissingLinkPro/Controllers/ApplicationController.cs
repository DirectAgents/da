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

namespace MissingLinkPro.Controllers
{
    [Authorize]
    public class ApplicationController : Controller
    {
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

        public async Task<ActionResult> Index()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            // Email not confirmed
            if (!user.EmailConfirmed)
            {
                return View("EmailNotConfirmed", new ApplicationUser { Email = user.Email });
            }

            // If user is accessing Index page, any previous Session should be flushed.
            if ((ParameterKeeper)Session["Params"] != null)
                Session["Params"] = null;

            ParameterKeeper pk = new ParameterKeeper
            {
                top = 50,
                skip = 1,
                ExcludeLinkbackResults = true,
                DisplayAllResults = true,
                ResultType = "web"
            };
            return View(pk);
        }

        public ActionResult DailyMaxReached() {
            Setting s = new Setting();
            int? i = SettingsHelper.RetrieveDailyLimitSetting();
            s.Value = i.ToString();
            return View(s);
        }

        public ActionResult EmailNotConfirmed(ApplicationUser a) {
            return View(a);
        }

        /**
         * The first if-statement was added to circumvent the need for the Next ActionResult below.
         * Should it ever need to be reverted back, simply remove this if-statement, and change the
         * else-if that follows into if, and change the Next Set of Results link in Process.cshtml
         * accordingly.
         **/
        public async Task<ActionResult> Process(ParameterKeeper param, bool NewSession = false)
        {

            //if (param.top % 50 > 0) return View("Index",param);

            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            // Email not confirmed
            if (!user.EmailConfirmed) {
                return View("EmailNotConfirmed", new ApplicationUser { Email = user.Email });
            }

            // Need to perform anniversary check; how will payment be integrated here?
            DateTime CurrentDate = DateTime.Now;

            // Reset number of queries performed for the month if current date is later than user's one-month-later date.
            if ((DateTime.Now).CompareTo(user.Anniversary.AddMonths(1)) >= 0)
            {
                bool ok = false;
                if (!user.IsActive)
                {   // the result of a cancelled subscription; reassign Freemium
                    user = StripeHelper.AssignNewSubscription(user, 1);
                    ok = true;
                }
                else {
                    string status = "";
                    try
                    {
                        status = StripeHelper.GetSubscriptionStatus(user);
                    }
                    catch (Stripe.StripeException) {
                        status = "active";
                        user = StripeHelper.AssignNewSubscription(user, 1);
                    }
                    if (status.Equals("active")) ok = true;
                    else if (status.Equals("past_due")) return View("PaymentError");
                    else if (status.Equals("unpaid")) return View("PaymentError");
                    else if (status.Equals("canceled")) return View("PaymentError");
                }

                if (ok)
                {
                    user.QueriesPerformed = 0;
                    user = StripeHelper.UpdateAnniversary(user);
                }
                //while ((user.Anniversary.AddMonths(1)).CompareTo(CurrentDate) < 0)      // Need to calculate new user end date.
                //    user.Anniversary = user.Anniversary.AddMonths(1);
            }

            Package userPackage = db.Packages.Find(user.PackageId);
            if (user.QueriesPerformed >= userPackage.SearchesPerMonth) return View("MonthlyMaxReached", user);

            // If-statement will occur if the param is coming from the Application Index page, where newSession is always set to true.
            // If coming via click on Next Set of Results, this will always be false.
            if (NewSession == true)
            {
                Session["Params"] = null;
            }

            ProcessHub ph = null;
            if (param.top > userPackage.MaxResults) param.top = userPackage.MaxResults;

            // If a session is continuing from the previous (user chooses to retrieve next set of results).
            if ((ParameterKeeper)Session["Params"] != null)
            {
                param = (ParameterKeeper)Session["Params"];
                ph = new ProcessHub(param);
                ph.ParsedResults = param.ParsedResults;
            }

            // Fresh run; should check validity of inputs.
            else if (ModelState.IsValid)
            {
                // Tamper-checking
                if ((param.top + param.skip) > 1000)
                {
                    if (param.skip > param.top)
                        param.top = 1000 - param.skip;
                    else if (param.top > param.skip)
                    {
                        param.top = 1;
                        param.skip = 1000;
                    }
                    else if (param.top == param.skip)
                    {
                        param.top = 1000 - param.top;
                    }
                }

                ph = new ProcessHub(param);
            }
            else
                return View("Index", param);

            ph.run();
            updateResults(ph, param);
            Session["Params"] = param;
            if (!ph.SearchErrorEncountered)     // If no search error returned from Bing API.
            {
                user.QueriesPerformed++;
                user.TotalQueriesPerformed++;
                user.DateTimeStamp = DateTime.Now;
                //user.DateTimeStamp = DateTime.Now.Date;       // Use this line if you care only about the specific date and not exact time.
                await UserManager.UpdateAsync(user);
            }
            return View(ph);
        } // Process

        /**
         * Depracated version of Next() that was used in previous versions of this application.
         * CURRENTLY NOT IN USE; CODE REQUIRES COMPLETE UPDATE IF RE-IMPLEMENTING.
         **/
        public async Task<ActionResult> Next()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            int DailyCap = Convert.ToInt32(ConfigurationManager.AppSettings["MaxQueriesPerDay"]);
            if (user.QueriesPerformed >= DailyCap) return View("DailyMaxReached");

            ParameterKeeper param = (ParameterKeeper)Session["Params"];
            ProcessHub p = new ProcessHub(param);
            p.ParsedResults = param.ParsedResults;
            p.run();
            updateResults(p, param);

            user.QueriesPerformed++;
            await UserManager.UpdateAsync(user);

            return View("Process",p);
        }

        /**
         * Performs updating of search results and potential error messages; saves to ParameterKeeper.
         * Para@    ProcessHub p
         *          ParameterKeeper param
         **/
        private void updateResults(ProcessHub p, ParameterKeeper param)
        {
            param.skip += param.top;
            //param.top += param.Increment;
            param.ParsedResults = p.ParsedResults;
            param.OmitCount = p.OmitCount;
            if (p.SearchErrorEncountered)
            {
                param.SearchErrorEncountered = p.SearchErrorEncountered;
                param.SearchErrorMsg = p.SearchErrorMsg;
            }
        } // updateResults

        public FileResult ExportResults() {

            ParameterKeeper param = (ParameterKeeper)Session["Params"];
            FileResult complete_file = WriteFile(param.ParsedResults, param);
            return complete_file;
        }

        private FileResult WriteFile(List<MissingLinkPro.Models.ProcessHub.SearchResult> list, ParameterKeeper param) {

            var output = new MemoryStream();
            var writer = new StreamWriter(output);
            var csv = new CsvWriter(writer);

            writer.WriteLine("MissingLink Data Export");
            writer.WriteLine(DateTime.Now.ToString() + "\n");

            csv.WriteField("Search Query");
            csv.WriteField(param.BingSearchQuery);
            //csv.NextRecord();
            //csv.WriteField("Phrase Search");
            //csv.WriteField(param.PhraseSearchString);
            csv.NextRecord();
            csv.WriteField("Target Website To Link To");
            csv.WriteField(param.ClientWebsite);
            csv.NextRecord();
            csv.WriteField("Result Type");
            csv.WriteField(param.ResultType);
            csv.NextRecord();
            csv.WriteField("Number of Results Found");
            csv.WriteField(param.ParsedResults.Count);
            csv.NextRecord();
            csv.WriteField("Results Omitted");
            csv.WriteField(param.OmitCount);
            csv.NextRecord();

            if (param.SearchErrorEncountered)
            {
                csv.WriteField("Search Error Encountered");
                csv.WriteField(param.SearchErrorMsg);
                csv.NextRecord();
            }

            csv.NextRecord();

            csv.WriteField("Rank ID");
            csv.WriteField("Title");
            csv.WriteField("URL");
            csv.WriteField("Links To Target");
            //csv.WriteField("Contains Phrase");
            if (param.ResultType.Equals("news")){
                csv.WriteField("Source");
                csv.WriteField("Indexed On");
            }
            csv.WriteField("Errors");
            csv.NextRecord();

            foreach (MissingLinkPro.Models.ProcessHub.SearchResult sr in list) {

                if (sr.SkipThisResult) continue;

                csv.WriteField(sr.Id);
                csv.WriteField(sr.Title);
                csv.WriteField(sr.Url);
                if (sr.LinksToClientWebsite) csv.WriteField("yes");
                else if (sr.ExceptionFound) csv.WriteField("n/a");
                else csv.WriteField("no");

                //if (sr.ExceptionFound || param.PhraseSearchString == null || param.PhraseSearchString.Equals("")) csv.WriteField("n/a");
                //else if (sr.ContainsSearchPhrase) csv.WriteField("yes");
                //else csv.WriteField("no");

                if (param.ResultType.Equals("news")) {
                    csv.WriteField(sr.Source);
                    csv.WriteField(sr.Date);
                }

                if (sr.ExceptionFound) csv.WriteField(sr.ErrorMsg);
                else csv.WriteField("none");
                csv.NextRecord();
            }

            writer.Flush();
            output.Position = 0;
            return File(output, "application/CSV", "ExportResults.csv");
        } // WriteFile

        /**
         * Quick shortcut method for printing to the diagnostic console, sans new line.
         * @para string s:  the string to be printed
         **/
        private void display(string s)
        {
            System.Diagnostics.Debug.Write(s);
        } // display

        /**
         * Quick shortcut method for printing to the diagnostic console, with new line.
         * @para string s:  the string to be printed
         **/
        private void displayln(string s)
        {
            System.Diagnostics.Debug.WriteLine(s);
        } // displayln

    } // class MainController
} // EOF
