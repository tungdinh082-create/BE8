using _24DH112073_MyStore.Models; 
using PagedList;
using System.Collections.Generic;

namespace _24DH112073_MyStore.Models.ViewModel
{
    public class ProductSearchVM
    {
        // Tiêu chí search theo tên, mô tả sp hoặc loại sản phẩm
        public string SearchTerm { get; set; }

        // Các tiêu chí search theo giá
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        // Thứ tự sắp xếp
        public string SortOrder { get; set; }

        // Các thuộc tính hỗ trợ phân trang
        public int PageNumber { get; set; } = 1; // Trang hiện tại
        public int PageSize { get; set; } = 10; // Số sản phẩm mỗi trang (có thể thay đổi)

        // Danh sách sản phẩm đã phân trang
        public IPagedList<Product> Products { get; set; }
    }
}