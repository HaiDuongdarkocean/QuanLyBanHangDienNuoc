using System;
using System.Collections.Generic;
using DAL;
using DTO;
using Validators;

namespace BLL
{
    public class ChiTietHoaDonBLL
    {
        private ChiTietHoaDonDAL chiTietHoaDonDal = new ChiTietHoaDonDAL();

        private HoaDonDAL hoaDonDAL = new HoaDonDAL();

        public bool ThemChiTietHoaDon(int maHoaDon, List<(int maSanPham, int soLuong)> sanPhamList)
        {
            if (!hoaDonDAL.HoaDonTonTai(maHoaDon))
                throw new ArgumentException("Hóa đơn không tồn tại!");

            foreach (var (maSanPham, soLuong) in sanPhamList)
            {
                if (!chiTietHoaDonDal.KiemTraTonKho(maSanPham, soLuong))
                    throw new InvalidOperationException($"Số lượng sản phẩm ID {maSanPham} không đủ trong kho!");
            }

            if (!CommonValidator.ConfirmAction($"Thêm {sanPhamList.Count} sản phẩm vào hóa đơn {maHoaDon}?"))
                return false;

            List<ChiTietHoaDonDTO> danhSachThem = chiTietHoaDonDal.ThemChiTietHoaDon(maHoaDon, sanPhamList);

            return danhSachThem.Count > 0; // Trả về true nếu có ít nhất một sản phẩm thêm thành công
        }

        public bool XoaChiTietHoaDon(int maChiTietHoaDon)
        {
            if (!chiTietHoaDonDal.ChiTietHoaDonTonTai(maChiTietHoaDon))
                throw new ArgumentException("Chi tiết hóa đơn không tồn tại!");

            // if (!ConfirmAction($"Xóa chi tiết hóa đơn ID: {maChiTietHoaDon}?"))
            //     return false;

            return chiTietHoaDonDal.XoaChiTietHoaDon(maChiTietHoaDon);
        }

        public List<ChiTietHoaDonDTO> LayChiTietHoaDon(int maHoaDon)
        {
            if (!hoaDonDAL.HoaDonTonTai(maHoaDon))
                throw new ArgumentException("Hóa đơn không tồn tại!");

            return chiTietHoaDonDal.LayChiTietHoaDon(maHoaDon);
        }

        public bool CapNhatSoLuongSanPham(int maChiTiet, int soLuongMoi)
        {
            if (!chiTietHoaDonDal.ChiTietHoaDonTonTai(maChiTiet))
                throw new ArgumentException("Chi tiết hóa đơn không tồn tại!");

            if (!ConfirmAction($"Cập nhật số lượng mới {soLuongMoi} cho chi tiết hóa đơn ID: {maChiTiet}?"))
                return false;

            return chiTietHoaDonDal.CapNhatSoLuongSanPham(maChiTiet, soLuongMoi);
        }

        private bool ConfirmAction(string message)
        {
            Console.Write($"{message} (Y/N): ");
            return Console.ReadLine()?.Trim().ToUpper() == "Y";
        }
    }
}
