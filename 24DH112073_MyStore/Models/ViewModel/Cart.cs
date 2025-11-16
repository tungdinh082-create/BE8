using PagedList;
using System.Collections.Generic;
using System.Linq;

namespace _24DH112073_MyStore.Models.ViewModel // <-- ĐÃ SỬA
{
    public class Cart
    {
        private List<CartItem> items = new List<CartItem>();
        public IEnumerable<CartItem> Items => items;

        // Thêm sản phẩm vào giỏ
        public void AddItem(int productId, string productName, decimal unitPrice, string productImage, string category, int quantity)
        {
            var existingItem = items.FirstOrDefault(i => i.ProductID == productId);
            if (existingItem == null)
            {
                items.Add(new CartItem
                {
                    ProductID = productId,
                    ProductName = productName,
                    UnitPrice = unitPrice,
                    ProductImage = productImage,
                    Category = category,
                    Quantity = quantity
                });
            }
            else
            {
                existingItem.Quantity += quantity;
            }
        }

        // Xóa sản phẩm khỏi giỏ
        public void RemoveItem(int productId)
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

        // === Phần cho giỏ hàng nâng cao ===
        // Nhóm các sản phẩm theo Category
        public List<IGrouping<string, CartItem>> GroupedItems => items.GroupBy(i => i.Category).ToList();

        // Thuộc tính phân trang
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 6;

        // Danh sách sản phẩm tương tự
        public IPagedList<Product> SimilarProducts { get; set; }
    }
}