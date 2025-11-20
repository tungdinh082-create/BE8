using _24DH112073_MyStore.Models;
using _24DH112073_MyStore.Models.ViewModel;
using PagedList;
using System.Linq;
using System.Net; // <-- Thư viện cần thiết
using System.Web.Mvc;

namespace _24DH112073_MyStore.Controllers
{
    public class HomeController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // Action Trang chủ (ĐÃ SỬA LỖI GÕ SAI)
        // 1. SỬA Ở ĐÂY: "searchTearm" -> "searchTerm"
        public ActionResult Index(string searchTerm, int? page)
        {
            var model = new HomeProductVM();
            var products = db.Products.AsQueryable();

            // 2. SỬA Ở ĐÂY: "searchTearm" -> "searchTerm"
            if (!string.IsNullOrEmpty(searchTerm))
            {
                // 3. SỬA Ở ĐÂY: "searchTearm" -> "searchTerm"
                model.SearchTerm = searchTerm;
                products = products.Where(p =>
                    // 4. SỬA Ở ĐÂY: "searchTearm" -> "searchTerm"
                    p.ProductName.Contains(searchTerm) ||
                    // 5. SỬA Ở ĐÂY: "searchTearm" -> "searchTerm"
                    p.ProductDecription.Contains(searchTerm) ||
                    // 6. SỬA Ở ĐÂY: "searchTearm" -> "searchTerm"
                    p.Category.CategoryName.Contains(searchTerm)
                );
            }

            int pageNumber = page ?? 1;
            int pageSize = 6;

            model.FeaturedProducts = products.OrderByDescending(p => p.OrderDetails.Count())
                                               .Take(10).ToList();

            model.NewProducts = products.OrderBy(p => p.OrderDetails.Count())
                                           .ToPagedList(pageNumber, pageSize);

            return View(model);
        }

        // Action Menu Danh mục (Code của bạn đã đúng)
        public ActionResult CategoryMenu()
        {
            var categories = db.Categories.ToList();
            return PartialView("_CategoryMenuPV", categories);
        }


        // Action ProductDetail (Code bạn gửi đã được sửa)
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

            var products = db.Products.Where(p => p.CategoryID == pro.CategoryID && p.ProductID != pro.ProductID).AsQueryable();
            ProductDetailsVM model = new ProductDetailsVM();

            int pageNumber = page ?? 1;
            int pageSize = model.PageSize;

            model.Product = pro;

            // Chú ý: .ToPagedList(1, 8) ở RelatedProducts có thể gây lỗi nếu products < 8
            // Tạm thời để theo code của bạn
            model.RelatedProducts = products.OrderBy(p => p.ProductID)
                                            .Take(8).ToPagedList(1, 8);

            model.TopProducts = products.OrderByDescending(p => p.OrderDetails.Count())
                                        .Take(8).ToPagedList(pageNumber, pageSize);

            if (quantity.HasValue)
            {
                model.Quantity = quantity.Value;
            }

            // Ghi rõ tên View để trả về (tên file là "ProductDetails.cshtml")
            // Đảm bảo bạn có file View tên là "ProductDetails.cshtml"
            return View("ProductDetails", model);
        }


        // Các Action mặc định (Giữ lại)
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
    }
}