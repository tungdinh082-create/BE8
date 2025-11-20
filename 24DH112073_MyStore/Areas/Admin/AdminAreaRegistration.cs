
using System.Web.Mvc;

namespace _24DH112073_MyStore.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
     "Admin_default",
     "Admin/{controller}/{action}/{id}",
     new { action = "Index", id = UrlParameter.Optional },
     // Thêm Namespaces để chỉ tìm ở thư mục Admin Controller
     namespaces: new[] { "_24DH112073_MyStore.Areas.Admin.Controllers" }
                            );
        }
    }
}
