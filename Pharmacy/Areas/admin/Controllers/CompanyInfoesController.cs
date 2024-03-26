using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Pharmacy.Models;

namespace Pharmacy.Areas.admin.Controllers
{
    [Authorize]
    public class CompanyInfoesController : Controller
    {
        private PharmacyEntities db = new PharmacyEntities();

        // GET: admin/CompanyInfoes
        public ActionResult Index()
        {
            return View(db.CompanyInfoes.ToList());
        }

        // GET: admin/CompanyInfoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyInfo companyInfo = db.CompanyInfoes.Find(id);
            if (companyInfo == null)
            {
                return HttpNotFound();
            }
            return View(companyInfo);
        }

        // GET: admin/CompanyInfoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: admin/CompanyInfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,tittle,content,img,link,hide,order,datebegin")] CompanyInfo companyInfo, HttpPostedFileBase img)
        {
            try
            {
                var path = "";
                var filename = "";
                if (ModelState.IsValid)
                {
                    if (img != null)
                    {
                        filename = DateTime.Now.ToString("dd-MM-yy-hh-mm-ss-") + img.FileName;
                        path = Path.Combine(Server.MapPath("~/wwwroot/upload/img/companyinfo"), filename);
                        img.SaveAs(path);
                        companyInfo.img = filename;
                    }
                    else
                    {
                        companyInfo.img = "logo.png";
                    }
                    companyInfo.link = (companyInfo.link != null) ? companyInfo.link : "";
                    companyInfo.order = 1;
                    companyInfo.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                    db.CompanyInfoes.Add(companyInfo);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DbEntityValidationException e)
            {
                throw e;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(companyInfo);
        }

        // GET: admin/CompanyInfoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyInfo companyInfo = db.CompanyInfoes.Find(id);
            if (companyInfo == null)
            {
                return HttpNotFound();
            }
            return View(companyInfo);
        }

        // POST: admin/CompanyInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,tittle,content,img,link,hide,order,datebegin")] CompanyInfo companyInfo, HttpPostedFileBase img)
        {
            try
            {
                var path = "";
                var filename = "";
                CompanyInfo temp = db.CompanyInfoes.Find(companyInfo.id);
                if (ModelState.IsValid)
                {
                    if (img != null)
                    {
                        filename = DateTime.Now.ToString("dd-MM-yy-hh-mm-ss-") + img.FileName;
                        path = Path.Combine(Server.MapPath("~/wwwroot/upload/img/companyinfo"), filename);
                        img.SaveAs(path);
                        db.Entry(temp).Property(x => x.img).CurrentValue = filename;
                    }
                    temp.tittle = companyInfo.tittle;
                    temp.content = companyInfo.content;
                    temp.link = (companyInfo.link != null) ? companyInfo.link : "";
                    temp.hide = companyInfo.hide;
                    temp.order = companyInfo.order;
                    temp.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                    db.Entry(temp).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DbEntityValidationException e)
            {
                throw e;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(companyInfo);
        }

        // GET: admin/CompanyInfoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyInfo companyInfo = db.CompanyInfoes.Find(id);
            if (companyInfo == null)
            {
                return HttpNotFound();
            }
            return View(companyInfo);
        }

        // POST: admin/CompanyInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CompanyInfo companyInfo = db.CompanyInfoes.Find(id);
            db.CompanyInfoes.Remove(companyInfo);
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
