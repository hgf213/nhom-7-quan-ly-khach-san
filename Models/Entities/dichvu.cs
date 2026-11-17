using System;
using System.Collections.Generic;

namespace DatPhongKhachSan.Models.Entities;

public partial class dichvu
{
    public int madv { get; set; }

    public string tendv { get; set; } = null!;

    public string? loaidv { get; set; }

    public string? donvitinh { get; set; }

    public decimal dongia { get; set; }

    public string? trangthai { get; set; }

    public virtual ICollection<chitietsudungdichvu> chitietsudungdichvus { get; set; } = new List<chitietsudungdichvu>();
}
