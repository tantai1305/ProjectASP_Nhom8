using System;
using System.Collections.Generic;

namespace ProjectASP_Nhom8.Models
{
    public partial class TaiKhoan
    {
        public TaiKhoan()
        {
            GiangViens = new HashSet<GiangVien>();
            SinhViens = new HashSet<SinhVien>();
        }

        public string TaiKhoan1 { get; set; } = null!;
        public string MatKhau { get; set; } = null!;
        public string Quyen { get; set; } = null!;

        public virtual ICollection<GiangVien> GiangViens { get; set; }
        public virtual ICollection<SinhVien> SinhViens { get; set; }
    }
}
