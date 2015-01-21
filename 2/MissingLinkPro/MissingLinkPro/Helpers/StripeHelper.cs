using IdentitySample.Models;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MissingLinkPro.Helpers
{
    /**
     * A collection of Helper methods that interface with the Stripe.Net library.
     * Most methods return an ApplicationUser object out of the expectation that the user
     * variables will be updated in a separate async call to the database.
     **/
    public static class StripeHelper
    {

        /**
         * Assigns a new subscription; assumes that there is no pre-existing subscription.
         * If no CustomerId exists, then one is assigned, and the plan is assigned at the
         * time the Customer in Stripe's database is created. If there is a pre-existing
         * CustomerId, then it simply assigns a new plan.
         * 
         **/
        public static ApplicationUser AssignNewSubscription(ApplicationUser user, int NewPlanId, string stripeToken = null)
        {
                if (user.CustomerId == null || user.CustomerId == "")    // brand-new user; must create Customer account.
                {
                    var myCustomer = new StripeCustomerCreateOptions();
                    myCustomer.Email = user.Email;
                    myCustomer.PlanId = NewPlanId.ToString();
                    //myCustomer.Quantity = 1;                               // optional, defaults to 1 (Stripe.Net note)
                    var customerService = new StripeCustomerService();
                    StripeCustomer stripeCustomer = customerService.Create(myCustomer);
                    user.CustomerId = stripeCustomer.Id;
                    user.SubscriptionId = stripeCustomer.StripeSubscriptionList.StripeSubscriptions.ElementAt(0).Id;
                }
                else // pre-existing customer with existing CustomerID.
                {

                    var subscriptionService = new StripeSubscriptionService();
                    StripeSubscription stripeSubscription = subscriptionService.Create(user.CustomerId, NewPlanId.ToString()); // optional StripeSubscriptionCreateOptions

                    var myCustomer = new StripeCustomerUpdateOptions();
                    if (stripeToken != null)
                        myCustomer.TokenId = stripeToken;
                    var customerService = new StripeCustomerService();
                    StripeCustomer stripeCustomer = customerService.Update(user.CustomerId, myCustomer);

                    user.Anniversary = stripeSubscription.PeriodStart.Value.Add(new TimeSpan(-5, 0, 0));
                    user.SubscriptionId = stripeSubscription.Id;
                    user.PackageId = NewPlanId;
                }

            user.DateTimeStamp = DateTime.Now;
            user.IsActive = true;
            return user;
        } // AssignPackagePlan

        /**
         * Changes the subscription for a given user; assumes that a subscription already exists.
         * Use the HasSubscription method to check for pre-existing subscriptions.
         **/
        public static ApplicationUser ChangePackagePlan(ApplicationUser user, int NewPlanId, string stripeToken = null)
        {
            var myCustomer = new StripeCustomerUpdateOptions();
            if (stripeToken != null)
                myCustomer.TokenId = stripeToken;
            var customerService = new StripeCustomerService();

                StripeCustomer stripeCustomer = customerService.Update(user.CustomerId, myCustomer);
                var subscriptionService = new StripeSubscriptionService();
                StripeSubscriptionUpdateOptions NewPlan = new StripeSubscriptionUpdateOptions();
                NewPlan.PlanId = NewPlanId.ToString();
                StripeSubscription stripeSubscription = subscriptionService.Update(stripeCustomer.Id, user.SubscriptionId, NewPlan);
                user.Anniversary = stripeSubscription.PeriodStart.Value.Add(new TimeSpan(-5, 0, 0));
                user.PackageId = NewPlanId;
                user.IsActive = true;

            return user;
        } // UpgradePackagePlan

        /**
         * Changes the package plan for a given user without the presence of a stripeToken.
         * This method is only accessible via UserAdmin controls, and is therefore an option
         * available only to Admin accounts.
         **/
        public static ApplicationUser AdminChangePackagePlan(ApplicationUser user, int NewPlanId)
        {
            var myCustomer = new StripeCustomerUpdateOptions();
            var customerService = new StripeCustomerService();

                StripeCustomer stripeCustomer = customerService.Update(user.CustomerId, myCustomer);
                var subscriptionService = new StripeSubscriptionService();
                StripeSubscriptionUpdateOptions NewPlan = new StripeSubscriptionUpdateOptions();
                NewPlan.PlanId = NewPlanId.ToString();
                StripeSubscription stripeSubscription = subscriptionService.Update(stripeCustomer.Id, user.SubscriptionId, NewPlan);
                user.Anniversary = stripeSubscription.PeriodStart.Value.Add(new TimeSpan(-5, 0, 0));
                user.PackageId = NewPlanId;
                user.IsActive = true;

            return user;
        }

        public static bool CheckForExistingSubscription(ApplicationUser user)
        {
            bool b = false;
            var customerService = new StripeCustomerService();
            var subscriptionService = new StripeSubscriptionService();
            IEnumerable<StripeSubscription> response = subscriptionService.List(user.CustomerId);
            List<StripeSubscription> list = response.ToList();
            if (list.Count > 0)
            {
                b = true;
            }
            return b;
        }

        public static StripeCard GetCreditCard(ApplicationUser user)
        {
            StripeCard retrieved = null;

                var cardService = new StripeCardService();
                IEnumerable<StripeCard> response = cardService.List(user.CustomerId);
                retrieved = response.ElementAt(0);

            return retrieved;
        } // GetCreditCard

        public static ApplicationUser CancelSubscription(ApplicationUser user)
        {
            var subscriptionService = new StripeSubscriptionService();
            subscriptionService.Cancel(user.CustomerId, user.SubscriptionId, true); // optional cancelAtPeriodEnd flag
            user.IsActive = false;
            return user;
        } // CancelSubscription

        public static ApplicationUser RemoveCreditCard(ApplicationUser user)
        {
            var cardService = new StripeCardService();
            IEnumerable<StripeCard> response = cardService.List(user.CustomerId); // optional StripeListOptions
            foreach (StripeCard card in response)
            {
                var retrieveCard = new StripeCardService();
                   retrieveCard.Delete(user.CustomerId, card.Id);
            }
            return user;
        }

        public static string GetSubscriptionStatus(ApplicationUser user)
        {
            string status = "";
            var subscriptionService = new StripeSubscriptionService();
            StripeSubscription stripeSubscription = subscriptionService.Get(user.CustomerId, user.SubscriptionId);
            status = stripeSubscription.Status;
            return status;
        }

        public static bool UserHasCreditCard(ApplicationUser user)
        {
            bool b = true;

                var cardService = new StripeCardService();
                IEnumerable<StripeCard> response = cardService.List(user.CustomerId); // optional StripeListOptions
                if (response.Count() <= 0) b = false;

            return b;
        } // UserHasCreditCard

        public static bool UserHasSubscription(ApplicationUser user)
        {
            bool b = true;

                var subscriptionService = new StripeSubscriptionService();
                IEnumerable<StripeSubscription> response = subscriptionService.List(user.CustomerId);
                if (response.Count() <= 0) b = false;

            return b;
        } // UserHasSubscription

        /**
         * Updates the Anniversary of the user in the local database, using the information retrieved from the user's
         * latest Stripe subscription.
         **/
        public static ApplicationUser UpdateAnniversary(ApplicationUser user)
        {
            var subscriptionService = new StripeSubscriptionService();
            StripeSubscription stripeSubscription = subscriptionService.Get(user.CustomerId, user.SubscriptionId);
            user.Anniversary = stripeSubscription.PeriodStart.Value.Add(new TimeSpan(-5, 0, 0));
            return user;
        } // UpdateAnniversary

        public static ApplicationUser UpdateCreditCard(ApplicationUser user, string stripeToken)
        {
            var subscriptionService = new StripeSubscriptionService();
            StripeSubscriptionUpdateOptions ssuo = new StripeSubscriptionUpdateOptions();
            ssuo.TokenId = stripeToken;

                StripeSubscription stripeSubscription = subscriptionService.Update(user.CustomerId, user.SubscriptionId, ssuo);
                user.Anniversary = stripeSubscription.PeriodStart.Value.Add(new TimeSpan(-5, 0, 0));
                user.IsActive = true;

            return user;
        } // UpdateCreditCard


        public static ApplicationUser CreateNewCustomer(ApplicationUser user, string stripeToken)
        {
            var myCustomer = new StripeCustomerCreateOptions();
            myCustomer.Email = user.Email;                         // Attach email to account
            myCustomer.TokenId = stripeToken;                      // Attach token representing credit card
            //myCustomer.Quantity = 1;                               // optional, defaults to 1 (Stripe.Net note)
            var customerService = new StripeCustomerService();
            StripeCustomer stripeCustomer = customerService.Create(myCustomer);
            user.CustomerId = stripeCustomer.Id;
            return user;
        }
    } // StripeHelper
} // namespace