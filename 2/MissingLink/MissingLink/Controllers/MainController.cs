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
            //SearchResult model = new SearchResult
            //{
            //    //url = link,
            //    //query = query,
            //};
            return View();
        }

        public ActionResult Process(ParameterKeeper param) {
            Session["Params"] = param;
            ProcessHub p = new ProcessHub(param.query, param.searchString, param.website, param.numResults, param.exclude, param.jump, param.delay);
            p.run();
            return View(p);
        }

        public ActionResult Next()
        {
            ParameterKeeper param = (ParameterKeeper)Session["Params"];

            param.jump += param.numResults;
            ProcessHub p = new ProcessHub(param.query, param.searchString, param.website, param.numResults, param.exclude, param.jump, param.delay);
            p.run();
            return View("Process",p);
            //return Content("okay");
        }

        public FileResult ExportResults(List<MvcApplication1.Models.ProcessHub.SearchResult> list) {

            throw new NotImplementedException();

            var output = new MemoryStream();
            var writer = new StreamWriter(output);

            writer.WriteLine("Exported via Direct Agents: MissingLink");
            writer.WriteLine(DateTime.Now.ToString() + "\n");

            var csv = new CsvWriter(writer);

        }

    } // class MainController
}
