using _24DH112073_MyStore.Models; 
using PagedList;
using System.Collections.Generic;

namespace _24DH112073_MyStore.Models.ViewModel 
{
    public class ProductDetailsVM
    {
        public Product product { get; set; }
        public int quantity { get; set; } = 1;

        // Thuộc tính phân trang
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 3;

        // Danh sách 8 sản phẩm cùng danh mục (Related)
        public IPagedList<Product> RelatedProducts { get; set; }

        // Danh sách 8 sản phẩm bán chạy nhất cùng danh mục (Top Deals)
        public IPagedList<Product> TopProducts { get; set; }
    }
}