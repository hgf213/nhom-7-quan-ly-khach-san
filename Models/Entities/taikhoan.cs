using System;
using System.Collections.Generic;

namespace DatPhongKhachSan.Models.Entities;

public partial class taikhoan
{
    public int mataikhoan { get; set; }

    public string tendangnhap { get; set; } = null!;

    public string matkhau { get; set; } = null!;

    public int? manv { get; set; }

    public int? makh { get; set; }

    public int? maquyen { get; set; }

    public string? trangthai { get; set; }

    public virtual khachhang? makhNavigation { get; set; }

    public virtual nhanvien? manvNavigation { get; set; }

    public virtual quyen? maquyenNavigation { get; set; }
}
