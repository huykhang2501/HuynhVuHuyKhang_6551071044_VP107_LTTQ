using System.Data;
using Microsoft.Data.SqlClient;

namespace Dulieu;

public partial class Form1 : Form
{
    private MonHocRepository? _repository;
    private string? _selectedCode;
    private bool _busy;
    private bool _ready;
    private bool _binding;
    private bool _resetting;
    private bool _addAllowed = true;
    private string _lastCreditsText = "";

    public Form1()
    {
        InitializeComponent();
        UpdateButtons();
    }

    private async void Form1_Shown(object? sender, EventArgs e) => await InitializeAsync();

    private async Task InitializeAsync()
    {
        await RunDatabaseActionAsync(async () =>
        {
            _ready = false;
            _repository = MonHocRepository.FromConfiguration();
            await _repository.InitializeAsync();
            await ReloadGridAsync();
            _ready = true;
            ResetEditor(true);
            statusLabel.Text = $"Đã kết nối • {dgvMonHoc.Rows.Count} môn học";
        }, "Đang kết nối cơ sở dữ liệu...");
    }

    private async Task ReloadGridAsync()
    {
        DataTable data = await _repository!.GetAllAsync();
        _binding = true;
        try
        {
            dgvMonHoc.DataSource = data;
            dgvMonHoc.ClearSelection();
            dgvMonHoc.CurrentCell = null;
            countLabel.Text = $"{data.Rows.Count} môn học";
        }
        finally { _binding = false; }
    }

    private void UpdateButtons()
    {
        btnThem.Enabled = !_busy && _ready && _selectedCode == null && _addAllowed;
        btnSua.Enabled = btnXoa.Enabled = !_busy && _ready && _selectedCode != null;
        btnThoat.Enabled = !_busy;
        txtMaMonHoc.Enabled = txtTenMonHoc.Enabled = txtSoTinChi.Enabled = !_busy && _ready;
        dgvMonHoc.Enabled = !_busy && _ready;
        UseWaitCursor = _busy;
    }

    private void ResetEditor(bool allowAdd)
    {
        _resetting = true;
        _binding = true;
        try
        {
            _selectedCode = null;
            txtMaMonHoc.Clear();
            txtTenMonHoc.Clear();
            txtSoTinChi.Clear();
            _lastCreditsText = "";
            errors.Clear();
            _addAllowed = allowAdd;
            dgvMonHoc.ClearSelection();
            dgvMonHoc.CurrentCell = null;
        }
        finally { _resetting = false; _binding = false; }
        UpdateButtons();
        if (txtMaMonHoc.Enabled) txtMaMonHoc.Focus();
    }

    private void dgvMonHoc_SelectionChanged(object? sender, EventArgs e)
    {
        if (_binding || _busy || dgvMonHoc.SelectedRows.Count == 0) return;
        if (dgvMonHoc.SelectedRows[0].DataBoundItem is not DataRowView row) return;
        _resetting = true;
        try
        {
            _selectedCode = row["MaMonHoc"].ToString()!;
            txtMaMonHoc.Text = _selectedCode;
            txtTenMonHoc.Text = row["TenMonHoc"].ToString();
            txtSoTinChi.Text = row["SoTinChi"].ToString();
            _lastCreditsText = txtSoTinChi.Text;
            errors.Clear();
        }
        finally { _resetting = false; }
        UpdateButtons();
        statusLabel.Text = $"Đang chọn môn {_selectedCode}. Sửa thông tin hoặc chọn Xóa.";
    }

    private void Editor_TextChanged(object? sender, EventArgs e)
    {
        if (_resetting) return;
        if (sender == txtSoTinChi)
        {
            if (!MonHocValidation.IsCreditsInput(txtSoTinChi.Text))
            {
                int caret = Math.Max(0, txtSoTinChi.SelectionStart - 1);
                _resetting = true;
                txtSoTinChi.Text = _lastCreditsText;
                txtSoTinChi.SelectionStart = Math.Min(caret, txtSoTinChi.TextLength);
                _resetting = false;
                errors.SetError(txtSoTinChi, "Chỉ nhập số nguyên hoặc số thực, tối đa 5 ký tự.");
                return;
            }
            _lastCreditsText = txtSoTinChi.Text;
        }
        if (sender is Control control) errors.SetError(control, "");
        if (_selectedCode == null) _addAllowed = true;
        UpdateButtons();
    }

    private void txtSoTinChi_KeyPress(object? sender, KeyPressEventArgs e)
    {
        if (char.IsControl(e.KeyChar)) return;
        string next = txtSoTinChi.Text.Remove(txtSoTinChi.SelectionStart, txtSoTinChi.SelectionLength)
            .Insert(txtSoTinChi.SelectionStart, e.KeyChar.ToString());
        e.Handled = !MonHocValidation.IsCreditsInput(next);
        if (e.Handled) errors.SetError(txtSoTinChi, "Chỉ nhập số nguyên hoặc số thực, tối đa 5 ký tự.");
    }

    private bool TryReadCourse(out MonHoc course)
    {
        errors.Clear();
        if (MonHocValidation.TryCreate(txtMaMonHoc.Text, txtTenMonHoc.Text, txtSoTinChi.Text,
                out MonHoc? result, out string field, out string message))
        {
            course = result!;
            return true;
        }
        Control target = field switch
        {
            "MaMonHoc" => txtMaMonHoc,
            "TenMonHoc" => txtTenMonHoc,
            _ => txtSoTinChi
        };
        errors.SetError(target, message);
        MessageBox.Show(this, message, "Kiểm tra dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        target.Focus();
        course = null!;
        return false;
    }

    private void ShowDuplicate()
    {
        const string message = "Mã môn học đã tồn tại. Vui lòng nhập mã khác.";
        statusLabel.Text = message;
        errors.SetError(txtMaMonHoc, message);
        MessageBox.Show(this, message, "Trùng mã môn học", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        txtMaMonHoc.Focus();
        txtMaMonHoc.SelectAll();
    }

    private async void btnThem_Click(object? sender, EventArgs e)
    {
        if (!TryReadCourse(out MonHoc course)) return;
        await RunDatabaseActionAsync(async () =>
        {
            if (await _repository!.ExistsAsync(course.MaMonHoc)) { ShowDuplicate(); return; }
            await _repository.AddAsync(course);
            ResetEditor(false); 
            await ReloadGridAsync();
            statusLabel.Text = $"Đã thêm môn {course.MaMonHoc}. Nhập dữ liệu mới để thêm tiếp.";
        }, "Đang thêm môn học...");
    }

    private async void btnSua_Click(object? sender, EventArgs e)
    {
        if (_selectedCode == null || !TryReadCourse(out MonHoc course)) return;
        string originalCode = _selectedCode;
        await RunDatabaseActionAsync(async () =>
        {
            if (await _repository!.ExistsAsync(course.MaMonHoc, originalCode)) { ShowDuplicate(); return; }
            await _repository.UpdateAsync(originalCode, course);
            ResetEditor(true);
            await ReloadGridAsync();
            statusLabel.Text = $"Đã cập nhật môn {course.MaMonHoc}.";
        }, "Đang cập nhật môn học...");
    }

    private async void btnXoa_Click(object? sender, EventArgs e)
    {
        if (_selectedCode == null) return;
        if (MessageBox.Show(this, "Bạn có muốn xóa môn học này không?", "Xác nhận xóa",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        {
            ResetEditor(true);
            statusLabel.Text = "Đã hủy xóa môn học.";
            return;
        }
        string originalCode = _selectedCode;
        await RunDatabaseActionAsync(async () =>
        {
            await _repository!.DeleteAsync(originalCode);
            ResetEditor(true);
            await ReloadGridAsync();
            statusLabel.Text = $"Đã xóa môn {originalCode}.";
        }, "Đang xóa môn học...");
    }

    private async Task RunDatabaseActionAsync(Func<Task> action, string status)
    {
        if (_busy) return;
        _busy = true;
        statusLabel.Text = status;
        UpdateButtons();
        try { await action(); }
        catch (SqlException ex) when (ex.Number is 2601 or 2627) { ShowDuplicate(); }
        catch (Exception ex)
        {
            statusLabel.Text = "Thao tác chưa hoàn tất. Nhấn F5 để tải lại hoặc thử kết nối lại.";
            string message = ex is SqlException
                ? "Không thể hoàn tất thao tác với SQL Server. Kiểm tra dịch vụ SQL Server, quyền truy cập và appsettings.json."
                : "Không thể hoàn tất thao tác.";
            MessageBox.Show(this, $"{message}\n\nChi tiết: {ex.Message}", "Lỗi dữ liệu",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _busy = false;
            UpdateButtons();
            if (_selectedCode == null && _ready) txtMaMonHoc.Focus();
        }
    }

    private void btnThoat_Click(object? sender, EventArgs e) => Close();

    private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (e.CloseReason == CloseReason.WindowsShutDown) return;
        if (_busy) { e.Cancel = true; return; }
        e.Cancel = MessageBox.Show(this, "Bạn có muốn thoát không?", "Xác nhận thoát",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes;
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (!_busy && keyData == Keys.Escape)
        {
            ResetEditor(true);
            statusLabel.Text = "Nhập thông tin để thêm môn học mới.";
            return true;
        }
        if (!_busy && keyData == Keys.F5)
        {
            _ = InitializeAsync();
            return true;
        }
        return base.ProcessCmdKey(ref msg, keyData);
    }
}
