using _24DH112073_MyStore.Models;
using _24DH112073_MyStore.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace _24DH112073_MyStore.Controllers
{
    public class OrderController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // GET: Order/Checkout
        [HttpGet]
        public ActionResult Checkout()
        {
            var cart = Session["Cart"] as Cart;
            if (cart == null || !cart.Items.Any())
            {
                return RedirectToAction("Index", "Home");
            }

            if (Session["UserRole"] == null || !Session["UserRole"].ToString().Equals("Customer", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Login", "Account");
            }

            var username = Session["Username"].ToString();
            var customer = db.Customers.SingleOrDefault(c => c.Username == username);
            if (customer == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new CheckoutVM
            {
                CartItems = cart.Items.ToList(),
                TotalAmount = cart.TotalValue(),
                CustomerID = customer.CustomerID,
                Username = customer.Username,
                ShippingAddress = customer.CustomerAddress
            };

            return View(model);
        }

        // POST: Order/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout(CheckoutVM model)
        {
            var cart = Session["Cart"] as Cart;

            // Kiểm tra session đăng nhập
            if (Session["Username"] == null) return RedirectToAction("Login", "Account");

            var username = Session["Username"].ToString();
            var customer = db.Customers.SingleOrDefault(c => c.Username == username);

            if (cart == null || !cart.Items.Any() || customer == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                model.CartItems = cart.Items.ToList();
                model.TotalAmount = cart.TotalValue();
                return View(model);
            }

            // --- [MỚI] TÍNH PHÍ VẬN CHUYỂN ---
            decimal shippingFee = 0;
            if (model.ShippingMethod == "Giao hàng nhanh")
            {
                shippingFee = 20000;
            }
            else if (model.ShippingMethod == "Giao hàng tiết kiệm")
            {
                shippingFee = 15000;
            }
            // ----------------------------------

            var order = new Order
            {
                CustomerID = customer.CustomerID,
                OrderDate = DateTime.Now,

                // [QUAN TRỌNG] Cộng tiền hàng + Tiền ship vào Tổng đơn hàng
                TotalAmount = cart.TotalValue() + shippingFee,

                ShippingAddress = model.ShippingAddress,
                PaymentMethod = model.PaymentMethod,
                DeliveryMethod = model.ShippingMethod,
                PaymentStatus = "Chưa thanh toán"
            };

            order.OrderDetails = cart.Items.Select(item => new OrderDetail
            {
                ProductID = item.ProductID,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            }).ToList();

            db.Orders.Add(order);
            db.SaveChanges();

            Session["Cart"] = null;

            return RedirectToAction("OrderSuccess", new { id = order.OrderID });
        }

        // GET: Order/OrderSuccess
        public ActionResult OrderSuccess(int id)
        {
            var order = db.Orders
                          .Include(o => o.OrderDetails.Select(od => od.Product))
                          .SingleOrDefault(o => o.OrderID == id);

            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
        }

        // GET: Order/OrderHistory
        public ActionResult OrderHistory()
        {
            if (Session["UserRole"] == null || !Session["UserRole"].ToString().Equals("Customer", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Login", "Account");
            }

            var username = Session["Username"].ToString();
            var customer = db.Customers.SingleOrDefault(c => c.Username == username);

            if (customer == null) return RedirectToAction("Login", "Account");

            var orders = db.Orders
                           .Where(o => o.CustomerID == customer.CustomerID)
                           .OrderByDescending(o => o.OrderDate)
                           .Include(o => o.OrderDetails.Select(od => od.Product))
                           .ToList();

            return View(orders);
        }
    }
}