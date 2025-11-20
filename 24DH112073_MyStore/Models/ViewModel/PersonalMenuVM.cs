using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// Đổi namespace này cho đúng với dự án của bạn
namespace _24DH112073_MyStore.Models.ViewModel
{
    public class PersonalMenuVM
    {
        // Dùng để kiểm tra đã đăng nhập hay chưa
        public bool IsLoggedIn { get; set; }

        // Dùng để lưu tên người dùng
        public string Username { get; set; }

        // Dùng để đếm số lượng hàng trong giỏ (Admin không cần nhưng Customer cần)
        public int CartCount { get; set; }
    }
}