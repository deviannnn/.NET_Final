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
    public class BannersController : Controller
    {
        private PharmacyEntities db = new PharmacyEntities();

        // GET: admin/Banners
        public ActionResult Index()
        {
            return View(db.Banners.ToList());
        }
        public void getPage(string selectedPage = null)
        {
            var pages = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Trang chủ", Value = "HOME" },
                new SelectListItem { Text = "Tin tức", Value = "NEWS" },
                new SelectListItem { Text = "Thông tin nhà thuốc", Value = "ABOUT" }
            };

            ViewBag.Page = new SelectList(pages, "Value", "Text", selectedPage);
        }

        // GET: admin/Banners/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banner banner = db.Banners.Find(id);
            if (banner == null)
            {
                return HttpNotFound();
            }
            return View(banner);
        }

        // GET: admin/Banners/Create
        public ActionResult Create()
        {
            getPage();
            return View();
        }

        // POST: admin/Banners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,tittle,content,page,img,hide,order,datebegin")] Banner banner, HttpPostedFileBase img)
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
                        path = Path.Combine(Server.MapPath("~/wwwroot/upload/img/banner"), filename);
                        img.SaveAs(path);
                        banner.img = filename;
                    }
                    else
                    {
                        banner.img = "logo.png";
                    }
                    banner.order = 1;
                    banner.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                    db.Banners.Add(banner);
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

            return View(banner);
        }

        // GET: admin/Banners/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banner banner = db.Banners.Find(id);
            if (banner == null)
            {
                return HttpNotFound();
            }
            getPage(banner.page);
            return View(banner);
        }

        // POST: admin/Banners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,tittle,content,page,img,hide,order,datebegin")] Banner banner, HttpPostedFileBase img)
        {
            try
            {
                var path = "";
                var filename = "";
                Banner temp = db.Banners.Find(banner.id);
                if (ModelState.IsValid)
                {
                    if (img != null)
                    {
                        filename = DateTime.Now.ToString("dd-MM-yy-hh-mm-ss-") + img.FileName;
                        path = Path.Combine(Server.MapPath("~/wwwroot/upload/img/banner"), filename);
                        img.SaveAs(path);
                        db.Entry(temp).Property(x => x.img).CurrentValue = filename;
                    }
                    temp.tittle = banner.tittle;
                    temp.content = banner.content;
                    temp.page = banner.page;
                    temp.hide = banner.hide;
                    temp.order = banner.order;
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

            return View(banner);
        }

        // GET: admin/Banners/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banner banner = db.Banners.Find(id);
            if (banner == null)
            {
                return HttpNotFound();
            }
            return View(banner);
        }

        // POST: admin/Banners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Banner banner = db.Banners.Find(id);
            db.Banners.Remove(banner);
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
