using _24DH112073_MyStore.Models;
using _24DH112073_MyStore.Models.ViewModel;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

// Namespace của bạn
namespace _24DH112073_MyStore.Areas.Admin.Controllers
{
    // BẮT ĐẦU CLASS
    public class AccountController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // GET: Admin/Account/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Admin/Account/Login (CODE MỚI ĐÃ SỬA)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVM model)
        {
            // TẤT CẢ CODE NÀY PHẢI NẰM BÊN TRONG CLASS

            if (ModelState.IsValid)
            {
                // 1. Kiểm tra User, không phân biệt hoa/thường
                var user = db.Users.SingleOrDefault(u =>
                    u.Username.Equals(model.Username, StringComparison.OrdinalIgnoreCase) &&
                    u.Password == model.Password &&
                    u.UserRole.Equals("Admin", StringComparison.OrdinalIgnoreCase)
                );

                if (user != null)
                {
                    // 2. TẠO VÉ (TICKET)
                    var authTicket = new FormsAuthenticationTicket(
                        1,
                        user.Username,
                        DateTime.Now,
                        DateTime.Now.AddMinutes(60),
                        false,
                        user.UserRole.Trim() // LƯU VAI TRÒ ĐÃ ĐƯỢC LÀM SẠCH
                    );

                    // 3. Mã hóa vé và tạo cookie
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    Response.Cookies.Add(authCookie);

                    Session.Clear(); // Xóa session (nếu có)

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập, mật khẩu không đúng hoặc bạn không có quyền Admin.");
                }
            }
            return View(model);
        }

        // GET: Admin/Account/Logout
        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

    } // <-- KẾT THÚC CLASS
}