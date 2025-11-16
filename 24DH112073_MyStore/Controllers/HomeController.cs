using _24DH112073_MyStore.Models;
using _24DH112073_MyStore.Models.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace _24DH112073_MyStore.Controllers
{
    public class HomeController : Controller
    {
        // Khởi tạo db context bên ngoài Action
        private MyStoreEntities db = new MyStoreEntities();

        // GET: Home/Index
        public ActionResult Index(string searchTerm, int? page)
        {
            var model = new HomeProductVM();
            var products = db.Products.Include(p => p.Category).AsQueryable();

            // 1. Tìm kiếm sản phẩm
            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p =>
                    p.ProductName.Contains(searchTerm) ||
                    p.ProductDecription.Contains(searchTerm) ||
                    p.Category.CategoryName.Contains(searchTerm)
                );
            }
            model.SearchTerm = searchTerm;

            // 2. Lấy top 10 sản phẩm bán chạy nhất (Featured)
            model.FeaturedProducts = products
                .OrderByDescending(p => p.OrderDetails.Count())
                .Take(10)
                .ToList();

            // 3. Lấy 20 sản phẩm ít bán nhất (New) và phân trang
            int pageNumber = page ?? model.PageNumber;
            int pageSize = model.PageSize;

            model.NewProducts = products
                .OrderBy(p => p.OrderDetails.Count()) // Sắp xếp theo ít người mua nhất
                .Take(20)
                .ToPagedList(pageNumber, pageSize);

            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        // (Bạn đã có Action Index() ở trên)

        // GET: Home/ProductDetails/5
        public ActionResult ProductDetails(int? id, int? quantity, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product pro = db.Products.Find(id);
            if (pro == null)
            {
                return HttpNotFound();
            }

            // Lấy tất cả sản phẩm cùng danh mục (trừ sản phẩm hiện tại)
            var products = db.Products.Include(p => p.Category).Include(p => p.OrderDetails)
                .Where(p => p.CategoryID == pro.CategoryID && p.ProductID != pro.ProductID)
                .AsQueryable();

            var model = new ProductDetailsVM();

            int pageNumber = page ?? model.PageNumber;
            int pageSize = model.PageSize; // 3 sản phẩm/trang

            model.product = pro;

            // Lấy 8 sản phẩm liên quan (cùng danh mục)
            model.RelatedProducts = products.Take(8).ToPagedList(1, 8);

            // Lấy 8 sản phẩm bán chạy nhất (cùng danh mục) và phân trang
            model.TopProducts = products
                .OrderByDescending(p => p.OrderDetails.Count())
                .Take(8)
                .ToPagedList(pageNumber, pageSize);

            if (quantity.HasValue)
            {
                model.quantity = quantity.Value;
            }

            return View(model);
        }

        // (Thêm cả hàm Dispose này vào cuối Controller)
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