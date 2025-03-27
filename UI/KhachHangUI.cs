using System;
using System.Collections.Generic;
using BLL;
using DTO;
using Validators;

namespace UI
{
    public static class KhachHangUI
    {
        private static KhachHangBLL bll = new KhachHangBLL();

        public static void MenuKhachHang()
        {
            while (true)
            {
                Console.Clear();
                HienThiKhachHang();
                Console.WriteLine("1. Thêm khách hàng");
                Console.WriteLine("2. Cập nhật khách hàng");
                Console.WriteLine("3. Xóa khách hàng");
                Console.WriteLine("4. Tìm khách hàng theo số điện thoại");
                Console.WriteLine("0. Quay lại");
                Console.Write("Chọn: ");
                string chon = Console.ReadLine();

                switch (chon)
                {
                    case "1": ThemKhachHang(); break;
                    case "2": CapNhatKhachHang(); break;
                    case "3": XoaKhachHang(); break;
                    case "4": TimKhachHangtheoSoDienThoai(); break;
                    case "0": return;
                    default:
                        CommonValidator.ShowErrorMessage("❌ Lựa chọn không hợp lệ! Nhấn Enter để tiếp tục.");
                        break;
                }
            }
        }

        private static void HienThiKhachHang()
        {
            Console.Clear();
            Console.WriteLine("\n=== DANH SÁCH KHÁCH HÀNG ===");
            List<KhachHangDTO> danhSach = bll.LayDanhSachKhachHang();
            if (danhSach.Count == 0)
            {
                Console.WriteLine("❌ Không có khách hàng nào.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine($"{"Mã",-5} {"Tên",-20} {"SĐT",-15} {"Địa chỉ"}");
            Console.WriteLine(new string('=', 60));
            foreach (var kh in danhSach)
            {
                Console.WriteLine($"{kh.MaKhachHang,-5} {kh.TenKhachHang,-20} {kh.SoDienThoai,-15} {kh.DiaChi}");
            }
        }

        private static void ThemKhachHang()
        {
            Console.Clear();
            Console.WriteLine("\n=== THÊM KHÁCH HÀNG ===");
            string ten = PromptString("Tên khách hàng (Nhấn ESC để hủy): ");
            if (ten == null) return;

            string sdt = PromptString("Số điện thoại (Nhấn ESC để hủy): ");
            if (sdt == null) return;

            string diaChi = PromptString("Địa chỉ (Nhấn ESC để hủy): ");
            if (diaChi == null) return;

            if (bll.ThemKhachHang(new KhachHangDTO { TenKhachHang = ten, SoDienThoai = sdt, DiaChi = diaChi }))
                CommonValidator.ShowSuccessMessage("✅ Thêm thành công!");
            else
                CommonValidator.ShowErrorMessage("❌ Thêm thất bại!");

            CommonValidator.ConfirmAndRetry(ThemKhachHang, $"❗ Bạn có muốn thêm khách hàng khác không? (Y/N): ");
        }

        private static void CapNhatKhachHang()
        {
            Console.Clear();
            HienThiKhachHang();
            Console.WriteLine("\n=== CẬP NHẬT KHÁCH HÀNG ===");
            int maKH = PromptInt("Nhập mã khách hàng (Nhấn ESC để hủy): ");
            if (maKH == -1) return;

            var kh = bll.TimKhachHangTheoMa(maKH);
            if (kh == null)
            {
                CommonValidator.ShowErrorMessage("❌ Không tìm thấy khách hàng!");
                Console.ReadLine();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"Tên hiện tại: {kh.TenKhachHang}");
            Console.WriteLine($"Số điện thoại hiện tại: {kh.SoDienThoai}");
            Console.WriteLine($"Địa chỉ hiện tại: {kh.DiaChi}");
            Console.ResetColor();

            string ten = PromptString("Tên mới (Nhấn ESC để hủy): ");
            if (ten == null) return;

            string sdt = PromptString("Số điện thoại mới (Nhấn ESC để hủy): ");
            if (sdt == null) return;

            string diaChi = PromptString("Địa chỉ mới (Nhấn ESC để hủy): ");
            if (diaChi == null) return;

            if (bll.CapNhatKhachHang(new KhachHangDTO { MaKhachHang = maKH, TenKhachHang = ten, SoDienThoai = sdt, DiaChi = diaChi }))
                CommonValidator.ShowSuccessMessage("✅ Cập nhật thành công!");
            else
                CommonValidator.ShowErrorMessage("❌ Cập nhật thất bại!");

            CommonValidator.ConfirmAndRetry(CapNhatKhachHang, $"❗ Bạn có muốn cập nhật khách hàng khác không? (Y/N): ");
        }

        private static void XoaKhachHang()
        {
            Console.Clear();
            HienThiKhachHang();
            Console.WriteLine("\n=== XÓA KHÁCH HÀNG ===");
            int maKH = PromptInt("Nhập mã khách hàng (Nhấn ESC để hủy): ");
            if (maKH == -1) return;

            var kh = bll.TimKhachHangTheoMa(maKH);
            if (kh == null)
            {
                CommonValidator.ShowErrorMessage("❌ Không tìm thấy khách hàng!");
                Console.ReadLine();
                return;
            }

            Console.WriteLine($"\nMã khách hàng: {kh.MaKhachHang}");
            Console.WriteLine($"Tên khách hàng: {kh.TenKhachHang}");
            Console.Write($"❗ Bạn có chắc muốn xóa khách hàng {kh.TenKhachHang}? (Y/N): ");
            if (Console.ReadLine()?.Trim().ToUpper() == "Y")
            {
                if (bll.XoaKhachHang(maKH))
                    CommonValidator.ShowSuccessMessage("✅ Xóa thành công!");
                else
                    CommonValidator.ShowErrorMessage("❌ Xóa thất bại!");
            }
            else
            {
                Console.WriteLine("❌ Hủy xóa!");
            }

            CommonValidator.ConfirmAndRetry(XoaKhachHang, "❗ Bạn có muốn xóa khách hàng khác không? (Y/N): ");
        }

        private static void TimKhachHangtheoSoDienThoai()
        {
            Console.Clear();
            Console.WriteLine("\n=== TÌM KHÁCH HÀNG ===");
            string sdt = PromptString("Nhập số điện thoại (Nhấn ESC để hủy): ");
            if (sdt == null) return;

            var kh = bll.TimKhachHangtheoSoDienThoai(sdt);
            if (kh == null)
            {
                CommonValidator.ShowErrorMessage("❌ Không tìm thấy khách hàng!");
                Console.ReadLine();
                return;
            }

            Console.WriteLine($"\nMã khách hàng: {kh.MaKhachHang}");
            Console.WriteLine($"Tên khách hàng: {kh.TenKhachHang}");
            Console.WriteLine($"Số điện thoại: {kh.SoDienThoai}");
            Console.WriteLine($"Địa chỉ: {kh.DiaChi}");
            Console.WriteLine($"Ngày đăng ký: {kh.NgayDangKy}");
            CommonValidator.ConfirmAndRetry(TimKhachHangtheoSoDienThoai, "❗ Bạn có muốn tìm khách hàng khác không? (Y/N): ");
        }

        // search for a customer by phone number => return customer info
        public static KhachHangDTO TimKhachHangTheoSoDienThoai(string soDienThoai)
        {
            var khachHang = bll.TimKhachHangtheoSoDienThoai(soDienThoai);
            if (khachHang == null)
            {
                CommonValidator.ShowErrorMessage("❌ Không tìm thấy khách hàng!");
                Console.ReadLine();
                return null;
            }

            return khachHang;
        }

        // search for a customer by ID => return customer info
        public static KhachHangDTO TimKhachHangTheoMa(int maKhachHang)
        {
            var khachHang = bll.TimKhachHangTheoMa(maKhachHang);
            if (khachHang == null)
            {
                CommonValidator.ShowErrorMessage("❌ Không tìm thấy khách hàng!");
                Console.ReadLine();
                return null;
            }

            return khachHang;
        }


        private static string PromptString(string message)
        {
            Console.Write(message);
            string input = "";
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine("\n❌ Đã hủy thao tác!");
                    return null;
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    if (!string.IsNullOrWhiteSpace(input))
                    {
                        Console.WriteLine();
                        return input;
                    }
                    else
                    {
                        Console.Write("\n❌ Không được để trống! Nhập lại: ");
                        input = "";
                    }
                }
                else
                {
                    input += key.KeyChar;
                    Console.Write(key.KeyChar);
                }
            }
        }

        private static int PromptInt(string message)
        {
            Console.Write(message);
            string input = "";
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine("\n❌ Đã hủy thao tác!");
                    return -1;
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    if (int.TryParse(input, out int value) && value > 0)
                    {
                        Console.WriteLine();
                        return value;
                    }
                    else
                    {
                        Console.Write("\n❌ Giá trị không hợp lệ! Nhập lại: ");
                        input = "";
                    }
                }
                else if (char.IsDigit(key.KeyChar))
                {
                    input += key.KeyChar;
                    Console.Write(key.KeyChar);
                }
                else if (key.Key == ConsoleKey.Backspace && input.Length > 0)
                {
                    input = input.Substring(0, input.Length - 1);
                    Console.Write("\b \b");
                }
            }
        }
    }
}
