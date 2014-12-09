using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;
using Stripe;

namespace MissingLinkPro.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PackagesController : HttpsBaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Packages/
        public ActionResult Index()
        {
            return View(db.Packages.ToList());
        }

        // GET: /Packages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Package package = db.Packages.Find(id);
            if (package == null)
            {
                return HttpNotFound();
            }
            return View(package);
        }

        // GET: /Packages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Packages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Name,SearchesPerMonth,MaxResults,CostPerMonth")] Package package)
        {
            if (ModelState.IsValid)
            {
                db.Packages.Add(package);
                db.SaveChanges();

                Package retrieve = db.Packages.Find(package.Id);


                /*Stripe Code begins here*/
                var myPlan = new StripePlanCreateOptions();
                myPlan.Id = retrieve.Id.ToString();
                myPlan.Amount = Convert.ToInt32(package.CostPerMonth*100);           // all amounts on Stripe are in cents, pence, etc
                myPlan.Currency = "usd";        // "usd" only supported right now
                myPlan.Interval = "month";      // "month" or "year"
                myPlan.IntervalCount = 1;       // optional
                myPlan.Name = package.Name;
                myPlan.TrialPeriodDays = 0;    // amount of time that will lapse before the customer is billed

                var planService = new StripePlanService();
                StripePlan response = planService.Create(myPlan);
                /*Stripe Code ends here*/

                return RedirectToAction("Index");
            }

            return View(package);
        }

        // GET: /Packages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Package package = db.Packages.Find(id);
            if (package == null)
            {
                return HttpNotFound();
            }
            return View(package);
        }

        // POST: /Packages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,SearchesPerMonth,MaxResults,StatementDescription")] Package package)
        {
            if (ModelState.IsValid)
            {
                /*Stripe Code begins here*/
                var myPlan = new StripePlanUpdateOptions();
                myPlan.Name = package.Name;
                myPlan.StatementDescription = package.StatementDescription;
                var planService = new StripePlanService();
                StripePlan response = planService.Update(package.Id.ToString(), myPlan);
                /*Stripe Code ends here*/

                db.Entry(package).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(package);
        }

        // GET: /Packages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Package package = db.Packages.Find(id);
            if (package == null)
            {
                return HttpNotFound();
            }
            return View(package);
        }

        // POST: /Packages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Package package = db.Packages.Find(id);

            var planService = new StripePlanService();
            planService.Delete(package.Id.ToString());

            db.Packages.Remove(package);
            db.SaveChanges();
            return RedirectToAction("Index");
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
