using System;
using BLL;

namespace UI
{
    public static class ThanhToanUI
    {
        private static ThanhToanBLL thanhToanBLL = new ThanhToanBLL();

        public static void ThanhToanHoaDon()
        {
            Console.Clear();
            Console.Write("Nhập mã hóa đơn cần thanh toán: ");
            int maHoaDon = int.Parse(Console.ReadLine());

            Console.Write("Nhập số tiền thanh toán: ");
            decimal soTien = decimal.Parse(Console.ReadLine());

            Console.Write("Chọn phương thức thanh toán (Tiền mặt / Chuyển khoản): ");
            string phuongThuc = Console.ReadLine();

            thanhToanBLL.ThemThanhToan(maHoaDon, phuongThuc, soTien);
            Console.WriteLine("✅ Thanh toán thành công!");
            Console.ReadLine();
        }
    }
}
