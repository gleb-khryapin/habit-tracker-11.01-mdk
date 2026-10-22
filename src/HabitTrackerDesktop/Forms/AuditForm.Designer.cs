namespace HabitTrackerDesktop.Forms;

partial class AuditForm
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
        colTime = new DataGridViewTextBoxColumn();
        colAction = new DataGridViewTextBoxColumn();
        colTable = new DataGridViewTextBoxColumn();
        colDetails = new DataGridViewTextBoxColumn();
        btnBackup = new Button();
        btnRestore = new Button();
        ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
        SuspendLayout();

        lblHeader.AutoSize = true;
        lblHeader.Font = new Font("Segoe UI Semibold", 14F);
        lblHeader.Location = new Point(20, 16);
        lblHeader.Name = "lblHeader";
        lblHeader.TabIndex = 0;
        lblHeader.Text = "Журнал действий";

        lblSubtitle.AutoSize = true;
        lblSubtitle.Location = new Point(23, 48);
        lblSubtitle.Name = "lblSubtitle";
        lblSubtitle.TabIndex = 1;
        lblSubtitle.Tag = "muted";
        lblSubtitle.Text = "Записи создают триггеры базы данных";

        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        grid.AllowUserToResizeRows = false;
        grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.ColumnHeadersHeight = 38;
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        grid.Columns.AddRange(new DataGridViewColumn[] { colTime, colAction, colTable, colDetails });
        grid.Location = new Point(24, 88);
        grid.MultiSelect = false;
        grid.Name = "grid";
        grid.ReadOnly = true;
        grid.RowHeadersVisible = false;
        grid.RowTemplate.Height = 34;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.Size = new Size(932, 440);
        grid.TabIndex = 2;

        colTime.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        colTime.HeaderText = "Время";
        colTime.Name = "colTime";
        colTime.ReadOnly = true;
        colTime.SortMode = DataGridViewColumnSortMode.NotSortable;
        colTime.Width = 160;

        colAction.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        colAction.HeaderText = "Действие";
        colAction.Name = "colAction";
        colAction.ReadOnly = true;
        colAction.SortMode = DataGridViewColumnSortMode.NotSortable;
        colAction.Width = 110;

        colTable.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        colTable.HeaderText = "Таблица";
        colTable.Name = "colTable";
        colTable.ReadOnly = true;
        colTable.SortMode = DataGridViewColumnSortMode.NotSortable;
        colTable.Width = 140;

        colDetails.HeaderText = "Что произошло";
        colDetails.Name = "colDetails";
        colDetails.ReadOnly = true;
        colDetails.SortMode = DataGridViewColumnSortMode.NotSortable;

        btnBackup.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnBackup.Location = new Point(24, 544);
        btnBackup.Name = "btnBackup";
        btnBackup.Size = new Size(240, 40);
        btnBackup.TabIndex = 3;
        btnBackup.Tag = "primary";
        btnBackup.Text = "Сделать резервную копию";
        btnBackup.Click += btnBackup_Click;

        btnRestore.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnRestore.Location = new Point(276, 544);
        btnRestore.Name = "btnRestore";
        btnRestore.Size = new Size(240, 40);
        btnRestore.TabIndex = 4;
        btnRestore.Tag = "danger";
        btnRestore.Text = "Восстановить из копии";
        btnRestore.Click += btnRestore_Click;

        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(980, 606);
        Controls.Add(lblHeader);
        Controls.Add(lblSubtitle);
        Controls.Add(grid);
        Controls.Add(btnBackup);
        Controls.Add(btnRestore);
        Font = new Font("Segoe UI", 10F);
        MinimizeBox = false;
        MinimumSize = new Size(700, 400);
        Name = "AuditForm";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Журнал действий";
        ((System.ComponentModel.ISupportInitialize)grid).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    private Label lblHeader;
    private Label lblSubtitle;
    private DataGridView grid;
    private DataGridViewTextBoxColumn colTime;
    private DataGridViewTextBoxColumn colAction;
    private DataGridViewTextBoxColumn colTable;
    private DataGridViewTextBoxColumn colDetails;
    private Button btnBackup;
    private Button btnRestore;
}
