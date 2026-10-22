namespace HabitTrackerDesktop.Forms;

partial class ProposalsForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        lblHeader = new Label();
        lblSubtitle = new Label();
        grid = new DataGridView();
        colEmoji = new DataGridViewTextBoxColumn();
        colTitle = new DataGridViewTextBoxColumn();
        colStatus = new DataGridViewTextBoxColumn();
        colDate = new DataGridViewTextBoxColumn();
        lblDetails = new Label();
        btnWithdraw = new Button();
        lblNew = new Label();
        lblTitle = new Label();
        txtTitle = new TextBox();
        lblEmoji = new Label();
        cmbEmoji = new ComboBox();
        lblCategory = new Label();
        cmbCategory = new ComboBox();
        lblDescription = new Label();
        txtDescription = new TextBox();
        lblText = new Label();
        txtMessage = new TextBox();
        btnSend = new Button();
        ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
        SuspendLayout();

        lblHeader.AutoSize = true;
        lblHeader.Font = new Font("Segoe UI Semibold", 14F);
        lblHeader.Location = new Point(20, 16);
        lblHeader.Name = "lblHeader";
        lblHeader.TabIndex = 0;
        lblHeader.Text = "Мои заявки";

        lblSubtitle.AutoSize = true;
        lblSubtitle.Location = new Point(23, 48);
        lblSubtitle.Name = "lblSubtitle";
        lblSubtitle.TabIndex = 1;
        lblSubtitle.Tag = "muted";
        lblSubtitle.Text = "Предложи привычку в общий список — администратор рассмотрит заявку";

        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        grid.AllowUserToResizeRows = false;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.ColumnHeadersHeight = 38;
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        grid.Columns.AddRange(new DataGridViewColumn[] { colEmoji, colTitle, colStatus, colDate });
        grid.Location = new Point(24, 88);
        grid.MultiSelect = false;
        grid.Name = "grid";
        grid.ReadOnly = true;
        grid.RowHeadersVisible = false;
        grid.RowTemplate.Height = 38;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.Size = new Size(540, 300);
        grid.TabIndex = 2;
        grid.SelectionChanged += grid_SelectionChanged;

        colEmoji.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        colEmoji.HeaderText = "";
        colEmoji.Name = "colEmoji";
        colEmoji.ReadOnly = true;
        colEmoji.SortMode = DataGridViewColumnSortMode.NotSortable;
        colEmoji.Width = 50;

        colTitle.HeaderText = "Название";
        colTitle.Name = "colTitle";
        colTitle.ReadOnly = true;
        colTitle.SortMode = DataGridViewColumnSortMode.NotSortable;

        colStatus.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        colStatus.HeaderText = "Статус";
        colStatus.Name = "colStatus";
        colStatus.ReadOnly = true;
        colStatus.SortMode = DataGridViewColumnSortMode.NotSortable;
        colStatus.Width = 140;

        colDate.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        colDate.HeaderText = "Отправлена";
        colDate.Name = "colDate";
        colDate.ReadOnly = true;
        colDate.SortMode = DataGridViewColumnSortMode.NotSortable;
        colDate.Width = 110;

        lblDetails.Location = new Point(24, 398);
        lblDetails.Name = "lblDetails";
        lblDetails.Size = new Size(540, 108);
        lblDetails.TabIndex = 3;
        lblDetails.Tag = "muted";
        lblDetails.Text = "Подробности выбранной заявки";

        btnWithdraw.Location = new Point(24, 520);
        btnWithdraw.Name = "btnWithdraw";
        btnWithdraw.Size = new Size(200, 40);
        btnWithdraw.TabIndex = 4;
        btnWithdraw.Tag = "danger";
        btnWithdraw.Text = "Отозвать заявку";
        btnWithdraw.Click += btnWithdraw_Click;

        lblNew.AutoSize = true;
        lblNew.Font = new Font("Segoe UI Semibold", 11F);
        lblNew.Location = new Point(589, 88);
        lblNew.Name = "lblNew";
        lblNew.TabIndex = 5;
        lblNew.Text = "Новая заявка";

        lblTitle.AutoSize = true;
        lblTitle.Location = new Point(589, 120);
        lblTitle.Name = "lblTitle";
        lblTitle.TabIndex = 6;
        lblTitle.Tag = "muted";
        lblTitle.Text = "Название";

        txtTitle.Location = new Point(592, 144);
        txtTitle.Name = "txtTitle";
        txtTitle.Size = new Size(324, 25);
        txtTitle.TabIndex = 7;

        lblEmoji.AutoSize = true;
        lblEmoji.Location = new Point(589, 180);
        lblEmoji.Name = "lblEmoji";
        lblEmoji.TabIndex = 8;
        lblEmoji.Tag = "muted";
        lblEmoji.Text = "Смайл";

        cmbEmoji.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbEmoji.Location = new Point(592, 204);
        cmbEmoji.Name = "cmbEmoji";
        cmbEmoji.Size = new Size(90, 25);
        cmbEmoji.TabIndex = 9;

        lblCategory.AutoSize = true;
        lblCategory.Location = new Point(697, 180);
        lblCategory.Name = "lblCategory";
        lblCategory.TabIndex = 10;
        lblCategory.Tag = "muted";
        lblCategory.Text = "Категория";

        cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbCategory.Location = new Point(700, 204);
        cmbCategory.Name = "cmbCategory";
        cmbCategory.Size = new Size(216, 25);
        cmbCategory.TabIndex = 11;

        lblDescription.AutoSize = true;
        lblDescription.Location = new Point(589, 242);
        lblDescription.Name = "lblDescription";
        lblDescription.TabIndex = 12;
        lblDescription.Tag = "muted";
        lblDescription.Text = "Описание (не обязательно)";

        txtDescription.Location = new Point(592, 266);
        txtDescription.Name = "txtDescription";
        txtDescription.Size = new Size(324, 25);
        txtDescription.TabIndex = 13;

        lblText.AutoSize = true;
        lblText.Location = new Point(589, 304);
        lblText.Name = "lblText";
        lblText.TabIndex = 14;
        lblText.Tag = "muted";
        lblText.Text = "Сообщение после выполнения";

        txtMessage.Location = new Point(592, 328);
        txtMessage.Multiline = true;
        txtMessage.Name = "txtMessage";
        txtMessage.Size = new Size(324, 178);
        txtMessage.TabIndex = 15;

        btnSend.Location = new Point(592, 520);
        btnSend.Name = "btnSend";
        btnSend.Size = new Size(324, 40);
        btnSend.TabIndex = 16;
        btnSend.Tag = "primary";
        btnSend.Text = "Отправить на рассмотрение";
        btnSend.Click += btnSend_Click;

        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(940, 582);
        Controls.Add(lblHeader);
        Controls.Add(lblSubtitle);
        Controls.Add(grid);
        Controls.Add(lblDetails);
        Controls.Add(btnWithdraw);
        Controls.Add(lblNew);
        Controls.Add(lblTitle);
        Controls.Add(txtTitle);
        Controls.Add(lblEmoji);
        Controls.Add(cmbEmoji);
        Controls.Add(lblCategory);
        Controls.Add(cmbCategory);
        Controls.Add(lblDescription);
        Controls.Add(txtDescription);
        Controls.Add(lblText);
        Controls.Add(txtMessage);
        Controls.Add(btnSend);
        Font = new Font("Segoe UI", 10F);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "ProposalsForm";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Мои заявки";
        ((System.ComponentModel.ISupportInitialize)grid).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    private Label lblHeader;
    private Label lblSubtitle;
    private DataGridView grid;
    private DataGridViewTextBoxColumn colEmoji;
    private DataGridViewTextBoxColumn colTitle;
    private DataGridViewTextBoxColumn colStatus;
    private DataGridViewTextBoxColumn colDate;
    private Label lblDetails;
    private Button btnWithdraw;
    private Label lblNew;
    private Label lblTitle;
    private TextBox txtTitle;
    private Label lblEmoji;
    private ComboBox cmbEmoji;
    private Label lblCategory;
    private ComboBox cmbCategory;
    private Label lblDescription;
    private TextBox txtDescription;
    private Label lblText;
    private TextBox txtMessage;
    private Button btnSend;
}
