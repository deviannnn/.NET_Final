using Pharmacy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pharmacy.Controllers
{
    public class ContactController : Controller
    {
        PharmacyEntities _db = new PharmacyEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult getOffice()
        {
            var v = from o in _db.Offices
                    where o.hide == true
                    orderby o.order ascending
                    select o;
            return PartialView(v.ToList());
        }
    }
}