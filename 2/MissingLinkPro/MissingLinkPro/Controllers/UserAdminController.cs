using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Stripe;
using MissingLinkPro.Helpers;
using MissingLinkPro.Controllers;

namespace IdentitySample.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersAdminController : HttpsBaseController
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        public UsersAdminController()
        {
        }

        public UsersAdminController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
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

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        //
        // GET: /Users/
        public async Task<ActionResult> Index()
        {
            if (TempData["Message"] != null) ViewBag.StatusMessage = (string)TempData["Message"];
            return View(await UserManager.Users.ToListAsync());
        }

        //
        // GET: /Users/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);

            ViewBag.RoleNames = await UserManager.GetRolesAsync(user.Id);

            return View(user);
        }

        //
        // GET: /Users/AttachCreditCard
        public async Task<ActionResult> AttachCreditCard(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            AttachCreditCardViewModel model = new AttachCreditCardViewModel { Email = user.Email, Id = id, stripeToken = null, CreditCardLastFour = "", HasCard = false };
            if (StripeHelper.UserHasCreditCard(user))
            {
                model.HasCard = true;
                StripeCard card = StripeHelper.GetCreditCard(user);
                model.CreditCardLastFour = card.Brand + " ************" + card.Last4 + ", EXP " + card.ExpirationMonth + "/" + card.ExpirationYear;
            }
            else
                model.CreditCardLastFour = "[None]";
            return View(model);
        }

        public async Task<ActionResult> DeleteCreditCard(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            bool cardFound = StripeHelper.UserHasCreditCard(user);
            PayUpdateCreditCardViewModel model = new PayUpdateCreditCardViewModel { HasCreditCard = cardFound, Id = id };
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> DeleteCreditCard(bool DeleteCard, string Id)
        {
            var user = await UserManager.FindByIdAsync(Id);
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
                TempData["Message"] = (string)"Card successfully deleted for " + user.Id + ".";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        //
        // POST: /Users/AttachCreditCard
        [HttpPost]
        public async Task<ActionResult> AttachCreditCard([Bind(Include = "Id,Email,stripeToken")] AttachCreditCardViewModel model)
        {
            var user = await UserManager.FindByIdAsync(model.Id);
            user = StripeHelper.UpdateCreditCard(user, model.stripeToken);
            StripeCard card = StripeHelper.GetCreditCard(user);
            model.CreditCardLastFour = card.Brand + " ************" + card.Last4 + ", EXP " + card.ExpirationMonth + "/" + card.ExpirationYear;
            model.HasCard = true;
            await UserManager.UpdateAsync(user);
            ViewBag.StatusMessage = "Credit card successfully updated.";
            return View(model);
        }

        public async Task<ActionResult> History(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
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
        private List<StripeInvoice> AdjustInvoiceItems(IEnumerable<StripeInvoice> list)
        {
            List<StripeInvoice> StripeInvoiceList = list.ToList();
            foreach (StripeInvoice si in StripeInvoiceList)
            {
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
        } // AdjustInvoiceItems

        //
        // GET: /Users/Create
        public async Task<ActionResult> Create()
        {
            //Get the list of Roles
            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
            return View();
        }

        //
        // POST: /Users/Create
        [HttpPost]
        public async Task<ActionResult> Create(RegisterViewModel userViewModel, params string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                Package freemium = db.Packages.Find(1);                    // We assume that "1" corresponds to the Freemium plan stored in database.
                var user = new ApplicationUser { UserName = userViewModel.Email, Email = userViewModel.Email, EmailConfirmed = true, FirstName = userViewModel.FirstName, LastName = userViewModel.LastName, CompanyName = userViewModel.CompanyName, TotalQueriesPerformed = 0, QueriesPerformed = 0, DateTimeStamp = DateTime.Now, Anniversary = DateTime.Now, PackageId = freemium.Id, DateCreated = DateTime.Now };
                var adminresult = await UserManager.CreateAsync(user, userViewModel.Password);

                //Add User to the selected Roles 
                if (adminresult.Succeeded)
                {
                    if (selectedRoles != null)
                    {
                        var result = await UserManager.AddToRolesAsync(user.Id, selectedRoles);
                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError("", result.Errors.First());
                            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
                            return View();
                        }
                    }
                    /*Stripe: New Customer Code Begins Here*/
                    var myCustomer = new StripeCustomerCreateOptions();
                    myCustomer.Email = user.Email;
                    myCustomer.PlanId = "1";                               // "1" currently assumes that Freemium plan on Stripe dashboard is set to Id = "1"
                    myCustomer.Quantity = 1;                               // optional, defaults to 1
                    var customerService = new StripeCustomerService();
                    StripeCustomer stripeCustomer = customerService.Create(myCustomer);
                    user.CustomerId = stripeCustomer.Id;
                    user.SubscriptionId = stripeCustomer.StripeSubscriptionList.StripeSubscriptions.ElementAt(0).Id;

                    await UserManager.UpdateAsync(user);
                    /*Stripe: New Customer Code Ends Here*/
                }
                else
                {
                    ModelState.AddModelError("", adminresult.Errors.First());
                    ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
                    return View();

                }
                return RedirectToAction("Index");
            }
            ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
            return View();
        }

        //
        // GET: /Users/Edit/1
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var userRoles = await UserManager.GetRolesAsync(user.Id);

            ViewBag.PackageId = new SelectList(db.Packages, "Id", "Name", user.PackageId);
            return View(new EditUserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CompanyName = user.CompanyName,
                CustomerId = user.CustomerId,
                SubscriptionId = user.SubscriptionId,
                QueriesPerformed = user.QueriesPerformed,
                TotalQueriesPerformed = user.TotalQueriesPerformed,
                LastQueryTime = user.DateTimeStamp,
                PackageId = user.PackageId,
                EmailConfirmed = user.EmailConfirmed,
                RolesList = RoleManager.Roles.ToList().Select(x => new SelectListItem()
                {
                    Selected = userRoles.Contains(x.Name),
                    Text = x.Name,
                    Value = x.Name
                })
            });
        }

        //
        // POST: /Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Email,Id,FirstName,LastName,CompanyName,QueriesPerformed,PackageId,EmailConfirmed,CustomerId,SubscriptionId")] EditUserViewModel editUser, params string[] selectedRole)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(editUser.Id);
                var userRoles = await UserManager.GetRolesAsync(user.Id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                user.UserName = editUser.Email;
                user.Email = editUser.Email;
                user.FirstName = editUser.FirstName;
                user.LastName = editUser.LastName;
                user.CompanyName = editUser.CompanyName;
                user.CustomerId = editUser.CustomerId;
                user.SubscriptionId = editUser.SubscriptionId;
                user.QueriesPerformed = editUser.QueriesPerformed;
                if (editUser.PackageId != null)
                {
                    if (user.PackageId != editUser.PackageId)
                    {
                        try
                        {
                            user = StripeHelper.AdminChangePackagePlan(user, editUser.PackageId.Value);

                        }
                        catch (StripeException)
                        {
                            ViewBag.StatusMessage = "Subscription change failed. Double check and see if there is a valid credit card attached to the account.";
                            ViewBag.PackageId = new SelectList(db.Packages, "Id", "Name", user.PackageId);
                            return View(new EditUserViewModel()
                            {
                                Id = user.Id,
                                Email = user.Email,
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                CompanyName = user.CompanyName,
                                CustomerId = user.CustomerId,
                                SubscriptionId = user.SubscriptionId,
                                QueriesPerformed = user.QueriesPerformed,
                                TotalQueriesPerformed = user.TotalQueriesPerformed,
                                LastQueryTime = user.DateTimeStamp,
                                PackageId = user.PackageId,
                                EmailConfirmed = user.EmailConfirmed,
                                RolesList = RoleManager.Roles.ToList().Select(x => new SelectListItem()
                                {
                                    Selected = userRoles.Contains(x.Name),
                                    Text = x.Name,
                                    Value = x.Name
                                })
                            });
                        }
                    }
                }

                if (editUser.EmailConfirmed && !user.EmailConfirmed)    // Should only occur if user's email failed to confirm.
                {
                    user.EmailConfirmed = true;
                    user = StripeHelper.AssignNewSubscription(user, user.PackageId.Value);
                }

                user.EmailConfirmed = editUser.EmailConfirmed;
                selectedRole = selectedRole ?? new string[] { };

                var result = await UserManager.AddToRolesAsync(user.Id, selectedRole.Except(userRoles).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    ViewBag.PackageId = new SelectList(db.Packages, "Id", "Name", editUser.PackageId);
                    return View(editUser);
                }
                result = await UserManager.RemoveFromRolesAsync(user.Id, userRoles.Except(selectedRole).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    ViewBag.PackageId = new SelectList(db.Packages, "Id", "Name", editUser.PackageId);
                    return View(editUser);
                }
                return RedirectToAction("Index");
            }
            ViewBag.PackageId = new SelectList(db.Packages, "Id", "Name", editUser.PackageId);
            ModelState.AddModelError("", "Something failed.");
            return View(editUser);
        }

        //
        // GET: /Users/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var user = await UserManager.FindByIdAsync(id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                /*Stripe Code*/
                try
                {
                    var customerService = new StripeCustomerService();
                    customerService.Delete(user.CustomerId);
                }
                catch (StripeException)     // This exception will occur in cases where a local account is created in DB but not on Stripe's end.
                {
                    
                }
                /*Stripe Code*/

                var result = await UserManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }

                return RedirectToAction("Index");
            }
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
