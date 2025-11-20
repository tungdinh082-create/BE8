using System.ComponentModel.DataAnnotations;

namespace _24DH112073_MyStore.Models.ViewModel
{
    public class ProfileVM
    {
        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Họ tên")]
        public string CustomerName { get; set; }

        [Required]
        [Display(Name = "Số điện thoại")]
        [DataType(DataType.PhoneNumber)]
        public string CustomerPhone { get; set; }

        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string CustomerEmail { get; set; }

        [Required]
        [Display(Name = "Địa chỉ")]
        public string CustomerAddress { get; set; }
    }
}