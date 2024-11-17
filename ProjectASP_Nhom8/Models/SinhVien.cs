using System;
using System.Collections.Generic;

namespace ProjectASP_Nhom8.Models
{
    public partial class SinhVien
    {
        public SinhVien()
        {
            DangKies = new HashSet<DangKy>();
        }

        public string Mssv { get; set; } = null!;
        public string HoTen { get; set; } = null!;
        public string Sdt { get; set; } = null!;
        public string? Email { get; set; }
        public string TaiKhoan { get; set; } = null!;

        public virtual TaiKhoan TaiKhoanNavigation { get; set; } = null!;
        public virtual ICollection<DangKy> DangKies { get; set; }
    }
}
