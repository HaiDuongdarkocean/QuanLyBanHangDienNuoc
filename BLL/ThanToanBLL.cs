using System;
using System.Collections.Generic;
using DAL;
using DTO;

namespace BLL
{
    public class ThanhToanBLL
    {
        private ThanhToanDAL thanhToanDAL = new ThanhToanDAL();
        private HoaDonDAL hoaDonDAL = new HoaDonDAL();

        // 1. Thêm thanh toán
        public bool ThemThanhToan(int maHoaDon, string phuongThuc, decimal soTien)
        {
            if (!hoaDonDAL.HoaDonTonTai(maHoaDon))
                throw new ArgumentException("❌ Hóa đơn không tồn tại!");

            if (!phuongThuc.Equals("Tiền mặt") && !phuongThuc.Equals("Chuyển khoản"))
                throw new ArgumentException("❌ Phương thức thanh toán không hợp lệ!");

            decimal tongNo = hoaDonDAL.LayTongNo(maHoaDon);
            if (soTien > tongNo)
                throw new InvalidOperationException("❌ Số tiền thanh toán vượt quá số tiền cần trả!");

            if (!ConfirmAction($"Thanh toán {soTien} bằng {phuongThuc} cho hóa đơn ID: {maHoaDon}?"))
                return false;

            return thanhToanDAL.ThemThanhToan(maHoaDon, phuongThuc, soTien);
        }

        // 2. Lấy danh sách thanh toán của hóa đơn
        public List<ThanhToanDTO> LayThongTinThanhToan(int maHoaDon)
        {
            if (!hoaDonDAL.HoaDonTonTai(maHoaDon))
                throw new ArgumentException("❌ Hóa đơn không tồn tại!");

            return thanhToanDAL.LayThongTinThanhToan(maHoaDon);
        }

        // 🔹 Hàm xác nhận trước khi thay đổi dữ liệu
        private bool ConfirmAction(string message)
        {
            Console.Write($"❗ {message} (Y/N): ");
            return Console.ReadLine()?.Trim().ToUpper() == "Y";
        }
    }
}
