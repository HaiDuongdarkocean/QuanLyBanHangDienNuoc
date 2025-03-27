using System;
using BLL;
using DTO;
using System.Collections.Generic;
using Validators;
using Microsoft.VisualBasic;

namespace UI
{
    public static class ChiTietHoaDonUI
    {
        private static ChiTietHoaDonBLL chiTietBLL = new ChiTietHoaDonBLL();
        private static SanPhamBLL sanPhamBLL = new SanPhamBLL();
        public static void QuanLySanPhamHoaDon(int maHoaDon)
        {
            while (true)
            {
                Console.Clear();
                // display the details of the invoice
                HienThiChiTietHoaDon(maHoaDon);
                Console.WriteLine("\n1. Thêm sản phẩm");
                Console.WriteLine("2. Cập nhật số lượng sản phẩm");
                Console.WriteLine("3. Xóa sản phẩm khỏi hóa đơn");
                Console.WriteLine("0. Quay lại");
                Console.Write("Chọn: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ThemSanPham(maHoaDon); break;
                    case "2": CapNhatSoLuongSanPham(maHoaDon); break;
                    case "3": XoaSanPham(maHoaDon); break;
                    case "0": return;
                    default:
                        Console.WriteLine("❌ Lựa chọn không hợp lệ! Nhấn Enter để tiếp tục.");
                        Console.ReadLine();
                        break;
                }
            }
        }
        private static void ThemSanPham(int maHoaDon)
        {
            Console.Clear();
            Console.WriteLine($"=== THÊM SẢN PHẨM VÀO HÓA ĐƠN {maHoaDon} ===");

            // display the list of products
            List<SanPhamDTO> danhSachSanPham = sanPhamBLL.LayDanhSachSanPham();
            Console.WriteLine($"{"Mã",-5} {"Tên sản phẩm",-25} {"Giá",-10} {"Số lượng",-10} {"Mô tả"}");
            Console.WriteLine(new string('=', 70));
            foreach (var sp in danhSachSanPham)
            {
                string tenSanPham = sp.TenSanPham.Length > 25 ? sp.TenSanPham.Substring(0, 22) + "..." : sp.TenSanPham;
                Console.WriteLine($"{sp.MaSanPham,-5} {tenSanPham,-25} {sp.DonGia,-10} {sp.SoLuongTon,-10} {sp.MoTa}");
            }

            // add products to the invoice
            List<(int maSanPham, int soLuong)> sanPhamList = new List<(int, int)>();

            while (true)
            {
                Console.Write("Nhập mã sản phẩm (Nhấn Enter để hoàn tất): ");
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) break;

                if (!int.TryParse(input, out int maSanPham))
                {
                    Console.WriteLine("❌ Mã sản phẩm không hợp lệ!");
                    continue;
                }

                Console.Write("Nhập số lượng: ");
                if (!int.TryParse(Console.ReadLine(), out int soLuong) || soLuong <= 0)
                {
                    Console.WriteLine("❌ Số lượng không hợp lệ!");
                    continue;
                }

                sanPhamList.Add((maSanPham, soLuong));
            }

            if (sanPhamList.Count == 0)
            {
                Console.WriteLine("❌ Không có sản phẩm nào được thêm.");
                return;
            }

            if (chiTietBLL.ThemChiTietHoaDon(maHoaDon, sanPhamList))
                Console.WriteLine("✅ Thêm sản phẩm thành công!");
            else
                Console.WriteLine("❌ Không thể thêm sản phẩm!");

            Console.ReadLine();
        }


        private static void CapNhatSoLuongSanPham(int maHoaDon)
        {
            HienThiChiTietHoaDon(maHoaDon);
            int maChiTietHoaDon, soLuongMoi;
            // Nhập mã chi tiết hóa đơn
            Console.Write("Nhập mã chi tiết hóa đơn cần cập nhật số lượng: ");
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input) || !int.TryParse(input, out maChiTietHoaDon))
            {
                Console.WriteLine("❌ Mã chi tiết hóa đơn không hợp lệ!");
                return;
            }
            // Nhập số lượng mới
            Console.Write("\nNhập số lượng mới: ");
            if (!int.TryParse(Console.ReadLine(), out soLuongMoi) || soLuongMoi <= 0)
            {
                Console.WriteLine("❌ Số lượng không hợp lệ!");
                return;
            }
            if (chiTietBLL.CapNhatSoLuongSanPham(maChiTietHoaDon, soLuongMoi))
                Console.WriteLine("✅ Cập nhật số lượng sản phẩm thành công!");
            else
                Console.WriteLine("❌ Không thể cập nhật số lượng sản phẩm!");
            Console.ReadLine();
        }

        /// <summary>
        /// Nhập danh sách mã chi tiết sản phẩm cần thao tác
        /// </summary>
        private static List<int> NhapDanhSachMaChiTiet()
        {
            Console.Write("\nNhập danh sách mã chi tiết sản phẩm cần thao tác (cách nhau bằng dấu phẩy, Enter để dừng): ");
            string input = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(input)) return new List<int>(); // Thoát nếu không nhập gì

            List<int> maChiTietCanThaoTac = new List<int>();
            string[] parts = input.Split(',');

            foreach (string part in parts)
            {
                if (int.TryParse(part.Trim(), out int maChiTiet))
                    maChiTietCanThaoTac.Add(maChiTiet);
                else
                    Console.WriteLine($"⚠️ Mã chi tiết không hợp lệ: {part.Trim()}");
            }

            return maChiTietCanThaoTac;
        }

        private static void XoaSanPham(int maHoaDon)
        {
            Console.Clear();

            while (true)
            {
                var danhSachChiTietHoaDon = HienThiChiTietHoaDon(maHoaDon);

                Console.Write("\nNhập danh sách mã chi tiết sản phẩm cần xóa (cách nhau bằng dấu phẩy, Enter để dừng): ");
                string input = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(input)) break; // Thoát nếu không nhập gì

                List<int> maChiTietCanXoa = new List<int>();
                string[] parts = input.Split(',');

                foreach (string part in parts)
                {
                    if (int.TryParse(part.Trim(), out int maChiTiet))
                        maChiTietCanXoa.Add(maChiTiet);
                    else
                        Console.WriteLine($"⚠️ Mã chi tiết không hợp lệ: {part.Trim()}");
                }

                var sanPhamXoa = danhSachChiTietHoaDon
                    .Where(ct => maChiTietCanXoa.Contains(ct.MaChiTiet))
                    .ToList();

                if (sanPhamXoa.Count == 0)
                {
                    Console.WriteLine("❌ Không tìm thấy sản phẩm nào hợp lệ để xóa.");
                    continue;
                }

                // Xác nhận trước khi xóa
                Console.WriteLine("\n❗ Xác nhận xóa sản phẩm:");
                foreach (var sp in sanPhamXoa)
                {
                    Console.WriteLine($"   - {sp.TenSanPham}");
                }

                if (!CommonValidator.ConfirmAction("Bạn có chắc muốn xóa?")) continue;

                bool xoaThanhCong = true;
                foreach (var sp in sanPhamXoa)
                {
                    if (!chiTietBLL.XoaChiTietHoaDon(sp.MaChiTiet))
                    {
                        Console.WriteLine($"❌ Không thể xóa sản phẩm {sp.TenSanPham}!");
                        xoaThanhCong = false;
                    }
                }

                if (xoaThanhCong)
                {
                    Console.WriteLine("✅ Xóa sản phẩm thành công!");
                    Console.ReadLine();
                }
            }
        }

        private static List<ChiTietHoaDonDTO>? HienThiChiTietHoaDon(int maHoaDon)
        {
            var danhSachChiTietHoaDon = chiTietBLL.LayChiTietHoaDon(maHoaDon);
            if (danhSachChiTietHoaDon.Count == 0)
            {
                Console.WriteLine("❌ Không có sản phẩm nào.");
                return null;
            }

            // Hiển thị danh sách sản phẩm trong hóa đơn
            Console.WriteLine($"\n=== DANH SÁCH SẢN PHẨM TRONG HÓA ĐƠN {maHoaDon} ===");
            Console.WriteLine("╔════════╦══════════════════════════════╦══════════╦═══════════════╦═══════════════╗");
            Console.WriteLine("║ Mã CT  ║ Sản phẩm                     ║ Số lượng ║ Đơn giá       ║ Thành tiền    ║");
            Console.WriteLine("╠════════╬══════════════════════════════╬══════════╬═══════════════╣═══════════════╣");

            foreach (var chiTiet in danhSachChiTietHoaDon)
            {
                string tenSanPham = chiTiet.TenSanPham.Length > 25 ? chiTiet.TenSanPham.Substring(0, 22) + "..." : chiTiet.TenSanPham;
                string donGia = $"{chiTiet.DonGia:N0} VND"; // Format số tiền
                string thanhTien = $"{chiTiet.ThanhTien:N0} VND"; // Format số tiền
                Console.WriteLine($"║ {chiTiet.MaChiTiet,-6} ║ {tenSanPham,-28} ║ {chiTiet.SoLuong,8} ║ {donGia,13} ║ {thanhTien,13} ║");
            }
            Console.WriteLine("╚════════╩══════════════════════════════╩══════════╩═══════════════╩═══════════════╝");

            return danhSachChiTietHoaDon;
        }

    }
}
