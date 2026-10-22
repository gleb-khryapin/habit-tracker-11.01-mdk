namespace HabitTrackerDesktop.Forms;

partial class ReviewForm
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
        colAuthor = new DataGridViewTextBoxColumn();
        colDate = new DataGridViewTextBoxColumn();
        lblSelected = new Label();
        lblDetails = new Label();
        lblReason = new Label();
        txtReason = new TextBox();
        btnApprove = new Button();
        btnReject = new Button();
        ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
        SuspendLayout();

        lblHeader.AutoSize = true;
        lblHeader.Font = new Font("Segoe UI Semibold", 14F);
        lblHeader.Location = new Point(20, 16);
        lblHeader.Name = "lblHeader";
        lblHeader.TabIndex = 0;
        lblHeader.Text = "Заявки пользователей";

        lblSubtitle.AutoSize = true;
        lblSubtitle.Location = new Point(23, 48);
        lblSubtitle.Name = "lblSubtitle";
        lblSubtitle.TabIndex = 1;
        lblSubtitle.Tag = "muted";
        lblSubtitle.Text = "Одобренная заявка сразу попадает в список предложенных привычек";

        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        grid.AllowUserToResizeRows = false;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.ColumnHeadersHeight = 38;
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        grid.Columns.AddRange(new DataGridViewColumn[] { colEmoji, colTitle, colAuthor, colDate });
        grid.Location = new Point(24, 88);
        grid.MultiSelect = false;
        grid.Name = "grid";
        grid.ReadOnly = true;
        grid.RowHeadersVisible = false;
        grid.RowTemplate.Height = 38;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.Size = new Size(540, 472);
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

        colAuthor.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        colAuthor.HeaderText = "Автор";
        colAuthor.Name = "colAuthor";
        colAuthor.ReadOnly = true;
        colAuthor.SortMode = DataGridViewColumnSortMode.NotSortable;
        colAuthor.Width = 120;

        colDate.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        colDate.HeaderText = "Отправлена";
        colDate.Name = "colDate";
        colDate.ReadOnly = true;
        colDate.SortMode = DataGridViewColumnSortMode.NotSortable;
        colDate.Width = 110;

        lblSelected.AutoSize = true;
        lblSelected.Font = new Font("Segoe UI Semibold", 11F);
        lblSelected.Location = new Point(589, 88);
        lblSelected.Name = "lblSelected";
        lblSelected.TabIndex = 3;
        lblSelected.Text = "Заявка";

        lblDetails.Location = new Point(592, 120);
        lblDetails.Name = "lblDetails";
        lblDetails.Size = new Size(324, 250);
        lblDetails.TabIndex = 4;
        lblDetails.Tag = "muted";
        lblDetails.Text = "Подробности выбранной заявки";

        lblReason.AutoSize = true;
        lblReason.Location = new Point(589, 384);
        lblReason.Name = "lblReason";
        lblReason.TabIndex = 5;
        lblReason.Tag = "muted";
        lblReason.Text = "Причина отказа (для «Отклонить»)";

        txtReason.Location = new Point(592, 408);
        txtReason.Multiline = true;
        txtReason.Name = "txtReason";
        txtReason.Size = new Size(324, 98);
        txtReason.TabIndex = 6;

        btnApprove.Location = new Point(592, 520);
        btnApprove.Name = "btnApprove";
        btnApprove.Size = new Size(156, 40);
        btnApprove.TabIndex = 7;
        btnApprove.Tag = "primary";
        btnApprove.Text = "Одобрить";
        btnApprove.Click += btnApprove_Click;

        btnReject.Location = new Point(760, 520);
        btnReject.Name = "btnReject";
        btnReject.Size = new Size(156, 40);
        btnReject.TabIndex = 8;
        btnReject.Tag = "danger";
        btnReject.Text = "Отклонить";
        btnReject.Click += btnReject_Click;

        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(940, 582);
        Controls.Add(lblHeader);
        Controls.Add(lblSubtitle);
        Controls.Add(grid);
        Controls.Add(lblSelected);
        Controls.Add(lblDetails);
        Controls.Add(lblReason);
        Controls.Add(txtReason);
        Controls.Add(btnApprove);
        Controls.Add(btnReject);
        Font = new Font("Segoe UI", 10F);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "ReviewForm";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Заявки пользователей";
        ((System.ComponentModel.ISupportInitialize)grid).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    private Label lblHeader;
    private Label lblSubtitle;
    private DataGridView grid;
    private DataGridViewTextBoxColumn colEmoji;
    private DataGridViewTextBoxColumn colTitle;
    private DataGridViewTextBoxColumn colAuthor;
    private DataGridViewTextBoxColumn colDate;
    private Label lblSelected;
    private Label lblDetails;
    private Label lblReason;
    private TextBox txtReason;
    private Button btnApprove;
    private Button btnReject;
}
