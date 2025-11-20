
using _24DH112073_MyStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _24DH112073_MyStore.Models.ViewModel
{
    public class MyOrderVM
    {
        /// <summary>
        /// Từ khóa tìm kiếm (theo Mã đơn hoặc Tên sản phẩm)
        /// </summary>
        public string SearchTerm { get; set; }

        /// <summary>
        /// Tab trạng thái đang được chọn (Tất cả, Đang xử lý, Đã giao...)
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 🔥 Cờ báo hiệu có hiển thị tất cả đơn hàng trong trạng thái hay không (tối đa 50)
        /// </summary>
        public bool ShowAll { get; set; }

        /// <summary>
        /// Danh sách tất cả đơn hàng của khách hàng (đã lọc)
        /// </summary>
        public List<Order> Orders { get; set; }

        public MyOrderVM()
        {
            // Khởi tạo để tránh lỗi null
            Orders = new List<Order>();
            Status = "Tất cả đơn"; // Mặc định là tab "Tất cả"
            // ShowAll mặc định là false
        }
    }
}
