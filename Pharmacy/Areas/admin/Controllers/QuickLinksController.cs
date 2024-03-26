using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Pharmacy.Models;

namespace Pharmacy.Areas.admin.Controllers
{
    [Authorize]
    public class QuickLinksController : Controller
    {
        private PharmacyEntities db = new PharmacyEntities();

        // GET: admin/QuickLinks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuickLink quickLink = db.QuickLinks.Find(id);
            if (quickLink == null)
            {
                return HttpNotFound();
            }
            return View(quickLink);
        }

        // GET: admin/QuickLinks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: admin/QuickLinks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,link,hide,order,datebegin")] QuickLink quickLink)
        {
            if (ModelState.IsValid)
            {
                quickLink.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                db.QuickLinks.Add(quickLink);
                db.SaveChanges();
                return RedirectToAction("Footer", "Home");
            }

            return View(quickLink);
        }

        // GET: admin/QuickLinks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuickLink quickLink = db.QuickLinks.Find(id);
            if (quickLink == null)
            {
                return HttpNotFound();
            }
            return View(quickLink);
        }

        // POST: admin/QuickLinks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,link,hide,order,datebegin")] QuickLink quickLink)
        {
            if (ModelState.IsValid)
            {
                quickLink.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                db.Entry(quickLink).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Footer", "Home");
            }
            return View(quickLink);
        }

        // GET: admin/QuickLinks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuickLink quickLink = db.QuickLinks.Find(id);
            if (quickLink == null)
            {
                return HttpNotFound();
            }
            return View(quickLink);
        }

        // POST: admin/QuickLinks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            QuickLink quickLink = db.QuickLinks.Find(id);
            db.QuickLinks.Remove(quickLink);
            db.SaveChanges();
            return RedirectToAction("Footer", "Home");
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
