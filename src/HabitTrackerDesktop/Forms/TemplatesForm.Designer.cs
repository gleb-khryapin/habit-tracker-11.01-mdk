namespace HabitTrackerDesktop.Forms;

partial class TemplatesForm
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
        colCategory = new DataGridViewTextBoxColumn();
        colDescription = new DataGridViewTextBoxColumn();
        lblMessage = new Label();
        btnDelete = new Button();
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
        btnAdd = new Button();
        ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
        SuspendLayout();

        lblHeader.AutoSize = true;
        lblHeader.Font = new Font("Segoe UI Semibold", 14F);
        lblHeader.Location = new Point(20, 16);
        lblHeader.Name = "lblHeader";
        lblHeader.TabIndex = 0;
        lblHeader.Text = "Предложенные привычки";

        lblSubtitle.AutoSize = true;
        lblSubtitle.Location = new Point(23, 48);
        lblSubtitle.Name = "lblSubtitle";
        lblSubtitle.TabIndex = 1;
        lblSubtitle.Tag = "muted";
        lblSubtitle.Text = "Пользователи выбирают из этого списка при создании привычки";

        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        grid.AllowUserToResizeRows = false;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.ColumnHeadersHeight = 38;
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        grid.Columns.AddRange(new DataGridViewColumn[] { colEmoji, colTitle, colCategory, colDescription });
        grid.Location = new Point(24, 88);
        grid.MultiSelect = false;
        grid.Name = "grid";
        grid.ReadOnly = true;
        grid.RowHeadersVisible = false;
        grid.RowTemplate.Height = 38;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.Size = new Size(540, 320);
        grid.TabIndex = 2;
        grid.SelectionChanged += grid_SelectionChanged;

        colEmoji.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        colEmoji.HeaderText = "";
        colEmoji.Name = "colEmoji";
        colEmoji.ReadOnly = true;
        colEmoji.SortMode = DataGridViewColumnSortMode.NotSortable;
        colEmoji.Width = 50;

        colTitle.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        colTitle.HeaderText = "Название";
        colTitle.Name = "colTitle";
        colTitle.ReadOnly = true;
        colTitle.SortMode = DataGridViewColumnSortMode.NotSortable;
        colTitle.Width = 150;

        colCategory.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        colCategory.HeaderText = "Категория";
        colCategory.Name = "colCategory";
        colCategory.ReadOnly = true;
        colCategory.SortMode = DataGridViewColumnSortMode.NotSortable;
        colCategory.Width = 130;

        colDescription.HeaderText = "Описание";
        colDescription.Name = "colDescription";
        colDescription.ReadOnly = true;
        colDescription.SortMode = DataGridViewColumnSortMode.NotSortable;

        lblMessage.Location = new Point(24, 420);
        lblMessage.Name = "lblMessage";
        lblMessage.Size = new Size(540, 86);
        lblMessage.TabIndex = 3;
        lblMessage.Tag = "muted";
        lblMessage.Text = "Сообщение выбранной привычки";

        btnDelete.Location = new Point(24, 516);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(200, 40);
        btnDelete.TabIndex = 4;
        btnDelete.Tag = "danger";
        btnDelete.Text = "Удалить выбранную";
        btnDelete.Click += btnDelete_Click;

        lblNew.AutoSize = true;
        lblNew.Font = new Font("Segoe UI Semibold", 11F);
        lblNew.Location = new Point(589, 88);
        lblNew.Name = "lblNew";
        lblNew.TabIndex = 5;
        lblNew.Text = "Добавить привычку";

        lblTitle.AutoSize = true;
        lblTitle.Location = new Point(589, 126);
        lblTitle.Name = "lblTitle";
        lblTitle.TabIndex = 6;
        lblTitle.Tag = "muted";
        lblTitle.Text = "Название";

        txtTitle.Location = new Point(592, 150);
        txtTitle.Name = "txtTitle";
        txtTitle.Size = new Size(324, 25);
        txtTitle.TabIndex = 7;

        lblEmoji.AutoSize = true;
        lblEmoji.Location = new Point(589, 190);
        lblEmoji.Name = "lblEmoji";
        lblEmoji.TabIndex = 8;
        lblEmoji.Tag = "muted";
        lblEmoji.Text = "Смайл";

        cmbEmoji.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbEmoji.Location = new Point(592, 214);
        cmbEmoji.Name = "cmbEmoji";
        cmbEmoji.Size = new Size(90, 25);
        cmbEmoji.TabIndex = 9;

        lblCategory.AutoSize = true;
        lblCategory.Location = new Point(699, 190);
        lblCategory.Name = "lblCategory";
        lblCategory.TabIndex = 10;
        lblCategory.Tag = "muted";
        lblCategory.Text = "Категория";

        cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbCategory.Location = new Point(702, 214);
        cmbCategory.Name = "cmbCategory";
        cmbCategory.Size = new Size(214, 25);
        cmbCategory.TabIndex = 11;

        lblDescription.AutoSize = true;
        lblDescription.Location = new Point(589, 254);
        lblDescription.Name = "lblDescription";
        lblDescription.TabIndex = 12;
        lblDescription.Tag = "muted";
        lblDescription.Text = "Описание (не обязательно)";

        txtDescription.Location = new Point(592, 278);
        txtDescription.Name = "txtDescription";
        txtDescription.Size = new Size(324, 25);
        txtDescription.TabIndex = 13;

        lblText.AutoSize = true;
        lblText.Location = new Point(589, 318);
        lblText.Name = "lblText";
        lblText.TabIndex = 14;
        lblText.Tag = "muted";
        lblText.Text = "Сообщение после выполнения";

        txtMessage.Location = new Point(592, 342);
        txtMessage.Multiline = true;
        txtMessage.Name = "txtMessage";
        txtMessage.Size = new Size(324, 154);
        txtMessage.TabIndex = 15;

        btnAdd.Location = new Point(592, 516);
        btnAdd.Name = "btnAdd";
        btnAdd.Size = new Size(324, 40);
        btnAdd.TabIndex = 16;
        btnAdd.Tag = "primary";
        btnAdd.Text = "Добавить в список";
        btnAdd.Click += btnAdd_Click;

        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(940, 578);
        Controls.Add(lblHeader);
        Controls.Add(lblSubtitle);
        Controls.Add(grid);
        Controls.Add(lblMessage);
        Controls.Add(btnDelete);
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
        Controls.Add(btnAdd);
        Font = new Font("Segoe UI", 10F);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "TemplatesForm";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Предложенные привычки";
        ((System.ComponentModel.ISupportInitialize)grid).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    private Label lblHeader;
    private Label lblSubtitle;
    private DataGridView grid;
    private DataGridViewTextBoxColumn colEmoji;
    private DataGridViewTextBoxColumn colTitle;
    private DataGridViewTextBoxColumn colCategory;
    private DataGridViewTextBoxColumn colDescription;
    private Label lblMessage;
    private Button btnDelete;
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
    private Button btnAdd;
}
