using System;
using System.Collections.Generic;

namespace ProjectASP_Nhom8.Models
{
    public partial class GiangVien
    {
        public GiangVien()
        {
            ChuyenThamQuans = new HashSet<ChuyenThamQuan>();
        }

        public string Msgv { get; set; } = null!;
        public string HoTen { get; set; } = null!;
        public string Sdt { get; set; } = null!;
        public string? Email { get; set; }
        public string TaiKhoan { get; set; } = null!;

        public virtual TaiKhoan TaiKhoanNavigation { get; set; } = null!;
        public virtual ICollection<ChuyenThamQuan> ChuyenThamQuans { get; set; }
    }
}
