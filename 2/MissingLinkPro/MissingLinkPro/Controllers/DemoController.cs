using IdentitySample.Models;
using MissingLinkPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace MissingLinkPro.Controllers
{
    /**
     * Controller class for the Demo; contains mostly the same code logic as ApplicationController
     * without any of the authentication restrictions and account checks.
     **/
    [AllowAnonymous]
    public class DemoController: BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            ParameterKeeper pk = new ParameterKeeper();
            return View(pk);
        }

        [HttpPost]
        public ActionResult Index(ParameterKeeper Parameters, string Email)
        {
            SendInfo(Parameters, Email);

            var url = Url.Action("Register", "Account");
            ViewBag.StatusMessage = "To get more results and features, <a href=\"" + url + "\">sign up for a Freemium account</a> at no cost!";
            Parameters.top = 15;     // Cap at 15 results.
            if ((Parameters.top + Parameters.skip) > 1001)    // Recalculating results based on skip value.
            {
                Parameters.top = 1000 - (Parameters.skip - 1);
                if (Parameters.top <= 0) Parameters.top = 1;
            }
            Parameters.InitialSkip = Parameters.skip;
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

        private void SendInfo(ParameterKeeper p, string email)
        {
            var toAddress = "marketing@directagents.com";
            var subject = "DirectLink Demo user";
            var format = "DirectLink Demo submission by: {0}<br><br><table><tr><td>Search Query:</td><td>{1}</td></tr><tr><td>Target Site To Scan for:</td><td>{2}</td></tr><tr><td>Exclude Terms:</td><td>{3}</td></tr><tr><td>Start At Search Rank#</td><td>{4}</td></tr><tr><td>Type of Results:</td><td>{5}</td></tr></table>";
            var body = String.Format(format, email, p.BingSearchQuery, p.ClientWebsite, p.ExcludeString, p.skip, p.ResultType);

            Emailer emailer = new Emailer(new System.Net.NetworkCredential(ConfigurationManager.AppSettings["Emailer_Username"], ConfigurationManager.AppSettings["Emailer_Password"]));
            emailer.SendEmail(
                ConfigurationManager.AppSettings["Emailer_Username"],
                toAddress,
                null, // ccAddresses
                subject,
                body,
                true // isHTML
            );
        }

    } // class DemoController
} // EOF