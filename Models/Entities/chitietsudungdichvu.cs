using System;
using System.Collections.Generic;

namespace DatPhongKhachSan.Models.Entities;

public partial class chitietsudungdichvu
{
    public int machitietdv { get; set; }

    public int? manhanphong { get; set; }

    public int? madv { get; set; }

    public int? manv { get; set; }

    public int soluong { get; set; }

    public decimal dongia { get; set; }

    public DateTime? thoigiansudung { get; set; }

    public decimal thanhtien { get; set; }

    public virtual dichvu? madvNavigation { get; set; }

    public virtual nhanphong? manhanphongNavigation { get; set; }

    public virtual nhanvien? manvNavigation { get; set; }
}
