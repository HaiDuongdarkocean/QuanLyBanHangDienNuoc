using System;
using System.Collections.Generic;
using DAL;
using DTO;
using Validators;

namespace BLL
{
    public class SanPhamBLL
    {
        private SanPhamDAL dal = new SanPhamDAL();

        public bool ThemSanPham(SanPhamDTO sp)
        {
            if (string.IsNullOrWhiteSpace(sp.TenSanPham) || sp.DonGia <= 0 || sp.SoLuongTon < 0)
                throw new ArgumentException("❌ Dữ liệu không hợp lệ!");

            return dal.ThemSanPham(sp);
        }

        public bool CapNhatSanPham(SanPhamDTO sp)
        {
            if (string.IsNullOrWhiteSpace(sp.TenSanPham) || sp.DonGia <= 0 || sp.SoLuongTon < 0)
                throw new ArgumentException("❌ Dữ liệu không hợp lệ!");

            return dal.CapNhatSanPham(sp);
        }

        public bool XoaSanPham(int maSP)
        {
            if (!dal.XoaSanPham(maSP))
                throw new ArgumentException("❌ Không thể xóa sản phẩm vì có trong hóa đơn!");
            return true;
        }

        public List<SanPhamDTO> LayDanhSachSanPham() => dal.LayDanhSachSanPham();
        public SanPhamDTO KiemTraTonKho(int maSP) => dal.KiemTraTonKho(maSP);
    }
}
