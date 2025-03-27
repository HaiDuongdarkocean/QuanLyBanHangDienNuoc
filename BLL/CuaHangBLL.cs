using System;
using System.Collections.Generic;
using DAL;
using DTO;

namespace BLL
{
    public class CuaHangBLL
    {
        private CuaHangDAL dal = new CuaHangDAL();

        public bool ThemCuaHang(CuaHangDTO cuaHang)
        {
            if (string.IsNullOrWhiteSpace(cuaHang.TenCuaHang) || string.IsNullOrWhiteSpace(cuaHang.DiaChi))
                throw new ArgumentException("Tên và địa chỉ cửa hàng không được để trống.");
            if (cuaHang.SoDienThoai.Length < 10 || cuaHang.SoDienThoai.Length > 15)
                throw new ArgumentException("Số điện thoại không hợp lệ.");

            return dal.ThemCuaHang(cuaHang);
        }

        public bool CapNhatCuaHang(CuaHangDTO cuaHang) => dal.CapNhatCuaHang(cuaHang);

        public bool XoaCuaHang(int cuaHangID) => dal.XoaCuaHang(cuaHangID);

        public CuaHangDTO LayThongTinCuaHang(int cuaHangID)
        {
            var cuaHang = dal.LayThongTinCuaHang(cuaHangID);
            if (cuaHang == null)
                throw new ArgumentException("Cửa hàng không tồn tại.");
            return cuaHang;
        }
        
        public List<CuaHangDTO> LayDanhSachCuaHang() => dal.LayDanhSachCuaHang();
    }
}
