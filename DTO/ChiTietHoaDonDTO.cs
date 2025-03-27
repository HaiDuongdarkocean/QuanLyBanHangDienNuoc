namespace DTO
{
    public class ChiTietHoaDonDTO
    {
        public int MaChiTiet { get; set; }
        public int MaHoaDon { get; set; }
        public int MaSanPham { get; set; }
        public string? TenSanPham { get; set; } // Add this line
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }
    }
}
