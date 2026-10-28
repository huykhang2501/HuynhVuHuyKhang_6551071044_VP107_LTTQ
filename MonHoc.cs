using System.Globalization;
using System.Text.RegularExpressions;

namespace Dulieu;

public sealed record MonHoc(string MaMonHoc, string TenMonHoc, string SoTinChi);

public static class MonHocValidation
{
    public static bool IsCreditsInput(string text) =>
        text.Length <= 5 && Regex.IsMatch(text, @"\A[+-]?[0-9]*([.,][0-9]*)?\z");

    public static bool TryCreate(string code, string name, string credits,
        out MonHoc? course, out string field, out string message)
    {
        course = null;
        code = code.Trim();
        name = name.Trim();
        credits = credits.Trim();
        field = "MaMonHoc";
        message = "Vui lòng nhập mã môn học.";
        if (code.Length == 0) return false;
        message = "Mã môn học tối đa 10 ký tự.";
        if (code.Length > 10) return false;
        field = "TenMonHoc";
        message = "Vui lòng nhập tên môn học.";
        if (name.Length == 0) return false;
        field = "SoTinChi";
        message = "Vui lòng nhập số tín chỉ.";
        if (credits.Length == 0) return false;
        message = "Số tín chỉ phải là số nguyên hoặc số thực, tối đa 5 ký tự (ví dụ: 3 hoặc 2,5).";
        string normalized = credits.Replace(',', '.');
        if (!IsCreditsInput(credits) || !decimal.TryParse(normalized,
                NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out _)) return false;
        course = new MonHoc(code, name, normalized);
        field = message = "";
        return true;
    }
}
