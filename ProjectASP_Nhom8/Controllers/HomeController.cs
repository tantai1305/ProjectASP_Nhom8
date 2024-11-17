using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectASP_Nhom8.Models;
using X.PagedList;
using System.Diagnostics;

namespace ProjectASP_Nhom8.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly QLThamQuanDNContext _DB;

        public HomeController(ILogger<HomeController> logger, QLThamQuanDNContext dB)
        {
            _logger = logger;
            _DB = dB;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult TrangChu()
        {
            return View();
        }

        //Danh sách sinh viên
        public IActionResult DSSinhVien(int? page)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return RedirectToAction("AccessDenied", "Home"); // Trang thông báo lỗi truy cập
            }

            var danhSach = _DB.SinhViens.ToPagedList(page ?? 1, 5);
            return View(danhSach);
        }

        //Trang thông báo từ chối truy cập
        public IActionResult AccessDenied()
        {
            return View();
        }

        //Thêm sinh viên 
        public IActionResult ThemSinhVien()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return RedirectToAction("AccessDenied", "Home"); // Trang thông báo lỗi truy cập
            }
            var dsTaiKhoan = _DB.TaiKhoans
                .Where(e => e.Quyen.Equals("Sinh Viên"))
                .Select(tk => tk.TaiKhoan1)
                .Except(_DB.SinhViens.Select(sv => sv.TaiKhoan)) //loại bỏ các tài khoản đã có trong bang SinhVien
                .ToList();
            ViewBag.DanhSachTaiKhoan = new SelectList(dsTaiKhoan, "TaiKhoan1", "TaiKhoan1");
            return View();
        }

        [HttpPost]
        public IActionResult ThemSinhVien(SinhVien sinhVien)
        {
            //Kiểm tra trùng mã
            if (_DB.SinhViens.Any(e => e.Mssv == sinhVien.Mssv))
            {
                ViewBag.ThongBao = "Mã sinh viên đã tồn tại. Vui lòng kiểm tra lại";
                var dsTaiKhoan = _DB.TaiKhoans
                .Where(e => e.Quyen.Equals("Sinh Viên"))
                .Select(tk => tk.TaiKhoan1)
                .Except(_DB.SinhViens.Select(sv => sv.TaiKhoan)) //loại bỏ các tài khoản đã có trong bang SinhVien
                .ToList();
                ViewBag.DanhSachTaiKhoan = new SelectList(dsTaiKhoan, "TaiKhoan1", "TaiKhoan1");
                return View("ThemSinhVien");
            }
            else
            {
                _DB.SinhViens.Add(sinhVien);
                _DB.SaveChanges();

                TempData["ThongBaoThem"] = "Thêm sinh viên thành công";
                return RedirectToAction("DSSinhVien");
            }
        }

        //Xóa sinh viên
        public IActionResult XoaSinhVien(string maSV)
        {
            var dk = _DB.DangKies.FirstOrDefault(d => d.Mssv == maSV);
            if (dk != null)
            {
                _DB.DangKies.Remove(dk);
                _DB.SaveChanges();
            }

            var sinhVien = _DB.SinhViens.FirstOrDefault(s => s.Mssv == maSV);

            if (sinhVien != null)
            {
                _DB.SinhViens.Remove(sinhVien);
                _DB.SaveChanges();
                TempData["ThongBaoXoa"] = "Xóa sinh viên thành công";
            }

            return RedirectToAction("DSSinhVien");
        }


        //Cập nhật thông tin sinh viên
        public IActionResult CapNhatSinhVien(string maSV)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return RedirectToAction("AccessDenied", "Home"); // Trang thông báo lỗi truy cập
            }
            var sinhVien = _DB.SinhViens.FirstOrDefault(e => e.Mssv == maSV);
            var dsTaiKhoan = _DB.TaiKhoans
                            .Where(e => e.Quyen.Equals("Sinh Viên"))
                            .Select(tk => tk.TaiKhoan1)
                            .Except(_DB.SinhViens.Select(sv => sv.TaiKhoan)) //loại bỏ các tài khoản đã có trong bang SinhVien
                            .ToList();
            ViewBag.DanhSachTaiKhoan = new SelectList(dsTaiKhoan, "TaiKhoan1", "TaiKhoan1");
            return View(sinhVien);
        }

        [HttpPost]
        public IActionResult CapNhatSinhVien(SinhVien sinhVien)
        {
            if (sinhVien != null)
            {
                _DB.SinhViens.Update(sinhVien);
                _DB.SaveChanges();
                TempData["ThongBaoSua"] = "Cập nhật sinh viên thành công";
            }
            return RedirectToAction("DSSinhVien");
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Xóa thông tin đăng nhập khỏi Session
            HttpContext.Session.Clear();

            // Chuyển hướng về trang đăng nhập
            return RedirectToAction("Index", "Home");
        }
    }
}