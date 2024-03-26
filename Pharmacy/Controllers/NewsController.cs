using Pharmacy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pharmacy.Controllers
{
    public class NewsController : Controller
    {
        PharmacyEntities _db = new PharmacyEntities();
        public ActionResult Index()
        {
            ViewBag.meta = "tin-tuc";
            var v = from t in _db.News
                    where t.hide == true
                    orderby t.order ascending, t.datebegin descending
                    select t;
            return View(v.ToList());
        }

        public ActionResult getNewsBanner()
        {
            var v = from t in _db.Banners
                    where t.page == "NEWS" && t.hide == true
                    orderby t.order ascending, t.datebegin descending
                    select t;
            return PartialView(v.FirstOrDefault());
        }

        public ActionResult Detail(String meta)
        {
            ViewBag.meta = "tin-tuc";
            ViewBag.HotNews = (from t in _db.News
                                where t.hide == true
                                orderby t.order ascending, t.datebegin descending
                                select t).Take(3).ToList();

            var v = from t in _db.News
                    where t.meta == meta
                    select t;
            return View(v.FirstOrDefault());
        }
    }
}