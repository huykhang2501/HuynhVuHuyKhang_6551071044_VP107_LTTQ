-- Chạy trong SSMS hoặc sqlcmd. Không xóa hay ghi đè dữ liệu hiện có.
USE [master];
GO
IF DB_ID(N'Dulieu') IS NULL
    EXEC(N'CREATE DATABASE [Dulieu]');
GO
USE [Dulieu];
GO
IF OBJECT_ID(N'dbo.MonHoc', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.MonHoc
    (
        MaMonHoc nvarchar(10) NOT NULL CONSTRAINT PK_MonHoc PRIMARY KEY,
        TenMonHoc ntext NOT NULL,
        SoTinChi nvarchar(5) NOT NULL
    );
    INSERT INTO dbo.MonHoc (MaMonHoc, TenMonHoc, SoTinChi) VALUES
        (N'MH001', N'Lập trình C# cơ bản', N'3'),
        (N'MH002', N'Cơ sở dữ liệu', N'4'),
        (N'MH003', N'Phân tích thiết kế hệ thống', N'3'),
        (N'MH004', N'Mạng máy tính', N'3'),
        (N'MH005', N'Trí tuệ nhân tạo cơ bản', N'3');
END;
GO
