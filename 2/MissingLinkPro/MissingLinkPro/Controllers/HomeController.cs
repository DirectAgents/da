using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using MissingLinkPro.Controllers;
using MissingLinkPro.Models;
using System.Configuration;
using System.Web.Mvc;

namespace IdentitySample.Controllers
{
    // public class HomeController : Controller
    [Authorize]
    public class HomeController : HttpsBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Message = "DirectAgents";
            ViewBag.Title = "DirectLink | Linkless Mention Finder";
            ViewBag.MetaDescription = "DirectLink is the first enterprise level tool that finds out who’s talking about your brand without linking back. Try it free!";
            return View();
        }

        public ActionResult DirectAgents()
        {
            return Redirect("http://www.directagents.com/");
        }

        //public ActionResult About()
        //{
        //    ViewBag.Message = "DirectAgents";
        //    ViewBag.Title = "About DirectLink";
        //    ViewBag.MetaDescription = "DirectLink is the first enterprise level tool that finds out who’s talking about your brand without linking back. Try it free!";

        //    return View();
        //}

        public ActionResult Contact()
        {
            ContactFormViewModel model = new ContactFormViewModel {};
            ViewBag.StatusMessage = "";
            return View(model);
        }

        [HttpPost]
        public ActionResult Contact(ContactFormViewModel model)
        {
            Emailer emailer = new Emailer(new System.Net.NetworkCredential(ConfigurationManager.AppSettings["Emailer_Username"], ConfigurationManager.AppSettings["Emailer_Password"]));
            IdentityMessage message = new IdentityMessage();
            message.Destination = "directlink@directagents.com";
            message.Subject = "Contact Form Msg: " + model.FirstName + " " + model.LastName + ", " + model.Company;
            message.Body = model.FirstName + " " + model.LastName + "<br />Phone: " + model.PhoneNum + "<br />Email " + model.Email + "<br />Company: " + model.Company + "<br />Message: " + model.Message;
            emailer.SendEmail(ConfigurationManager.AppSettings["Emailer_Username"], message.Destination, new string[2] { "dinesh@directagents.com","ricardo@directagents.com" }, message.Subject, message.Body, true);

            ViewBag.StatusMessage = "Thank you. Your message has been sent. You will be contacted by a representative during operating hours.";

            return View(model);
        }

        public ActionResult Manual()
        {
            ViewBag.Message = "User Manual";
            return View();
        }

        public ActionResult FAQ()
        {
            ViewBag.Mesage = "Frequently Asked Questions";
            return View();
        }
    } // HomeController class
} // EOF