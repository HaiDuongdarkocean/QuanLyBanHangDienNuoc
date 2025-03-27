using System;
using System.Collections.Generic;
using System.Data;
using DTO;
using MySql.Data.MySqlClient;

namespace DAL
{
    public class ThanhToanDAL
    {
        // 1. Thêm thanh toán
        public bool ThemThanhToan(int maHoaDon, string phuongThuc, decimal soTien)
        {
            string query = @"
                INSERT INTO ThanhToan (MaHoaDon, PhuongThuc, SoTien)
                VALUES (@MaHoaDon, @PhuongThuc, @SoTien)";

            return HelperDB.ExecuteNonQuery(query, new MySqlParameter[]
            {
                new MySqlParameter("@MaHoaDon", maHoaDon),
                new MySqlParameter("@PhuongThuc", phuongThuc),
                new MySqlParameter("@SoTien", soTien)
            }) > 0;
        }

        // 2. Lấy danh sách thanh toán của hóa đơn
        public List<ThanhToanDTO> LayThongTinThanhToan(int maHoaDon)
        {
            string query = "SELECT * FROM ThanhToan WHERE MaHoaDon = @MaHoaDon";
            DataTable dt = HelperDB.ExecuteQuery(query, new MySqlParameter[]
            {
                new MySqlParameter("@MaHoaDon", maHoaDon)
            });

            List<ThanhToanDTO> ds = new List<ThanhToanDTO>();
            foreach (DataRow row in dt.Rows)
            {
                ds.Add(new ThanhToanDTO
                {
                    MaThanhToan = Convert.ToInt32(row["MaThanhToan"]),
                    MaHoaDon = Convert.ToInt32(row["MaHoaDon"]),
                    PhuongThuc = row["PhuongThuc"].ToString(),
                    SoTien = Convert.ToDecimal(row["SoTien"])
                });
            }
            return ds;
        }
        
    }
}
