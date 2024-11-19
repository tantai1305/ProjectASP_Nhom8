using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectASP_Nhom8.Models;
using X.PagedList;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

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

        //Danh sách giảng viên
        public IActionResult DSGiangVien(int? page)
        {
            var danhSach = _DB.GiangViens.ToPagedList(page ?? 1, 5);
            return View(danhSach);
        }

        //Thêm giảng viên 
        public IActionResult ThemGiangVien()
        {
            var dsTaiKhoan = _DB.TaiKhoans.Where(t => t.TaiKhoan1.Contains("GV")).ToList();
            ViewBag.DanhSachTaiKhoan = new SelectList(dsTaiKhoan, "TaiKhoan1", "TaiKhoan1");
            return View();
        }

        [HttpPost]
        public IActionResult ThemGiangVien(GiangVien giangVien)
        {
            //Kiểm tra trùng mã
            if (_DB.GiangViens.Any(e => e.Msgv == giangVien.Msgv))
            {
                ViewBag.ThongBao = "Mã giảng viên đã tồn tại. Vui lòng kiểm tra lại";
                var dsTaiKhoan = _DB.TaiKhoans.Where(t => t.TaiKhoan1.Contains("GV")).ToList();
                ViewBag.DanhSachTaiKhoan = new SelectList(dsTaiKhoan, "TaiKhoan1", "TaiKhoan1");
                return View("ThemGiangVien");
            }
            if (_DB.GiangViens.Any(e => e.Msgv != giangVien.Msgv && e.TaiKhoan == giangVien.TaiKhoan))
            {
                ViewBag.ThongBao = "Tài khoản này đã được phân cho giảng viên khác. Vui lòng kiểm tra lại";
                var dsTaiKhoan = _DB.TaiKhoans.Where(t => t.TaiKhoan1.Contains("GV")).ToList();
                ViewBag.DanhSachTaiKhoan = new SelectList(dsTaiKhoan, "TaiKhoan1", "TaiKhoan1");
                return View("ThemGiangVien");
            }
            else
            {
                _DB.GiangViens.Add(giangVien);
                _DB.SaveChanges();

                TempData["ThongBaoThem"] = "Thêm giảng viên thành công";
                return RedirectToAction("DSGiangVien");
            }
        }

        //Xóa giảng viên
        public IActionResult XoaGiangVien(string maGV)
        {
            // Tìm tất cả bản ghi trong ChuyenThamQuan có Msgv bằng với maGV
            var dsChuyenThamQuan = _DB.ChuyenThamQuans.Where(d => d.Msgv == maGV).ToList();
            if (dsChuyenThamQuan.Any())
            {
                // Đặt Msgv thành null cho mỗi chuyến tham quan tìm được
                foreach (var ctq in dsChuyenThamQuan)
                {
                    ctq.Msgv = null;
                }

                // Lưu thay đổi vào cơ sở dữ liệu sau khi cập nhật tất cả
                _DB.SaveChanges();
                TempData["ThongBaoXoa"] = "Đã xóa mã giảng viên khỏi tất cả các chuyến tham quan thành công";
            }
            var giangVien = _DB.GiangViens.FirstOrDefault(s => s.Msgv == maGV);

            if (giangVien != null)
            {
                _DB.GiangViens.Remove(giangVien);
                _DB.SaveChanges();
                TempData["ThongBaoXoa"] = "Xóa giảng viên thành công";
            }

            return RedirectToAction("DSGiangVien");
        }

        //Cập nhật thông tin sinh viên
        public IActionResult CapNhatGiangVien(string maGV)
        {
            var giangVien = _DB.GiangViens.Where(e => e.Msgv == maGV).FirstOrDefault();
            var dsTaiKhoan = _DB.TaiKhoans.Where(t => t.TaiKhoan1.Contains("GV")).ToList();
            ViewBag.DanhSachTaiKhoan = new SelectList(dsTaiKhoan, "TaiKhoan1", "TaiKhoan1");
            return View(giangVien);
        }
        [HttpPost]
        public IActionResult CapNhatGiangVien(GiangVien giangVien)
        {
            _DB.GiangViens.Update(giangVien);
            _DB.SaveChanges();
            TempData["ThongBaoSua"] = "Cập nhật giảng viên thành công";

            return RedirectToAction("DSGiangVien");
        }

        public IActionResult DSDangKyTheoMaSV(string maSV, int? page)
        {
            maSV = HttpContext.Session.GetString("MaSV");
            if (string.IsNullOrEmpty(maSV))
            {
                TempData["ThongBao"] = "Session đã hết hạn hoặc không tồn tại. Vui lòng đăng nhập lại.";
                return RedirectToAction("Logout", "Home");
            }
            var danhSach = _DB.DangKies.Where(dk => dk.Mssv == maSV).Include(d => d.MaThamQuanNavigation).ThenInclude(mtq => mtq.MaDnNavigation).ToPagedList(page ?? 1, 1);
            return View(danhSach);
        }
        public IActionResult DSChuyenThamQuanHienTai(int? page)
        {

            var danhSach = _DB.ChuyenThamQuans.Where(tq => tq.NgayToChuc >= DateTime.Now.Date).Include(d => d.MaDnNavigation).ToPagedList(page ?? 1, 2);
            return View(danhSach);
        }
        public IActionResult DangKyChuyenThamQuan(DangKy sinhVien)
        {
            // Lấy MaSV từ session
            var maSV = HttpContext.Session.GetString("MaSV");

            // Kiểm tra nếu session không tồn tại hoặc đã hết hạn
            if (string.IsNullOrEmpty(maSV))
            {
                TempData["ThongBao"] = "Session đã hết hạn hoặc không tồn tại. Vui lòng đăng nhập lại.";
                return RedirectToAction("Logout", "Home");
            }

            // Kiểm tra nếu MaThamQuan hoặc sinhVien không hợp lệ
            if (sinhVien == null || sinhVien.MaThamQuan == null)
            {
                TempData["ThongBao"] = "Thông tin chuyến tham quan không hợp lệ.";
                return RedirectToAction("DSChuyenThamQuanHienTai");
            }
            var chuyenThamQuan = _DB.ChuyenThamQuans.FirstOrDefault(ctq => ctq.MaThamQuan == sinhVien.MaThamQuan);
            var soLuongDaDangKy = _DB.DangKies.Count(dk => dk.MaThamQuan == sinhVien.MaThamQuan);
            if (soLuongDaDangKy >= chuyenThamQuan.SoLuongNguoiThamGia)
            {
                TempData["VuotGioiHan"] = "Số lượng đăng ký đã đạt giới hạn. Không thể đăng ký thêm.";
                return RedirectToAction("DSChuyenThamQuanHienTai");
            }
            if (DateTime.Now > chuyenThamQuan.NgayToChuc.AddDays(-7))
            {
                TempData["HetHan"] = "Đăng ký thất bại! Bạn chỉ có thể đăng ký trước 1 tuần so với ngày tổ chức.";
                return RedirectToAction("DSChuyenThamQuanHienTai");
            }
            // Kiểm tra trùng mã sinh viên và mã tham quan
            if (_DB.DangKies.Any(e => e.Mssv == maSV && e.MaThamQuan == sinhVien.MaThamQuan))
            {
                TempData["CanhBao"] = "Bạn đã đăng ký chuyến tham quan này trước đó.";
                return RedirectToAction("DSChuyenThamQuanHienTai");
            }

            // Thêm thông tin đăng ký
            sinhVien.Mssv = maSV; // Gán mã sinh viên từ session
            sinhVien.NgayDangKy = DateTime.Now;

            try
            {
                _DB.DangKies.Add(sinhVien);
                _DB.SaveChanges();
                TempData["ThongBaoThem"] = "Đăng ký chuyến tham quan thành công!";
            }
            catch (Exception ex)
            {
                // Log lỗi (nếu cần)
                TempData["ThongBao"] = "Đã xảy ra lỗi trong quá trình đăng ký. Vui lòng thử lại.";
                throw new Exception(ex.Message);
            }

            return RedirectToAction("DSChuyenThamQuanHienTai");
        }


        //Xóa đăng ký
        public IActionResult HuyDangKy(string maSV, string maTQ)
        {
            // Tìm tất cả bản ghi trong ChuyenThamQuan có Msgv bằng với maGV
            var dsDangKy = _DB.DangKies.FirstOrDefault(d => d.Mssv == maSV && d.MaThamQuan == maTQ);
            var chuyenThamQuan = _DB.ChuyenThamQuans.FirstOrDefault(ctq => ctq.MaThamQuan == maTQ);
            if (DateTime.Now >= chuyenThamQuan.NgayToChuc)
            {
                TempData["DaDienRa"] = "Không thể hủy đăng ký. Chuyến tham quan đã diễn ra";
                return RedirectToAction("DSDangKyTheoMaSV");
            }
            if ((chuyenThamQuan.NgayToChuc - DateTime.Now).Days < 7)
            {
                TempData["QuaTH"] = "Không thể hủy đăng ký. Bạn phải hủy ít nhất 1 tuần trước ngày tổ chức.";
                return RedirectToAction("DSDangKyTheoMaSV");
            }
            if (dsDangKy != null)
            {
                _DB.DangKies.Remove(dsDangKy);
                _DB.SaveChanges();
                TempData["ThongBaoXoa"] = "Hủy đăng ký thành công";
            }

            return RedirectToAction("DSDangKyTheoMaSV");
        }
    }
}