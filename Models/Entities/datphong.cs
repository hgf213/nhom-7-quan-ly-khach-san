using System;
using System.Collections.Generic;

namespace DatPhongKhachSan.Models.Entities;

public partial class datphong
{
    public int madatphong { get; set; }

    public int? makh { get; set; }

    public DateTime? ngaytao { get; set; }

    public DateTime hethanthanhtoan { get; set; }

    public decimal tongtiendukien { get; set; }

    public decimal tongtiencoc { get; set; }

    public string? trangthai { get; set; }

    public string? lydohuy { get; set; }

    public virtual ICollection<chitietdatphong> chitietdatphongs { get; set; } = new List<chitietdatphong>();

    public virtual khachhang? makhNavigation { get; set; }
}
