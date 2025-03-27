CREATE DATABASE IF NOT EXISTS QuanLyBanHang;
USE QuanLyBanHang;

-- -- -- -- --
SET FOREIGN_KEY_CHECKS = 0;
DROP TABLE IF EXISTS CuaHang;
DROP TABLE IF EXISTS ChiTietHoaDon;
DROP TABLE IF EXISTS HoaDon;
DROP TABLE IF EXISTS SanPham;
DROP TABLE IF EXISTS KhachHang;
SET FOREIGN_KEY_CHECKS = 1;
-- -- -- -- --

DROP TABLE IF EXISTS `CuaHang`;
CREATE TABLE CuaHang (
    CuaHangID INT PRIMARY KEY,
    TenCuaHang NVARCHAR(255),
    DiaChi NVARCHAR(255),
    SoDienThoai VARCHAR(20)
);

DROP TABLE IF EXISTS `SanPham`;
CREATE TABLE SanPham (
    MaSanPham INT AUTO_INCREMENT PRIMARY KEY,
    TenSanPham VARCHAR(255) NOT NULL,
    DonGia DECIMAL(10,2) NOT NULL,
    SoLuongTon INT NOT NULL,
    MoTa TEXT,
    NgayTao TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

INSERT INTO SanPham (TenSanPham, DonGia, SoLuongTon, MoTa) VALUES
('Bóng đèn LED', 50000, 100, 'Bóng đèn tiết kiệm điện 9W'),
('Công tắc điện', 20000, 150, 'Công tắc đơn 16A'),
('Ổ cắm điện', 35000, 120, 'Ổ cắm 3 chấu chịu tải cao'),
('Quạt treo tường', 450000, 50, 'Quạt treo tường 3 tốc độ'),
('Bình nóng lạnh', 3200000, 30, 'Bình nóng lạnh 20L'),
('Máy bơm nước', 1500000, 40, 'Máy bơm 1HP chuyên dụng'),
('Dây điện 2.5mm', 8000, 500, 'Cuộn dây điện 2.5mm dài 100m'),
('Đèn pha LED', 220000, 80, 'Đèn pha LED 50W ngoài trời'),
('Quạt trần', 890000, 20, 'Quạt trần cánh gỗ 3 tốc độ'),
('Ổ cắm thông minh', 450000, 60, 'Ổ cắm điều khiển từ xa qua WiFi');


DROP TABLE IF EXISTS `KhachHang`;
CREATE TABLE KhachHang (
    MaKhachHang INT AUTO_INCREMENT PRIMARY KEY,
    TenKhachHang VARCHAR(255) NOT NULL,
    SoDienThoai VARCHAR(15) UNIQUE NOT NULL,
    DiaChi TEXT,
    NgayDangKy TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

INSERT INTO KhachHang (TenKhachHang, SoDienThoai, DiaChi) VALUES
('Nguyễn Văn Dương', '0987654321', '123 Đường ABC, Hà Nội'),
('Trần Thị Mai', '0978123456', '456 Đường XYZ, TP.HCM'),
('Lê Hoàng Phong', '0965234789', '789 Đường LMN, Đà Nẵng'),
('Phạm Minh Nguyệt', '0945566778', '15 Nguyễn Trãi, Hải Phòng'),
('Hoàng Thị Phúc', '0909988776', '99 Lê Lợi, Cần Thơ'),
('Ngô Văn Nam', '0912233445', '12 Hoàng Hoa Thám, Hà Nội'),
('Bùi Hữu Duyên', '0988543212', '567 Lạc Long Quân, TP.HCM'),
('Đặng Thị Hằng', '0934125678', '321 Võ Văn Kiệt, Đà Nẵng'),
('Nguyễn Thanh Trúc', '0977543210', '88 Trần Hưng Đạo, Nha Trang'),
('Phan Văn Kiên', '0921122334', '42 Phạm Ngũ Lão, Huế');

