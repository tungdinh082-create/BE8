using _24DH112073_MyStore.Models;
using _24DH112073_MyStore.Models.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace _24DH112073_MyStore.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        // Khởi tạo db context bên ngoài Action
        private MyStoreEntities db = new MyStoreEntities();

        // GET: Home/Index
        // GET: Admin/Products
        public ActionResult Index(string searchTerm, decimal? minPrice, decimal? maxPrice, string sortOrder, int? page)
        {
            var model = new ProductSearchVM();
            var products = db.Products.Include(p => p.Category).AsQueryable(); // Lấy tất cả sản phẩm

            // 1. Tìm kiếm sản phẩm dựa trên từ khóa (Tên, Mô tả, Danh mục)
            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p =>
                    p.ProductName.Contains(searchTerm) ||
                    p.ProductDecription.Contains(searchTerm) ||
                    p.Category.CategoryName.Contains(searchTerm)
                );
            }

            // 2. Tìm kiếm sản phẩm dựa trên giá tối thiểu
            if (minPrice.HasValue)
            {
                products = products.Where(p => p.ProductPrice >= minPrice.Value);
            }

            // 3. Tìm kiếm sản phẩm dựa trên giá tối đa
            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.ProductPrice <= maxPrice.Value);
            }

            // 4. Áp dụng sắp xếp dựa trên lựa chọn của người dùng
            switch (sortOrder)
            {
                case "name_asc":
                    products = products.OrderBy(p => p.ProductName);
                    break;
                case "name_desc":
                    products = products.OrderByDescending(p => p.ProductName);
                    break;
                case "price_asc":
                    products = products.OrderBy(p => p.ProductPrice);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(p => p.ProductPrice);
                    break;
                default:
                    products = products.OrderBy(p => p.ProductName); // Mặc định sắp xếp theo tên
                    break;
            }

            // 5. Đoạn code xử lý phân trang
            int pageNumber = page ?? 1; // Lấy số trang hiện tại (mặc định là 1)
            int pageSize = 2; // Số sản phẩm mỗi trang (SỬA LẠI SỐ LƯỢNG BẠN MUỐN)

            // Gán các giá trị tìm kiếm vào model để View giữ lại
            model.SearchTerm = searchTerm;
            model.MinPrice = minPrice;
            model.MaxPrice = maxPrice;
            model.SortOrder = sortOrder;

            // Đóng gói lệnh query, ToPagedList sẽ lấy danh sách đã phân trang
            model.Products = products.ToPagedList(pageNumber, pageSize);

            return View(model);
        }

        // GET: Admin/Products/Details/5
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

        // GET: Admin/Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,CategoryID,ProductName,ProductPrice,ProductImage,ProductDescription")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: Admin/Products/Edit/5
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
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,CategoryID,ProductName,ProductPrice,ProductImage,ProductDescription")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: Admin/Products/Delete/5
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

        // POST: Admin/Products/Delete/5
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
