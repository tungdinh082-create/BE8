using _24DH112073_MyStore.Models;
using _24DH112073_MyStore.Models.ViewModel; 
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
        [Authorize] // Bắt buộc đăng nhập
        public ActionResult Checkout()
        {
            // Kiểm tra giỏ hàng
            var cart = Session["Cart"] as Cart;
            if (cart == null || !cart.Items.Any())
            {
                return RedirectToAction("Index", "Home"); // Giỏ rỗng, về trang chủ
            }

            // Xác thực người dùng
            var user = db.Users.SingleOrDefault(u => u.Username == User.Identity.Name);
            if (user == null)
            {
                return RedirectToAction("Login", "Account"); // Chưa đăng nhập
            }

            // Lấy thông tin khách hàng
            var customer = db.Customers.SingleOrDefault(c => c.Username == user.Username);

            // Tạo CheckoutVM
            var model = new CheckoutVM
            {
                CartItems = cart.Items.ToList(),
                TotalAmount = cart.TotalValue(),
                OrderDate = System.DateTime.Now,
                ShippingAddress = customer.CustomerAddress, // Lấy địa chỉ mặc định (ĐÃ SỬA TÊN CỘT)
                CustomerID = customer.CustomerID,
                Username = customer.Username
            };

            return View(model);
        }

        // POST: Order/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Checkout(CheckoutVM model)
        {
            var cart = Session["Cart"] as Cart;
            if (cart == null || !cart.Items.Any())
            {
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                model.CartItems = cart.Items.ToList(); // Gán lại giỏ hàng nếu model không hợp lệ
                return View(model);
            }

            // Xác thực người dùng
            var user = db.Users.SingleOrDefault(u => u.Username == User.Identity.Name);
            var customer = db.Customers.SingleOrDefault(c => c.Username == user.Username);

            // Thiết lập trạng thái thanh toán
            string paymentStatus = "Chưa thanh toán";
            switch (model.PaymentMethod)
            {
                case "Tiền mặt":
                    paymentStatus = "Thanh toán tiền mặt"; break;
                case "Paypal":
                    paymentStatus = "Thanh toán Paypal"; break;
                //...
                default:
                    paymentStatus = "Chưa thanh toán"; break;
            }

            // Xử lý nếu chọn Paypal (chuyển hướng)
            if (model.PaymentMethod == "Paypal")
            {
                // Chuyển hướng tới PaypalController
                return RedirectToAction("PaymentWithPaypal", "Paypal", model);
            }

            // Tạo đơn hàng (Order)
            var order = new Order
            {
                CustomerID = customer.CustomerID,
                OrderDate = System.DateTime.Now,
                TotalAmount = cart.TotalValue(),
                PaymentStatus = paymentStatus,
                PaymentMethod = model.PaymentMethod,
                ShippingMethod = model.ShippingMethod,
                ShippingAddress = model.ShippingAddress, // Tên cột đã sửa
                OrderDetails = new List<OrderDetail>()
            };

            // Tạo chi tiết đơn hàng (OrderDetail)
            foreach (var item in cart.Items)
            {
                order.OrderDetails.Add(new OrderDetail
                {
                    ProductID = item.ProductID,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                    // TotalPrice sẽ được DB tự tính
                });
            }

            // Lưu đơn hàng vào CSDL
            db.Orders.Add(order);
            db.SaveChanges();

            // Xóa giỏ hàng
            Session["Cart"] = null;

            // Chuyển tới trang Xác nhận đơn hàng
            return RedirectToAction("OrderSuccess", new { id = order.OrderID });
        }

        // GET: Order/OrderSuccess
        public ActionResult OrderSuccess(int id)
        {
            var order = db.Orders.Include(o => o.OrderDetails.Select(od => od.Product))
                                 .SingleOrDefault(o => o.OrderID == id);

            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }
    }
}