using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using DTO;

namespace DAL
{
    public class KhachHangDAL
    {
        public bool ThemKhachHang(KhachHangDTO khachHang)
        {
            string query = "INSERT INTO KhachHang (TenKhachHang, SoDienThoai, DiaChi) VALUES (@TenKhachHang, @SoDienThoai, @DiaChi)";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@TenKhachHang", khachHang.TenKhachHang),
                new MySqlParameter("@SoDienThoai", khachHang.SoDienThoai),
                new MySqlParameter("@DiaChi", khachHang.DiaChi)
            };
            return HelperDB.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool CapNhatKhachHang(KhachHangDTO khachHang)
        {
            string query = "UPDATE KhachHang SET TenKhachHang = @TenKhachHang, SoDienThoai = @SoDienThoai, DiaChi = @DiaChi WHERE MaKhachHang = @MaKhachHang";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@MaKhachHang", khachHang.MaKhachHang),
                new MySqlParameter("@TenKhachHang", khachHang.TenKhachHang),
                new MySqlParameter("@SoDienThoai", khachHang.SoDienThoai),
                new MySqlParameter("@DiaChi", khachHang.DiaChi)
            };
            return HelperDB.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool XoaKhachHang(int maKhachHang)
        {
            string query = "DELETE FROM KhachHang WHERE MaKhachHang = @MaKhachHang";
            MySqlParameter[] parameters = { new MySqlParameter("@MaKhachHang", maKhachHang) };
            return HelperDB.ExecuteNonQuery(query, parameters) > 0;
        }

        public List<KhachHangDTO> LayDanhSachKhachHang()
        {
            string query = "SELECT * FROM KhachHang";
            DataTable dt = HelperDB.ExecuteQuery(query);

            List<KhachHangDTO> danhSach = new List<KhachHangDTO>();
            foreach (DataRow row in dt.Rows)
            {
                danhSach.Add(new KhachHangDTO
                {
                    MaKhachHang = Convert.ToInt32(row["MaKhachHang"]),
                    TenKhachHang = row["TenKhachHang"].ToString(),
                    SoDienThoai = row["SoDienThoai"].ToString(),
                    DiaChi = row["DiaChi"].ToString(),
                    NgayDangKy = Convert.ToDateTime(row["NgayDangKy"])
                });
            }
            return danhSach;
        }

        public KhachHangDTO TimKhachHangtheoSoDienThoai(string soDienThoai)
        {
            string query = "SELECT * FROM KhachHang WHERE SoDienThoai = @SoDienThoai";
            MySqlParameter[] parameters = { new MySqlParameter("@SoDienThoai", soDienThoai) };

            DataTable dt = HelperDB.ExecuteQuery(query, parameters);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new KhachHangDTO
                {
                    MaKhachHang = Convert.ToInt32(row["MaKhachHang"]),
                    TenKhachHang = row["TenKhachHang"].ToString(),
                    SoDienThoai = row["SoDienThoai"].ToString(),
                    DiaChi = row["DiaChi"].ToString(),
                    NgayDangKy = Convert.ToDateTime(row["NgayDangKy"])
                };
            }
            return null;
        }

        public KhachHangDTO TimKhachHangTheoMa(int maKhachHang)
        {
            string query = "SELECT * FROM KhachHang WHERE MaKhachHang = @MaKhachHang";
            MySqlParameter[] parameters = { new MySqlParameter("@MaKhachHang", maKhachHang) };

            DataTable dt = HelperDB.ExecuteQuery(query, parameters);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new KhachHangDTO
                {
                    MaKhachHang = Convert.ToInt32(row["MaKhachHang"]),
                    TenKhachHang = row["TenKhachHang"].ToString(),
                    SoDienThoai = row["SoDienThoai"].ToString(),
                    DiaChi = row["DiaChi"].ToString(),
                    NgayDangKy = Convert.ToDateTime(row["NgayDangKy"])
                };
            }
            return null;
        }
    }
}
