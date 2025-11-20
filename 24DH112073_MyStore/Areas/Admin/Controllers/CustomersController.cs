using _24DH112073_MyStore.Models;
using _24DH112073_MyStore.Models.ViewModel;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace _24DH112073_MyStore.Areas.Admin.Controllers
{

    public class CustomersController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // GET: Admin/Customers
        public ActionResult Index()
        {
            var customers = db.Customers.Include(c => c.User);
            return View(customers.ToList());
        }

        // GET: Admin/Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Admin/Customers/Create
        public ActionResult Create()
        {
            var viewModel = new CustomerViewModel();
            return View(viewModel);
        }

        // POST: Admin/Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomerViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // 1. Tạo đối tượng User
                var user = new User();
                user.Username = viewModel.Username;
                user.Password = viewModel.Password; // (Nhớ mã hóa mật khẩu)
                user.UserRole = viewModel.UserRole;
                db.Users.Add(user);

                // 2. Tạo đối tượng Customer
                var customer = new Customer();
                customer.CustomerName = viewModel.CustomerName;
                customer.CustomerEmail = viewModel.CustomerEmail;
                customer.Username = viewModel.Username; // Liên kết 2 bảng

                // 3. Xử lý upload ảnh
                if (viewModel.UploadImg != null)
                {
                    string filename = Path.GetFileName(viewModel.UploadImg.FileName);
                    string savePath = Server.MapPath("~/Content/images/");
                    viewModel.UploadImg.SaveAs(Path.Combine(savePath, filename));
                    customer.CustomerImage = "~/Content/images/" + filename;
                }

                db.Customers.Add(customer);

                // BƯỚC 4: SỬA LẠI TRY-CATCH CHO ĐÚNG
                try
                {
                    // DI CHUYỂN SaveChanges() VÀO TRONG TRY
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    // Đoạn code này sẽ đào sâu vào lỗi và hiển thị chi tiết
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.GetType().Name,
                                validationError.ErrorMessage);
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise; // Ném ra lỗi chi tiết để bạn đọc
                }
            }

            return View(viewModel); // Nếu lỗi, trả về form
        }

        // GET: Admin/Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.Username = new SelectList(db.Users, "Username", "Password", customer.Username);
            return View(customer);
        }

        // POST: Admin/Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerID,CustomerName,CustomerPhone,CustomerEmail,CustomerAddress,Username")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Username = new SelectList(db.Users, "Username", "Password", customer.Username);
            return View(customer);
        }

        // GET: Admin/Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Admin/Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
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