﻿using IdentitySample.Models;
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
using MissingLinkPro.Helpers;

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

            // Email not confirmed
            if (!user.EmailConfirmed)
                return View("EmailNotConfirmed", new ApplicationUser { Email = user.Email });

            Package package = db.Packages.Find(id);
            if (package == null)
            {
                return HttpNotFound();
            }

            bool cardCheck = StripeHelper.UserHasCreditCard(user);

            return View(new PayViewModel { SubscribedPackageId = user.Package.Id, UserHasCreditCard = cardCheck, PackageId = id ?? default(int), PackageName = package.Name, PackageCost = package.CostPerMonth, IsActive = user.IsActive });
        } // Pay

        /**
         * The HttpPost version of Pay receives two variables: a string token via Stripe.js and the PackageId of the target package
         * to be upgraded to.
         **/
        [HttpPost]
        public async Task<ActionResult> Pay(PostPayViewModel form)
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            try
            {
                if (StripeHelper.UserHasSubscription(user))
                    user = StripeHelper.ChangePackagePlan(user, form.PackageId, form.stripeToken);
                else
                    user = StripeHelper.AssignNewSubscription(user, form.PackageId, form.stripeToken);
            }
            catch (StripeException s)
            {
                string msg = s.StripeError.Message;
                return View("ProcessingError", new PayErrorModel { Error = msg });
            }
            await UserManager.UpdateAsync(user);
            return View("SubscriptionSet", user.Package);
        }

        public async Task<ActionResult> UpdateCreditCard()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> UpdateCreditCard(string stripeToken)
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            //if (user.PackageId == 1) return View();

            try
            {
                user = StripeHelper.UpdateCreditCard(user, stripeToken);
            }
            catch (StripeException) { return View("Error"); }
            await UserManager.UpdateAsync(user);

            return View("CardUpdated");
        }

        public async Task<ActionResult> History()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            // Email not confirmed
            if (!user.EmailConfirmed)
            {
                return View("EmailNotConfirmed", new ApplicationUser { Email = user.Email });
            }

            StripeInvoiceListOptions silo = new StripeInvoiceListOptions();
            silo.CustomerId = user.CustomerId;
            var invoiceService = new StripeInvoiceService();
            IEnumerable<StripeInvoice> response = invoiceService.List(silo);

            //foreach (StripeInvoice si in response)
            //{
            //    displayln("Customer ID: " + si.CustomerId);
            //    displayln("Subscription ID:  " + si.SubscriptionId);
            //    displayln("Invoice Date: " + si.Date);
            //    displayln("Period: " + si.PeriodStart.ToString() + " " + si.PeriodEnd.ToString());
            //    displayln("Receipt Number: " + si.ReceiptNumber);
            //    displayln("Amount Due: " + si.AmountDue.ToString());
            //}
            return View(response);
        }

        public async Task<ActionResult> Cancel()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            return View(user);
        } // Cancel

        [HttpPost]
        public async Task<ActionResult> Cancel(bool CancelSubscription)
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            //displayln(CancelSubscription.ToString());
            if (CancelSubscription == true)
            {
                try
                {
                    user = StripeHelper.CancelSubscription(user);
                    user = StripeHelper.RemoveCreditCard(user);
                }
                catch (StripeException)
                {
                    return View("Error");
                }
                await UserManager.UpdateAsync(user);

                return View("CancelConfirmed");
            }
            else return View("Index", db.Packages.ToList());
        } // Cancel

        public async Task<ActionResult> Test()
        {
            return View("Error");
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