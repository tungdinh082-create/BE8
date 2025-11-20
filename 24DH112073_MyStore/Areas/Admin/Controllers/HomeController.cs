using _24DH112073_MyStore.Models;
using _24DH112073_MyStore.Models.ViewModel;
using System.Linq;
using System.Web.Mvc;

namespace _24DH112073_MyStore.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        public ActionResult Index()
        {
            // Sửa truy vấn để trả về một List<StatisticsViewModel>
            var stats = db.Products
                .GroupBy(p => p.Category.CategoryName)
                .Select(g => new StatisticsViewModel // <-- SỬA Ở ĐÂY
                {
                    LoaiHang = g.Key,
                    SoLuong = g.Count(),
                    GiaCaoNhat = g.Max(p => p.ProductPrice),
                    GiaThapNhat = g.Min(p => p.ProductPrice),
                    GiaTrungBinh = g.Average(p => p.ProductPrice)
                })
                .ToList();

            // Gửi cả list "stats" sang View, không dùng ViewBag
            return View(stats); // <-- SỬA Ở ĐÂY
        }
    }
}