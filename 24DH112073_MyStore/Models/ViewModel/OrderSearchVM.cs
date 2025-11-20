
using _24DH112073_MyStore.Models; // <-- Thêm namespace chứa Model (Order, Customer...)
using PagedList; // <-- Cần cài package PagedList.Mvc
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _24DH112073_MyStore.Models.ViewModel
{
    public class OrderSearchVM
    {
        // --- 1. Tiêu chí tìm kiếm ---

        /// <summary>
        /// Tìm kiếm chung (để xử lý Mã đơn hàng và Tên khách hàng kết hợp)
        /// </summary>
        public string SearchTerm { get; set; } // <--- ĐÃ THÊM DÒNG NÀY (FIX LỖI CS0234)

        /// <summary>
        /// Tìm theo Tên khách hàng [cite: 1020]
        /// </summary>
        public string SearchCustomerName { get; set; }

        /// <summary>
        /// Tìm theo Trạng thái thanh toán [cite: 1020]
        /// </summary>
        public string SearchPaymentStatus { get; set; }

        /// <summary>
        /// Tìm theo Ngày đặt hàng [cite: 1020]
        /// </summary>
        public DateTime? SearchOrderDate { get; set; }


        // --- 2. Thuộc tính hỗ trợ Phân trang (yêu cầu 15 đơn/trang)  ---

        /// <summary>
        /// Số trang hiện tại
        /// </summary>
        public int? PageNumber { get; set; }

        /// <summary>
        /// Kích thước trang (Set mặc định là 15 theo yêu cầu) 
        /// </summary>
        public int PageSize { get; set; } = 5; // Đã sửa lại thành 15


        // --- 3. Kết quả trả về cho View ---

        /// <summary>
        /// Danh sách đơn hàng kết quả đã được phân trang
        /// </summary>
        public IPagedList<Order> Results { get; set; }
    }
}
