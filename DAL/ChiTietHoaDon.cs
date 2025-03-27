using System;
using System.Collections.Generic;
using System.Data;
using DTO;
using MySql.Data.MySqlClient;

namespace DAL
{
    public class ChiTietHoaDonDAL
    {
        HoaDonDAL hoaDonDAL = new HoaDonDAL();
        // 1. Thêm chi tiết hóa đơn (có thể thêm nhiều sản phẩm vào chi tiết hóa đơn)
        public List<ChiTietHoaDonDTO> ThemChiTietHoaDon(int maHoaDon, List<(int maSanPham, int soLuong)> sanPhamList)
        {
            List<ChiTietHoaDonDTO> danhSachThemThanhCong = new List<ChiTietHoaDonDTO>();

            using (var conn = HelperDB.GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (var (maSanPham, soLuong) in sanPhamList)
                        {
                            // 🔹 Lấy giá sản phẩm từ bảng SanPham
                            string giaQuery = "SELECT DonGia FROM SanPham WHERE MaSanPham = @MaSanPham";
                            object donGiaObj = HelperDB.ExecuteScalar(giaQuery, new MySqlParameter[] { new MySqlParameter("@MaSanPham", maSanPham) });

                            if (donGiaObj == null)
                                throw new ArgumentException($"Sản phẩm ID {maSanPham} không tồn tại!");

                            decimal donGia = Convert.ToDecimal(donGiaObj);

                            // 🔹 Thêm sản phẩm vào ChiTietHoaDon
                            string insertQuery = @"
                                INSERT INTO ChiTietHoaDon (MaHoaDon, MaSanPham, SoLuong, DonGia) 
                                VALUES (@MaHoaDon, @MaSanPham, @SoLuong, @DonGia)";

                            int result = HelperDB.ExecuteNonQuery(insertQuery, new MySqlParameter[]
                            {
                                new MySqlParameter("@MaHoaDon", maHoaDon),
                                new MySqlParameter("@MaSanPham", maSanPham),
                                new MySqlParameter("@SoLuong", soLuong),
                                new MySqlParameter("@DonGia", donGia)
                            });

                            if (result > 0)
                            {
                                danhSachThemThanhCong.Add(new ChiTietHoaDonDTO
                                {
                                    MaHoaDon = maHoaDon,
                                    MaSanPham = maSanPham,
                                    SoLuong = soLuong,
                                    DonGia = donGia,
                                    ThanhTien = soLuong * donGia
                                });
                            }
                        }

                        // 🔹 Cập nhật tổng tiền hóa đơn
                        string updateHoaDon = @"
                            UPDATE HoaDon 
                            SET TongTien = (SELECT SUM(ThanhTien) FROM ChiTietHoaDon WHERE MaHoaDon = @MaHoaDon)
                            WHERE MaHoaDon = @MaHoaDon";

                        HelperDB.ExecuteNonQuery(updateHoaDon, new MySqlParameter[] { new MySqlParameter("@MaHoaDon", maHoaDon) });

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine("❌ Lỗi khi thêm chi tiết hóa đơn: " + ex.Message);
                        return new List<ChiTietHoaDonDTO>(); // Trả về danh sách rỗng nếu lỗi
                    }
                }
            }

            return danhSachThemThanhCong; // ✅ Trả về danh sách sản phẩm đã thêm
        }

        // 2. Xóa chi tiết hóa đơn
        public bool XoaChiTietHoaDon(int maChiTiet)
        {
            string query = "DELETE FROM ChiTietHoaDon WHERE MaChiTiet = @MaChiTiet";
            return HelperDB.ExecuteNonQuery(query, new MySqlParameter[]
            {
                new MySqlParameter("@MaChiTiet", maChiTiet)
            }) > 0;
        }

        // 3. Lấy danh sách chi tiết hóa đơn (có tên sản phẩm & đơn giá)
        public List<ChiTietHoaDonDTO> LayChiTietHoaDon(int maHoaDon)
        {
            string query = @"
                SELECT 
                    cthd.MaChiTiet, cthd.MaHoaDon, cthd.MaSanPham, 
                    sp.TenSanPham, sp.DonGia, 
                    cthd.SoLuong, cthd.ThanhTien 
                FROM ChiTietHoaDon cthd
                JOIN SanPham sp ON cthd.MaSanPham = sp.MaSanPham
                WHERE cthd.MaHoaDon = @MaHoaDon";

            DataTable dt = HelperDB.ExecuteQuery(query,
                new MySqlParameter[]{ new MySqlParameter("@MaHoaDon", maHoaDon) });

            List<ChiTietHoaDonDTO> ds = new List<ChiTietHoaDonDTO>();
            foreach (DataRow row in dt.Rows)
            {
                ds.Add(new ChiTietHoaDonDTO
                {
                    MaChiTiet = Convert.ToInt32(row["MaChiTiet"]),
                    MaHoaDon = Convert.ToInt32(row["MaHoaDon"]),
                    MaSanPham = Convert.ToInt32(row["MaSanPham"]),
                    TenSanPham = row["TenSanPham"].ToString(),
                    DonGia = Convert.ToDecimal(row["DonGia"]),
                    SoLuong = Convert.ToInt32(row["SoLuong"]),
                    ThanhTien = Convert.ToDecimal(row["ThanhTien"])
                });
            }
            return ds;
        }

        public bool CapNhatSoLuongSanPham(int maChiTiet, int soLuongMoi)
        {
            // Kiểm tra xem chi tiết hóa đơn có tồn tại không
            if (!HelperDB.RecordExists("ChiTietHoaDon", "MaChiTiet", maChiTiet))
                throw new ArgumentException("❌ Chi tiết hóa đơn không tồn tại!");

            // Kiểm tra số lượng hợp lệ
            if (soLuongMoi <= 0)
                throw new ArgumentException("❌ Số lượng sản phẩm không hợp lệ!");

            // Lấy mã sản phẩm, số lượng cũ và mã hóa đơn
            string querySanPham = "SELECT MaSanPham, SoLuong, MaHoaDon FROM ChiTietHoaDon WHERE MaChiTiet = @MaChiTiet";
            DataTable dt = HelperDB.ExecuteQuery(querySanPham, new MySqlParameter[] { new MySqlParameter("@MaChiTiet", maChiTiet) });
            if (dt.Rows.Count == 0) return false;

            int maSanPham = Convert.ToInt32(dt.Rows[0]["MaSanPham"]);
            int soLuongCu = Convert.ToInt32(dt.Rows[0]["SoLuong"]);
            int maHoaDon = Convert.ToInt32(dt.Rows[0]["MaHoaDon"]);

            // Kiểm tra tồn kho
            string queryTonKho = "SELECT SoLuongTon FROM SanPham WHERE MaSanPham = @MaSanPham";
            int soLuongTon = Convert.ToInt32(HelperDB.ExecuteScalar(queryTonKho, new MySqlParameter[] { new MySqlParameter("@MaSanPham", maSanPham) }));

            if (soLuongMoi > soLuongCu) // Nếu tăng số lượng
            {
                int chenhLech = soLuongMoi - soLuongCu;
                if (chenhLech > soLuongTon)
                    throw new ArgumentException("❌ Không đủ hàng trong kho!");

                string queryUpdateKho = "UPDATE SanPham SET SoLuongTon = SoLuongTon - @ChenhLech WHERE MaSanPham = @MaSanPham";
                HelperDB.ExecuteNonQuery(queryUpdateKho, new MySqlParameter[] { new MySqlParameter("@ChenhLech", chenhLech), new MySqlParameter("@MaSanPham", maSanPham) });
            }
            else if (soLuongMoi < soLuongCu) // Nếu giảm số lượng
            {
                int chenhLech = soLuongCu - soLuongMoi;
                string queryUpdateKho = "UPDATE SanPham SET SoLuongTon = SoLuongTon + @ChenhLech WHERE MaSanPham = @MaSanPham";
                HelperDB.ExecuteNonQuery(queryUpdateKho, new MySqlParameter[] { new MySqlParameter("@ChenhLech", chenhLech), new MySqlParameter("@MaSanPham", maSanPham) });
            }

            // Cập nhật số lượng chi tiết hóa đơn
            string queryUpdateChiTiet = @"
                UPDATE ChiTietHoaDon 
                SET SoLuong = @SoLuongMoi
                WHERE MaChiTiet = @MaChiTiet";
            HelperDB.ExecuteNonQuery(queryUpdateChiTiet, new MySqlParameter[] { new MySqlParameter("@SoLuongMoi", soLuongMoi), new MySqlParameter("@MaChiTiet", maChiTiet) });

            // Cập nhật tổng tiền hóa đơn
            hoaDonDAL.CapNhatTongTienHoaDon(maHoaDon);

            return true;
        }

        public bool ChiTietHoaDonTonTai(int maChiTietHoaDon)
        {
            string query = "SELECT COUNT(*) FROM ChiTietHoaDon WHERE MaChiTiet = @MaChiTiet";
            MySqlParameter[] parameters = { new MySqlParameter("@MaChiTiet", maChiTietHoaDon) };

            int count = Convert.ToInt32(HelperDB.ExecuteScalar(query, parameters));
            return count > 0;
        }

        public bool KiemTraTonKho(int maSanPham, int soLuongCanDat)
        {
            string query = "SELECT SoLuongTon FROM SanPham WHERE MaSanPham = @MaSanPham";
            MySqlParameter[] parameters = { new MySqlParameter("@MaSanPham", maSanPham) };

            object result = HelperDB.ExecuteScalar(query, parameters);

            if (result == null) return false; // Sản phẩm không tồn tại
            int soLuongTon = Convert.ToInt32(result);

            return soLuongCanDat <= soLuongTon; // Trả về `true` nếu đủ hàng, `false` nếu không đủ
        }

    }
}
