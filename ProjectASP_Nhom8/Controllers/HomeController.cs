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
        //Hiển thị doanh nghiệp ở trang chủ
        public IActionResult DSDoanhNghiep()
        {
            var danhSach = _DB.DoanhNghieps.ToList();
            return View(danhSach);
        }

        //Hiển thị doanh nghiệp ở admin
        public IActionResult DSDoanhNghiepAdmin()
        {
            var danhSach = _DB.DoanhNghieps.ToList();
            return View(danhSach);
        }

        //Thêm doanh nghiệp 
        public IActionResult ThemDoanhNghiep()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ThemDoanhNghiep(DoanhNghiep doanhNghiep)
        {
            try
            {
                // Kiểm tra giá trị đầu vào
                if (string.IsNullOrWhiteSpace(doanhNghiep.MaDn) || string.IsNullOrWhiteSpace(doanhNghiep.TenDn))
                {
                    ViewBag.ThongBao = "Mã và Tên Doanh nghiệp không được để trống.";
                    var dsDoanhNghiep = _DB.DoanhNghieps.ToList();
                    ViewBag.DanhSachDoanhNghiep = new SelectList(dsDoanhNghiep, "MaDn", "TenDn");
                    return View("ThemDoanhNghiep");
                }

                // Kiểm tra trùng lặp Mã Doanh nghiệp
                if (_DB.DoanhNghieps.Any(e => e.MaDn == doanhNghiep.MaDn))
                {
                    ViewBag.ThongBao = "Mã Doanh nghiệp đã tồn tại.";
                    var dsDoanhNghiep = _DB.DoanhNghieps.ToList();
                    ViewBag.DanhSachDoanhNghiep = new SelectList(dsDoanhNghiep, "MaDn", "TenDn");
                    return View("ThemDoanhNghiep");
                }

                // Kiểm tra trùng lặp Tên Doanh nghiệp
                if (_DB.DoanhNghieps.Any(e => e.DiaChi == doanhNghiep.DiaChi && e.TenDn == doanhNghiep.TenDn))
                {
                    ViewBag.ThongBao = "Doanh nghiệp đã tồn tại.";
                    var dsDoanhNghiep = _DB.DoanhNghieps.ToList();
                    ViewBag.DanhSachDoanhNghiep = new SelectList(dsDoanhNghiep, "MaDn", "TenDn");
                    return View("ThemDoanhNghiep");
                }

                // Thêm mới doanh nghiệp
                _DB.DoanhNghieps.Add(doanhNghiep);
                _DB.SaveChanges();

                TempData["ThongBaoThem"] = "Thêm Doanh nghiệp thành công.";
                return RedirectToAction("DSDoanhNghiepAdmin");
            }
            catch (DbUpdateException ex)
            {
                // Xử lý lỗi cập nhật cơ sở dữ liệu
                ViewBag.ThongBao = "Lỗi khi lưu dữ liệu: " + ex.InnerException?.Message;
                var dsDoanhNghiep = _DB.DoanhNghieps.ToList();
                ViewBag.DanhSachDoanhNghiep = new SelectList(dsDoanhNghiep, "MaDn", "TenDn");
                return View("ThemDoanhNghiep");
            }
            catch (Exception ex)
            {
                // Xử lý lỗi không mong muốn
                ViewBag.ThongBao = "Đã xảy ra lỗi: " + ex.Message;
                var dsDoanhNghiep = _DB.DoanhNghieps.ToList();
                ViewBag.DanhSachDoanhNghiep = new SelectList(dsDoanhNghiep, "MaDn", "TenDn");
                return View("ThemDoanhNghiep");
            }
        }

        //Xóa doanh nghiệp
        public IActionResult XoaDoanhNghiep(string maDN)
        {
            var dn = _DB.DoanhNghieps.FirstOrDefault(d => d.MaDn == maDN);

            if (dn != null)
            {
                // Kiểm tra nếu có chuyến tham quan liên quan
                bool hasRelatedRecords = _DB.ChuyenThamQuans.Any(ctq => ctq.MaDn == maDN);
                if (hasRelatedRecords)
                {
                    TempData["ThongBaoXoa"] = "Không thể xóa doanh nghiệp vì có chuyến tham quan liên quan.";
                    return RedirectToAction("DSDoanhNghiepAdmin");
                }

                // Xóa doanh nghiệp nếu không có bản ghi liên quan
                _DB.DoanhNghieps.Remove(dn);
                _DB.SaveChanges();

                TempData["ThongBaoXoa"] = "Xóa thành công";
            }
            return RedirectToAction("DSDoanhNghiepAdmin");
        }

        //Cập nhật doanh nghiệp
        public IActionResult CapNhatDoanhNghiep(string maDN)
        {
            var doanhNghiep = _DB.DoanhNghieps.Where(e => e.MaDn == maDN).FirstOrDefault();
            return View(doanhNghiep);
        }

        [HttpPost]
        public IActionResult CapNhatDoanhNghiep(DoanhNghiep doanhNghiep)
        {
            if (doanhNghiep != null)
            {
                // Kiểm tra xem tên và địa chỉ đã tồn tại trong cơ sở dữ liệu chưa, ngoại trừ bản ghi hiện tại
                var existingDoanhNghiep = _DB.DoanhNghieps
                    .FirstOrDefault(dn => dn.TenDn == doanhNghiep.TenDn && dn.DiaChi == doanhNghiep.DiaChi && dn.MaDn != doanhNghiep.MaDn);

                if (existingDoanhNghiep != null)
                {
                    // Nếu tìm thấy bản ghi trùng lặp, trả về thông báo lỗi
                    TempData["ThongBaoSua"] = "Tên doanh nghiệp và địa chỉ đã tồn tại trong hệ thống!";
                    return RedirectToAction("DSDoanhNghiepAdmin");
                }

                // Nếu không trùng lặp, thực hiện cập nhật
                _DB.DoanhNghieps.Update(doanhNghiep);
                _DB.SaveChanges();
                TempData["ThongBaoSua"] = "Cập nhật doanh nghiệp thành công!";
            }
            return RedirectToAction("DSDoanhNghiepAdmin");
        }
        public IActionResult DSChuyenThamQuanAdmin()
        {

            var dsctq = _DB.ChuyenThamQuans.Include(dn => dn.MaDnNavigation).ToList();
            return View(dsctq);
        }

        /// <summary>
        /// Thêm chuyến tham quan.
        /// </summary>
        /// <returns></returns>
        public IActionResult TaoChuyenThamQuan()
        {
            var TKList = _DB.GiangViens.ToList();
            var DNList = _DB.DoanhNghieps.ToList();

            ViewBag.GiangVienList = new SelectList(TKList, "Msgv", "Msgv");
            ViewBag.DoanhNghiepList = new SelectList(DNList, "MaDn", "TenDn");
            return View();
        }
        [HttpPost]
        public IActionResult TaoChuyenThamQuan(ChuyenThamQuan chuyenThamQuan)
        {
            _DB.ChuyenThamQuans.Add(chuyenThamQuan);
            _DB.SaveChanges();
            return RedirectToAction("DSChuyenThamQuanAdmin");
        }

        /// <summary>
        /// Xóa chuyến tham quan.
        /// </summary>
        /// <param name="chuyenThamQuan"></param>
        /// <returns></returns>
        public IActionResult XoaChuyenThamQuan(string id)
        {
            var CTQ = _DB.ChuyenThamQuans.Where(t => t.MaThamQuan == id).FirstOrDefault();
            _DB.Remove(CTQ);
            _DB.SaveChanges();
            return RedirectToAction("DSChuyenThamQuanAdmin");
        }

        /// <summary>
        /// Sửa chuyến tham quan.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public IActionResult SuaChuyenThamQuan(string id)
        {
            var TKList = _DB.GiangViens.ToList();
            var DNList = _DB.DoanhNghieps.ToList();

            ViewBag.GiangVienList = new SelectList(TKList, "Msgv", "Msgv");
            ViewBag.DoanhNghiepList = new SelectList(DNList, "MaDn", "TenDn");

            var CTQ = _DB.ChuyenThamQuans.Where(t => t.MaThamQuan == id).FirstOrDefault();
            return View(CTQ);
        }

        [HttpPost]
        public IActionResult SuaChuyenThamQuan(ChuyenThamQuan ctq)
        {
            _DB.ChuyenThamQuans.Update(ctq);
            _DB.SaveChanges();
            return RedirectToAction("DSChuyenThamQuanAdmin");
        }

        public IActionResult XemDanhSachDangKy(string maTQ)
        {

            if (string.IsNullOrEmpty(maTQ))
            {
                return BadRequest("Mã tham quan không hợp lệ.");
            }

            var danhSachDangKy = _DB.DangKies
                .Include(dk => dk.MaThamQuanNavigation) // Load bảng ChuyenThamQuan
                .Include(dk => dk.MssvNavigation)      // Load bảng SinhVien
                .Where(dk => dk.MaThamQuan == maTQ)
                .ToList();

            if (!danhSachDangKy.Any())
            {
                return Content("Không có dữ liệu đăng ký cho chuyến tham quan này.");
            }

            return View(danhSachDangKy); // Trả về view với dữ liệu
        }
    }
}