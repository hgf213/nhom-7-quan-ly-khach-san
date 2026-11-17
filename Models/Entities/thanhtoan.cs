using System;
using System.Collections.Generic;

namespace DatPhongKhachSan.Models.Entities;

public partial class thanhtoan
{
    public int mathanhtoan { get; set; }

    public int? machitietdatphong { get; set; }

    public int? mahoadon { get; set; }

    public DateTime? ngaythanhtoan { get; set; }

    public decimal sotien { get; set; }

    public string loaigiaodich { get; set; } = null!;

    public string phuongthuc { get; set; } = null!;

    public string? magiaodich { get; set; }

    public string? trangthai { get; set; }

    public virtual chitietdatphong? machitietdatphongNavigation { get; set; }

    public virtual hoadon? mahoadonNavigation { get; set; }
}
