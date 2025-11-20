using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace _24DH112073_MyStore.Models.ViewModel
{
    public class Cart
    {
        private List<CartItem> items = new List<CartItem>();
        public IEnumerable<CartItem> Items => items;

        // ==========================================================
        // BƯỚC 2: THÊM CÁC THUỘC TÍNH MỚI CHO GIỎ HÀNG NÂNG CAO 
        // ==========================================================

        // 1. Tự động nhóm các sản phẩm theo Tên Danh mục
        public List<IGrouping<string, CartItem>> GroupedItems =>
            items.GroupBy(i => i.Category).ToList();

        // 2. Thuộc tính cho "Sản phẩm tương tự" (phân trang)
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 6; // Hiển thị 6 sản phẩm tương tự
        public PagedList.IPagedList<Product> SimilarProducts { get; set; }

        // ==========================================================
        // CÁC HÀM CŨ (GIỮ NGUYÊN)
        // ==========================================================

        // Thêm sản phẩm vào giỏ
        public void AddItem(int productId, string productName, decimal unitPrice, string productImage, int quantity = 1, string category = "")
        {
            var existingItem = items.FirstOrDefault(i => i.ProductID == productId);
            if (existingItem == null)
            {
                items.Add(new CartItem
                {
                    ProductID = productId,
                    ProductImage = productImage,
                    ProductName = productName,
                    UnitPrice = unitPrice,
                    Quantity = quantity,
                    Category = category // <-- Quan trọng: Gán danh mục
                });
            }
            else
            {
                existingItem.Quantity += quantity;
            }
        }
        // Xóa sản phẩm khỏi giỏ
        public void RemoveAll(int productId)
        {
            items.RemoveAll(i => i.ProductID == productId);
        }
        // Tính tổng giá trị giỏ hàng
        public decimal TotalValue()
        {
            return items.Sum(i => i.TotalPrice);
        }
        // Làm trống giỏ hàng
        public void Clear()
        {
            items.Clear();
        }
        // Cập nhật số lượng
        public void UpdateQuantity(int productId, int quantity)
        {
            var item = items.FirstOrDefault(i => i.ProductID == productId);
            if (item != null)
            {
                item.Quantity = quantity;
            }
        }
    }
}