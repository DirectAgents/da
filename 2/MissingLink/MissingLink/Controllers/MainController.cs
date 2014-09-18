using MvcApplication1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Threading;

using CsvHelper;
using System.IO;

namespace MvcApplication1.Controllers
{
    public class MainController : Controller
    {

        public ActionResult Index()
        {
            ParameterKeeper pk = new ParameterKeeper
            {
                top = 50,
                skip = 1,
                exclude = "yes",
                displayall = "yes"
            };
            return View(pk);
        }

        public ActionResult Process(ParameterKeeper param) {

            if (ModelState.IsValid)
            {
                if ((param.top + param.skip) > 1001)
                    param.top = 1001 - param.skip;

                Session["Params"] = param;
                ProcessHub p = new ProcessHub(param);
                p.run();
                updateResults(p, param);
                return View(p);
            }
            else {
                return View("Index",param);
            }
        }

        public ActionResult Next()
        {
            ParameterKeeper param = (ParameterKeeper)Session["Params"];
            ProcessHub p = new ProcessHub(param);
            p.results = param.results;
            p.run();
            updateResults(p, param);
            return View("Process",p);
        }

        /**
         * Performs updating of search results and potential error messages.
         * Para@    ProcessHub p
         *          ParameterKeeper param
         **/
        private void updateResults(ProcessHub p, ParameterKeeper param)
        {
            param.skip += param.top;
            param.results = p.results;
            param.omit_count = p.omit_count;
            if (p.search_error_encountered)
            {
                param.search_error_encountered = p.search_error_encountered;
                param.search_error_msg = p.search_error_msg;
            }
        } // updateResults

        public FileResult ExportResults() {

            //return Content("Not Yet Implemented");
            ParameterKeeper param = (ParameterKeeper)Session["Params"];
            FileResult complete_file = WriteFile(param.results, param);
            return complete_file;
        }

        private FileResult WriteFile(List<MvcApplication1.Models.ProcessHub.SearchResult> list, ParameterKeeper param) {

            var output = new MemoryStream();
            var writer = new StreamWriter(output);
            var csv = new CsvWriter(writer);

            writer.WriteLine("MissingLink Data Export");
            writer.WriteLine(DateTime.Now.ToString() + "\n");

            csv.WriteField("Google Search Query");
            csv.WriteField(param.query);
            csv.NextRecord();
            csv.WriteField("Phrase Search");
            csv.WriteField(param.searchString);
            csv.NextRecord();
            csv.WriteField("Target Website To Link To");
            csv.WriteField(param.website);
            csv.NextRecord();
            csv.WriteField("Number of Results Found");
            csv.WriteField(param.results.Count);
            csv.NextRecord();
            csv.WriteField("Results Omitted");
            csv.WriteField(param.omit_count);
            csv.NextRecord();

            if (param.search_error_encountered)
            {
                csv.WriteField("Search Error Encountered");
                csv.WriteField(param.search_error_msg);
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

            foreach (MvcApplication1.Models.ProcessHub.SearchResult sr in list) {

                if (sr.skip) continue;

                csv.WriteField(sr.id);
                csv.WriteField(sr.title);
                csv.WriteField(sr.url);
                if (sr.linksToTarget) csv.WriteField("yes");
                else if (sr.exception) csv.WriteField("n/a");
                else csv.WriteField("no");

                if (sr.exception || param.searchString == null || param.searchString.Equals("")) csv.WriteField("n/a");
                else if (sr.containsPhrase) csv.WriteField("yes");
                else csv.WriteField("no");

                if (sr.exception) csv.WriteField(sr.errorMsg);
                else csv.WriteField("none");
                csv.NextRecord();
            }

            writer.Flush();
            output.Position = 0;
            return File(output, "application/CSV", "ExportResults.csv");
        } // WriteFile

    } // class MainController
} // EOF
