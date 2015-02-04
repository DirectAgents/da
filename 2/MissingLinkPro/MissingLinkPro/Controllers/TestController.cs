using IdentitySample.Models;
using Stripe;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using System.Data.Entity;
using MissingLinkPro.Helpers;
using System;

namespace MissingLinkPro.Controllers
{
    public class TestController : HttpsBaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Admin")]
        public ActionResult Seed()
        {
            ApplicationDbInitializer.InitializeIdentityForEF(db);

            return Content("okay");
        }

        /**
         * Method will create a Freemium account if one doesn't exist.
         * Should only be used once Stripe is switched to Live Mode.
         **/
        private void CreateFreemiumLive()
        {
            bool PlanOneExists = true;
            try
            {
                var planService = new StripePlanService();
                StripePlan response = planService.Get("1");
                if (response.Id.Equals("1")) return;   // Plan 1 exists; no problem.
            }
            catch (StripeException) // This should occur if Plan 1 doesn't exist.
            {
                PlanOneExists = false;
            }
            if (!PlanOneExists) // Create new Freemium Plan, assigned ID = "1"
            {
                var myPlan = new StripePlanCreateOptions();
                myPlan.Amount = 0;
                myPlan.Currency = "usd";
                myPlan.Interval = "month";
                myPlan.Name = "Freemium";
                var planService = new StripePlanService();
                StripePlan response = planService.Create(myPlan);
            }
            return;
        }

        /**
         * This is to be used once. After switching Stripe to Live Mode, run this method to use the local DB entries
         * to generate new customers in Stripe's database. Method will also update local DB with new CustomerId and
         * SubscriptionId entries.
         **/
        [Authorize(Roles = "Admin")]
        public ActionResult CreateLiveAccounts()
        {
            CreateFreemiumLive();                               // Create a Freemium plan if one does not exist.
            List<ApplicationUser> Users = db.Users.ToList();    // This will retrieve a full list of email addresses for us.

            foreach (ApplicationUser u in Users)
            {
                if (!u.EmailConfirmed) continue;  // Skip unconfirmed accounts.

                ApplicationUser user = db.Users.SingleOrDefault(select => select.Email == u.Email);
                var myCustomer = new StripeCustomerCreateOptions();
                myCustomer.PlanId = "1";
                myCustomer.Email = u.Email;
                var customerService = new StripeCustomerService();
                StripeCustomer stripeCustomer = customerService.Create(myCustomer);
                user.CustomerId = stripeCustomer.Id;

                var subscriptionService = new StripeSubscriptionService();      // Retrieve new subscription.
                user.SubscriptionId = stripeCustomer.StripeSubscriptionList.StripeSubscriptions.ElementAt(0).Id;    // Assign SubscriptionId to DB entry.
                user.DateTimeStamp = stripeCustomer.StripeSubscriptionList.StripeSubscriptions.ElementAt(0).PeriodStart.Value;
                user.Anniversary = stripeCustomer.StripeSubscriptionList.StripeSubscriptions.ElementAt(0).PeriodStart.Value;
                user.QueriesPerformed = 0;
                user.TotalQueriesPerformed = 0;
                user.IsActive = true;
                user.PackageId = 1;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
            }
            return Content("OK");
        }

        [AllowAnonymous]
        public ActionResult IDKFA()
        {
            return Content("ALL WEAPONS, KEYS, AMMO UNLOCKED");
        }

        [AllowAnonymous]
        public ActionResult Cards()
        {
            return View();
        }
	}
}