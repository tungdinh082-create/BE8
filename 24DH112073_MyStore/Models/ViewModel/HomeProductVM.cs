
using _24DH112073_MyStore.Models;
using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace _24DH112073_MyStore.Models.ViewModel
{
    public class HomeProductVM
    {
        public string SearchTerm { get; set; }

        // Các thuộc tính hỗ trợ phân trang
        public int PageNumber { get; set; } // Trang hiện tại
        public int PageSize { get; set; } = 10; // Số sản phẩm mỗi trang (slide ghi 6, 10 đều được)

        // danh sách sản phẩm nổi bật
        public List<Product> FeaturedProducts { get; set; }

        // Danh sách sản phẩm mới đã phân trang
        public PagedList.IPagedList<Product> NewProducts { get; set; }
    }
}
