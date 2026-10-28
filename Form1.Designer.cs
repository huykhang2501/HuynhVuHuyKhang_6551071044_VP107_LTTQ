#nullable enable
namespace Dulieu;

partial class Form1
{
    private System.ComponentModel.IContainer? components;
    private GroupBox informationGroup = null!;
    private Label codeLabel = null!;
    private Label nameLabel = null!;
    private Label creditsLabel = null!;
    private Label inputHintLabel = null!;
    private TextBox txtMaMonHoc = null!;
    private TextBox txtTenMonHoc = null!;
    private TextBox txtSoTinChi = null!;
    private Button btnThem = null!;
    private Button btnSua = null!;
    private Button btnXoa = null!;
    private Button btnThoat = null!;
    private GroupBox listGroup = null!;
    private DataGridView dgvMonHoc = null!;
    private DataGridViewTextBoxColumn maColumn = null!;
    private DataGridViewTextBoxColumn tenColumn = null!;
    private DataGridViewTextBoxColumn tinChiColumn = null!;
    private Label countLabel = null!;
    private StatusStrip statusStrip = null!;
    private ToolStripStatusLabel statusLabel = null!;
    private ToolStripStatusLabel shortcutLabel = null!;
    private ErrorProvider errors = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
        informationGroup = new GroupBox();
        codeLabel = new Label();
        nameLabel = new Label();
        creditsLabel = new Label();
        inputHintLabel = new Label();
        txtMaMonHoc = new TextBox();
        txtTenMonHoc = new TextBox();
        txtSoTinChi = new TextBox();
        btnThem = new Button();
        btnSua = new Button();
        btnXoa = new Button();
        btnThoat = new Button();
        listGroup = new GroupBox();
        dgvMonHoc = new DataGridView();
        maColumn = new DataGridViewTextBoxColumn();
        tenColumn = new DataGridViewTextBoxColumn();
        tinChiColumn = new DataGridViewTextBoxColumn();
        countLabel = new Label();
        statusStrip = new StatusStrip();
        statusLabel = new ToolStripStatusLabel();
        shortcutLabel = new ToolStripStatusLabel();
        errors = new ErrorProvider(components);
        informationGroup.SuspendLayout();
        listGroup.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvMonHoc).BeginInit();
        statusStrip.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)errors).BeginInit();
        SuspendLayout();
        // 
        // informationGroup
        // 
        informationGroup.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
        informationGroup.BackColor = Color.White;
        informationGroup.Controls.Add(codeLabel);
        informationGroup.Controls.Add(nameLabel);
        informationGroup.Controls.Add(creditsLabel);
        informationGroup.Controls.Add(inputHintLabel);
        informationGroup.Controls.Add(txtMaMonHoc);
        informationGroup.Controls.Add(txtTenMonHoc);
        informationGroup.Controls.Add(txtSoTinChi);
        informationGroup.Controls.Add(btnThem);
        informationGroup.Controls.Add(btnSua);
        informationGroup.Controls.Add(btnXoa);
        informationGroup.Controls.Add(btnThoat);
        informationGroup.Location = new Point(21, 36);
        informationGroup.Name = "informationGroup";
        informationGroup.Size = new Size(288, 455);
        informationGroup.TabIndex = 0;
        informationGroup.TabStop = false;
        informationGroup.Text = "Thông tin môn học";
        // 
        // codeLabel
        // 
        codeLabel.AutoSize = true;
        codeLabel.Location = new Point(18, 37);
        codeLabel.Name = "codeLabel";
        codeLabel.Size = new Size(166, 37);
        codeLabel.TabIndex = 0;
        codeLabel.Text = "Mã môn học";
        // 
        // nameLabel
        // 
        nameLabel.AutoSize = true;
        nameLabel.Location = new Point(18, 110);
        nameLabel.Name = "nameLabel";
        nameLabel.Size = new Size(168, 37);
        nameLabel.TabIndex = 1;
        nameLabel.Text = "Tên môn học";
        // 
        // creditsLabel
        // 
        creditsLabel.AutoSize = true;
        creditsLabel.Location = new Point(18, 183);
        creditsLabel.Name = "creditsLabel";
        creditsLabel.Size = new Size(126, 37);
        creditsLabel.TabIndex = 2;
        creditsLabel.Text = "Số tín chỉ";
        // 
        // inputHintLabel
        // 
        inputHintLabel.Font = new Font("Segoe UI", 9F);
        inputHintLabel.ForeColor = Color.FromArgb(93, 108, 125);
        inputHintLabel.Location = new Point(18, 249);
        inputHintLabel.Name = "inputHintLabel";
        inputHintLabel.Size = new Size(249, 54);
        inputHintLabel.TabIndex = 3;
        inputHintLabel.Text = "Mã: tối đa 10 ký tự.\nTín chỉ: số nguyên hoặc số thực (2,5).";
        // 
        // txtMaMonHoc
        // 
        txtMaMonHoc.AccessibleName = "Mã môn học";
        txtMaMonHoc.Location = new Point(21, 64);
        txtMaMonHoc.MaxLength = 10;
        txtMaMonHoc.Name = "txtMaMonHoc";
        txtMaMonHoc.Size = new Size(242, 43);
        txtMaMonHoc.TabIndex = 0;
        txtMaMonHoc.TextChanged += Editor_TextChanged;
        // 
        // txtTenMonHoc
        // 
        txtTenMonHoc.AccessibleName = "Tên môn học";
        txtTenMonHoc.Location = new Point(21, 137);
        txtTenMonHoc.Name = "txtTenMonHoc";
        txtTenMonHoc.Size = new Size(242, 43);
        txtTenMonHoc.TabIndex = 1;
        txtTenMonHoc.TextChanged += Editor_TextChanged;
        // 
        // txtSoTinChi
        // 
        txtSoTinChi.AccessibleName = "Số tín chỉ";
        txtSoTinChi.Location = new Point(21, 210);
        txtSoTinChi.MaxLength = 5;
        txtSoTinChi.Name = "txtSoTinChi";
        txtSoTinChi.Size = new Size(242, 43);
        txtSoTinChi.TabIndex = 2;
        txtSoTinChi.TextChanged += Editor_TextChanged;
        txtSoTinChi.KeyPress += txtSoTinChi_KeyPress;
        // 
        // btnThem
        // 
        btnThem.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnThem.BackColor = Color.FromArgb(32, 105, 169);
        btnThem.FlatStyle = FlatStyle.Flat;
        btnThem.ForeColor = Color.White;
        btnThem.Location = new Point(21, 322);
        btnThem.Name = "btnThem";
        btnThem.Size = new Size(116, 47);
        btnThem.TabIndex = 3;
        btnThem.Text = "&Thêm";
        btnThem.UseVisualStyleBackColor = false;
        btnThem.Click += btnThem_Click;
        // 
        // btnSua
        // 
        btnSua.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnSua.Location = new Point(147, 322);
        btnSua.Name = "btnSua";
        btnSua.Size = new Size(116, 47);
        btnSua.TabIndex = 4;
        btnSua.Text = "&Sửa";
        btnSua.Click += btnSua_Click;
        // 
        // btnXoa
        // 
        btnXoa.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnXoa.Location = new Point(21, 382);
        btnXoa.Name = "btnXoa";
        btnXoa.Size = new Size(116, 47);
        btnXoa.TabIndex = 5;
        btnXoa.Text = "&Xóa";
        btnXoa.Click += btnXoa_Click;
        // 
        // btnThoat
        // 
        btnThoat.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnThoat.Location = new Point(147, 382);
        btnThoat.Name = "btnThoat";
        btnThoat.Size = new Size(116, 47);
        btnThoat.TabIndex = 6;
        btnThoat.Text = "T&hoát";
        btnThoat.Click += btnThoat_Click;
        // 
        // listGroup
        // 
        listGroup.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        listGroup.BackColor = Color.White;
        listGroup.Controls.Add(dgvMonHoc);
        listGroup.Controls.Add(countLabel);
        listGroup.Location = new Point(334, 36);
        listGroup.Name = "listGroup";
        listGroup.Size = new Size(654, 455);
        listGroup.TabIndex = 1;
        listGroup.TabStop = false;
        listGroup.Text = "Danh sách môn học";
        // 
        // dgvMonHoc
        // 
        dgvMonHoc.AccessibleName = "Danh sách môn học";
        dgvMonHoc.AllowUserToAddRows = false;
        dgvMonHoc.AllowUserToDeleteRows = false;
        dgvMonHoc.AllowUserToResizeRows = false;
        dataGridViewCellStyle1.BackColor = Color.FromArgb(247, 250, 253);
        dgvMonHoc.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
        dgvMonHoc.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvMonHoc.AutoGenerateColumns = false;
        dgvMonHoc.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvMonHoc.BackgroundColor = Color.White;
        dgvMonHoc.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        dgvMonHoc.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
        dgvMonHoc.ColumnHeadersHeight = 40;
        dgvMonHoc.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dgvMonHoc.Columns.AddRange(new DataGridViewColumn[] { maColumn, tenColumn, tinChiColumn });
        dgvMonHoc.EnableHeadersVisualStyles = false;
        dgvMonHoc.Font = new Font("Segoe UI", 10F);
        dgvMonHoc.GridColor = Color.FromArgb(226, 232, 239);
        dgvMonHoc.Location = new Point(18, 33);
        dgvMonHoc.MultiSelect = false;
        dgvMonHoc.Name = "dgvMonHoc";
        dgvMonHoc.ReadOnly = true;
        dgvMonHoc.RowHeadersVisible = false;
        dgvMonHoc.RowHeadersWidth = 82;
        dgvMonHoc.RowTemplate.Height = 36;
        dgvMonHoc.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvMonHoc.Size = new Size(617, 396);
        dgvMonHoc.TabIndex = 0;
        dgvMonHoc.SelectionChanged += dgvMonHoc_SelectionChanged;
        //
        // maColumn
        //
        maColumn.DataPropertyName = "MaMonHoc";
        maColumn.HeaderText = "Mã môn học";
        maColumn.Name = "maColumn";
        maColumn.FillWeight = 25F;
        maColumn.MinimumWidth = 110;
        //
        // tenColumn
        //
        tenColumn.DataPropertyName = "TenMonHoc";
        tenColumn.HeaderText = "Tên môn học";
        tenColumn.Name = "tenColumn";
        tenColumn.FillWeight = 55F;
        tenColumn.MinimumWidth = 160;
        //
        // tinChiColumn
        //
        tinChiColumn.DataPropertyName = "SoTinChi";
        tinChiColumn.HeaderText = "Số tín chỉ";
        tinChiColumn.Name = "tinChiColumn";
        tinChiColumn.FillWeight = 20F;
        tinChiColumn.MinimumWidth = 100;
        tinChiColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // countLabel
        // 
        countLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        countLabel.ForeColor = Color.FromArgb(93, 108, 125);
        countLabel.Location = new Point(463, 0);
        countLabel.Name = "countLabel";
        countLabel.Size = new Size(169, 24);
        countLabel.TabIndex = 1;
        countLabel.Text = "0 môn học";
        countLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // statusStrip
        // 
        statusStrip.BackColor = Color.FromArgb(233, 240, 247);
        statusStrip.ImageScalingSize = new Size(32, 32);
        statusStrip.Items.AddRange(new ToolStripItem[] { statusLabel, shortcutLabel });
        statusStrip.Location = new Point(0, 572);
        statusStrip.Name = "statusStrip";
        statusStrip.Size = new Size(1000, 42);
        statusStrip.SizingGrip = false;
        statusStrip.TabIndex = 2;
        // 
        // statusLabel
        // 
        statusLabel.Name = "statusLabel";
        statusLabel.Size = new Size(696, 32);
        statusLabel.Spring = true;
        statusLabel.Text = "Đang khởi động...";
        statusLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // shortcutLabel
        // 
        shortcutLabel.Name = "shortcutLabel";
        shortcutLabel.Size = new Size(289, 32);
        shortcutLabel.Text = "Esc: nhập mới  |  F5: tải lại";
        // 
        // errors
        // 
        errors.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        errors.ContainerControl = this;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(192F, 192F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(243, 246, 250);
        ClientSize = new Size(1000, 614);
        Controls.Add(informationGroup);
        Controls.Add(listGroup);
        Controls.Add(statusStrip);
        Font = new Font("Segoe UI", 10F);
        MinimumSize = new Size(940, 650);
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Quản lý môn học - VP107";
        FormClosing += Form1_FormClosing;
        Shown += Form1_Shown;
        informationGroup.ResumeLayout(false);
        informationGroup.PerformLayout();
        listGroup.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvMonHoc).EndInit();
        statusStrip.ResumeLayout(false);
        statusStrip.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)errors).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
