using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectASP_Nhom8.Models
{
    public partial class DoanhNghiep
    {
        public DoanhNghiep()
        {
            ChuyenThamQuans = new HashSet<ChuyenThamQuan>();
        }
        [Display(Name = "Mã")]
        public string MaDn { get; set; }
        [Display(Name = "Tên Doanh Nghiệp")]
        public string TenDn { get; set; }

        [Display(Name = "Địa Chỉ")]
        public string DiaChi { get; set; }

        public virtual ICollection<ChuyenThamQuan> ChuyenThamQuans { get; set; }
    }
}
