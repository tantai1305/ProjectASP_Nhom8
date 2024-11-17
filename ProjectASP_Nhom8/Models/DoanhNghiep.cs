using System;
using System.Collections.Generic;

namespace ProjectASP_Nhom8.Models
{
    public partial class DoanhNghiep
    {
        public DoanhNghiep()
        {
            ChuyenThamQuans = new HashSet<ChuyenThamQuan>();
        }

        public string MaDn { get; set; } = null!;
        public string TenDn { get; set; } = null!;
        public string DiaChi { get; set; } = null!;

        public virtual ICollection<ChuyenThamQuan> ChuyenThamQuans { get; set; }
    }
}
