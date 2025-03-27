using System;
using System.Collections.Generic;
using BLL;
using DTO;

namespace UI
{
    public static class SanPhamUI
    {
        private static SanPhamBLL sanPhamBll = new SanPhamBLL();

        public static void MenuSanPham()
        {
            while (true)
            {
                Console.Clear();
                HienThiSanPham();

                Console.WriteLine("\n=== QUẢN LÝ SẢN PHẨM ===");
                Console.WriteLine("1. Thêm sản phẩm");
                Console.WriteLine("2. Cập nhật sản phẩm");
                Console.WriteLine("3. Xóa sản phẩm");
                Console.WriteLine("4. Kiểm tra tồn kho");
                Console.WriteLine("0. Quay lại");
                Console.Write("Chọn chức năng: ");
                string? chon = Console.ReadLine()?.Trim();

                switch (chon)
                {
                    case "1": ThemSanPham(); break;
                    case "2": CapNhatSanPham(); break;
                    case "3": XoaSanPham(); break;
                    case "4": KiemTraTonKho(); break;
                    case "0": return;
                    default:
                        Console.WriteLine("❌ Lựa chọn không hợp lệ! Nhấn Enter để tiếp tục.");
                        Console.ReadLine();
                        break;
                }
            }
        }

        private static void HienThiSanPham()
        {
            Console.Clear();
            Console.WriteLine("\n=== DANH SÁCH SẢN PHẨM ===");

            List<SanPhamDTO> danhSach = sanPhamBll.LayDanhSachSanPham();
            if (danhSach.Count == 0)
            {
                Console.WriteLine("❌ Không có sản phẩm nào trong kho.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine($"{"Mã",-5} {"Tên sản phẩm",-25} {"Giá",-10} {"Số lượng",-10} {"Mô tả"}");
            Console.WriteLine(new string('=', 70));
            foreach (var sp in danhSach)
            {
                string tenSanPham = sp.TenSanPham.Length > 25 ? sp.TenSanPham.Substring(0, 22) + "..." : sp.TenSanPham;
                Console.WriteLine($"{sp.MaSanPham,-5} {tenSanPham,-25} {sp.DonGia,-10} {sp.SoLuongTon,-10} {sp.MoTa}");
            }
        }

        private static void ThemSanPham()
        {
            Console.Clear();HienThiSanPham();
            Console.WriteLine("\n=== THÊM SẢN PHẨM ===");

            Console.Write("Tên sản phẩm: ");
            string? ten = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(ten)) return;

            Console.Write("Giá bán: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal gia) || gia < 0) return;

            Console.Write("Số lượng tồn: ");
            if (!int.TryParse(Console.ReadLine(), out int sl) || sl < 0) return;

            Console.Write("Mô tả: ");
            string? mota = Console.ReadLine()?.Trim();

            Console.Write($"❗ Bạn có chắc muốn thêm sản phẩm {ten}? (Y/N): ");
            if (Console.ReadLine()?.Trim().ToUpper() != "Y") return;

            sanPhamBll.ThemSanPham(new SanPhamDTO { TenSanPham = ten, DonGia = gia, SoLuongTon = sl, MoTa = mota });
            Console.WriteLine("✅ Thêm sản phẩm thành công!");
            Console.ReadLine();
            HienThiSanPham();
        }

        private static void CapNhatSanPham()
        {
            Console.Clear();
            HienThiSanPham();
            Console.WriteLine("\n=== CẬP NHẬT SẢN PHẨM ===");

            Console.Write("Nhập mã sản phẩm cần cập nhật: ");
            if (!int.TryParse(Console.ReadLine(), out int maSP)) return;

            var sp = sanPhamBll.KiemTraTonKho(maSP);
            if (sp == null)
            {
                Console.WriteLine("❌ Không tìm thấy sản phẩm!");
                Console.ReadLine();
                return;
            }

            Console.WriteLine($"Tên hiện tại: {sp.TenSanPham}");
            Console.WriteLine($"Giá hiện tại: {sp.DonGia}");
            Console.WriteLine($"Số lượng hiện tại: {sp.SoLuongTon}");
            Console.WriteLine($"Mô tả hiện tại: {sp.MoTa}");

            Console.Write("Tên mới (Nhấn Enter để giữ nguyên): ");
            string? ten = Console.ReadLine()?.Trim();
            Console.Write("Giá mới (Nhấn Enter để giữ nguyên): ");
            string? giaStr = Console.ReadLine();
            Console.Write("Số lượng mới (Nhấn Enter để giữ nguyên): ");
            string? slStr = Console.ReadLine();
            Console.Write("Mô tả mới (Nhấn Enter để giữ nguyên): ");
            string? mota = Console.ReadLine()?.Trim();

            Console.Write($"❗ Bạn có chắc muốn cập nhật sản phẩm {sp.TenSanPham}? (Y/N): ");
            if (Console.ReadLine()?.Trim().ToUpper() != "Y") return;

            sp.TenSanPham = !string.IsNullOrWhiteSpace(ten) ? ten : sp.TenSanPham;
            if (decimal.TryParse(giaStr, out decimal gia) && gia >= 0) sp.DonGia = gia;
            if (int.TryParse(slStr, out int sl) && sl >= 0) sp.SoLuongTon = sl;
            sp.MoTa = !string.IsNullOrWhiteSpace(mota) ? mota : sp.MoTa;

            sanPhamBll.CapNhatSanPham(sp);
            Console.WriteLine("✅ Cập nhật sản phẩm thành công!");
            Console.ReadLine();
            HienThiSanPham();
        }

        private static void XoaSanPham()
        {
            Console.Clear();
            HienThiSanPham();
            Console.WriteLine("\n=== XÓA SẢN PHẨM ===");

            Console.Write("Nhập mã sản phẩm cần xóa: ");
            if (!int.TryParse(Console.ReadLine(), out int maSP)) return;

            var sp = sanPhamBll.KiemTraTonKho(maSP);
            if (sp == null)
            {
                Console.WriteLine("❌ Không tìm thấy sản phẩm!");
                Console.ReadLine();
                return;
            }

            Console.WriteLine($"Mã sản phẩm: {sp.MaSanPham}");
            Console.WriteLine($"Tên sản phẩm: {sp.TenSanPham}");
            Console.Write($"❗ Bạn có chắc chắn muốn xóa sản phẩm {sp.TenSanPham}? (Y/N): ");
            if (Console.ReadLine()?.Trim().ToUpper() != "Y") return;

            if (sanPhamBll.XoaSanPham(maSP))
                Console.WriteLine("✅ Xóa sản phẩm thành công!");
            else
                Console.WriteLine("❌ Xóa thất bại! Sản phẩm đã có trong hóa đơn.");

            Console.ReadLine();
            HienThiSanPham();
        }

        private static void KiemTraTonKho()
        {
            Console.Clear();
            Console.WriteLine("\n=== KIỂM TRA TỒN KHO ===");

            Console.Write("Nhập mã sản phẩm: ");
            if (!int.TryParse(Console.ReadLine(), out int maSP)) return;

            var sp = sanPhamBll.KiemTraTonKho(maSP);
            if (sp == null)
            {
                Console.WriteLine("❌ Không tìm thấy sản phẩm!");
                Console.ReadLine();
                return;
            }

            Console.WriteLine($"Mã sản phẩm: {sp.MaSanPham}");
            Console.WriteLine($"Tên sản phẩm: {sp.TenSanPham}");
            Console.WriteLine($"Số lượng tồn: {sp.SoLuongTon}");
            Console.ReadLine();
        }
    }
}
