
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // <-- Thư viện quan trọng
using System.Linq;
using System.Web;

namespace _24DH112073_MyStore.Models.ViewModel // (Đảm bảo namespace này đúng)
{
    public class LoginVM
    {
        [Required]
        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
    }
}
