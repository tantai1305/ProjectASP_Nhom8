using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Display(Name = "Ngày tổ chức")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime NgayToChuc { get; set; }
        [Display(Name = "Thời gian bắt đầu")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime ThoiGianBatDau { get; set; }
        [Display(Name = "Thời gian kết thúc")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
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
