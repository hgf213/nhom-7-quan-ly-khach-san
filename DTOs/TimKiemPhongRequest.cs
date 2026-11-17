
namespace DatPhongKhachSan.DTOs;

public class TimKiemPhongRequest
{
    public DateOnly NgayNhan { get; set; }

    public DateOnly NgayTra { get; set; }

    public int SoNguoiLon { get; set; }

    public int SoTreEm { get; set; } = 0;

    public int? MaLoaiPhong { get; set; }

    public decimal? GiaTu { get; set; }

    public decimal? GiaDen { get; set; }
}
