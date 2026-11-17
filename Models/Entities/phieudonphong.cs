using System;
using System.Collections.Generic;

namespace DatPhongKhachSan.Models.Entities;

public partial class phieudonphong
{
    public int madonphong { get; set; }

    public int? matraphong { get; set; }

    public int? manv { get; set; }

    public DateTime? thoigianbatdau { get; set; }

    public DateTime? thoigianhoanthanh { get; set; }

    public string? trangthai { get; set; }

    public string? ghichu { get; set; }

    public virtual nhanvien? manvNavigation { get; set; }

    public virtual traphong? matraphongNavigation { get; set; }
}
