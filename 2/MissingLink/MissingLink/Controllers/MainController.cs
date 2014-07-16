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

        public ActionResult Process(string query, string searchString, string website, int numResults, string exclude, int delay) {
            p = new ProcessHub(query, searchString, website, numResults, exclude, delay);
            p.run();
            return View(p);
        }

    } // class MainController
}
