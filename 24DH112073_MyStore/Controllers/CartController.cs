using _24DH112073_MyStore.Models;
using _24DH112073_MyStore.Models.ViewModel;
using PagedList;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace _24DH112073_MyStore.Controllers
{
    public class CartController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        private CartService GetCartService()
        {
            return new CartService(Session);
        }

        // ==========================================================
        // SỬA LỖI ACTION INDEX
        // ==========================================================
        public ActionResult Index(int? page)
        {
            var cart = GetCartService().GetCart();
            var similarProducts = new List<Product>();

            if (cart.Items != null && cart.Items.Any())
            {
                // === BƯỚC SỬA LỖI: Lấy danh sách ID và Category ra TRƯỚC ===

                // 1. Lấy danh sách TÊN DANH MỤC (kiểu string) từ giỏ hàng
                var categoriesInCart = cart.Items.Select(ci => ci.Category).Distinct().ToList();

                // 2. Lấy danh sách ID SẢN PHẨM (kiểu int) từ giỏ hàng
                var productIdsInCart = cart.Items.Select(ci => ci.ProductID).ToList();

                // 3. Dùng 2 danh sách đơn giản (string, int) này trong câu truy vấn SQL
                similarProducts = db.Products.Where(p =>
                    categoriesInCart.Contains(p.Category.CategoryName) && // So sánh string với string
                    !productIdsInCart.Contains(p.ProductID) // So sánh int với int
                ).ToList();

                // === KẾT THÚC SỬA LỖI ===
            }

            // Phân trang cho sản phẩm tương tự
            int pageNumber = page ?? 1;
            int pageSize = cart.PageSize;
            cart.SimilarProducts = similarProducts.OrderBy(p => p.ProductID)
                                                 .ToPagedList(pageNumber, pageSize);

            return View(cart);
        }

        // ==========================================================
        // CÁC ACTION CŨ (GIỮ NGUYÊN)
        // ==========================================================

        // Action AddToCart (Code của bạn đã đúng)
        public ActionResult AddToCart(int id, int quantity = 1)
        {
            var product = db.Products.Find(id);
            if (product != null)
            {
                var cartService = GetCartService();
                cartService.GetCart().AddItem(
                    product.ProductID,
                    product.ProductName,
                    (decimal)product.ProductPrice,
                    product.ProductImage,
                    quantity,
                    product.Category.CategoryName
                );
            }
            return RedirectToAction("Index");
        }

        // Action RemoveFromCart
        public ActionResult RemoveFromCart(int id)
        {
            var cartService = GetCartService();
            cartService.GetCart().RemoveAll(id);
            return RedirectToAction("Index");
        }

        // Action ClearCart
        public ActionResult ClearCart()
        {
            GetCartService().ClearCart();
            return RedirectToAction("Index");
        }

        // Action UpdateQuantity
        [HttpPost]
        public ActionResult UpdateQuantity(int id, int quantity)
        {
            if (quantity > 0)
            {
                var cartService = GetCartService();
                cartService.GetCart().UpdateQuantity(id, quantity);
            }
            return RedirectToAction("Index");
        }
    }
}