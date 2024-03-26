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
    public class ProductsController : Controller
    {
        private PharmacyEntities db = new PharmacyEntities();

        // GET: admin/Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category);
            return View(products.ToList());
        }

        public void getCategory(int? selectedId = null)
        {
            ViewBag.Category = new SelectList(db.Categories.Where(x => x.hide == true)
                .OrderBy(x => x.order), "id", "name", selectedId);
        }

        // GET: admin/Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: admin/Products/Create
        public ActionResult Create()
        {
            getCategory();
            return View();
        }

        // POST: admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,img,price,description,quantity,purchase,isSale,priceSale,meta,hide,order,datebegin,id_category")] Product product, HttpPostedFileBase img)
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
                        path = Path.Combine(Server.MapPath("~/wwwroot/upload/img/product"), filename);
                        img.SaveAs(path);
                        product.img = filename;
                    }
                    else
                    {
                        product.img = "logo.png";
                    }
                    product.purchase = 0;
                    product.priceSale = (product.priceSale != null) ? product.priceSale : 0;
                    product.order = 1;
                    product.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                    db.Products.Add(product);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                    //return RedirectToAction("Index", "product", new { id = product.id_category });
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

            ViewBag.id_category = new SelectList(db.Categories, "id", "name", product.id_category);
            return View(product);
        }

        // GET: admin/Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            getCategory(product.id_category);
            return View(product);
        }

        // POST: admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "id,name,img,price,description,quantity,purchase,isSale,priceSale,meta,hide,order,datebegin,id_category")] Product product, HttpPostedFileBase img)
        {
            try
            {
                var path = "";
                var filename = "";
                Product temp = db.Products.Find(product.id);
                if (ModelState.IsValid)
                {
                    if (img != null)
                    {
                        filename = DateTime.Now.ToString("dd-MM-yy-hh-mm-ss-") + img.FileName;
                        path = Path.Combine(Server.MapPath("~/Content/upload/img/product"), filename);
                        img.SaveAs(path);
                        db.Entry(temp).Property(x => x.img).CurrentValue = filename;
                    }
                    temp.name = product.name;
                    temp.price = product.price;
                    temp.description = product.description;
                    temp.quantity = product.quantity;
                    temp.isSale = product.isSale;
                    temp.priceSale = (product.priceSale != null) ? product.priceSale : 0;
                    temp.meta = product.meta;
                    temp.hide = product.hide;
                    temp.order = product.order;
                    temp.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    temp.id_category = product.id_category;

                    db.Entry(temp).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                    //return RedirectToAction("Index", "product", new { id = product.id_category });
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

            return View(product);
        }

        // GET: admin/Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
