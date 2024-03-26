using Pharmacy.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pharmacy.Controllers
{
    public class CartController : Controller
    {
        private PharmacyEntities db = new PharmacyEntities();

        // GET: Cart
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
                TempData["css"] = "alert-danger";
                return RedirectToAction("Index", "Home");
            }

            List<CartItem> cartItems = Session["CartItems"] as List<CartItem>;

            if (cartItems == null)
            {
                cartItems = new List<CartItem>();
                Session["CartItems"] = cartItems;
                Session["CartQuantity"] = 0;
            }

            return View(cartItems);
        }

        [HttpPost]
        public ActionResult AddToCart(int productId, int quantity)
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
                TempData["css"] = "alert-danger";
                return RedirectToAction("Index", "Home");
            }

            List<CartItem> currentCart = Session["CartItems"] as List<CartItem>;

            if (currentCart == null)
            {
                currentCart = new List<CartItem>();
                Session["CartItems"] = currentCart;
                Session["CartQuantity"] = 0;
            }

            Save(productId, quantity, currentCart);

            TempData["Message"] = "Đã thêm sản phẩm vào giỏ hàng";
            TempData["css"] = "alert-success";
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        public ActionResult UpdateCart(Dictionary<string, string> id_quantity)
        {
            if (id_quantity.Keys.Any(key => key != "controller" && key != "action"))
            {
                Session.Remove("CartItems");
                Session.Remove("CartQuantity");
                Session["CartItems"] = new List<CartItem>();
                Session["CartQuantity"] = 0;

                foreach (var item in id_quantity)
                {
                    List<CartItem> currentCart = Session["CartItems"] as List<CartItem>;

                    int productId = int.Parse(item.Key);
                    int quantity = int.Parse(item.Value);

                    if (quantity == 0) { continue; }

                    Save(productId, quantity, currentCart);
                }

                TempData["Message"] = "Đã cập nhật sản phẩm trong giỏ hàng";
                TempData["css"] = "alert-success";
            }
            else
            {
                TempData["Message"] = "Giỏ hàng của bạn đang trống";
                TempData["css"] = "alert-danger";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public void RemoveCartItem(int productId)
        {
            List<CartItem> currentCart = Session["CartItems"] as List<CartItem>;

            if (currentCart != null)
            {
                CartItem cartItemToRemove = currentCart.FirstOrDefault(item => item.product.id == productId);
                if (cartItemToRemove != null)
                {
                    currentCart.Remove(cartItemToRemove);

                    Session["CartItems"] = currentCart;

                    int cartQuantity = currentCart.Sum(item => item.quantity);
                    Session["CartQuantity"] = cartQuantity;

                }
            }
        }

        private void Save(int productId, int quantity, List<CartItem> currentCart)
        {
            List<CartItem> cartItems = currentCart;

            Product product = db.Products.Find(productId);
            int amount = (int)((bool)product.isSale ? quantity * product.priceSale : quantity * product.price);

            CartItem existingItem = cartItems.FirstOrDefault(item => item.product.id == productId);
            if (existingItem != null)
            {
                existingItem.quantity += quantity;
                existingItem.amount += amount;
            }
            else
            {
                cartItems.Add(new CartItem { product = product, quantity = quantity, amount = amount });
            }

            Session["CartItems"] = cartItems;

            int cartQuantity = cartItems.Sum(item => item.quantity);
            Session["CartQuantity"] = cartQuantity;
        }
    }

}