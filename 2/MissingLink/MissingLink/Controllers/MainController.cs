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
            return View();
        }

        public ActionResult Process(ParameterKeeper param) {
            Session["Params"] = param;
            ProcessHub p = new ProcessHub(param.query, param.searchString, param.website, param.numResults, param.exclude, param.jump, param.delay);
            p.run();
            param.results = p.results;
            param.omit_count = p.omit_count;
            return View(p);
        }

        public ActionResult Next()
        {
            ParameterKeeper param = (ParameterKeeper)Session["Params"];

            param.jump += param.numResults;
            ProcessHub p = new ProcessHub(param.query, param.searchString, param.website, param.numResults, param.exclude, param.jump, param.delay);
            p.run();
            param.results = p.results;
            param.omit_count = p.omit_count;
            return View("Process",p);
        }

        public FileResult ExportResults() {

            //return Content("Not Yet Implemented");
            ParameterKeeper param = (ParameterKeeper)Session["Params"];
            FileResult complete_file = WriteFile(param.results, param);
            return complete_file;
        }

        private FileResult WriteFile(List<MvcApplication1.Models.ProcessHub.SearchResult> l, ParameterKeeper param) {

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
            csv.NextRecord();

            csv.WriteField("Rank ID");
            csv.WriteField("URL");
            csv.WriteField("Links To Target");
            csv.WriteField("Contains Phrase");
            csv.WriteField("Errors");
            csv.NextRecord();
            

            foreach (MvcApplication1.Models.ProcessHub.SearchResult sr in l) {

                if (sr.skip) continue;

                csv.WriteField(sr.id);
                csv.WriteField(sr.url);
                if (sr.linksToTarget) csv.WriteField("yes");
                else if (sr.exception) csv.WriteField("n/a");
                else csv.WriteField("no");
                if (sr.containsPhrase) csv.WriteField("yes");
                else if (sr.exception) csv.WriteField("n/a");
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
