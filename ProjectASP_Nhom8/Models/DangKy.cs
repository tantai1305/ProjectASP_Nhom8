using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectASP_Nhom8.Models
{
    public partial class DangKy
    {
        public string Mssv { get; set; } = null!;
        public string MaThamQuan { get; set; } = null!;
        public DateTime? NgayDangKy { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual ChuyenThamQuan MaThamQuanNavigation { get; set; } = null!;
        public virtual SinhVien MssvNavigation { get; set; } = null!;
    }
}
