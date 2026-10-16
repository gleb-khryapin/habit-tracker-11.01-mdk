namespace HabitTrackerDesktop.Forms;

partial class AchievementsForm
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
        colDescription = new DataGridViewTextBoxColumn();
        colDate = new DataGridViewTextBoxColumn();
        btnClose = new Button();
        ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
        SuspendLayout();

        lblHeader.AutoSize = true;
        lblHeader.Font = new Font("Segoe UI Semibold", 14F);
        lblHeader.Location = new Point(20, 16);
        lblHeader.Name = "lblHeader";
        lblHeader.TabIndex = 0;
        lblHeader.Text = "Достижения";

        lblSubtitle.AutoSize = true;
        lblSubtitle.Location = new Point(23, 48);
        lblSubtitle.Name = "lblSubtitle";
        lblSubtitle.TabIndex = 1;
        lblSubtitle.Tag = "muted";
        lblSubtitle.Text = "Получено: 0 из 0";

        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        grid.AllowUserToResizeRows = false;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.ColumnHeadersHeight = 38;
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        grid.Columns.AddRange(new DataGridViewColumn[] { colEmoji, colTitle, colDescription, colDate });
        grid.Location = new Point(24, 88);
        grid.MultiSelect = false;
        grid.Name = "grid";
        grid.ReadOnly = true;
        grid.RowHeadersVisible = false;
        grid.RowTemplate.Height = 38;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.Size = new Size(672, 270);
        grid.TabIndex = 2;

        colEmoji.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        colEmoji.HeaderText = "";
        colEmoji.Name = "colEmoji";
        colEmoji.ReadOnly = true;
        colEmoji.SortMode = DataGridViewColumnSortMode.NotSortable;
        colEmoji.Width = 50;

        colTitle.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        colTitle.HeaderText = "Достижение";
        colTitle.Name = "colTitle";
        colTitle.ReadOnly = true;
        colTitle.SortMode = DataGridViewColumnSortMode.NotSortable;
        colTitle.Width = 170;

        colDescription.HeaderText = "Условие";
        colDescription.Name = "colDescription";
        colDescription.ReadOnly = true;
        colDescription.SortMode = DataGridViewColumnSortMode.NotSortable;

        colDate.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        colDate.HeaderText = "Получено";
        colDate.Name = "colDate";
        colDate.ReadOnly = true;
        colDate.SortMode = DataGridViewColumnSortMode.NotSortable;
        colDate.Width = 120;

        btnClose.DialogResult = DialogResult.Cancel;
        btnClose.Location = new Point(576, 374);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(120, 40);
        btnClose.TabIndex = 3;
        btnClose.Text = "Закрыть";

        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        CancelButton = btnClose;
        ClientSize = new Size(720, 436);
        Controls.Add(lblHeader);
        Controls.Add(lblSubtitle);
        Controls.Add(grid);
        Controls.Add(btnClose);
        Font = new Font("Segoe UI", 10F);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "AchievementsForm";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Достижения";
        ((System.ComponentModel.ISupportInitialize)grid).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    private Label lblHeader;
    private Label lblSubtitle;
    private DataGridView grid;
    private DataGridViewTextBoxColumn colEmoji;
    private DataGridViewTextBoxColumn colTitle;
    private DataGridViewTextBoxColumn colDescription;
    private DataGridViewTextBoxColumn colDate;
    private Button btnClose;
}
