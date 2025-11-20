
using _24DH112073_MyStore.Models;
using _24DH112073_MyStore.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

// Đổi namespace này cho đúng
namespace _24DH112073_MyStore.Models.ViewModel
{
    public class CheckoutVM
    {
        // Thông tin Giỏ hàng
        public List<CartItem> CartItems { get; set; }

        // Thông tin Khách hàng
        public int CustomerID { get; set; }
        public string Username { get; set; }

        [Display(Name = "Địa chỉ giao hàng")]
        public string ShippingAddress { get; set; }

        // Thông tin Đơn hàng
        [Display(Name = "Ngày đặt hàng")]
        public DateTime OrderDate { get; set; }

        [Display(Name = "Tổng giá trị")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "Trạng thái thanh toán")]
        public string PaymentStatus { get; set; }

        [Display(Name = "Phương thức thanh toán")]
        public string PaymentMethod { get; set; }

        [Display(Name = "Phương thức giao hàng")]
        public string ShippingMethod { get; set; }

        // Dùng để tạo OrderDetail
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
