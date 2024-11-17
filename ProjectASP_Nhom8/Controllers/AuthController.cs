using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectASP_Nhom8.Models;
using ProjectASP_Nhom8.DTOs;
using Microsoft.CodeAnalysis.Scripting;
using System;
using Microsoft.EntityFrameworkCore;

namespace ProjectASP_Nhom8.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly QLThamQuanDNContext _QLThamQuanDNContext;

        public AuthController(QLThamQuanDNContext qLThamQuanDNContext)
        {
            _QLThamQuanDNContext = qLThamQuanDNContext;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            if (await _QLThamQuanDNContext.TaiKhoans.AnyAsync(t => t.TaiKhoan1 == dto.TaiKhoan))
                return BadRequest("Tài khoản đã tồn tại.");

            var taiKhoan = new TaiKhoan
            {
                TaiKhoan1 = dto.TaiKhoan,
                MatKhau = BCrypt.Net.BCrypt.HashPassword(dto.MatKhau), // Băm mật khẩu
                Quyen = dto.Quyen
            };

            _QLThamQuanDNContext.TaiKhoans.Add(taiKhoan);
            await _QLThamQuanDNContext.SaveChangesAsync();

            return Ok("Đăng ký thành công.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginDTO dto)
        {
            // Tìm tài khoản dựa trên username
            var taiKhoan = await _QLThamQuanDNContext.TaiKhoans
                .FirstOrDefaultAsync(t => t.TaiKhoan1 == dto.TaiKhoan);

            // Kiểm tra tài khoản tồn tại
            if (taiKhoan == null)
                return Unauthorized("Sai tài khoản hoặc mật khẩu.");

            // Kiểm tra mật khẩu
            if (!BCrypt.Net.BCrypt.Verify(dto.MatKhau, taiKhoan.MatKhau))
                return Unauthorized("Sai tài khoản hoặc mật khẩu.");

            // Đăng nhập thành công
            // Lưu quyền vào session hoặc cookie
            HttpContext.Session.SetString("UserRole", taiKhoan.Quyen);

            // Đăng nhập thành công và chuyển hướng về trang chủ
            return RedirectToAction("TrangChu", "Home");
        }
    }
}
