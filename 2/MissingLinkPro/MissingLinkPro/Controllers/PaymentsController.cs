using IdentitySample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using Stripe;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace MissingLinkPro.Controllers
{
    [Authorize]
    public class PaymentsController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        public PaymentsController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public PaymentsController()
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

        //
        // GET: /Payments/
        public ActionResult Index()
        {
            return View(db.Packages.ToList());
        }

        public async Task<ActionResult> Pay(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            Package package = db.Packages.Find(id);
            if (package == null)
            {
                return HttpNotFound();
            }
            return View(new PayViewModel { SubscribedPackageId = user.Package.Id, PackageId = id ?? default(int), PackageName = package.Name, PackageCost = package.CostPerMonth });
        } // Pay

        [HttpPost]
        public async Task<ActionResult> Pay(PostPayViewModel form)
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (form.PackageId == 1) { }

            /*This section updates data on Stripe's end*/
            var myCustomer = new StripeCustomerUpdateOptions();
            myCustomer.TokenId = form.stripeToken;
            var customerService = new StripeCustomerService();
            try
            {
            StripeCustomer stripeCustomer = customerService.Update(user.CustomerId, myCustomer);
            var subscriptionService = new StripeSubscriptionService();
            StripeSubscriptionUpdateOptions NewPlan = new StripeSubscriptionUpdateOptions();
            NewPlan.PlanId = form.PackageId.ToString();
            StripeSubscription stripeSubscription = subscriptionService.Update(stripeCustomer.Id, user.SubscriptionId, NewPlan);
            }
            catch (StripeException s)
            {
                string msg = s.StripeError.Message;
                return View("ProcessingError", new PayErrorModel { Error = msg });
            }
            user.PackageId = form.PackageId;
            await UserManager.UpdateAsync(user);
            return View("SubscriptionSet", user.Package);
        }

        public ActionResult Test()
        {
            return View();
        }

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
    }

}