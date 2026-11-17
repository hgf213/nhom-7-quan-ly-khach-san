using System;
using System.Collections.Generic;

namespace DatPhongKhachSan.Models.Entities;

public partial class hoadon
{
    public int mahoadon { get; set; }

    public int? machitietdatphong { get; set; }

    public int? manv { get; set; }

    public DateTime? ngaylap { get; set; }

    public decimal tienphong { get; set; }

    public decimal? tienphuthu { get; set; }

    public decimal? tiendichvu { get; set; }

    public decimal tiencocdatru { get; set; }

    public decimal tongtien { get; set; }

    public string? trangthai { get; set; }

    public virtual chitietdatphong? machitietdatphongNavigation { get; set; }

    public virtual nhanvien? manvNavigation { get; set; }

    public virtual ICollection<thanhtoan> thanhtoans { get; set; } = new List<thanhtoan>();
}
