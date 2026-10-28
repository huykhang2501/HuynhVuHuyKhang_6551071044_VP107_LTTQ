USE DuLieu;
GO

CREATE TABLE dbo.MonHoc
(
    MaMonHoc NVARCHAR(10) NOT NULL PRIMARY KEY,
    TenMonHoc NVARCHAR(100) NOT NULL,
    SoTinChi INT NOT NULL
);
GO

INSERT INTO dbo.MonHoc (MaMonHoc, TenMonHoc, SoTinChi)
VALUES
    (N'MON1', N'TOAN', 2),
    (N'MON2', N'lTTQ', 2),
    (N'MON3', N'TKW', 3),
    (N'MON4', N'HDH', 2),
    (N'MON5', N'HDH', 1);
GO

SELECT *
FROM dbo.MonHoc
ORDER BY MaMonHoc;