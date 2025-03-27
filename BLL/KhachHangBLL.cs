using System;
using System.Collections.Generic;
using DAL;
using DTO;

namespace BLL
{
    public class KhachHangBLL
    {
        private KhachHangDAL dal = new KhachHangDAL();

        public bool ThemKhachHang(KhachHangDTO khachHang)
        {
            if (string.IsNullOrWhiteSpace(khachHang.TenKhachHang) || string.IsNullOrWhiteSpace(khachHang.DiaChi))
                throw new ArgumentException("Tên và địa chỉ không được để trống.");
            if (khachHang.SoDienThoai.Length < 10 || khachHang.SoDienThoai.Length > 15)
                throw new ArgumentException("Số điện thoại không hợp lệ.");

            return dal.ThemKhachHang(khachHang);
        }

        public bool CapNhatKhachHang(KhachHangDTO khachHang) => dal.CapNhatKhachHang(khachHang);

        public bool XoaKhachHang(int maKhachHang) => dal.XoaKhachHang(maKhachHang);

        public List<KhachHangDTO> LayDanhSachKhachHang() => dal.LayDanhSachKhachHang();

        public KhachHangDTO TimKhachHangtheoSoDienThoai(string soDienThoai)
        {
            var khachHang = dal.TimKhachHangtheoSoDienThoai(soDienThoai);
            if (khachHang == null)
                throw new ArgumentException("Không tìm thấy khách hàng.");
            return khachHang;
        }
        public KhachHangDTO TimKhachHangTheoMa(int maKhachHang)
        {
            var khachHang = dal.TimKhachHangTheoMa(maKhachHang);
            if (khachHang == null)
                throw new ArgumentException("Không tìm thấy khách hàng.");
            return khachHang;
        }
    }
}
