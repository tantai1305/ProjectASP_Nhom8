using System;
using System.Collections.Generic;

namespace ProjectASP_Nhom8.Models
{
    public partial class DangKy
    {
        public string Mssv { get; set; } = null!;
        public string MaThamQuan { get; set; } = null!;
        public DateTime? NgayDangKy { get; set; }

        public virtual ChuyenThamQuan MaThamQuanNavigation { get; set; } = null!;
        public virtual SinhVien MssvNavigation { get; set; } = null!;
    }
}
