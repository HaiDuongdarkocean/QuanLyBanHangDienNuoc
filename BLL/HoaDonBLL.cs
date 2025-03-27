using System;
using System.Collections.Generic;
using DAL;
using DTO;

namespace BLL
{
    public class HoaDonBLL
    {
        private HoaDonDAL dal = new HoaDonDAL();

        public int TaoHoaDon(string soDienThoai)
        {
            if (string.IsNullOrWhiteSpace(soDienThoai))
                throw new ArgumentException("Mã khách hàng không hợp lệ!");
            return dal.TaoHoaDon(soDienThoai);
        }

        public bool XoaHoaDon(int maHoaDon)
        {
            if (maHoaDon <= 0)
                throw new ArgumentException("Mã hóa đơn không hợp lệ!");

            return dal.XoaHoaDon(maHoaDon);
        }

        public List<HoaDonDTO> LayDanhSachHoaDon() => dal.LayDanhSachHoaDon();

        public HoaDonDTO TimHoaDon(int maHoaDon)
        {
            if (maHoaDon <= 0)
                throw new ArgumentException("Mã hóa đơn không hợp lệ!");

            return dal.TimHoaDon(maHoaDon);
        }

        public bool CapNhatTongTienHoaDon(int maHoaDon) => dal.CapNhatTongTienHoaDon(maHoaDon);

        public bool CapNhatTrangThaiHoaDon(int maHoaDon, string trangThai)
        {
            if (string.IsNullOrWhiteSpace(trangThai))
                throw new ArgumentException("Trạng thái không hợp lệ!");

            return dal.CapNhatTrangThaiHoaDon(maHoaDon, trangThai);
        }
    }
}
