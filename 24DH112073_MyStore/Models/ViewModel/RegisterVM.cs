
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _24DH112073_MyStore.Models.ViewModel
{
    // 3.1 Trang Đăng ký [cite: 346-353]
    public class RegisterVM
    {
        [Required(ErrorMessage = "Vui lòng nhập Tên đăng nhập")]
        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Mật khẩu")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Xác nhận mật khẩu")]
        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu và xác nhận mật khẩu không khớp.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Họ tên")]
        [Display(Name = "Họ tên")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Số điện thoại")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Số điện thoại")]
        public string CustomerPhone { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string CustomerEmail { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Địa chỉ")]
        [Display(Name = "Địa chỉ")]
        public string CustomerAddress { get; set; }
    }
}
