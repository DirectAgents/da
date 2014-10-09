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

        public ActionResult Index()
        {
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

        /**
         * The first if-statement was added to circumvent the need for the Next ActionResult below.
         * Should it ever need to be reverted back, simply remove this if-statement, and change the
         * else-if that follows into if, and change the Next Set of Results link in Process.cshtml
         * accordingly.
         **/
        public async Task<ActionResult> Process(ParameterKeeper param, bool NewSession = false)
        {

            // Initial verification; checks if user has exceeded daily limit.
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            int? DailyCap = SettingsHelper.RetrieveDailyLimitSetting();
            if (user.QueriesPerformed >= DailyCap)
            {
                DateTime EndTime = DateTime.Now.Date;
                user.DateTimeStamp = user.DateTimeStamp.Date;
                displayln("endTime = " + EndTime);
                displayln("userDTS = " + user.DateTimeStamp);
                if (EndTime <= user.DateTimeStamp)
                {
                    return View("DailyMaxReached", new Setting { Value = DailyCap.ToString() });
                }
                else
                {
                    user.QueriesPerformed = 0;
                }
            }

            // If-staement will occur if the param is coming from the Application Index page, where newSession is always set to true.
            // If coming via click on Next Set of Results, this will always be false.
            if (NewSession == true)
            {
                Session["Params"] = null;
            }

            ProcessHub ph = null;

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
                if ((param.top + param.skip) > 1001)
                {
                    if (param.skip > param.top)
                        param.top = 1001 - param.skip;
                    else if (param.top > param.skip)
                    {
                        param.top = 1;
                        param.skip = 1001;
                    }
                    else if (param.top == param.skip)
                    {
                        param.top = 1001 - param.top;
                    }
                }

                ph = new ProcessHub(param);
            }
            else
                return View("Index", param);

            ph.run();
            updateResults(ph, param);
            Session["Params"] = param;
            user.QueriesPerformed++;
            user.TotalQueriesPerformed++;
            user.DateTimeStamp = DateTime.Now;
            //user.DateTimeStamp = DateTime.Now.Date;       // Use this line if you care only about the specific date and not exact time.
            await UserManager.UpdateAsync(user);
            return View(ph);
        } // Process

        /**
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

            csv.WriteField("Google Search Query");
            csv.WriteField(param.BingSearchQuery);
            csv.NextRecord();
            csv.WriteField("Phrase Search");
            csv.WriteField(param.PhraseSearchString);
            csv.NextRecord();
            csv.WriteField("Target Website To Link To");
            csv.WriteField(param.ClientWebsite);
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
            csv.WriteField("Contains Phrase");
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

                if (sr.ExceptionFound || param.PhraseSearchString == null || param.PhraseSearchString.Equals("")) csv.WriteField("n/a");
                else if (sr.ContainsSearchPhrase) csv.WriteField("yes");
                else csv.WriteField("no");

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
