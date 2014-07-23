using MvcApplication1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Threading;

namespace MvcApplication1.Controllers
{
    public class MainController : Controller
    {
        public static ProcessHub p;

        public ActionResult Index()
        {
            //SearchResult model = new SearchResult
            //{
            //    //url = link,
            //    //query = query,
            //};

            //ViewData["abc"] = 555;
            return View();
        }

        public ActionResult Process(ParameterKeeper param) {
            Session["Params"] = param;
            p = new ProcessHub(param.query, param.searchString, param.website, param.numResults, param.exclude, param.jump, param.delay);
            p.run();
            return View(p);
        }

        public ActionResult Next()
        {
            ParameterKeeper param = (ParameterKeeper)Session["Params"];

            param.jump += param.numResults;
            p = new ProcessHub(param.query, param.searchString, param.website, param.numResults, param.exclude, param.jump, param.delay);
            p.run();
            return View("Process",p);
            //return Content("okay");
        }

    } // class MainController
}
