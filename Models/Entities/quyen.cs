using System;
using System.Collections.Generic;

namespace DatPhongKhachSan.Models.Entities;

public partial class quyen
{
    public int maquyen { get; set; }

    public string tenquyen { get; set; } = null!;

    public string? mota { get; set; }

    public virtual ICollection<taikhoan> taikhoans { get; set; } = new List<taikhoan>();
}
