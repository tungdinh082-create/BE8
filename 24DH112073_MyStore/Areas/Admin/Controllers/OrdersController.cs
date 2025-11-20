using _24DH112073_MyStore.Models; // Thay bằng namespace Models của bạn
using _24DH112073_MyStore.Models.ViewModel; // Cần cho OrderSearchVM
using PagedList; // Cần cho .ToPagedList()
using System;
using System.Data.Entity; // Cần cho .Include() và DbFunctions
using System.Linq;
using System.Web.Mvc;

namespace _24DH112073_MyStore.Areas.Admin.Controllers
{
    public class OrdersController : Controller
    {
        // Khởi tạo DbContext
        private MyStoreEntities db = new MyStoreEntities();

        // GET: Admin/Orders (Trang Danh sách đơn hàng)
        public ActionResult Index(OrderSearchVM searchModel)
        {
            // 1. Tạo câu query cơ bản
            // .Include("Customer") để có thể lọc và hiển thị tên Khách hàng
            var ordersQuery = db.Orders.Include(o => o.Customer).AsQueryable();

            // 2. Xây dựng query lọc dựa trên ViewModel
            // Lọc theo Tên khách hàng
            if (!string.IsNullOrEmpty(searchModel.SearchCustomerName))
            {
                ordersQuery = ordersQuery.Where(o =>
                    o.Customer.CustomerName.Contains(searchModel.SearchCustomerName));
            }

            // Lọc theo Trạng thái thanh toán
            if (!string.IsNullOrEmpty(searchModel.SearchPaymentStatus))
            {
                ordersQuery = ordersQuery.Where(o =>
                    o.PaymentStatus == searchModel.SearchPaymentStatus);
            }

            // Lọc theo Ngày đặt hàng
            if (searchModel.SearchOrderDate.HasValue)
            {
                // Lấy ngày_tháng_năm từ giá trị tìm kiếm
                DateTime searchDate = searchModel.SearchOrderDate.Value.Date;

                // So sánh phần ngày_tháng_năm của OrderDate với ngày tìm kiếm
                // Cần using System.Data.Entity; để dùng DbFunctions
                ordersQuery = ordersQuery.Where(o =>
                    DbFunctions.TruncateTime(o.OrderDate) == searchDate);
            }

            // 3. Sắp xếp kết quả (mặc định theo ngày đặt mới nhất)
            ordersQuery = ordersQuery.OrderByDescending(o => o.OrderDate);

            // 4. Phân trang
            // Lấy số trang, nếu null thì là trang 1
            int pageNum = (searchModel.PageNumber ?? 1);
            // Lấy kích thước trang từ VM (đã set mặc định là 15)
            int pageSize = searchModel.PageSize;

            // 5. Gán kết quả đã lọc và phân trang vào ViewModel
            searchModel.Results = ordersQuery.ToPagedList(pageNum, pageSize);

            // (Optional) Tạo SelectList cho Dropdown Trạng thái thanh toán
            ViewBag.PaymentStatusList = new SelectList(new[]
            {
                new { Value = "", Text = "Tất cả trạng thái" },
                new { Value = "Chưa thanh toán", Text = "Chưa thanh toán" },
                new { Value = "Thanh toán tiền mặt", Text = "Thanh toán tiền mặt" },
                new { Value = "Thanh toán Paypal", Text = "Thanh toán Paypal" }
                // Thêm các trạng thái khác nếu có
            }, "Value", "Text", searchModel.SearchPaymentStatus); // Tham số cuối là giá trị đang được chọn

            // 6. Trả ViewModel về View
            return View(searchModel);
        }

        // GET: Admin/Orders/DetailedOrder/5 (Trang Chi tiết đơn hàng)
        public ActionResult DetailedOrder(int? id)
        {
            if (id == null)
            {
                // Trả về lỗi 400 nếu không có id
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            // Lấy đơn hàng, bao gồm thông tin Customer
            // và danh sách OrderDetails, mỗi OrderDetail lại kèm thông tin Product
            Order order = db.Orders
                            .Include(o => o.Customer)
                            .Include(o => o.OrderDetails.Select(od => od.Product))
                            .SingleOrDefault(o => o.OrderID == id);

            if (order == null)
            {
                // Trả về lỗi 404 nếu không tìm thấy đơn hàng
                return HttpNotFound();
            }

            // Trả về View "DetailedOrder.cshtml" với model là đơn hàng tìm được
            return View(order);
        }

        // --- BẮT ĐẦU CODE THÊM MỚI ---

        // GET: Admin/Orders/UpdateStatus/5
        // Action này được gọi khi bấm nút "Cập nhật trạng thái"
        public ActionResult UpdateStatus(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            // Tạo danh sách các trạng thái để hiển thị trong Dropdown
            var statusList = new[]
            {
                "Chưa thanh toán",
                "Thanh toán tiền mặt",
                "Thanh toán Paypal",
                "Đang xử lý",
                "Đang vận chuyển",
                "Đã giao",
                "Đã hủy"
            };

            // Dùng ViewBag để gửi danh sách này sang View
            // (tham số thứ 2 là trạng thái đang được chọn)
            ViewBag.PaymentStatus = new SelectList(statusList, order.PaymentStatus);

            return View(order);
        }

        // POST: Admin/Orders/UpdateStatus/5
        // Action này được gọi khi bấm nút "Lưu" trên trang UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateStatus(int id, string PaymentStatus)
        {
            // Tìm đơn hàng gốc trong DB
            Order orderToUpdate = db.Orders.Find(id);

            if (orderToUpdate == null)
            {
                return HttpNotFound();
            }

            // Cập nhật trạng thái mới (lấy từ form)
            orderToUpdate.PaymentStatus = PaymentStatus;

            // Đánh dấu là đã sửa đổi và lưu lại
            db.Entry(orderToUpdate).State = EntityState.Modified;
            db.SaveChanges();

            // Quay về trang Chi tiết đơn hàng để xem kết quả
            return RedirectToAction("DetailedOrder", new { id = id });
        }

        // --- KẾT THÚC CODE THÊM MỚI ---

        // Giải phóng DbContext khi Controller bị hủy
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}