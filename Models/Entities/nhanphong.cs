using System;
using System.Collections.Generic;

namespace DatPhongKhachSan.Models.Entities;

public partial class nhanphong
{
    public int manhanphong { get; set; }

    public int? machitietdatphong { get; set; }

    public int? manv { get; set; }

    public DateTime? thoigiannhan { get; set; }

    public string? ghichu { get; set; }

    public virtual ICollection<chitietsudungdichvu> chitietsudungdichvus { get; set; } = new List<chitietsudungdichvu>();

    public virtual chitietdatphong? machitietdatphongNavigation { get; set; }

    public virtual nhanvien? manvNavigation { get; set; }
}
