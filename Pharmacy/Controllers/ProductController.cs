using Pharmacy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pharmacy.Controllers
{
    public class ProductController : Controller
    {
        PharmacyEntities _db = new PharmacyEntities();
        public ActionResult Index()
        {
            ViewBag.meta = "san-pham";
            ViewBag.Categories = (from cate in _db.Categories
                                  where cate.hide == true
                                  orderby cate.order ascending
                                  select cate).ToList();

            var v = from t in _db.Products
                    where t.hide == true
                    orderby t.order ascending
                    select t;
            return View(v.ToList());
        }

        public ActionResult getByMeta(string meta)
        {
            ViewBag.meta = "san-pham";
            ViewBag.Categories = (from cate in _db.Categories
                                  where cate.hide == true
                                  orderby cate.order ascending
                                  select cate).ToList();

            var v = from t in _db.Categories
                    where t.meta == meta
                    select t;
            return View(v.FirstOrDefault());
        }

        public ActionResult Detail(int id)
        {
            var v = from t in _db.Products
                    where t.id == id
                    select t;
            return View(v.FirstOrDefault());
        }
    }
}