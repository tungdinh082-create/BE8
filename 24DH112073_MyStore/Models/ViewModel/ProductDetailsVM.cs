
using _24DH112073_MyStore.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace _24DH112073_MyStore.Models.ViewModel
{
    public class ProductDetailsVM
    {
        // Sửa thành chữ hoa để khớp với View
        public Product Product { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal EstimatedValue { get; set; } // Giá trị tạm tính

        // Các thuộc tính phân trang
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 3;

        public IPagedList<Product> RelatedProducts { get; set; }
        public IPagedList<Product> TopProducts { get; set; }
    }
}
