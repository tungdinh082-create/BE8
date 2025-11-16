using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _24DH112073_MyStore.Models.ViewModel // <-- THAY TÊN PROJECT
{
    public class PersonalMenuVM
    {
        public bool IsLoggedIn { get; set; }
        public string Username { get; set; }
        public int CartCount { get; set; }
    }
}