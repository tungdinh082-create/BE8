using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // <-- THƯ VIỆN NÀY LÀ BẮT BUỘC
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _24DH112073_MyStore.Models.ViewModel
{
    public class CustomerViewModel
    {
        public int CustomerID { get; set; } // Dùng cho Edit

        // ===== BƯỚC SỬA LỖI: Thêm [Required] =====
        [Required(ErrorMessage = "Vui lòng nhập Mã khách hàng")]
        [Display(Name = "Mã khách hàng")]
        public string Username { get; set; }

        // ===== BƯỚC SỬA LỖI: Thêm [Required] =====
        [Required(ErrorMessage = "Vui lòng nhập Họ tên")]
        [Display(Name = "Họ và tên")]
        public string CustomerName { get; set; }

        // ===== BƯỚC SỬA LỖI: Thêm [Required] =====
        [Required(ErrorMessage = "Vui lòng nhập Mật khẩu")]
        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Xác nhận mật khẩu")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu không khớp.")]
        public string ConfirmPassword { get; set; }

        // ===== BƯỚC SỬA LỖI: Thêm [Required] =====
        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [Display(Name = "Địa chỉ Email")]
        [EmailAddress]
        public string CustomerEmail { get; set; }

        // Phần upload hình ảnh (giống slide 263-272)
        [NotMapped]
        public HttpPostedFileBase UploadImg { get; set; }
        public string CustomerImage { get; set; } // Để lưu đường dẫn

        [Display(Name = "Vai trò")]
        public string UserRole { get; set; } // "Customer" hoặc "Admin"

        // ===== BƯỚC SỬA LỖI: Thêm [Required] (Lỗi của bạn ở đây) =====
        [Required(ErrorMessage = "Vui lòng nhập Số điện thoại")]
        [Display(Name = "Số điện thoại")]
        public string CustomerPhone { get; set; }

        // ===== BƯỚC SỬA LỖI: Thêm [Required] (Phòng lỗi tiếp theo) =====
        [Required(ErrorMessage = "Vui lòng nhập Địa chỉ")]
        [Display(Name = "Địa chỉ")]
        public string CustomerAddress { get; set; }
    }
}
