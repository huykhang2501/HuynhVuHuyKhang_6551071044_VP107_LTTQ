using System.Data;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;

namespace Dulieu;

public sealed class MonHocRepository
{
    private readonly string _connectionString;
    public MonHocRepository(string connectionString) => _connectionString = connectionString;

    public static MonHocRepository FromConfiguration()
    {
        string path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        using JsonDocument settings = JsonDocument.Parse(File.ReadAllText(path));
        string? value = settings.RootElement.GetProperty("ConnectionString").GetString();
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException("Thiếu ConnectionString trong appsettings.json.");
        var builder = new SqlConnectionStringBuilder(value);
        if (!string.Equals(builder.InitialCatalog, "Dulieu", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Database trong appsettings.json phải là Dulieu.");
        return new MonHocRepository(builder.ConnectionString);
    }

    public async Task InitializeAsync()
    {
        
        try
        {
            await using var existing = new SqlConnection(_connectionString);
            await existing.OpenAsync();
            await using var check = new SqlCommand("SELECT OBJECT_ID(N'dbo.MonHoc', N'U');", existing);
            if (await check.ExecuteScalarAsync() is not DBNull) return;
        }
        catch (SqlException ex) when (ex.Number == 4060) { }

        var builder = new SqlConnectionStringBuilder(_connectionString) { InitialCatalog = "master" };
        await using var connection = new SqlConnection(builder.ConnectionString);
        await connection.OpenAsync();
        using Stream stream = typeof(MonHocRepository).Assembly
            .GetManifestResourceStream("Dulieu.Database.Setup.sql")
            ?? throw new InvalidOperationException("Không tìm thấy Database/Setup.sql.");
        using var reader = new StreamReader(stream);
        string script = await reader.ReadToEndAsync();
        foreach (string batch in Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase))
        {
            if (string.IsNullOrWhiteSpace(batch)) continue;
            await using var command = new SqlCommand(batch, connection);
            await command.ExecuteNonQueryAsync();
        }
    }

    public async Task<DataTable> GetAllAsync()
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new SqlCommand(
            "SELECT MaMonHoc, CONVERT(nvarchar(max), TenMonHoc) AS TenMonHoc, SoTinChi FROM dbo.MonHoc ORDER BY MaMonHoc;", connection);
        await using var reader = await command.ExecuteReaderAsync();
        var table = new DataTable();
        table.Load(reader);
        return table;
    }

    public async Task<bool> ExistsAsync(string code, string? originalCode = null)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
       
        await using var command = new SqlCommand("""
            SELECT COUNT(*) FROM dbo.MonHoc WHERE MaMonHoc = @Ma
              AND (@MaCu IS NULL OR MaMonHoc <> @MaCu);
            """, connection);
        command.Parameters.Add("@Ma", SqlDbType.NVarChar, 10).Value = code;
        command.Parameters.Add("@MaCu", SqlDbType.NVarChar, 10).Value = (object?)originalCode ?? DBNull.Value;
        return Convert.ToInt32(await command.ExecuteScalarAsync()) > 0;
    }

    public Task AddAsync(MonHoc course) => ExecuteAsync("""
        INSERT INTO dbo.MonHoc (MaMonHoc, TenMonHoc, SoTinChi) VALUES (@Ma, @Ten, @TinChi);
        """, course, null);

    public Task UpdateAsync(string originalCode, MonHoc course) => ExecuteAsync("""
        UPDATE dbo.MonHoc SET MaMonHoc = @Ma, TenMonHoc = @Ten, SoTinChi = @TinChi WHERE MaMonHoc = @MaCu;
        """, course, originalCode);

    private async Task ExecuteAsync(string sql, MonHoc course, string? originalCode)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@Ma", SqlDbType.NVarChar, 10).Value = course.MaMonHoc;
        command.Parameters.Add("@Ten", SqlDbType.NVarChar, -1).Value = course.TenMonHoc;
        command.Parameters.Add("@TinChi", SqlDbType.NVarChar, 5).Value = course.SoTinChi;
        if (originalCode != null) command.Parameters.Add("@MaCu", SqlDbType.NVarChar, 10).Value = originalCode;
        if (await command.ExecuteNonQueryAsync() == 0)
            throw new InvalidOperationException("Môn học đã bị xóa ở nơi khác. Nhấn F5 để tải lại danh sách.");
    }

    public async Task DeleteAsync(string code)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new SqlCommand("DELETE FROM dbo.MonHoc WHERE MaMonHoc = @Ma;", connection);
        command.Parameters.Add("@Ma", SqlDbType.NVarChar, 10).Value = code;
        if (await command.ExecuteNonQueryAsync() == 0)
            throw new InvalidOperationException("Môn học đã bị xóa ở nơi khác. Nhấn F5 để tải lại danh sách.");
    }
}
