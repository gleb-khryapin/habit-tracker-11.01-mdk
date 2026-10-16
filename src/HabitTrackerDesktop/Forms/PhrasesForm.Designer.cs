namespace HabitTrackerDesktop.Forms;

partial class PhrasesForm
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
        colText = new DataGridViewTextBoxColumn();
        lblPhrase = new Label();
        lblNew = new Label();
        txtText = new TextBox();
        btnDelete = new Button();
        btnAdd = new Button();
        ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
        SuspendLayout();

        lblHeader.AutoSize = true;
        lblHeader.Font = new Font("Segoe UI Semibold", 14F);
        lblHeader.Location = new Point(20, 16);
        lblHeader.Name = "lblHeader";
        lblHeader.TabIndex = 0;
        lblHeader.Text = "Мотивирующие фразы";

        lblSubtitle.AutoSize = true;
        lblSubtitle.Location = new Point(23, 48);
        lblSubtitle.Name = "lblSubtitle";
        lblSubtitle.TabIndex = 1;
        lblSubtitle.Tag = "muted";
        lblSubtitle.Text = "Общий пул: одна из фраз закрепляется за своей привычкой при создании";

        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        grid.AllowUserToResizeRows = false;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.ColumnHeadersHeight = 38;
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        grid.Columns.AddRange(new DataGridViewColumn[] { colText });
        grid.Location = new Point(24, 88);
        grid.MultiSelect = false;
        grid.Name = "grid";
        grid.ReadOnly = true;
        grid.RowHeadersVisible = false;
        grid.RowTemplate.Height = 38;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.Size = new Size(612, 232);
        grid.TabIndex = 2;
        grid.SelectionChanged += grid_SelectionChanged;

        colText.HeaderText = "Фраза";
        colText.Name = "colText";
        colText.ReadOnly = true;
        colText.SortMode = DataGridViewColumnSortMode.NotSortable;

        lblPhrase.Location = new Point(24, 332);
        lblPhrase.Name = "lblPhrase";
        lblPhrase.Size = new Size(612, 66);
        lblPhrase.TabIndex = 3;
        lblPhrase.Tag = "muted";
        lblPhrase.Text = "Текст выбранной фразы";

        lblNew.AutoSize = true;
        lblNew.Location = new Point(21, 406);
        lblNew.Name = "lblNew";
        lblNew.TabIndex = 4;
        lblNew.Tag = "muted";
        lblNew.Text = "Новая фраза";

        txtText.Location = new Point(24, 432);
        txtText.Multiline = true;
        txtText.Name = "txtText";
        txtText.Size = new Size(612, 80);
        txtText.TabIndex = 5;

        btnDelete.Location = new Point(24, 530);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(200, 40);
        btnDelete.TabIndex = 6;
        btnDelete.Tag = "danger";
        btnDelete.Text = "Удалить выбранную";
        btnDelete.Click += btnDelete_Click;

        btnAdd.Location = new Point(436, 530);
        btnAdd.Name = "btnAdd";
        btnAdd.Size = new Size(200, 40);
        btnAdd.TabIndex = 7;
        btnAdd.Tag = "primary";
        btnAdd.Text = "Добавить в пул";
        btnAdd.Click += btnAdd_Click;

        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(660, 592);
        Controls.Add(lblHeader);
        Controls.Add(lblSubtitle);
        Controls.Add(grid);
        Controls.Add(lblPhrase);
        Controls.Add(lblNew);
        Controls.Add(txtText);
        Controls.Add(btnDelete);
        Controls.Add(btnAdd);
        Font = new Font("Segoe UI", 10F);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "PhrasesForm";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Мотивирующие фразы";
        ((System.ComponentModel.ISupportInitialize)grid).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    private Label lblHeader;
    private Label lblSubtitle;
    private DataGridView grid;
    private DataGridViewTextBoxColumn colText;
    private Label lblPhrase;
    private Label lblNew;
    private TextBox txtText;
    private Button btnDelete;
    private Button btnAdd;
}
