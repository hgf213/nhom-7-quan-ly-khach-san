using System;
using System.Collections.Generic;

namespace DatPhongKhachSan.Models.Entities;

public partial class traphong
{
    public int matraphong { get; set; }

    public int? machitietdatphong { get; set; }

    public int? manv { get; set; }

    public DateTime? thoigiantra { get; set; }

    public string? ghichu { get; set; }

    public virtual chitietdatphong? machitietdatphongNavigation { get; set; }

    public virtual nhanvien? manvNavigation { get; set; }

    public virtual phieudonphong? phieudonphong { get; set; }
}
