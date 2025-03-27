using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using DTO;

namespace DAL
{
    public class SanPhamDAL
    {
        public bool ThemSanPham(SanPhamDTO sp)
        {
            string query = "INSERT INTO SanPham (TenSanPham, DonGia, SoLuongTon, MoTa) VALUES (@Ten, @Gia, @SoLuong, @MoTa)";
            MySqlParameter[] parameters = {
                new MySqlParameter("@Ten", sp.TenSanPham),
                new MySqlParameter("@Gia", sp.DonGia),
                new MySqlParameter("@SoLuong", sp.SoLuongTon),
                new MySqlParameter("@MoTa", sp.MoTa)
            };
            return HelperDB.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool CapNhatSanPham(SanPhamDTO sp)
        {
            string query = "UPDATE SanPham SET TenSanPham=@Ten, DonGia=@Gia, SoLuongTon=@SoLuong, MoTa=@MoTa WHERE MaSanPham=@Ma";
            MySqlParameter[] parameters = {
                new MySqlParameter("@Ma", sp.MaSanPham),
                new MySqlParameter("@Ten", sp.TenSanPham),
                new MySqlParameter("@Gia", sp.DonGia),
                new MySqlParameter("@SoLuong", sp.SoLuongTon),
                new MySqlParameter("@MoTa", sp.MoTa)
            };
            return HelperDB.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool XoaSanPham(int maSP)
        {
            string checkQuery = "SELECT COUNT(*) FROM ChiTietHoaDon WHERE MaSanPham=@MaSP";
            MySqlParameter[] checkParams = { new MySqlParameter("@MaSP", maSP) };
            if ((long)HelperDB.ExecuteScalar(checkQuery, checkParams) > 0)
                return false;

            string deleteQuery = "DELETE FROM SanPham WHERE MaSanPham=@MaSP";
            MySqlParameter[] deleteParams = { new MySqlParameter("@MaSP", maSP) };
            return HelperDB.ExecuteNonQuery(deleteQuery, deleteParams) > 0;
        }

        public List<SanPhamDTO> LayDanhSachSanPham()
        {
            string query = "SELECT * FROM SanPham ORDER BY NgayTao DESC";
            DataTable dt = HelperDB.ExecuteQuery(query);
            List<SanPhamDTO> list = new List<SanPhamDTO>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new SanPhamDTO
                {
                    MaSanPham = Convert.ToInt32(row["MaSanPham"]),
                    TenSanPham = row["TenSanPham"].ToString(),
                    DonGia = Convert.ToDecimal(row["DonGia"]),
                    SoLuongTon = Convert.ToInt32(row["SoLuongTon"]),
                    MoTa = row["MoTa"].ToString(),
                    NgayTao = Convert.ToDateTime(row["NgayTao"])
                });
            }
            return list;
        }

        public SanPhamDTO KiemTraTonKho(int maSP)
        {
            string query = "SELECT * FROM SanPham WHERE MaSanPham=@MaSP";
            MySqlParameter[] parameters = { new MySqlParameter("@MaSP", maSP) };
            DataTable dt = HelperDB.ExecuteQuery(query, parameters);

            if (dt.Rows.Count == 0) return null;
            DataRow row = dt.Rows[0];

            return new SanPhamDTO
            {
                MaSanPham = Convert.ToInt32(row["MaSanPham"]),
                TenSanPham = row["TenSanPham"].ToString(),
                DonGia = Convert.ToDecimal(row["DonGia"]),
                SoLuongTon = Convert.ToInt32(row["SoLuongTon"]),
                MoTa = row["MoTa"].ToString(),
                NgayTao = Convert.ToDateTime(row["NgayTao"])
            };
        }
    }
}
