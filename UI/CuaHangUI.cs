using System;
using System.Collections.Generic;
using BLL;
using DTO;

namespace UI
{
    public static class CuaHangUI
    {
        private static CuaHangBLL bll = new CuaHangBLL();

        public static void MenuCuaHang()
        {
            while (true)
            {
                Console.WriteLine("\n=== QUẢN LÝ CỬA HÀNG ===");
                Console.WriteLine("1. Thêm cửa hàng");
                Console.WriteLine("2. Cập nhật cửa hàng");
                Console.WriteLine("3. Xóa cửa hàng");
                Console.WriteLine("4. Xem danh sách cửa hàng");
                Console.WriteLine("5. Xem thông tin một cửa hàng");
                Console.WriteLine("0. Thoát");
                Console.Write("Chọn: ");
                string? chon = Console.ReadLine();

                switch (chon)
                {
                    case "1":
                        ThemCuaHang();
                        break;
                    case "2":
                        CapNhatCuaHang();
                        break;
                    case "3":
                        XoaCuaHang();
                        break;
                    case "4":
                        XemDanhSachCuaHang();
                        break;
                    case "5":
                        XemThongTinCuaHang();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("❌ Lựa chọn không hợp lệ! Vui lòng chọn lại.");
                        break;
                }
            }
        }

        private static void ThemCuaHang()
        {
            Console.Write("Tên cửa hàng: ");
            string? ten = Console.ReadLine();
            Console.Write("Địa chỉ: ");
            string? diaChi = Console.ReadLine();
            Console.Write("Số điện thoại: ");
            string? sdt = Console.ReadLine();

            try
            {
                CuaHangDTO cuaHang = new CuaHangDTO
                {
                    TenCuaHang = ten,
                    DiaChi = diaChi,
                    SoDienThoai = sdt
                };

                if (bll.ThemCuaHang(cuaHang))
                    Console.WriteLine("✅ Thêm cửa hàng thành công!");
                else
                    Console.WriteLine("❌ Thêm cửa hàng thất bại.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi: {ex.Message}");
            }
        }

        private static void CapNhatCuaHang()
        {
            Console.Write("Nhập mã cửa hàng cần cập nhật: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("❌ Mã cửa hàng không hợp lệ.");
                return;
            }

            Console.Write("Tên cửa hàng mới: ");
            string ten = Console.ReadLine();
            Console.Write("Địa chỉ mới: ");
            string diaChi = Console.ReadLine();
            Console.Write("Số điện thoại mới: ");
            string sdt = Console.ReadLine();

            try
            {
                CuaHangDTO cuaHang = new CuaHangDTO
                {
                    CuaHangID = id,
                    TenCuaHang = ten,
                    DiaChi = diaChi,
                    SoDienThoai = sdt
                };

                if (bll.CapNhatCuaHang(cuaHang))
                    Console.WriteLine("✅ Cập nhật cửa hàng thành công!");
                else
                    Console.WriteLine("❌ Cập nhật cửa hàng thất bại.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi: {ex.Message}");
            }
        }

        private static void XoaCuaHang()
        {
            Console.Write("Nhập mã cửa hàng cần xóa: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("❌ Mã cửa hàng không hợp lệ.");
                return;
            }

            try
            {
                if (bll.XoaCuaHang(id))
                    Console.WriteLine("✅ Xóa cửa hàng thành công!");
                else
                    Console.WriteLine("❌ Xóa cửa hàng thất bại.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi: {ex.Message}");
            }
        }

        private static void XemDanhSachCuaHang()
        {
            List<CuaHangDTO> danhSach = bll.LayDanhSachCuaHang();
            if (danhSach.Count == 0)
            {
                Console.WriteLine("❌ Không có cửa hàng nào trong hệ thống.");
                return;
            }

            Console.WriteLine("\n=== DANH SÁCH CỬA HÀNG ===");
            foreach (var c in danhSach)
            {
                Console.WriteLine($"🔹 {c.CuaHangID}: {c.TenCuaHang} - {c.DiaChi} - {c.SoDienThoai}");
            }
        }

        private static void XemThongTinCuaHang()
        {
            Console.Write("Nhập mã cửa hàng cần xem: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("❌ Mã cửa hàng không hợp lệ.");
                return;
            }

            try
            {
                var cuaHang = bll.LayThongTinCuaHang(id);
                Console.WriteLine("\n=== THÔNG TIN CỬA HÀNG ===");
                Console.WriteLine($"🔹 Mã cửa hàng: {cuaHang.CuaHangID}");
                Console.WriteLine($"🔹 Tên cửa hàng: {cuaHang.TenCuaHang}");
                Console.WriteLine($"🔹 Địa chỉ: {cuaHang.DiaChi}");
                Console.WriteLine($"🔹 Số điện thoại: {cuaHang.SoDienThoai}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi: {ex.Message}");
            }
        }
    }
}
