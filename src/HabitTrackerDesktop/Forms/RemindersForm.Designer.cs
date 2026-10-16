namespace HabitTrackerDesktop.Forms;

partial class RemindersForm
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
        colState = new DataGridViewTextBoxColumn();
        lblNew = new Label();
        numHour = new NumericUpDown();
        lblColon = new Label();
        numMinute = new NumericUpDown();
        btnAdd = new Button();
        lblHint = new Label();
        btnToggle = new Button();
        btnDelete = new Button();
        ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numHour).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numMinute).BeginInit();
        SuspendLayout();

        lblHeader.AutoSize = true;
        lblHeader.Font = new Font("Segoe UI Semibold", 14F);
        lblHeader.Location = new Point(20, 16);
        lblHeader.Name = "lblHeader";
        lblHeader.TabIndex = 0;
        lblHeader.Text = "Напоминания";

        lblSubtitle.Location = new Point(23, 48);
        lblSubtitle.Name = "lblSubtitle";
        lblSubtitle.Size = new Size(374, 24);
        lblSubtitle.TabIndex = 1;
        lblSubtitle.Tag = "muted";
        lblSubtitle.Text = "Привычка";

        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        grid.AllowUserToResizeRows = false;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.ColumnHeadersHeight = 38;
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        grid.Columns.AddRange(new DataGridViewColumn[] { colTime, colState });
        grid.Location = new Point(24, 88);
        grid.MultiSelect = false;
        grid.Name = "grid";
        grid.ReadOnly = true;
        grid.RowHeadersVisible = false;
        grid.RowTemplate.Height = 38;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.Size = new Size(372, 232);
        grid.TabIndex = 2;
        grid.SelectionChanged += grid_SelectionChanged;

        colTime.HeaderText = "Время";
        colTime.Name = "colTime";
        colTime.ReadOnly = true;
        colTime.SortMode = DataGridViewColumnSortMode.NotSortable;

        colState.HeaderText = "Состояние";
        colState.Name = "colState";
        colState.ReadOnly = true;
        colState.SortMode = DataGridViewColumnSortMode.NotSortable;

        lblNew.AutoSize = true;
        lblNew.Location = new Point(21, 334);
        lblNew.Name = "lblNew";
        lblNew.TabIndex = 3;
        lblNew.Tag = "muted";
        lblNew.Text = "Новое напоминание: часы и минуты";

        numHour.Location = new Point(24, 360);
        numHour.Maximum = new decimal(new int[] { 23, 0, 0, 0 });
        numHour.Name = "numHour";
        numHour.Size = new Size(70, 25);
        numHour.TabIndex = 4;
        numHour.Value = new decimal(new int[] { 20, 0, 0, 0 });

        lblColon.AutoSize = true;
        lblColon.Location = new Point(98, 362);
        lblColon.Name = "lblColon";
        lblColon.TabIndex = 5;
        lblColon.Text = ":";

        numMinute.Location = new Point(114, 360);
        numMinute.Maximum = new decimal(new int[] { 59, 0, 0, 0 });
        numMinute.Name = "numMinute";
        numMinute.Size = new Size(70, 25);
        numMinute.TabIndex = 6;

        btnAdd.Location = new Point(206, 354);
        btnAdd.Name = "btnAdd";
        btnAdd.Size = new Size(190, 36);
        btnAdd.TabIndex = 7;
        btnAdd.Tag = "primary";
        btnAdd.Text = "Добавить";
        btnAdd.Click += btnAdd_Click;

        lblHint.Location = new Point(24, 402);
        lblHint.Name = "lblHint";
        lblHint.Size = new Size(372, 44);
        lblHint.TabIndex = 8;
        lblHint.Tag = "muted";
        lblHint.Text = "Напоминание появится, если приложение открыто, а привычка сегодня ещё не отмечена.";

        btnToggle.Location = new Point(24, 458);
        btnToggle.Name = "btnToggle";
        btnToggle.Size = new Size(180, 40);
        btnToggle.TabIndex = 9;
        btnToggle.Text = "Выключить";
        btnToggle.Click += btnToggle_Click;

        btnDelete.Location = new Point(216, 458);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(180, 40);
        btnDelete.TabIndex = 10;
        btnDelete.Tag = "danger";
        btnDelete.Text = "Удалить";
        btnDelete.Click += btnDelete_Click;

        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(420, 520);
        Controls.Add(lblHeader);
        Controls.Add(lblSubtitle);
        Controls.Add(grid);
        Controls.Add(lblNew);
        Controls.Add(numHour);
        Controls.Add(lblColon);
        Controls.Add(numMinute);
        Controls.Add(btnAdd);
        Controls.Add(lblHint);
        Controls.Add(btnToggle);
        Controls.Add(btnDelete);
        Font = new Font("Segoe UI", 10F);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "RemindersForm";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Напоминания";
        ((System.ComponentModel.ISupportInitialize)grid).EndInit();
        ((System.ComponentModel.ISupportInitialize)numHour).EndInit();
        ((System.ComponentModel.ISupportInitialize)numMinute).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    private Label lblHeader;
    private Label lblSubtitle;
    private DataGridView grid;
    private DataGridViewTextBoxColumn colTime;
    private DataGridViewTextBoxColumn colState;
    private Label lblNew;
    private NumericUpDown numHour;
    private Label lblColon;
    private NumericUpDown numMinute;
    private Button btnAdd;
    private Label lblHint;
    private Button btnToggle;
    private Button btnDelete;
}
