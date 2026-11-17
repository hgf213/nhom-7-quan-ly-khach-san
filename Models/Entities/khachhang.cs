using System;
using System.Collections.Generic;

namespace DatPhongKhachSan.Models.Entities;

public partial class khachhang
{
    public int makh { get; set; }

    public string hoten { get; set; } = null!;

    public DateOnly? ngaysinh { get; set; }

    public string? gioitinh { get; set; }

    public string sodienthoai { get; set; } = null!;

    public string? email { get; set; }

    public string? diachi { get; set; }

    public virtual ICollection<datphong> datphongs { get; set; } = new List<datphong>();

    public virtual ICollection<taikhoan> taikhoans { get; set; } = new List<taikhoan>();
}
