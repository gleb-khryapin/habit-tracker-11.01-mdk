namespace HabitTrackerDesktop.Forms;

partial class CategoriesForm
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
        colName = new DataGridViewTextBoxColumn();
        lblNew = new Label();
        txtName = new TextBox();
        btnAdd = new Button();
        btnDelete = new Button();
        ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
        SuspendLayout();

        lblHeader.AutoSize = true;
        lblHeader.Font = new Font("Segoe UI Semibold", 14F);
        lblHeader.Location = new Point(20, 16);
        lblHeader.Name = "lblHeader";
        lblHeader.TabIndex = 0;
        lblHeader.Text = "Категории";

        lblSubtitle.AutoSize = true;
        lblSubtitle.Location = new Point(23, 48);
        lblSubtitle.Name = "lblSubtitle";
        lblSubtitle.TabIndex = 1;
        lblSubtitle.Tag = "muted";
        lblSubtitle.Text = "По категориям пользователи группируют привычки";

        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        grid.AllowUserToResizeRows = false;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.ColumnHeadersHeight = 38;
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        grid.Columns.AddRange(new DataGridViewColumn[] { colName });
        grid.Location = new Point(24, 88);
        grid.MultiSelect = false;
        grid.Name = "grid";
        grid.ReadOnly = true;
        grid.RowHeadersVisible = false;
        grid.RowTemplate.Height = 38;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.Size = new Size(412, 250);
        grid.TabIndex = 2;

        colName.HeaderText = "Название";
        colName.Name = "colName";
        colName.ReadOnly = true;
        colName.SortMode = DataGridViewColumnSortMode.NotSortable;

        lblNew.AutoSize = true;
        lblNew.Location = new Point(21, 352);
        lblNew.Name = "lblNew";
        lblNew.TabIndex = 3;
        lblNew.Tag = "muted";
        lblNew.Text = "Новая категория";

        txtName.Location = new Point(24, 378);
        txtName.Name = "txtName";
        txtName.Size = new Size(280, 25);
        txtName.TabIndex = 4;

        btnAdd.Location = new Point(316, 372);
        btnAdd.Name = "btnAdd";
        btnAdd.Size = new Size(120, 36);
        btnAdd.TabIndex = 5;
        btnAdd.Tag = "primary";
        btnAdd.Text = "Добавить";
        btnAdd.Click += btnAdd_Click;

        btnDelete.Location = new Point(24, 428);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(200, 40);
        btnDelete.TabIndex = 6;
        btnDelete.Tag = "danger";
        btnDelete.Text = "Удалить выбранную";
        btnDelete.Click += btnDelete_Click;

        AcceptButton = btnAdd;
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(460, 490);
        Controls.Add(lblHeader);
        Controls.Add(lblSubtitle);
        Controls.Add(grid);
        Controls.Add(lblNew);
        Controls.Add(txtName);
        Controls.Add(btnAdd);
        Controls.Add(btnDelete);
        Font = new Font("Segoe UI", 10F);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "CategoriesForm";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Категории";
        ((System.ComponentModel.ISupportInitialize)grid).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    private Label lblHeader;
    private Label lblSubtitle;
    private DataGridView grid;
    private DataGridViewTextBoxColumn colName;
    private Label lblNew;
    private TextBox txtName;
    private Button btnAdd;
    private Button btnDelete;
}
