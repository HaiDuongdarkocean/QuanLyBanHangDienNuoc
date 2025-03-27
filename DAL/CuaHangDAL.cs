using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using DTO;

namespace DAL
{
    public class CuaHangDAL
    {
        public bool ThemCuaHang(CuaHangDTO cuaHang)
        {
            string query = "INSERT INTO CuaHang (TenCuaHang, DiaChi, SoDienThoai) VALUES (@TenCuaHang, @DiaChi, @SoDienThoai)";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@TenCuaHang", cuaHang.TenCuaHang),
                new MySqlParameter("@DiaChi", cuaHang.DiaChi),
                new MySqlParameter("@SoDienThoai", cuaHang.SoDienThoai)
            };
            return HelperDB.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool CapNhatCuaHang(CuaHangDTO cuaHang)
        {
            string query = "UPDATE CuaHang SET TenCuaHang = @TenCuaHang, DiaChi = @DiaChi, SoDienThoai = @SoDienThoai WHERE CuaHangID = @CuaHangID";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@CuaHangID", cuaHang.CuaHangID),
                new MySqlParameter("@TenCuaHang", cuaHang.TenCuaHang),
                new MySqlParameter("@DiaChi", cuaHang.DiaChi),
                new MySqlParameter("@SoDienThoai", cuaHang.SoDienThoai)
            };
            return HelperDB.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool XoaCuaHang(int cuaHangID)
        {
            string query = "DELETE FROM CuaHang WHERE CuaHangID = @CuaHangID";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@CuaHangID", cuaHangID)
            };
            return HelperDB.ExecuteNonQuery(query, parameters) > 0;
        }

        public List<CuaHangDTO> LayDanhSachCuaHang()
        {
            string query = "SELECT * FROM CuaHang";
            DataTable dt = HelperDB.ExecuteQuery(query);

            List<CuaHangDTO> danhSach = new List<CuaHangDTO>();
            foreach (DataRow row in dt.Rows)
            {
                danhSach.Add(new CuaHangDTO
                {
                    CuaHangID = Convert.ToInt32(row["CuaHangID"]),
                    TenCuaHang = row["TenCuaHang"].ToString(),
                    DiaChi = row["DiaChi"].ToString(),
                    SoDienThoai = row["SoDienThoai"].ToString()
                });
            }
            return danhSach;
        }

        public CuaHangDTO LayThongTinCuaHang(int cuaHangID)
        {
            string query = "SELECT * FROM CuaHang WHERE CuaHangID = @CuaHangID";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@CuaHangID", cuaHangID)
            };

            DataTable dt = HelperDB.ExecuteQuery(query, parameters);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new CuaHangDTO
                {
                    CuaHangID = Convert.ToInt32(row["CuaHangID"]),
                    TenCuaHang = row["TenCuaHang"].ToString(),
                    DiaChi = row["DiaChi"].ToString(),
                    SoDienThoai = row["SoDienThoai"].ToString()
                };
            }
            return null;
        }
    }
}
