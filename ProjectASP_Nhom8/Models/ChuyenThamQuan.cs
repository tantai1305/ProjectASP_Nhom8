using System;
using System.Collections.Generic;

namespace ProjectASP_Nhom8.Models
{
    public partial class ChuyenThamQuan
    {
        public ChuyenThamQuan()
        {
            DangKies = new HashSet<DangKy>();
        }

        public string MaThamQuan { get; set; } = null!;
        public int SoLuongNguoiThamGia { get; set; }
        public string? MoTa { get; set; }
        public DateTime NgayToChuc { get; set; }
        public DateTime ThoiGianBatDau { get; set; }
        public DateTime ThoiGianKetThuc { get; set; }
        public string NienKhoa { get; set; } = null!;
        public string HocKy { get; set; } = null!;
        public string? Msgv { get; set; }
        public string MaDn { get; set; } = null!;

        public virtual DoanhNghiep MaDnNavigation { get; set; } = null!;
        public virtual GiangVien? MsgvNavigation { get; set; }
        public virtual ICollection<DangKy> DangKies { get; set; }
    }
}
