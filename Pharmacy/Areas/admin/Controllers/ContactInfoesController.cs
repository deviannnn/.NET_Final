using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Pharmacy.Models;

namespace Pharmacy.Areas.admin.Controllers
{
    [Authorize]
    public class ContactInfoesController : Controller
    {
        private PharmacyEntities db = new PharmacyEntities();

        public void getType(string selectedType = null)
        {
            var types = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Địa chỉ", Value = "address" },
                new SelectListItem { Text = "Số điện thoại", Value = "phone" },
                new SelectListItem { Text = "Email", Value = "email" }
            };

            ViewBag.Type = new SelectList(types, "Value", "Text", selectedType);
        }

        // GET: admin/ContactInfoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactInfo contactInfo = db.ContactInfoes.Find(id);
            if (contactInfo == null)
            {
                return HttpNotFound();
            }
            return View(contactInfo);
        }

        // GET: admin/ContactInfoes/Create
        public ActionResult Create()
        {
            getType();
            return View();
        }

        // POST: admin/ContactInfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,content,type,hide,order,datebegin")] ContactInfo contactInfo)
        {
            if (ModelState.IsValid)
            {
                contactInfo.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                db.ContactInfoes.Add(contactInfo);
                db.SaveChanges();
                return RedirectToAction("Footer", "Home");
            }

            return View(contactInfo);
        }

        // GET: admin/ContactInfoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactInfo contactInfo = db.ContactInfoes.Find(id);
            if (contactInfo == null)
            {
                return HttpNotFound();
            }
            getType(contactInfo.type);
            return View(contactInfo);
        }

        // POST: admin/ContactInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,content,type,hide,order,datebegin")] ContactInfo contactInfo)
        {
            if (ModelState.IsValid)
            {
                contactInfo.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                db.Entry(contactInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Footer", "Home");
            }
            return View(contactInfo);
        }

        // GET: admin/ContactInfoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactInfo contactInfo = db.ContactInfoes.Find(id);
            if (contactInfo == null)
            {
                return HttpNotFound();
            }
            return View(contactInfo);
        }

        // POST: admin/ContactInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ContactInfo contactInfo = db.ContactInfoes.Find(id);
            db.ContactInfoes.Remove(contactInfo);
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
