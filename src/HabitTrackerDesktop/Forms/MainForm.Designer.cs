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
        components = new System.ComponentModel.Container();
        lblTitle = new Label();
        lblInfo = new Label();
        lblStreak = new Label();
        btnRequests = new Button();
        btnAudit = new Button();
        btnAchievements = new Button();
        btnLogout = new Button();
        cmbCategory = new ComboBox();
        btnTemplates = new Button();
        btnCategories = new Button();
        btnPhrases = new Button();
        grid = new DataGridView();
        colEmoji = new DataGridViewTextBoxColumn();
        colTitle = new DataGridViewTextBoxColumn();
        colCategory = new DataGridViewTextBoxColumn();
        colProgress = new DataGridViewTextBoxColumn();
        colHours = new DataGridViewTextBoxColumn();
        colStart = new DataGridViewTextBoxColumn();
        colStatus = new DataGridViewTextBoxColumn();
        lblDetails = new Label();
        btnCheckIn = new Button();
        btnReminders = new Button();
        btnAdd = new Button();
        btnEdit = new Button();
        btnDelete = new Button();
        timer = new System.Windows.Forms.Timer(components);
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

        lblStreak.Font = new Font("Segoe UI Semibold", 11F);
        lblStreak.Location = new Point(200, 22);
        lblStreak.Name = "lblStreak";
        lblStreak.Size = new Size(506, 28);
        lblStreak.TabIndex = 20;
        lblStreak.Text = "🔥 Огонёк";
        lblStreak.TextAlign = ContentAlignment.MiddleLeft;

        btnRequests.Location = new Point(256, 80);
        btnRequests.Name = "btnRequests";
        btnRequests.Size = new Size(130, 36);
        btnRequests.TabIndex = 21;
        btnRequests.Text = "Мои заявки";
        btnRequests.Click += btnRequests_Click;

        btnAudit.Location = new Point(396, 80);
        btnAudit.Name = "btnAudit";
        btnAudit.Size = new Size(110, 36);
        btnAudit.TabIndex = 22;
        btnAudit.Text = "Журнал";
        btnAudit.Click += btnAudit_Click;

        btnAchievements.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnAchievements.Location = new Point(716, 24);
        btnAchievements.Name = "btnAchievements";
        btnAchievements.Size = new Size(130, 36);
        btnAchievements.TabIndex = 2;
        btnAchievements.Text = "Достижения";
        btnAchievements.Click += btnAchievements_Click;

        btnLogout.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnLogout.Location = new Point(856, 24);
        btnLogout.Name = "btnLogout";
        btnLogout.Size = new Size(100, 36);
        btnLogout.TabIndex = 3;
        btnLogout.Text = "Выйти";
        btnLogout.Click += btnLogout_Click;

        cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbCategory.Location = new Point(24, 86);
        cmbCategory.Name = "cmbCategory";
        cmbCategory.Size = new Size(220, 25);
        cmbCategory.TabIndex = 4;
        cmbCategory.SelectedIndexChanged += cmbCategory_SelectedIndexChanged;

        btnTemplates.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnTemplates.Location = new Point(516, 80);
        btnTemplates.Name = "btnTemplates";
        btnTemplates.Size = new Size(210, 36);
        btnTemplates.TabIndex = 5;
        btnTemplates.Text = "Предложенные привычки";
        btnTemplates.Click += btnTemplates_Click;

        btnCategories.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnCategories.Location = new Point(736, 80);
        btnCategories.Name = "btnCategories";
        btnCategories.Size = new Size(110, 36);
        btnCategories.TabIndex = 6;
        btnCategories.Text = "Категории";
        btnCategories.Click += btnCategories_Click;

        btnPhrases.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnPhrases.Location = new Point(856, 80);
        btnPhrases.Name = "btnPhrases";
        btnPhrases.Size = new Size(100, 36);
        btnPhrases.TabIndex = 7;
        btnPhrases.Text = "Фразы";
        btnPhrases.Click += btnPhrases_Click;

        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        grid.AllowUserToResizeRows = false;
        grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.ColumnHeadersHeight = 38;
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        grid.Columns.AddRange(new DataGridViewColumn[] { colEmoji, colTitle, colCategory, colProgress, colHours, colStart, colStatus });
        grid.Location = new Point(24, 128);
        grid.MultiSelect = false;
        grid.Name = "grid";
        grid.ReadOnly = true;
        grid.RowHeadersVisible = false;
        grid.RowTemplate.Height = 38;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.Size = new Size(932, 372);
        grid.TabIndex = 8;
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

        colCategory.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        colCategory.HeaderText = "Категория";
        colCategory.Name = "colCategory";
        colCategory.ReadOnly = true;
        colCategory.SortMode = DataGridViewColumnSortMode.NotSortable;
        colCategory.Width = 150;

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
        lblDetails.Location = new Point(24, 512);
        lblDetails.Name = "lblDetails";
        lblDetails.Size = new Size(932, 62);
        lblDetails.TabIndex = 9;
        lblDetails.Tag = "muted";
        lblDetails.Text = "Подсказка по выбранной привычке";

        btnCheckIn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnCheckIn.Location = new Point(24, 592);
        btnCheckIn.Name = "btnCheckIn";
        btnCheckIn.Size = new Size(200, 40);
        btnCheckIn.TabIndex = 10;
        btnCheckIn.Tag = "primary";
        btnCheckIn.Text = "Отметить сегодня";
        btnCheckIn.Click += btnCheckIn_Click;

        btnReminders.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnReminders.Location = new Point(236, 592);
        btnReminders.Name = "btnReminders";
        btnReminders.Size = new Size(150, 40);
        btnReminders.TabIndex = 11;
        btnReminders.Text = "Напоминания";
        btnReminders.Click += btnReminders_Click;

        btnAdd.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnAdd.Location = new Point(602, 592);
        btnAdd.Name = "btnAdd";
        btnAdd.Size = new Size(110, 40);
        btnAdd.TabIndex = 12;
        btnAdd.Text = "Добавить";
        btnAdd.Click += btnAdd_Click;

        btnEdit.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnEdit.Location = new Point(724, 592);
        btnEdit.Name = "btnEdit";
        btnEdit.Size = new Size(110, 40);
        btnEdit.TabIndex = 13;
        btnEdit.Text = "Изменить";
        btnEdit.Click += btnEdit_Click;

        btnDelete.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnDelete.Location = new Point(846, 592);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(110, 40);
        btnDelete.TabIndex = 14;
        btnDelete.Tag = "danger";
        btnDelete.Text = "Удалить";
        btnDelete.Click += btnDelete_Click;

        timer.Enabled = true;
        timer.Interval = 20000;
        timer.Tick += timer_Tick;

        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(980, 652);
        Controls.Add(lblTitle);
        Controls.Add(lblInfo);
        Controls.Add(lblStreak);
        Controls.Add(btnRequests);
        Controls.Add(btnAudit);
        Controls.Add(btnAchievements);
        Controls.Add(btnLogout);
        Controls.Add(cmbCategory);
        Controls.Add(btnTemplates);
        Controls.Add(btnCategories);
        Controls.Add(btnPhrases);
        Controls.Add(grid);
        Controls.Add(lblDetails);
        Controls.Add(btnCheckIn);
        Controls.Add(btnReminders);
        Controls.Add(btnAdd);
        Controls.Add(btnEdit);
        Controls.Add(btnDelete);
        Font = new Font("Segoe UI", 10F);
        MinimumSize = new Size(996, 600);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Привычки";
        ((System.ComponentModel.ISupportInitialize)grid).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    private Label lblTitle;
    private Label lblInfo;
    private Label lblStreak;
    private Button btnRequests;
    private Button btnAudit;
    private Button btnAchievements;
    private Button btnLogout;
    private ComboBox cmbCategory;
    private Button btnTemplates;
    private Button btnCategories;
    private Button btnPhrases;
    private DataGridView grid;
    private DataGridViewTextBoxColumn colEmoji;
    private DataGridViewTextBoxColumn colTitle;
    private DataGridViewTextBoxColumn colCategory;
    private DataGridViewTextBoxColumn colProgress;
    private DataGridViewTextBoxColumn colHours;
    private DataGridViewTextBoxColumn colStart;
    private DataGridViewTextBoxColumn colStatus;
    private Label lblDetails;
    private Button btnCheckIn;
    private Button btnReminders;
    private Button btnAdd;
    private Button btnEdit;
    private Button btnDelete;
    private System.Windows.Forms.Timer timer;
}
