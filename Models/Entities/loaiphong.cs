using System;
using System.Collections.Generic;

namespace DatPhongKhachSan.Models.Entities;

public partial class loaiphong
{
    public int maloaiphong { get; set; }

    public string tenloaiphong { get; set; } = null!;

    public string? mota { get; set; }

    public decimal giaphong { get; set; }

    public int songuoilontieuchuan { get; set; }

    public decimal? phuthunguoilon { get; set; }

    public decimal? phuthutreem { get; set; }

    public string? trangthai { get; set; }

    public virtual ICollection<phong> phongs { get; set; } = new List<phong>();
}
