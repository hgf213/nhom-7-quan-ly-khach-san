using System;
using System.Collections.Generic;

namespace DatPhongKhachSan.Models.Entities;

public partial class phong
{
    public int maphong { get; set; }

    public int? maloaiphong { get; set; }

    public string tenphong { get; set; } = null!;

    public int? tang { get; set; }

    public string? trangthai { get; set; }

    public virtual ICollection<chitietdatphong> chitietdatphongs { get; set; } = new List<chitietdatphong>();

    public virtual loaiphong? maloaiphongNavigation { get; set; }
}
