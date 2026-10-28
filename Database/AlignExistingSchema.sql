-- Chỉ dùng khi dbo.MonHoc cũ có TenMonHoc nvarchar(100), SoTinChi int.
-- Sao lưu database trước khi chạy. Không xóa dữ liệu hoặc các bảng khác.
USE [Dulieu];
GO
SET XACT_ABORT ON;
BEGIN TRANSACTION;
IF EXISTS (SELECT 1 FROM dbo.MonHoc WHERE LEN(CONVERT(nvarchar(max), SoTinChi)) > 5)
BEGIN
    ROLLBACK TRANSACTION;
    THROW 50001, N'Có số tín chỉ dài hơn 5 ký tự; chưa thay đổi cấu trúc.', 1;
END;
ALTER TABLE dbo.MonHoc ALTER COLUMN TenMonHoc ntext NOT NULL;
ALTER TABLE dbo.MonHoc ALTER COLUMN SoTinChi nvarchar(5) NOT NULL;
COMMIT TRANSACTION;
GO
