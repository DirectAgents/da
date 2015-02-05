using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MissingLinkPro.Controllers;
using MissingLinkPro.Models;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace IdentitySample.Controllers
{
    public class HomeController : HttpsBaseController
    {
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

        //public ActionResult Index()
        //{
        //    //ViewBag.Message = "DirectAgents";
        //    ViewBag.Title = "DirectLink | Linkless Mention Finder";
        //    ViewBag.MetaDescription = "DirectLink is the first enterprise level tool that finds out who’s talking about your brand without linking back. Try it free!";
        //    return View();
        //}

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
            IdentityMessage message = new IdentityMessage();
            message.Subject = "Contact Form Msg: " + model.FirstName + " " + model.LastName + ", " + model.Company;
            message.Body = model.FirstName + " " + model.LastName + "<br />Phone: " + model.PhoneNum + "<br />Email " + model.Email + "<br />Company: " + model.Company + "<br />Message: " + model.Message;
            SendMessage(message, true);

            ViewBag.StatusMessage = "Thank you. Your message has been sent. You will be contacted by a representative during operating hours.";

            return View(model);
        }

        private void SendMessage(IdentityMessage message, bool isHTML)
        {
            message.Destination = "directlink@directagents.com";

            Emailer emailer = new Emailer(new System.Net.NetworkCredential(ConfigurationManager.AppSettings["Emailer_Username"], ConfigurationManager.AppSettings["Emailer_Password"]));
            var ccAddresses = new string[2] { "dinesh@directagents.com", "ricardo@directagents.com" };
            emailer.SendEmail(ConfigurationManager.AppSettings["Emailer_Username"], message.Destination, ccAddresses, message.Subject, message.Body, isHTML);
        }

        [Authorize]
        public async Task<ActionResult> Feedback()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            var model = new FeedbackViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Feedback(string feedback)
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            IdentityMessage message = new IdentityMessage();
            message.Subject = "DirectLink Feedback From: " + user.FirstName + " " + user.LastName;
            message.Body = "FROM:\n" + user.FirstName + " " + user.LastName + "\n" + user.Email + "\n\nFEEDBACK:\n" + feedback;
            SendMessage(message, false);

            ViewBag.StatusMessage = "Thank you. Your feedback has been submitted.";
            return View(new FeedbackViewModel());
        }

        [Authorize]
        public ActionResult Help()
        {
            //ViewBag.Message = "User Manual";
            return View();
        }

        [Authorize]
        public ActionResult FAQ()
        {
            //ViewBag.Mesage = "Frequently Asked Questions";
            return View();
        }
    } // HomeController class
} // EOF