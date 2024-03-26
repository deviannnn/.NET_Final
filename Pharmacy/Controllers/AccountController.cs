using Pharmacy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Security.Cryptography;
using System.Text;

using BCryptNet = BCrypt.Net.BCrypt;

namespace Pharmacy.Controllers
{
    public class AccountController : Controller
    {
        private PharmacyEntities db = new PharmacyEntities();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            Account customer = IsValidCustomer(email, password);
            if (customer != null)
            {
                Session["CustomerId"] = customer.id;
                Session["CustomerPermit"] = customer.permission;

                return RedirectToAction("Index", "Home");
            }
            else if (IsValidAdmin(email, password))
            {
                FormsAuthentication.SetAuthCookie(email, false);

                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            else
            {
                ModelState.AddModelError("", "Thông tin đăng nhập không hợp lệ! Vui lòng thử lại.");
                return View();
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(string email, string password, string confirmPassword)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                ModelState.AddModelError("", "Thông tin đăng ký không hợp lệ! Vui lòng thử lại.");
                return View();
            }
            else if (IsExistedEmail(email))
            {
                ModelState.AddModelError("", "Email đã tồn tại! Vui lòng sử dụng một Email khác.");
                return View();
            }
            else if (password.Length < 6)
            {
                ModelState.AddModelError("", "Mật khẩu quá ngắn! Vui lòng sử dụng mật khẩu dài hơn 6 kí tự.");
                return View();
            }
            else if (!password.Equals(confirmPassword))
            {
                ModelState.AddModelError("", "Mật khẩu nhập lại không trùng khớp! Vui lòng nhập lại.");
                return View();
            }
            else
            {
                Account account = new Account();
                account.email = email;
                account.password = password;
                account.role = 0;
                account.permission = 1;

                string token = Guid.NewGuid().ToString();
                Session["NewAccount"] = account;
                Session["ValidatedToken"] = token;

                string subject = "Xác thực tài khoản";
                string url = Url.Action("VerifyAccount", "Account", new { token = token }, Request.Url.Scheme);
                string body = $"<p><i>Pharmacy xin chào,</i></p> <p><i>Vui lòng nhấp vào liên kết dưới đây để xác thực tài khoản:</i></p> <p><i>{url}</i></p>";


                SendEmail(email, subject, body);

                TempData["Message"] = "Chúng tôi đã gửi mã xác nhận đến email của bạn. Vui lòng kiểm tra hộp thư để tiếp tục đăng ký tài khoản";
                TempData["css"] = "text-success";
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult VerifyAccount(string token)
        {
            Account account = (Account)Session["NewAccount"];
            string validatedToken = (string)Session["ValidatedToken"];

            if (account != null && token == validatedToken)
            {
                string hashedPassword = BCryptNet.HashPassword(account.password);

                account.password = hashedPassword;
                db.Accounts.Add(account);
                db.SaveChanges();

                Session.Remove("NewAccount");
                Session.Remove("ValidatedToken");

                ViewBag.Message = "Xác thực thành công!";
                ViewBag.Success = true;
                return View();
            }
            else
            {
                ViewBag.Message = "Xác thực không thành công!";
                ViewBag.Success = false;
                return View();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        private Account IsValidCustomer(string email, string password)
        {
            var customer = db.Accounts.FirstOrDefault(a => a.email == email && a.role == 0);

            if (customer != null && BCryptNet.Verify(password, customer.password))
            {
                return customer;
            }

            return null;
        }

        private bool IsValidAdmin(string email, string password)
        {
            var admin = db.Accounts.FirstOrDefault(a => a.email == email && a.role == 1);

            return (admin != null && BCryptNet.Verify(password, admin.password));
        }

        private bool IsExistedEmail(string email)
        {
            var account = db.Accounts.FirstOrDefault(a => a.email == email);

            return (account != null) ? true : false;
        }

        private void SendEmail(string toEmail, string subject, string body)
        {
            var fromEmail = "lytuanan1911@gmail.com";
            var fromPassword = "fgiqbwmwpbtcefed";
            var fromDisplayName = "Pharmacy";

            var smtpClient = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail, fromPassword)
            };

            var fromAddress = new MailAddress(fromEmail, fromDisplayName);
            var toAddress = new MailAddress(toEmail);

            var mailMessage = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            smtpClient.Send(mailMessage);
        }
    }
}