using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;

namespace MissingLinkPro.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SettingsController : HttpsBaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Settings/
        public ActionResult Index()
        {
            return View(db.Settings.ToList());
        }

        // GET: /Settings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Setting settings = db.Settings.Find(id);
            if (settings == null)
            {
                return HttpNotFound();
            }
            return View(settings);
        }

        // GET: /Settings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Settings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,SettingName,Value")] Setting settings)
        {
            if (ModelState.IsValid)
            {
                db.Settings.Add(settings);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(settings);
        }

        // GET: /Settings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Setting settings = db.Settings.Find(id);
            if (settings == null)
            {
                return HttpNotFound();
            }
            return View(settings);
        }

        // POST: /Settings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,SettingName,Value")] Setting settings)
        {
            if (ModelState.IsValid)
            {
                db.Entry(settings).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(settings);
        }

        // GET: /Settings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Setting settings = db.Settings.Find(id);
            if (settings == null)
            {
                return HttpNotFound();
            }
            return View(settings);
        }

        // POST: /Settings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Setting settings = db.Settings.Find(id);
            db.Settings.Remove(settings);
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
