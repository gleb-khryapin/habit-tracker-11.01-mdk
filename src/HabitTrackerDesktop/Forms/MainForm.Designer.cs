namespace HabitTrackerDesktop.Forms;

partial class MainForm
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
        lblTitle = new Label();
        lblInfo = new Label();
        btnTemplates = new Button();
        btnLogout = new Button();
        grid = new DataGridView();
        colEmoji = new DataGridViewTextBoxColumn();
        colTitle = new DataGridViewTextBoxColumn();
        colProgress = new DataGridViewTextBoxColumn();
        colHours = new DataGridViewTextBoxColumn();
        colStart = new DataGridViewTextBoxColumn();
        colStatus = new DataGridViewTextBoxColumn();
        lblDetails = new Label();
        btnCheckIn = new Button();
        btnAdd = new Button();
        btnEdit = new Button();
        btnDelete = new Button();
        ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
        SuspendLayout();

        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI Semibold", 16F);
        lblTitle.Location = new Point(20, 16);
        lblTitle.Name = "lblTitle";
        lblTitle.TabIndex = 0;
        lblTitle.Text = "Мои привычки";

        lblInfo.AutoSize = true;
        lblInfo.Location = new Point(24, 52);
        lblInfo.Name = "lblInfo";
        lblInfo.TabIndex = 1;
        lblInfo.Tag = "muted";
        lblInfo.Text = "demo · сегодня 01.01.2026";

        btnTemplates.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnTemplates.Location = new Point(636, 24);
        btnTemplates.Name = "btnTemplates";
        btnTemplates.Size = new Size(210, 36);
        btnTemplates.TabIndex = 2;
        btnTemplates.Text = "Предложенные привычки";
        btnTemplates.Click += btnTemplates_Click;

        btnLogout.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnLogout.Location = new Point(856, 24);
        btnLogout.Name = "btnLogout";
        btnLogout.Size = new Size(100, 36);
        btnLogout.TabIndex = 3;
        btnLogout.Text = "Выйти";
        btnLogout.Click += btnLogout_Click;

        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        grid.AllowUserToResizeRows = false;
        grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.ColumnHeadersHeight = 38;
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        grid.Columns.AddRange(new DataGridViewColumn[] { colEmoji, colTitle, colProgress, colHours, colStart, colStatus });
        grid.Location = new Point(24, 92);
        grid.MultiSelect = false;
        grid.Name = "grid";
        grid.ReadOnly = true;
        grid.RowHeadersVisible = false;
        grid.RowTemplate.Height = 38;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.Size = new Size(932, 372);
        grid.TabIndex = 4;
        grid.SelectionChanged += grid_SelectionChanged;

        colEmoji.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        colEmoji.HeaderText = "";
        colEmoji.Name = "colEmoji";
        colEmoji.ReadOnly = true;
        colEmoji.SortMode = DataGridViewColumnSortMode.NotSortable;
        colEmoji.Width = 50;

        colTitle.HeaderText = "Привычка";
        colTitle.Name = "colTitle";
        colTitle.ReadOnly = true;
        colTitle.SortMode = DataGridViewColumnSortMode.NotSortable;

        colProgress.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        colProgress.HeaderText = "Прогресс";
        colProgress.Name = "colProgress";
        colProgress.ReadOnly = true;
        colProgress.SortMode = DataGridViewColumnSortMode.NotSortable;
        colProgress.Width = 110;

        colHours.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        colHours.HeaderText = "Часов в день";
        colHours.Name = "colHours";
        colHours.ReadOnly = true;
        colHours.SortMode = DataGridViewColumnSortMode.NotSortable;
        colHours.Width = 120;

        colStart.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        colStart.HeaderText = "Начата";
        colStart.Name = "colStart";
        colStart.ReadOnly = true;
        colStart.SortMode = DataGridViewColumnSortMode.NotSortable;
        colStart.Width = 120;

        colStatus.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        colStatus.HeaderText = "Статус";
        colStatus.Name = "colStatus";
        colStatus.ReadOnly = true;
        colStatus.SortMode = DataGridViewColumnSortMode.NotSortable;
        colStatus.Width = 130;

        lblDetails.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        lblDetails.Location = new Point(24, 476);
        lblDetails.Name = "lblDetails";
        lblDetails.Size = new Size(932, 62);
        lblDetails.TabIndex = 5;
        lblDetails.Tag = "muted";
        lblDetails.Text = "Подсказка по выбранной привычке";

        btnCheckIn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnCheckIn.Location = new Point(24, 556);
        btnCheckIn.Name = "btnCheckIn";
        btnCheckIn.Size = new Size(200, 40);
        btnCheckIn.TabIndex = 6;
        btnCheckIn.Tag = "primary";
        btnCheckIn.Text = "Отметить сегодня";
        btnCheckIn.Click += btnCheckIn_Click;

        btnAdd.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnAdd.Location = new Point(602, 556);
        btnAdd.Name = "btnAdd";
        btnAdd.Size = new Size(110, 40);
        btnAdd.TabIndex = 7;
        btnAdd.Text = "Добавить";
        btnAdd.Click += btnAdd_Click;

        btnEdit.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnEdit.Location = new Point(724, 556);
        btnEdit.Name = "btnEdit";
        btnEdit.Size = new Size(110, 40);
        btnEdit.TabIndex = 8;
        btnEdit.Text = "Изменить";
        btnEdit.Click += btnEdit_Click;

        btnDelete.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnDelete.Location = new Point(846, 556);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(110, 40);
        btnDelete.TabIndex = 9;
        btnDelete.Tag = "danger";
        btnDelete.Text = "Удалить";
        btnDelete.Click += btnDelete_Click;

        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(980, 616);
        Controls.Add(lblTitle);
        Controls.Add(lblInfo);
        Controls.Add(btnTemplates);
        Controls.Add(btnLogout);
        Controls.Add(grid);
        Controls.Add(lblDetails);
        Controls.Add(btnCheckIn);
        Controls.Add(btnAdd);
        Controls.Add(btnEdit);
        Controls.Add(btnDelete);
        Font = new Font("Segoe UI", 10F);
        MinimumSize = new Size(860, 520);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Привычки";
        ((System.ComponentModel.ISupportInitialize)grid).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    private Label lblTitle;
    private Label lblInfo;
    private Button btnTemplates;
    private Button btnLogout;
    private DataGridView grid;
    private DataGridViewTextBoxColumn colEmoji;
    private DataGridViewTextBoxColumn colTitle;
    private DataGridViewTextBoxColumn colProgress;
    private DataGridViewTextBoxColumn colHours;
    private DataGridViewTextBoxColumn colStart;
    private DataGridViewTextBoxColumn colStatus;
    private Label lblDetails;
    private Button btnCheckIn;
    private Button btnAdd;
    private Button btnEdit;
    private Button btnDelete;
}
