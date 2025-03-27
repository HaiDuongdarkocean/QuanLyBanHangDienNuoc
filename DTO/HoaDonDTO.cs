using System;

namespace DTO
{
    public class HoaDonDTO
    {
        public int MaHoaDon { get; set; }
        public int MaKhachHang { get; set; }
        public string NgayTao { get; set; }
        public string? TenKhachHang { get; set; }
        public decimal TongTien { get; set; }
        public decimal NoCu { get; set; }
        public decimal TongNo { get; set; }
        public decimal ConLai { get; set; }
        public string TrangThai { get; set; }
    }
}
