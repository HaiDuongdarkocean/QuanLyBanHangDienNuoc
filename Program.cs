using System;
using System.Text;
using UI;

namespace BTLQLBHDN
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ResetColor();
            while (true)
            {
                Console.InputEncoding = Encoding.UTF8;
                Console.OutputEncoding = Encoding.UTF8;

                Console.Clear();
                Console.WriteLine("===== CHƯƠNG TRÌNH QUẢN LÝ BÁN HÀNG ĐIỆN NƯỚC =====");
                Console.WriteLine("1. Quản lý Cửa hàng");
                Console.WriteLine("2. Quản lý Sản phẩm");
                Console.WriteLine("3. Quản lý Khách hàng");
                Console.WriteLine("4. Quản lý Hóa đơn");
                Console.WriteLine("5. Xuất báo cáo");
                Console.WriteLine("0. Thoát");
                Console.Write("Chọn chức năng: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        CuaHangUI.MenuCuaHang();
                        break;
                    case "2":
                        SanPhamUI.MenuSanPham();
                        break;
                    case "3":
                        KhachHangUI.MenuKhachHang();
                        break;
                    case "4":
                        HoaDonUI.MenuHoaDon();
                        break;
                    case "5":
                        Console.WriteLine("Đang phát triển...");
                        // BaoCaoUI.MenuBaoCao();
                        break;
                    case "0":
                        Console.WriteLine("Đã thoát chương trình.");
                        return;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ! Nhấn Enter để thử lại.");
                        Console.ReadLine();
                        break;
                }
            }
        }
    }
}