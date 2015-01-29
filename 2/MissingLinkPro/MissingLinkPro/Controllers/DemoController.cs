using MissingLinkPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MissingLinkPro.Controllers
{
    /**
     * Controller class for the Demo; contains mostly the same code logic as ApplicationController
     * without any of the authentication restrictions and account checks.
     **/
    [AllowAnonymous]
    public class DemoController : HttpsBaseController
    {
        public ActionResult Index()
        {
            ParameterKeeper pk = new ParameterKeeper
            {
                top = 15,
                skip = 1,
                ExcludeLinkbackResults = true,
                DisplayAllResults = true,
                ResultType = "web",
                MaxResultRange = 1000
            };
            return View(pk);
        } // Index

        [HttpPost]
        public ActionResult Index(ParameterKeeper Parameters)
        {
            var url = Url.Action("Register", "Account");
            ViewBag.StatusMessage = "To get more results and features, <a href=\"" + url + "\">sign up for a Freemium account</a> at no cost!";
            Parameters.top = 15;     // Cap at 15 results.
            if ((Parameters.top + Parameters.skip) > 1001)    // Recalculating results based on skip value.
            {
                Parameters.top = 1000 - (Parameters.skip - 1);
                if (Parameters.top <= 0) Parameters.top = 1;
            }
            ProcessHub Engine = new ProcessHub(Parameters);
            Integer Limit;
            if ((Integer)Session["Limit"] != null)          // If fresh user, create a new session.
            {
                Limit = (Integer)Session["Limit"];
                if (Limit.Value <= 0)
                    return View("LimitReached");
            }
            else
                Limit = new Integer { Value = 3 };      // Set the number of allowed runs per session here.

            Engine.run();
            Limit.Value = Limit.Value - 1;
            Session["Limit"] = Limit;
            return View("Results", Engine);
        } // Index [Post]
	} // class DemoController
} // EOF