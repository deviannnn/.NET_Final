using Pharmacy.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Pharmacy.Controllers
{
    public class OrdersController : Controller
    {
        private PharmacyEntities db = new PharmacyEntities();

        // GET: Orders
        public ActionResult Index()
        {
            if (Session["CustomerId"] == null)
            {
                TempData["Message"] = "Vui lòng đăng nhập để tiếp tục tạo giỏ hàng";
                TempData["css"] = "text-danger";
                return RedirectToAction("Login", "Account");
            }
            if ((int)Session["CustomerPermit"] == 0)
            {
                TempData["Message"] = "Xin lỗi! Hiện tài khoản của bạn đã bị hạn chế chức năng này";
                TempData["css"] = "text-danger";
                return Redirect(Request.UrlReferrer.ToString());
            }
            if (Session["CartItems"] == null || !(Session["CartItems"] as List<CartItem>).Any())
            {
                TempData["Message"] = "Giỏ hàng của bạn đang trống";
                TempData["css"] = "alert-danger";

                return RedirectToAction("Index", "Cart");
            }
            List<CartItem> cartItems = Session["CartItems"] as List<CartItem>;
            return View(cartItems);
        }

        public ActionResult OrderHistory()
        {
            if (Session["CustomerId"] == null)
            {
                TempData["Message"] = "Vui lòng đăng nhập để tiếp tục xem lịch sử đặt hàng";
                TempData["css"] = "text-danger";
                return RedirectToAction("Login", "Account");
            }
            int accountId = (int)Session["CustomerId"];

            var orders = db.Orders.Where(o => o.id_account == accountId).ToList();

            return View(orders);
        }


        [HttpPost]
        public ActionResult Create([Bind(Include = "id,name,phone,address,ward,district,city,datecreate,total_amount,status,id_account")] Order order)
        {
            List<CartItem> currentCart = Session["CartItems"] as List<CartItem>;
            int totalAmount = currentCart.Sum(item => item.amount);

            if (validateInput(order))
            {
                order.datecreate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                order.total_amount = totalAmount;
                order.status = 0;
                order.id_account = (int)Session["CustomerId"];

                db.Orders.Add(order);
                db.SaveChanges();

                foreach (var cartItem in currentCart)
                {
                    var orderDetail = new OrdersDetail
                    {
                        quantity = cartItem.quantity,
                        price = (int)((bool)cartItem.product.isSale ? cartItem.product.priceSale : cartItem.product.price),
                        amount = cartItem.amount,
                        id_orders = order.id,
                        id_product = cartItem.product.id
                    };
                    db.OrdersDetails.Add(orderDetail);
                }
                db.SaveChanges();

                Session["CartItems"] = new List<CartItem>();
                Session["CartQuantity"] = 0;
                return RedirectToAction("OrderHistory");
            }
            else
            {
                TempData["OrderFail"] = "Vui lòng không được bỏ trống bất kì thông tin giao hàng!";
                return RedirectToAction("Index");
            }
        }

        public bool validateInput(Order order)
        {
            if (string.IsNullOrEmpty(order.phone) || string.IsNullOrEmpty(order.address) || string.IsNullOrEmpty(order.ward) || string.IsNullOrEmpty(order.district) || string.IsNullOrEmpty(order.city))
            {
                return false;
            }
            else
            { return true; }
        }
    }
}