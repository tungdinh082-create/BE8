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

        // Hàm lấy dịch vụ giỏ hàng
        private CartService GetCartService()
        {
            return new CartService(Session);
        }

        // GET: Cart/Index (Hiển thị giỏ hàng cơ bản)
        public ActionResult Index()
        {
            var cart = GetCartService().GetCart();
            return View(cart); // Dùng View Index.cshtml cơ bản
        }

        // GET: Cart/Index2 (Hiển thị giỏ hàng nâng cao)
        public ActionResult Index2(int? page)
        {
            var cart = GetCartService().GetCart();
            var products = db.Products.ToList(); // Lấy tất cả sản phẩm
            var similarProducts = new List<Product>();

            if (cart.Items != null && cart.Items.Any())
            {
                // Tìm sản phẩm tương tự
                similarProducts = products.Where(p =>
                    cart.Items.Any(ci => ci.Category == p.Category.CategoryName) &&
                    !cart.Items.Any(ci => ci.ProductID == p.ProductID)
                ).ToList();
            }

            // Phân trang
            int pageNumber = page ?? 1;
            int pageSize = cart.PageSize;
            cart.SimilarProducts = similarProducts.OrderBy(p => p.ProductID).ToPagedList(pageNumber, pageSize);

            return View(cart); // Dùng View Index2.cshtml nâng cao
        }


        // POST: Cart/AddToCart
        public ActionResult AddToCart(int id, int quantity = 1)
        {
            var product = db.Products.Find(id);
            if (product != null)
            {
                var cartService = GetCartService();
                cartService.GetCart().AddItem(
                    product.ProductID,
                    product.ProductName,
                    product.ProductPrice,
                    product.ProductImage,
                    product.Category.CategoryName, // Truyền tên Category
                    quantity
                );
            }
            return RedirectToAction("Index"); // Về trang giỏ hàng
        }

        // POST: Cart/RemoveFromCart
        public ActionResult RemoveFromCart(int id)
        {
            var cartService = GetCartService();
            cartService.GetCart().RemoveItem(id);
            return RedirectToAction("Index");
        }

        // POST: Cart/ClearCart
        public ActionResult ClearCart()
        {
            GetCartService().ClearCart();
            return RedirectToAction("Index");
        }

        // POST: Cart/UpdateQuantity
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