
namespace DatPhongKhachSan.DTOs;

public class PhongResponse
{
    public int MaPhong { get; set; }

    public string TenPhong { get; set; } = string.Empty;

    public int? Tang { get; set; }

    public int MaLoaiPhong { get; set; }

    public string TenLoaiPhong { get; set; } = string.Empty;

    public decimal GiaPhong { get; set; }

    public int SoNguoiLonTieuChuan { get; set; }

    public decimal PhuThuNguoiLon { get; set; }

    public decimal PhuThuTreEm { get; set; }
}
