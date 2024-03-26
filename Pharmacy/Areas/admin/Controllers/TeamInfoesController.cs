using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Pharmacy.Models;

namespace Pharmacy.Areas.admin.Controllers
{
    [Authorize]
    public class TeamInfoesController : Controller
    {
        private PharmacyEntities db = new PharmacyEntities();

        // GET: admin/TeamInfoes
        public ActionResult Index()
        {
            return View(db.TeamInfoes.ToList());
        }

        // GET: admin/TeamInfoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeamInfo teamInfo = db.TeamInfoes.Find(id);
            if (teamInfo == null)
            {
                return HttpNotFound();
            }
            return View(teamInfo);
        }

        // GET: admin/TeamInfoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: admin/TeamInfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,role,description,img,hide,order,datebegin,link")] TeamInfo teamInfo, HttpPostedFileBase img)
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
                        path = Path.Combine(Server.MapPath("~/wwwroot/upload/img/teaminfo"), filename);
                        img.SaveAs(path);
                        teamInfo.img = filename;
                    }
                    else
                    {
                        teamInfo.img = "logo.png";
                    }
                    teamInfo.link = (teamInfo.link != null) ? teamInfo.link : "";
                    teamInfo.order = 1;
                    teamInfo.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                    db.TeamInfoes.Add(teamInfo);
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

            return View(teamInfo);
        }

        // GET: admin/TeamInfoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeamInfo teamInfo = db.TeamInfoes.Find(id);
            if (teamInfo == null)
            {
                return HttpNotFound();
            }
            return View(teamInfo);
        }

        // POST: admin/TeamInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,role,description,img,hide,order,datebegin,link")] TeamInfo teamInfo, HttpPostedFileBase img)
        {
            try
            {
                var path = "";
                var filename = "";
                TeamInfo temp = db.TeamInfoes.Find(teamInfo.id);
                if (ModelState.IsValid)
                {
                    if (img != null)
                    {
                        filename = DateTime.Now.ToString("dd-MM-yy-hh-mm-ss-") + img.FileName;
                        path = Path.Combine(Server.MapPath("~/wwwroot/upload/img/teaminfo"), filename);
                        img.SaveAs(path);
                        db.Entry(temp).Property(x => x.img).CurrentValue = filename;
                    }
                    temp.name = teamInfo.name;
                    temp.role = teamInfo.role;
                    temp.description = teamInfo.description;
                    temp.link = (teamInfo.link != null) ? teamInfo.link : "";
                    temp.hide = teamInfo.hide;
                    temp.order = teamInfo.order;
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

            return View(teamInfo);
        }

        // GET: admin/TeamInfoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeamInfo teamInfo = db.TeamInfoes.Find(id);
            if (teamInfo == null)
            {
                return HttpNotFound();
            }
            return View(teamInfo);
        }

        // POST: admin/TeamInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TeamInfo teamInfo = db.TeamInfoes.Find(id);
            db.TeamInfoes.Remove(teamInfo);
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
