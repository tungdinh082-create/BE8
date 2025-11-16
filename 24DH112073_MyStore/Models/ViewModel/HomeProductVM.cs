using _24DH112073_MyStore.Models;
using PagedList;
using System.Collections.Generic;

namespace _24DH112073_MyStore.Models.ViewModel
{
    public class HomeProductVM
    {
        // Tiêu chí search theo tên, mô tả sp hoặc loại sản phẩm
        public string SearchTerm { get; set; }

        // Các thuộc tính hỗ trợ phân trang
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 6; // Slide 1117 ghi 6 sản phẩm/trang

        // Danh sách sản phẩm nổi bật
        public List<Product> FeaturedProducts { get; set; }

        // Danh sách sản phẩm mới đã phân trang
        public IPagedList<Product> NewProducts { get; set; }
    }
}