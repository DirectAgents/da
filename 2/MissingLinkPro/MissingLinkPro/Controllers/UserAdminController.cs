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
                var user = new ApplicationUser { UserName = userViewModel.Email, Email = userViewModel.Email, EmailConfirmed = true, FirstName = userViewModel.FirstName, LastName = userViewModel.LastName, TotalQueriesPerformed = 0, QueriesPerformed = 0, DateTimeStamp = DateTime.Now, Anniversary = DateTime.Now, PackageId = freemium.Id };
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
        public async Task<ActionResult> Edit([Bind(Include = "Email,Id,FirstName,LastName,QueriesPerformed,PackageId,EmailConfirmed,CustomerId,SubscriptionId")] EditUserViewModel editUser, params string[] selectedRole)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(editUser.Id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                user.UserName = editUser.Email;
                user.Email = editUser.Email;
                user.FirstName = editUser.FirstName;
                user.LastName = editUser.LastName;
                user.CustomerId = editUser.CustomerId;
                user.SubscriptionId = editUser.SubscriptionId;
                user.QueriesPerformed = editUser.QueriesPerformed;
                if (editUser.PackageId != null)
                {
                    if (user.PackageId != editUser.PackageId)
                        user = StripeHelper.AdminChangePackagePlan(user, editUser.PackageId.Value);
                }
                user.EmailConfirmed = editUser.EmailConfirmed;

                var userRoles = await UserManager.GetRolesAsync(user.Id);

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
