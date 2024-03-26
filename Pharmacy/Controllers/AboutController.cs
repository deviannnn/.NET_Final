using Pharmacy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pharmacy.Controllers
{
    public class AboutController : Controller
    {
        PharmacyEntities _db = new PharmacyEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult getAboutBanner()
        {
            var v = from t in _db.Banners
                    where t.page == "ABOUT" && t.hide == true
                    orderby t.order ascending, t.datebegin descending
                    select t;
            return PartialView(v.FirstOrDefault());
        }

        public ActionResult getCompanyInfo()
        {
            var v = from t in _db.CompanyInfoes
                    where t.hide == true && t.order > 0
                    orderby t.order ascending
                    select t;
            return PartialView(v.ToList());
        }

        public ActionResult getTeamInfo()
        {
            var v = from t in _db.TeamInfoes
                    where t.hide == true
                    orderby t.order ascending
                    select t;
            return PartialView(v.ToList());
        }

        public ActionResult getOffice()
        {
            var v = from t in _db.Offices
                    where t.hide == true
                    orderby t.order descending
                    select t;
            return PartialView(v.ToList());
        }
    }
}