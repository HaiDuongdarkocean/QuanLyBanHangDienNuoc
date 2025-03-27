using System;
using BLL;
using DTO;
using System.Collections.Generic;
using Validators;

namespace UI
{
    public static class HoaDonUI
    {
        private static HoaDonBLL hoaDonBLL = new HoaDonBLL();
        private static ChiTietHoaDonBLL chiTietBLL = new ChiTietHoaDonBLL();
        private static ThanhToanBLL thanhToanBLL = new ThanhToanBLL();
        private static KhachHangBLL khachHangBLL = new KhachHangBLL();

        public static void MenuHoaDon()
        {
            while (true)
            {
                // clear the console
                Console.Clear();

                // display the list of invoices
                XemDanhSachHoaDon();

                // display the menu
                Console.WriteLine("\n=== QUẢN LÝ HÓA ĐƠN ===");
                Console.WriteLine("1. Thêm hóa đơn mới");
                Console.WriteLine("3. Xác nhận và in hóa đơn");
                Console.WriteLine("4. Thanh toán hóa đơn");
                Console.WriteLine("5. Xem danh sách hóa đơn");
                Console.WriteLine("0. Quay lại");

                // prompt the user to choose an option
                Console.Write("Chọn: ");

                // read the user's choice
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": ChiTietHoaDonUI.QuanLySanPhamHoaDon(TaoHoaDon()); break;
                    case "3": XacNhanInHoaDon(); break;
                    case "4": ThanhToanUI.ThanhToanHoaDon(); break;
                    case "5": XemDanhSachHoaDon(); break;
                    case "0": return;
                    default:
                        Console.WriteLine("❌ Lựa chọn không hợp lệ! Nhấn Enter để tiếp tục.");
                        Console.ReadLine();
                        break;
                }
            }
        }

        public static int TaoHoaDon()
        {
            Console.Clear();
            Console.WriteLine("\n=== TẠO HÓA ĐƠN MỚI ===");

            string soDienThoai = CommonValidator.PromptString("Nhập số điện thoại (Nhấn ESC để hủy): ");
            if (soDienThoai == null) return -1; // ESC to cancel

            // Validate customer
            var khachHang = khachHangBLL.TimKhachHangtheoSoDienThoai(soDienThoai);
            if (khachHang == null)
            {
                Console.WriteLine("❌ Khách hàng không tồn tại!");
                Console.ReadLine();
                return -1;
            }

            // Display customer info
            Console.WriteLine($"\n👤 Khách hàng: {khachHang.TenKhachHang}");
            Console.WriteLine($"📞 Số điện thoại: {khachHang.SoDienThoai}");
            Console.WriteLine($"🏠 Địa chỉ: {khachHang.DiaChi}");

            // Confirm action
            if (!CommonValidator.ConfirmAction($"Xác nhận thông tin khách hàng {khachHang.TenKhachHang}?"))
            {
                TaoHoaDon();
                Console.WriteLine("🔄 Vui lòng nhập lại thông tin!");
            }
            else
            {
                Console.WriteLine("✅ Thông tin khách hàng đã được xác nhận!");
            }

            // Create invoice
            int maHoaDon = hoaDonBLL.TaoHoaDon(khachHang.SoDienThoai);
            if (maHoaDon == -1)
            {
                Console.WriteLine("❌ Không thể tạo hóa đơn!");
                return -1;
            }

            Console.WriteLine($"✅ Hóa đơn #{maHoaDon} đã được tạo thành công!");
            return maHoaDon;
        }

        private static void XacNhanInHoaDon()
        {
            Console.Clear();
            Console.WriteLine("\n=== XÁC NHẬN VÀ IN HÓA ĐƠN ===");

            int maHoaDon = NhapMaHoaDon();
            if (maHoaDon == -1) return;

            var chiTietHoaDon = chiTietBLL.LayChiTietHoaDon(maHoaDon);
            if (chiTietHoaDon.Count == 0)
            {
                Console.WriteLine("❌ Hóa đơn chưa có sản phẩm, không thể xác nhận!");
                Console.ReadLine();
                return;
            }

            var hoaDon = hoaDonBLL.TimHoaDon(maHoaDon);
            if (hoaDon == null)
            {
                Console.WriteLine("❌ Không tìm thấy hóa đơn!");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("\n--- HÓA ĐƠN CHI TIẾT ---");
            Console.WriteLine($"Mã Hóa Đơn: {hoaDon.MaHoaDon}");
            Console.WriteLine($"Ngày Tạo: {hoaDon.NgayTao}");
            Console.WriteLine($"Tổng Tiền: {hoaDon.TongTien}");
            Console.WriteLine($"Trạng Thái: {hoaDon.TrangThai}");
            Console.WriteLine("------------------------");

            Console.Write("\n❗ Bạn có chắc chắn xác nhận hóa đơn này? (Y/N): ");
            if (Console.ReadLine()?.Trim().ToUpper() == "Y")
            {
                hoaDonBLL.CapNhatTrangThaiHoaDon(maHoaDon, "Chưa thanh toán");
                Console.WriteLine("✅ Hóa đơn đã được xác nhận!");
            }
            else
            {
                Console.WriteLine("❌ Hủy xác nhận!");
            }

            Console.ReadLine();
        }

        private static void XemDanhSachHoaDon()
        {
            Console.Clear();
            Console.WriteLine("\n=== DANH SÁCH HÓA ĐƠN ===\n");

            var danhSach = hoaDonBLL.LayDanhSachHoaDon();
            if (danhSach.Count == 0)
            {
                Console.WriteLine("❌ Không có hóa đơn nào.");
                Console.ReadLine();
                return;
            }

            // Header
            Console.WriteLine("╔════════╦════════════════════╦══════════════════════╦═══════════════╦═══════════════╦═══════════════╦═══════════════╦═════════════════╗");
            Console.WriteLine("║ Mã HD  ║ Khách Hàng         ║ Ngày Tạo             ║ Tổng Tiền     ║ Nợ Cũ         ║ Tổng Nợ       ║ Còn Lại       ║ Trạng Thái      ║");
            Console.WriteLine("╠════════╬════════════════════╬══════════════════════╬═══════════════╬═══════════════╬═══════════════╬═══════════════╬═════════════════╣");

            // Content
            foreach (var hd in danhSach)
            {
                string tenKhachHang = hd.TenKhachHang.Length > 18 ? hd.TenKhachHang.Substring(0, 15) + "..." : hd.TenKhachHang;
                string ngayTao = hd.NgayTao;
                string tongTien = $"{hd.TongTien:N0} VND";
                string noCu = $"{hd.NoCu:N0} VND";
                string tongNo = $"{hd.TongNo:N0} VND";
                string conLai = $"{hd.ConLai:N0} VND";
                string trangThai = hd.TrangThai;

                Console.WriteLine($"║ {hd.MaHoaDon,-6} ║ {tenKhachHang,-18} ║ {ngayTao,-20} ║ {tongTien,13} ║ {noCu,13} ║ {tongNo,13} ║ {conLai,13} ║ {trangThai,-14} ║");
            }

            // Footer
            Console.WriteLine("╚════════╩════════════════════╩══════════════════════╩═══════════════╩═══════════════╩═══════════════╩═══════════════╩═════════════════╝");
            Console.ReadLine();

            // display the menu
                Console.WriteLine("1. Thêm hóa đơn mới");
                Console.WriteLine("0. Quay lại");

                // prompt the user to choose an option
                Console.Write("Chọn: ");

                // read the user's choice
                string? choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": ChiTietHoaDonUI.QuanLySanPhamHoaDon(TaoHoaDon()); break;
                    case "0": return;
                    default:
                        Console.WriteLine("❌ Lựa chọn không hợp lệ! Nhấn Enter để tiếp tục.");
                        Console.ReadLine();
                        break;
                }
        }



        private static int NhapMaHoaDon()
        {
            Console.Write("Nhập mã hóa đơn: ");
            return int.TryParse(Console.ReadLine(), out int maHoaDon) ? maHoaDon : -1;
        }
    }
}
