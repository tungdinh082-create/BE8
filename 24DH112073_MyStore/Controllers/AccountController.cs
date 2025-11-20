using _24DH112073_MyStore.Models;
using _24DH112073_MyStore.Models.ViewModel;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace _24DH112073_MyStore.Controllers
{
    public class AccountController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // 1. REGISTER (GET)
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        // 2. REGISTER (POST) - Đã sửa lỗi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                // BƯỚC 1: Tạo đối tượng "User" (để đăng nhập)
                var newUser = new User
                {
                    Username = model.Username,
                    Password = model.Password, // Ghi chú: Nên mã hóa (hash) mật khẩu
                    UserRole = "Customer"      // Gán quyền mặc định
                };

                // BƯỚC 2: Tạo đối tượng "Customer" (để lưu thông tin)
                var newCustomer = new Customer
                {
                    CustomerName = model.CustomerName,
                    CustomerPhone = model.CustomerPhone,
                    CustomerEmail = model.CustomerEmail,
                    CustomerAddress = model.CustomerAddress,
                    Username = model.Username // Đây là khóa ngoại liên kết với bảng User
                };

                try
                {
                    // BƯỚC 3: Thêm CẢ HAI đối tượng vào DB
                    db.Users.Add(newUser);
                    db.Customers.Add(newCustomer);

                    // BƯỚC 4: Lưu thay đổi (chỉ 1 lần)
                    db.SaveChanges();

                    return RedirectToAction("Index", "Home");
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    // Lỗi validation
                    var errorMessages = ex.EntityValidationErrors.SelectMany(e => e.ValidationErrors).Select(e => $"Property: {e.PropertyName} - Lỗi: {e.ErrorMessage}");
                    ModelState.AddModelError("", "Lỗi Validation: " + string.Join("; ", errorMessages));
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                {
                    // Lỗi trùng lặp Username (Primary Key)
                    ModelState.AddModelError("", "Tên đăng nhập này đã tồn tại.");
                }
                catch (Exception ex)
                {
                    // Lỗi chung
                    ModelState.AddModelError("", "Đã xảy ra lỗi: " + ex.Message);
                }
            }

            // Nếu có lỗi, trả về View với model
            return View(model);
        }

        // 3. LOGIN (GET)
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // 4. LOGIN (POST) - Đã sửa lỗi (Đăng nhập bằng "Users")
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                // ĐĂNG NHẬP LÀ PHẢI KIỂM TRA BẢNG "Users"
                var user = db.Users.SingleOrDefault(u =>
                    u.Username.Equals(model.Username, StringComparison.OrdinalIgnoreCase) &&
                    u.Password == model.Password
                );

                if (user != null)
                {
                    // Kiểm tra vai trò
                    if (user.UserRole.Trim().Equals("Customer", StringComparison.OrdinalIgnoreCase))
                    {
                        // LƯU LẠI VÀO SESSION
                        Session["Username"] = user.Username;
                        Session["UserRole"] = user.UserRole.Trim();

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Tài khoản không có quyền đăng nhập.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                }
            }
            return View(model);
        }

        // 5. LOGOUT
        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        // 6. PROFILE INFO (GET)
        [HttpGet]
        public ActionResult ProfileInfo()
        {
            if (Session["UserRole"] == null || !Session["UserRole"].ToString().Equals("Customer", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Login");
            }

            var username = Session["Username"].ToString();
            var customer = db.Customers.SingleOrDefault(c => c.Username == username);

            if (customer == null)
            {
                Session.Clear();
                return RedirectToAction("Index", "Home");
            }

            var model = new ProfileVM
            {
                Username = customer.Username,
                CustomerName = customer.CustomerName,
                CustomerPhone = customer.CustomerPhone,
                CustomerEmail = customer.CustomerEmail,
                CustomerAddress = customer.CustomerAddress
            };

            return View(model);
        }

        // 7. EDIT PROFILE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(ProfileVM model)
        {
            if (Session["UserRole"] == null || !Session["UserRole"].ToString().Equals("Customer", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Login");
            }

            if (ModelState.IsValid)
            {
                string username = Session["Username"].ToString();
                var customer = db.Customers.SingleOrDefault(c => c.Username == username);

                if (customer == null)
                {
                    return RedirectToAction("Login");
                }

                customer.CustomerName = model.CustomerName;
                customer.CustomerPhone = model.CustomerPhone;
                customer.CustomerEmail = model.CustomerEmail;
                customer.CustomerAddress = model.CustomerAddress;

                db.SaveChanges();
                ViewBag.SuccessMessage = "Cập nhật thông tin thành công!";
            }

            return View("ProfileInfo", model);
        }

    } // KẾT THÚC CLASS
}