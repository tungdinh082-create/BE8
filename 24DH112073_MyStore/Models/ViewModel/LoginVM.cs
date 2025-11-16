using System.ComponentModel.DataAnnotations;

namespace _24DH112073_MyStore.Models.ViewModel // <-- ĐÃ SỬA
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