using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MissingLinkPro.Controllers;
using MissingLinkPro.Helpers;
using Stripe;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IdentitySample.Controllers
{
    [Authorize]
    public class ManageController : HttpsBaseController
    {
        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        private ApplicationDbContext db = new ApplicationDbContext();
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
        // GET: /Account/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.ChangeNameSuccess ? "Your new name has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two factor provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "The phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : message == ManageMessageId.ChangeUserInfoSuccess ? "Your user information has been successfully updated."
                : message == ManageMessageId.ChangeUserPasswordSuccess ? "Your user information has been updated, and your password has been changed."
                : message == ManageMessageId.CreditCardUpdateSuccess ? "Your credit card has been successfully updated."
                : message == ManageMessageId.CardDeleteSuccess ? "Your credit card has been successfully deleted."
                : message == ManageMessageId.SubscriptionActivated ? "Your new subscription has been successfully activated."
                : message == ManageMessageId.SubscriptionCancelled ? "Your subscription has been successfully cancelled, and you will not be billed at the end of your cycle."
                : message == ManageMessageId.SubscriptionChanged ? "Your subscription has been successfully changed."
                : message == ManageMessageId.SubscriptionAlreadyCancelled ? "Your subscription has already been cancelled at an earlier time."
                : "";

            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (!user.EmailConfirmed) { return View("EmailNotConfirmed", new ApplicationUser { Email = user.Email }); }

            try
            {
                if ((DateTime.Now).CompareTo(user.Anniversary.AddMonths(1)) >= 0)
                    user = StripeHelper.UpdateAnniversary(user);
                await UserManager.UpdateAsync(user);
            }
            catch (StripeException e)
            {
                displayln(e.ToString());
                return View("Error");
            }
            IndexViewModel model = GenerateManageIndexModel(user);
            return View(model);
        } // Index

        public async Task<ActionResult> CreditCardManagement(CreditCardManagementMessageId? message)
        {
            ViewBag.StatusMessage =
                message == CreditCardManagementMessageId.NoCardFoundError ? "Currently there is no credit card linked to your account."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            bool HasExistingCard = StripeHelper.UserHasCreditCard(user);
            PayUpdateCreditCardViewModel model = new PayUpdateCreditCardViewModel();
            if (HasExistingCard)
            {
                model.Card = StripeHelper.GetCreditCard(user);
                model.HasCreditCard = true;
                if (model.Card.AddressLine2 == null) model.Card.AddressLine2 = "";
            }
            else
                return RedirectToAction("UpdateCreditCard" /*, new { Message = CreditCardManagementMessageId.NoCardFoundError }*/);
            return View(model);
        } // CreditCardManagement

        [HttpPost]
        public async Task<ActionResult> CreditCardManagement(string stripeToken)
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            try { user = StripeHelper.UpdateCreditCard(user, stripeToken); }
            catch (StripeException) { return View("Error"); }
            await UserManager.UpdateAsync(user);
            return RedirectToAction("Index", new { Message = ManageMessageId.CreditCardUpdateSuccess });
        } // CreditCardManagement POST

        private List<StripeInvoice> AdjustInvoiceItems(IEnumerable<StripeInvoice> list) {
            List<StripeInvoice> StripeInvoiceList = list.ToList();
            foreach (StripeInvoice si in StripeInvoiceList) {
                StripeInvoiceItem[] ItemArray = si.StripeInvoiceLines.StripeInvoiceItems.ToArray();
                for (int i = 0; i < ItemArray.Length; i++)
                {
                    if (ItemArray[i].Description == null)
                    {
                        int caseSwitch = ItemArray[i].Amount.Value;
                        switch (caseSwitch)
                        {
                            case 0:
                                ItemArray[i].Description = "Freemium Subscription";
                                break;
                            case 1999:
                                ItemArray[i].Description = "Bronze Subscription";
                                break;
                            case 3999:
                                ItemArray[i].Description = "Silver Subscription";
                                break;
                            case 5999:
                                ItemArray[i].Description = "Gold Subscription";
                                break;
                            case 9999:
                                ItemArray[i].Description = "Platinum Subscription";
                                break;
                            default:
                                ItemArray[i].Description = "Other";
                                break;
                        }
                    }
                }
            }
            return StripeInvoiceList;
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
            IEnumerable<StripeInvoice> response = AdjustInvoiceItems(invoiceService.List(silo));

            // IEnumerable<StripeInvoice> response = invoiceService.List(silo);

            //foreach (stripeinvoice si in response)
            //{
            //    stripeinvoiceitem[] array = si.stripeinvoicelines.stripeinvoiceitems.toarray();
            //    for (int i = 0; i < array.length; i++)
            //    {
            //        if (array[i].description == null) array[i].description = "empty";
            //    }
            //}

            //foreach (StripeInvoice si in response)
            //{
            //    displayln("Customer ID: " + si.CustomerId);
            //    displayln("Subscription ID:  " + si.SubscriptionId);
            //    displayln("Invoice Date: " + si.Date);
            //    displayln("Period: " + si.PeriodStart.ToString() + " " + si.PeriodEnd.ToString());
            //    displayln("Receipt Number: " + si.ReceiptNumber);
            //    displayln("Amount Due: " + si.AmountDue.ToString());
            //}
            return View(new List<StripeInvoice>(response));
        } // History

        public async Task<ActionResult> UpdateCreditCard(CreditCardManagementMessageId? message)
        {
            ViewBag.StatusMessage =
                message == CreditCardManagementMessageId.NoCardFoundError ? "We could not find a credit card linked to your account."
                : message == CreditCardManagementMessageId.Error ? "An error occurred while trying to process your credit card."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            bool cardFound = StripeHelper.UserHasCreditCard(user);
            PayUpdateCreditCardViewModel model = new PayUpdateCreditCardViewModel { HasMessage = false, HasCreditCard = cardFound };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateCreditCard(string stripeToken)
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            try
            {
                user = StripeHelper.UpdateCreditCard(user, stripeToken);
            }
            catch (StripeException e)
            {
                ViewBag.StatusMessage = "Error: " + e.Message;
                return View();
                //return View(new PayUpdateCreditCardViewModel() { HasMessage = true, Message = "Error: " + e.Message });
            }
            await UserManager.UpdateAsync(user);

            return RedirectToAction("Index", new { Message = ManageMessageId.CreditCardUpdateSuccess });
        }

        public async Task<ActionResult> DeleteCreditCard()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            bool cardFound = StripeHelper.UserHasCreditCard(user);
            PayUpdateCreditCardViewModel model = new PayUpdateCreditCardViewModel { HasCreditCard = cardFound };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteCreditCard(bool DeleteCard)
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (DeleteCard == true)
            {
                try
                {
                    user = StripeHelper.RemoveCreditCard(user);
                }
                catch (StripeException)
                {
                    return View("Error");
                }
                await UserManager.UpdateAsync(user);
            return RedirectToAction("Index", new { Message = ManageMessageId.CardDeleteSuccess });
            }
            return RedirectToAction("Index");
        }

        //
        // GET: /Subscriptions/
        public async Task<ActionResult> Subscriptions(SubscriptionsMessageId? message)
        {

            ViewBag.Message =
                message == SubscriptionsMessageId.Error ? "An error occurred while trying to process your request."
                : "";

            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            return View(new PayIndexViewModel { PackageId = user.PackageId.Value, ListofPackages = db.Packages.ToList() });
        } // Subscriptions

        public async Task<ActionResult> PaySubscription(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (!user.EmailConfirmed)             // Email not confirmed
                return View("EmailNotConfirmed", new ApplicationUser { Email = user.Email });

            Package package = db.Packages.Find(id);
            if (package == null)
            {
                return HttpNotFound();
            }

            bool cardCheck = StripeHelper.UserHasCreditCard(user);

            var model = new PayViewModel
            {
                SubscribedPackageId = user.Package.Id,
                UserHasCreditCard = cardCheck,
                PackageId = id ?? default(int),
                PackageName = package.Name,
                PackageCost = package.CostPerMonth,
                SearchesPerMonth = package.SearchesPerMonth,
                MaxResults = package.MaxResults,
                IsActive = user.IsActive
            };
            return View(model);
        } // Pay

        /**
         * The HttpPost version of Pay receives two variables: a string token via Stripe.js and the PackageId of the target package
         * to be upgraded to.
         **/
        [HttpPost]
        public async Task<ActionResult> PaySubscription(PostPayViewModel form)
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            try
            {
                user = StripeHelper.UpdateCreditCard(user, form.stripeToken);

                if (StripeHelper.UserHasSubscription(user))
                    user = StripeHelper.ChangePackagePlan(user, form.PackageId, form.stripeToken);
                else
                    user = StripeHelper.AssignNewSubscription(user, form.PackageId, form.stripeToken);
            }
            catch (StripeException e)
            {
                Package p = db.Packages.Find(form.PackageId);
                string msg = e.StripeError.Message;
                ViewBag.StatusMessage = "Error: " + e.Message;
                PayViewModel ErrorModel = new PayViewModel { PackageId = p.Id, PackageName = p.Name, SearchesPerMonth = p.SearchesPerMonth, MaxResults = p.MaxResults, PackageCost = p.CostPerMonth };
                //PayIndexViewModel ErrorModel = new PayIndexViewModel { PackageId = user.PackageId.Value, ListofPackages = db.Packages.ToList(), HasMessage = true, Message = "Error: " + e.Message };
                return View(ErrorModel);
            }
            await UserManager.UpdateAsync(user);
            return RedirectToAction("Index", new { Message = ManageMessageId.SubscriptionChanged });
        } // Pay[Post]

        public async Task<ActionResult> CancelSubscription()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (!user.IsActive)
                return RedirectToAction("Index", new { Message = ManageMessageId.SubscriptionAlreadyCancelled });
            return View(user);
        } // Cancel

        [HttpPost]
        public async Task<ActionResult> CancelSubscription(bool CancelSubscription)
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            PayIndexViewModel model = new PayIndexViewModel { PackageId = user.PackageId.Value, ListofPackages = db.Packages.ToList(), HasMessage = false, Message = "" };

            if (CancelSubscription == true)
            {
                try
                {
                    user = StripeHelper.CancelSubscription(user);
                    user = StripeHelper.RemoveCreditCard(user);
                }
                catch (StripeException e)
                {
                    model.HasMessage = true;
                    model.Message = e.Message;
                    return View("Subscriptions",model);
                }
                await UserManager.UpdateAsync(user);
                return RedirectToAction("Index", new { Message = ManageMessageId.SubscriptionCancelled });
            }
            return RedirectToAction("Subscription");
        } // Cancel

        //
        // GET: /Account/RemoveLogin
        public ActionResult RemoveLogin()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return View(linkedAccounts);
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Account/UpdateUser
        public async Task<ActionResult> UpdateUser()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            UpdateUserViewModel model = new UpdateUserViewModel { FirstName = user.FirstName, LastName = user.LastName, Number = user.PhoneNumber };
            return View(model);
        }

        //
        // POST: /Account/UpdateUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateUser(UpdateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null) 
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.PhoneNumber = model.Number;
                var userSynced = await UserManager.UpdateAsync(user);
                if (!userSynced.Succeeded) return View("Error");

                bool PasswordChanged = false;
                if (model.OldPassword != null && model.NewPassword != null && model.ConfirmPassword != null && (model.NewPassword.Equals(model.ConfirmPassword)))
                {
                    var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (!result.Succeeded)
                    {
                        AddErrors(result);
                        return View(model);
                    }
                    await SignInAsync(user, isPersistent: false);
                    PasswordChanged = true;
                }

                if (PasswordChanged) return RedirectToAction("Index", new { Message = ManageMessageId.ChangeUserPasswordSuccess });
                else return RedirectToAction("Index", new { Message = ManageMessageId.ChangeUserInfoSuccess });
            }
            return View(model);
        }

        /**
         * Shortcut method that generates the model to be viewed in Manage/Index, using the user's stored data.
         **/
        private IndexViewModel GenerateManageIndexModel(ApplicationUser user, bool HasMessageArg = false, string MessageArg = "") {

            string CardMessage = "";
            if (StripeHelper.UserHasCreditCard(user))
            {
                StripeCard card = StripeHelper.GetCreditCard(user);
                CardMessage = card.Brand + " ************" + card.Last4 + ", EXP " + card.ExpirationMonth + "/" + card.ExpirationYear;
            }
            else
                CardMessage = "[None]";

            var model = new IndexViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Package = user.Package,
                PhoneNumber = user.PhoneNumber,
                QueriesPerformed = user.QueriesPerformed,
                DateTimeStamp = user.DateTimeStamp,
                Anniversary = user.Anniversary,
                HasPassword = HasPassword(),
                CustomerId = user.CustomerId,
                SubscriptionId = user.SubscriptionId,
                IsActive = user.IsActive,
                CardSummary = CardMessage,
                HasMessage = HasMessageArg,
                Message = MessageArg
                //PhoneNumber = await UserManager.GetPhoneNumberAsync(User.Identity.GetUserId()),
                //TwoFactor = await UserManager.GetTwoFactorEnabledAsync(User.Identity.GetUserId()),
                //Logins = await UserManager.GetLoginsAsync(User.Identity.GetUserId()),
                //BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(User.Identity.GetUserId())
            };
            return model;
        } // GenerateManageIndexModel

        //
        // GET: /Account/AddPhoneNumber
        //public ActionResult AddPhoneNumber()
        //{
        //    return View();
        //}

        //
        // POST: /Account/AddPhoneNumber
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }
        //    // Generate the token and send it
        //    var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
        //    if (UserManager.SmsService != null)
        //    {
        //        var message = new IdentityMessage
        //        {
        //            Destination = model.Number,
        //            Body = "Your security code is: " + code
        //        };
        //        await UserManager.SmsService.SendAsync(message);
        //    }
        //    return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        //}

        //
        // POST: /Manage/RememberBrowser
        [HttpPost]
        public ActionResult RememberBrowser()
        {
            var rememberBrowserIdentity = AuthenticationManager.CreateTwoFactorRememberBrowserIdentity(User.Identity.GetUserId());
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = true }, rememberBrowserIdentity);
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/ForgetBrowser
        [HttpPost]
        public ActionResult ForgetBrowser()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/EnableTFA
        //[HttpPost]
        //public async Task<ActionResult> EnableTFA()
        //{
        //    await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
        //    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
        //    if (user != null)
        //    {
        //        await SignInAsync(user, isPersistent: false);
        //    }
        //    return RedirectToAction("Index", "Manage");
        //}

        //
        // POST: /Manage/DisableTFA
        //[HttpPost]
        //public async Task<ActionResult> DisableTFA()
        //{
        //    await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
        //    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
        //    if (user != null)
        //    {
        //        await SignInAsync(user, isPersistent: false);
        //    }
        //    return RedirectToAction("Index", "Manage");
        //}

        //
        // GET: /Account/VerifyPhoneNumber
        //public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        //{
        //    // This code allows you exercise the flow without actually sending codes
        //    // For production use please register a SMS provider in IdentityConfig and generate a code here.
        //    var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
        //    ViewBag.Status = "For DEMO purposes only, the current code is " + code;
        //    return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        //}

        //
        // POST: /Account/VerifyPhoneNumber
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }
        //    var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
        //    if (result.Succeeded)
        //    {
        //        var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
        //        if (user != null)
        //        {
        //            await SignInAsync(user, isPersistent: false);
        //        }
        //        return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
        //    }
        //    // If we got this far, something failed, redisplay form
        //    ModelState.AddModelError("", "Failed to verify phone");
        //    return View(model);
        //}

        //
        // GET: /Account/RemovePhoneNumber
        //public async Task<ActionResult> RemovePhoneNumber()
        //{
        //    var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
        //    if (!result.Succeeded)
        //    {
        //        return RedirectToAction("Index", new { Message = ManageMessageId.Error });
        //    }
        //    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
        //    if (user != null)
        //    {
        //        await SignInAsync(user, isPersistent: false);
        //    }
        //    return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        //}

        //
        // GET: /Manage/ChangePassword
        //public ActionResult ChangePassword()
        //{
        //    return View();
        //}

        //
        // GET: /Manage/ChangeName
        //public ActionResult ChangeName()
        //{
        //    return View();
        //}

        //
        // POST: Manage/ChangeName
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ChangeName(ChangeNameViewModel editName)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
        //        //var user = await UserManager.FindByIdAsync(editName.Id);
        //        if (user == null)
        //        {
        //            return HttpNotFound();
        //        }

        //        user.FirstName = editName.NewFirstName;
        //        user.LastName = editName.NewLastName;

        //        var userSynced = await UserManager.UpdateAsync(user);

        //        if (!userSynced.Succeeded)
        //        {
        //            ModelState.AddModelError("", userSynced.Errors.First());
        //            return View();
        //        }
        //        return RedirectToAction("Index", new { Message = ManageMessageId.ChangeNameSuccess} );
        //    }
        //    ModelState.AddModelError("", "Something failed.");
        //    return View(editName);
        //}

        //
        // POST: /Account/Manage
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }
        //    var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
        //    if (result.Succeeded)
        //    {
        //        var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
        //        if (user != null)
        //        {
        //            await SignInAsync(user, isPersistent: false);
        //        }
        //        return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
        //    }
        //    AddErrors(result);
        //    return View(model);
        //}

        //
        // GET: /Manage/SetPassword
        //public ActionResult SetPassword()
        //{
        //    return View();
        //}

        ////
        //// POST: /Manage/SetPassword
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
        //        if (result.Succeeded)
        //        {
        //            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
        //            if (user != null)
        //            {
        //                await SignInAsync(user, isPersistent: false);
        //            }
        //            return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
        //        }
        //        AddErrors(result);
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        //
        // GET: /Account/Manage
        //public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        //{
        //    ViewBag.StatusMessage =
        //        message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
        //        : message == ManageMessageId.Error ? "An error has occurred."
        //        : "";
        //    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
        //    if (user == null)
        //    {
        //        return View("Error");
        //    }
        //    var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
        //    var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
        //    ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
        //    return View(new ManageLoginsViewModel
        //    {
        //        CurrentLogins = userLogins,
        //        OtherLogins = otherLogins
        //    });
        //}

        //
        // POST: /Manage/LinkLogin
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult LinkLogin(string provider)
        //{
        //    // Request a redirect to the external login provider to link a login for the current user
        //    return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        //}

        //
        // GET: /Manage/LinkLoginCallback
        //public async Task<ActionResult> LinkLoginCallback()
        //{
        //    var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
        //    if (loginInfo == null)
        //    {
        //        return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        //    }
        //    var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
        //    return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        //}

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

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            ChangeNameSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            ChangeUserInfoSuccess,
            ChangeUserPasswordSuccess,
            Error,
            CreditCardUpdateSuccess,
            CardDeleteSuccess,
            SubscriptionActivated,
            SubscriptionCancelled,
            SubscriptionChanged,
            SubscriptionAlreadyCancelled
        }

        public enum CreditCardManagementMessageId
        {
            NoCardFoundError,
            Error
        }

        public enum SubscriptionsMessageId
        {
            Error
        }

        #endregion
    }


}