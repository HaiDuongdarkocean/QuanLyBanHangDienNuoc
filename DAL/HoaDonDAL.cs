using System;
using System.Collections.Generic;
using System.Data;
using DTO;
using MySql.Data.MySqlClient;

namespace DAL
{
    public class HoaDonDAL
    {
        // 1. Tạo hóa đơn
        // Tạo hóa đơn dựa trên số điện thoại khách hàng
        public int TaoHoaDon(string soDienThoai)
        {
            // Kiểm tra khách hàng có tồn tại không
            string queryKhachHang = "SELECT MaKhachHang FROM KhachHang WHERE SoDienThoai = @SoDienThoai";
            object maKhachHangObj = HelperDB.ExecuteScalar(queryKhachHang, new MySqlParameter[]
            {
                new MySqlParameter("@SoDienThoai", soDienThoai)
            });

            if (maKhachHangObj == null)
            {
                Console.WriteLine("❌ Khách hàng không tồn tại!");
                return -1;
            }

            int maKhachHang = Convert.ToInt32(maKhachHangObj);

            // Tạo hóa đơn mới
            string queryTaoHoaDon = "INSERT INTO HoaDon (MaKhachHang) VALUES (@MaKhachHang)";
            if (HelperDB.ExecuteNonQuery(queryTaoHoaDon, new MySqlParameter[]
            {
                new MySqlParameter("@MaKhachHang", maKhachHang)
            }) > 0)
            {
                // Lấy mã hóa đơn vừa tạo
                return Convert.ToInt32(HelperDB.ExecuteScalar("SELECT LAST_INSERT_ID()"));
            }

            return -1; // Trả về -1 nếu tạo thất bại
        }


        // 2. Xóa hóa đơn (Chỉ khi chưa thanh toán)
        public bool XoaHoaDon(int maHoaDon)
        {
            if (HelperDB.RecordExists("SELECT COUNT(*) FROM ThanhToan WHERE MaHoaDon = @MaHoaDon",
                new MySqlParameter[] { new MySqlParameter("@MaHoaDon", maHoaDon) }))
            {
                Console.WriteLine("❌ Hóa đơn đã có thanh toán, không thể xóa!");
                return false;
            }

            string query = "DELETE FROM HoaDon WHERE MaHoaDon = @MaHoaDon";
            return HelperDB.ExecuteNonQuery(query, new MySqlParameter[]
            {
                new MySqlParameter("@MaHoaDon", maHoaDon)
            }) > 0;
        }

        public List<HoaDonDTO> LayDanhSachHoaDon()
        {
            string query = @"
                SELECT hd.MaHoaDon, hd.MaKhachHang, kh.TenKhachHang, hd.NgayTao, hd.TongTien, hd.NoCu, hd.TongNo, hd.ConLai, hd.TrangThai
                FROM HoaDon hd
                INNER JOIN KhachHang kh ON hd.MaKhachHang = kh.MaKhachHang";

            DataTable dt = HelperDB.ExecuteQuery(query);
            List<HoaDonDTO> ds = new List<HoaDonDTO>();

            foreach (DataRow row in dt.Rows)
            {
                ds.Add(new HoaDonDTO
                {
                    MaHoaDon = Convert.ToInt32(row["MaHoaDon"]),
                    MaKhachHang = Convert.ToInt32(row["MaKhachHang"]),
                    TenKhachHang = row["TenKhachHang"].ToString(),
                    NgayTao = Convert.ToDateTime(row["NgayTao"]).ToString(new System.Globalization.CultureInfo("vi-VN")),
                    TongTien = Convert.ToDecimal(row["TongTien"]),
                    NoCu = Convert.ToDecimal(row["NoCu"]),
                    TongNo = Convert.ToDecimal(row["TongNo"]),
                    ConLai = Convert.ToDecimal(row["ConLai"]),
                    TrangThai = row["TrangThai"].ToString()
                });
            }
            return ds;
        }


        // 4. Tìm hóa đơn theo mã
        public HoaDonDTO TimHoaDon(int maHoaDon)
        {
            string query = "SELECT * FROM HoaDon WHERE MaHoaDon = @MaHoaDon";
            DataTable dt = HelperDB.ExecuteQuery(query, new MySqlParameter[]
            {
                new MySqlParameter("@MaHoaDon", maHoaDon)
            });

            if (dt.Rows.Count == 0) return null;

            DataRow row = dt.Rows[0];
            return new HoaDonDTO
            {
                MaHoaDon = Convert.ToInt32(row["MaHoaDon"]),
                MaKhachHang = Convert.ToInt32(row["MaKhachHang"]),
                TongTien = Convert.ToDecimal(row["TongTien"]),
                TrangThai = row["TrangThai"].ToString()
            };
        }

        // 5. Cập nhật tổng tiền hóa đơn
        public bool CapNhatTongTienHoaDon(int maHoaDon)
        {
            string query = @"
                UPDATE HoaDon 
                SET TongTien = (SELECT SUM(ThanhTien) FROM ChiTietHoaDon WHERE MaHoaDon = @MaHoaDon) 
                WHERE MaHoaDon = @MaHoaDon";

            return HelperDB.ExecuteNonQuery(query, new MySqlParameter[]
            {
                new MySqlParameter("@MaHoaDon", maHoaDon)
            }) > 0;
        }

        //6. Cập nhật trạng thái hóa đơn
        public bool CapNhatTrangThaiHoaDon(int maHoaDon, string trangThai)
        {
            // Kiểm tra xem hóa đơn có tồn tại không
            if (!HelperDB.RecordExists("HoaDon", "MaHoaDon", maHoaDon))
                throw new ArgumentException("❌ Hóa đơn không tồn tại!");

            // Kiểm tra giá trị hợp lệ của trạng thái
            List<string> trangThaiHopLe = new List<string> { "Chưa thanh toán", "Còn nợ", "Đã thanh toán" };
            if (!trangThaiHopLe.Contains(trangThai))
                throw new ArgumentException("❌ Trạng thái không hợp lệ!");

            string query = "UPDATE HoaDon SET TrangThai = @TrangThai WHERE MaHoaDon = @MaHoaDon";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@TrangThai", trangThai),
                new MySqlParameter("@MaHoaDon", maHoaDon)
            };

            return HelperDB.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool HoaDonTonTai(int maHoaDon)
        {
            string query = "SELECT COUNT(*) FROM HoaDon WHERE MaHoaDon = @MaHoaDon";
            MySqlParameter[] parameters = { new MySqlParameter("@MaHoaDon", maHoaDon) };

            int count = Convert.ToInt32(HelperDB.ExecuteScalar(query, parameters));
            return count > 0;
        }

        public decimal LayTongNo(int maHoaDon)
        {
            string query = "SELECT TongNo FROM HoaDon WHERE MaHoaDon = @MaHoaDon";
            object result = HelperDB.ExecuteScalar(query, new MySqlParameter[]
            {
        new MySqlParameter("@MaHoaDon", maHoaDon)
            });

            return result != null ? Convert.ToDecimal(result) : 0;
        }

    }
}
