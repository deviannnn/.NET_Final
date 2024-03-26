using Pharmacy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pharmacy.Controllers
{
    public class HomeController : Controller
    {
        PharmacyEntities _db = new PharmacyEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult getMenu()
        {
            var v = from t in _db.Menus
                    where t.hide == true
                    orderby t.order ascending
                    select t;
            return PartialView(v.ToList());
        }

        public ActionResult getFooterQuickLink()
        {
            var v = from t in _db.QuickLinks
                    where t.hide == true
                    orderby t.order ascending
                    select t;
            return PartialView(v.ToList());
        }

        public ActionResult getFooterContactInfo()
        {
            var v = from t in _db.ContactInfoes
                    where t.hide == true
                    orderby t.order ascending
                    select t;
            return PartialView(v.ToList());
        }

        public ActionResult getHomeBanner()
        {
            var v = from t in _db.Banners
                    where t.page == "HOME" && t.hide == true
                    orderby t.order ascending, t.datebegin descending
                    select t;
            return PartialView(v.FirstOrDefault());
        }

        public ActionResult getHotNews()
        {
            ViewBag.meta = "tin-tuc";
            var v = (from t in _db.News
                     where t.hide == true
                     orderby t.order ascending, t.datebegin descending 
                     select t).Take(3);
            return PartialView(v.ToList());
        }

        public ActionResult getPopularProduct()
        {
            ViewBag.meta = "san-pham";
            var v = (from t in _db.Products
                     where t.hide == true
                     orderby t.purchase descending
                     select t).Take(6);
            return PartialView("ProductList", v.ToList());
        }

        public ActionResult getNewProduct()
        {
            ViewBag.meta = "san-pham";
            var v = (from t in _db.Products
                     where t.hide == true
                     orderby t.datebegin descending
                     select t).Take(6);
            return PartialView("ProductList", v.ToList());
        }

        public ActionResult getCategory()
        {
            ViewBag.meta = "san-pham";
            var v = from t in _db.Categories
                    where t.hide == true
                    orderby t.order ascending
                    select t;
            return PartialView(v.ToList());
        }

        public ActionResult getProduct(int id, String metatittle)
        {
            ViewBag.meta = metatittle;
            var v = from t in _db.Products
                    where t.id_category == id && t.hide == true
                    orderby t.order ascending
                    select t;
            return PartialView("ProductList", v.ToList());
        }   
    }
}