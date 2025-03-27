using System;
using System.Text.RegularExpressions;

namespace Validators
{
    public static class KhachHangValidator
    {
        public static void KiemTraKhachHang(string ten, string diaChi, string soDienThoai)
        {
            CommonValidator.KiemTraChuoiRong(ten, "Tên khách hàng");
            CommonValidator.KiemTraChuoiRong(diaChi, "Địa chỉ");
            KiemTraSoDienThoai(soDienThoai);
        }

        private static void KiemTraSoDienThoai(string soDienThoai)
        {
            if (!Regex.IsMatch(soDienThoai, @"^(0[1-9][0-9]{8,9})$"))
            {
                throw new ArgumentException("Số điện thoại không hợp lệ.");
            }
        }
    }
}
