using System;

namespace Validators
{
    public static class HoaDonValidator
    {
        public static void KiemTraHoaDon(int maKhachHang, DateTime ngayLap)
        {
            CommonValidator.KiemTraSoDuong(maKhachHang, "Mã khách hàng");
            CommonValidator.KiemTraNgay(ngayLap, "Ngày lập hóa đơn");
        }
    }
}