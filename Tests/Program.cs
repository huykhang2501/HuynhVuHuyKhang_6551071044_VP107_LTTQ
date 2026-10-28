using System.Data;
using Dulieu;
using Microsoft.Data.SqlClient;

int assertions = 0;
void Check(bool passed, string label)
{
    if (!passed) throw new Exception("FAIL: " + label);
    assertions++;
    Console.WriteLine("PASS: " + label);
}

bool Valid(string code, string name, string credits) =>
    MonHocValidation.TryCreate(code, name, credits, out _, out _, out _);

Check(!Valid("", "Toán", "3"), "Bắt buộc mã môn");
Check(!Valid("MH1", "  ", "3"), "Bắt buộc tên môn");
Check(!Valid("MH1", "Toán", ""), "Bắt buộc tín chỉ");
Check(!Valid("ABCDEFGHIJK", "Toán", "3"), "Giới hạn mã 10 ký tự");
foreach (string invalid in new[] { "abc", "2a", "2..5", "1,2.3", "123456", "1e3", ".", "-", "+", "NaN" })
    Check(!Valid("MH1", "Toán", invalid), "Từ chối tín chỉ: " + invalid);
foreach (string valid in new[] { "3", "2.5", "2,5", "0.5", "-2.5", "99999" })
    Check(Valid("MH1", "Toán", valid), "Nhận số: " + valid);
Check(MonHocValidation.TryCreate(" MH1 ", " Toán ", "2,5", out var normalized, out _, out _)
    && normalized == new MonHoc("MH1", "Toán", "2.5"), "Chuẩn hóa khoảng trắng và dấu phẩy");
Check(!MonHocValidation.IsCreditsInput("2a"), "Chặn nội dung dán có chữ");

var repository = MonHocRepository.FromConfiguration();
await repository.InitializeAsync();
DataTable original = await repository.GetAllAsync();
string Snapshot(DataTable table) => string.Join("\n", table.Rows.Cast<DataRow>()
    .Select(row => string.Join("|", row.ItemArray.Select(value => value?.ToString()))));
string snapshotBefore = Snapshot(original);
string codeA = "Z" + Guid.NewGuid().ToString("N")[..8];
string codeB = "Z" + Guid.NewGuid().ToString("N")[..8];
bool createdA = false, createdB = false;
try
{
    await repository.AddAsync(new MonHoc(codeA, "Môn kiểm thử tiếng Việt ' ; --", "2.5"));
    createdA = true;
    Check(await repository.ExistsAsync(codeA), "Thêm và tìm môn trong SQL Server");
    DataRow added = (await repository.GetAllAsync()).Rows.Cast<DataRow>()
        .Single(row => row["MaMonHoc"].ToString() == codeA);
    Check(added["TenMonHoc"].ToString() == "Môn kiểm thử tiếng Việt ' ; --"
        && added["SoTinChi"].ToString() == "2.5", "Lưu Unicode, dấu nháy và tín chỉ thập phân");
    Check(await repository.ExistsAsync(codeA.ToLowerInvariant()), "Mã trùng theo collation SQL Server");
    bool duplicateRejected = false;
    try { await repository.AddAsync(new MonHoc(codeA, "Không được lưu", "1")); }
    catch (SqlException ex) when (ex.Number is 2601 or 2627) { duplicateRejected = true; }
    Check(duplicateRejected, "Khóa chính bảo vệ thêm mã trùng");

    await repository.UpdateAsync(codeA, new MonHoc(codeB, "Tên môn đã sửa", "3.75"));
    createdA = false;
    createdB = true;
    Check(!await repository.ExistsAsync(codeA) && await repository.ExistsAsync(codeB), "Sửa cả mã môn theo mã gốc");
    Check(!await repository.ExistsAsync(codeB, codeB), "Sửa chính bản ghi không bị báo trùng");
    DataRow updated = (await repository.GetAllAsync()).Rows.Cast<DataRow>()
        .Single(row => row["MaMonHoc"].ToString() == codeB);
    Check(updated["TenMonHoc"].ToString() == "Tên môn đã sửa" && updated["SoTinChi"].ToString() == "3.75", "Cập nhật tên và tín chỉ");
    await repository.DeleteAsync(codeB);
    createdB = false;
    Check(!await repository.ExistsAsync(codeB), "Xóa môn kiểm thử khỏi SQL Server");

    bool missingRejected = false;
    try { await repository.UpdateAsync(codeA, new MonHoc(codeA, "Đã mất", "1")); }
    catch (InvalidOperationException) { missingRejected = true; }
    Check(missingRejected, "Phát hiện bản ghi bị xóa ở nơi khác");
}
finally
{
    // Chỉ dọn đúng các bản ghi ngẫu nhiên do bài kiểm thử này tạo.
    if (createdA) await repository.DeleteAsync(codeA);
    if (createdB) await repository.DeleteAsync(codeB);
}
Check(snapshotBefore == Snapshot(await repository.GetAllAsync()), "Giữ nguyên toàn bộ dữ liệu có trước kiểm thử");
Console.WriteLine($"SUCCESS: {assertions} checks passed.");
