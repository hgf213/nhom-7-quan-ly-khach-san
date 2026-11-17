using System;
using System.Collections.Generic;

namespace DatPhongKhachSan.Models.Entities;

public partial class chitietdatphong
{
    public int machitietdatphong { get; set; }

    public int? madatphong { get; set; }

    public int? maphong { get; set; }

    public DateOnly ngaynhandukien { get; set; }

    public DateOnly ngaytradukien { get; set; }

    public int songuoilon { get; set; }

    public int? sotreem { get; set; }

    public decimal dongiaphong { get; set; }

    public decimal? tienphuthu { get; set; }

    public decimal tiencoc { get; set; }

    public string? trangthai { get; set; }

    public virtual hoadon? hoadon { get; set; }

    public virtual datphong? madatphongNavigation { get; set; }

    public virtual phong? maphongNavigation { get; set; }

    public virtual nhanphong? nhanphong { get; set; }

    public virtual ICollection<thanhtoan> thanhtoans { get; set; } = new List<thanhtoan>();

    public virtual traphong? traphong { get; set; }
}
