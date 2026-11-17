using System;
using System.Collections.Generic;

namespace DatPhongKhachSan.Models.Entities;

public partial class nhanvien
{
    public int manv { get; set; }

    public string hoten { get; set; } = null!;

    public DateOnly? ngaysinh { get; set; }

    public string? gioitinh { get; set; }

    public string sodienthoai { get; set; } = null!;

    public string? email { get; set; }

    public string? diachi { get; set; }

    public string? trangthai { get; set; }

    public virtual ICollection<chitietsudungdichvu> chitietsudungdichvus { get; set; } = new List<chitietsudungdichvu>();

    public virtual ICollection<hoadon> hoadons { get; set; } = new List<hoadon>();

    public virtual ICollection<nhanphong> nhanphongs { get; set; } = new List<nhanphong>();

    public virtual ICollection<phieudonphong> phieudonphongs { get; set; } = new List<phieudonphong>();

    public virtual ICollection<taikhoan> taikhoans { get; set; } = new List<taikhoan>();

    public virtual ICollection<traphong> traphongs { get; set; } = new List<traphong>();
}
