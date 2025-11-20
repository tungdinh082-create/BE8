
using _24DH112073_MyStore.Models; // <-- 🔥 THÊM DÒNG NÀY (BẮT BUỘC cho Product)
using PagedList; // <-- 🔥 THÊM DÒNG NÀY (BẮT BUỘC cho IPagedList)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _24DH112073_MyStore.Models.ViewModel
{
    public class ProductSearchVM
    {
        // --- 1. Tiêu chí tìm kiếm (theo slide p2) ---
        public string SearchTerm { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string SortOrder { get; set; }
        public int? page { get; set; } // Giữ 'page' (chữ thường) để khớp với Controller

        // --- 2. Kết quả trả về ---

        // 🔥 LỖI NẰM Ở ĐÂY: BẠN BỊ THIẾU THUỘC TÍNH NÀY
        public IPagedList<Product> Products { get; set; }
    }
}
