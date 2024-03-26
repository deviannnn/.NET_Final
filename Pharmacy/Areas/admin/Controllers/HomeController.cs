using Pharmacy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pharmacy.Areas.admin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private PharmacyEntities db = new PharmacyEntities();

        // GET: admin/Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Footer()
        {
            var quickLinks = db.QuickLinks.ToList();
            var contactInfoes = db.ContactInfoes.ToList();
            var viewModel = new FooterViewModel { QuickLinks = quickLinks, ContactInfoes = contactInfoes };
            return View(viewModel);
        }
    }
}