namespace HabitTrackerDesktop.Forms;

partial class HabitForm
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
        rbTemplate = new RadioButton();
        rbCustom = new RadioButton();
        pnlTemplate = new Panel();
        lblTemplate = new Label();
        cmbTemplate = new ComboBox();
        lblDescription = new Label();
        pnlCustom = new Panel();
        lblTitle = new Label();
        txtTitle = new TextBox();
        lblEmoji = new Label();
        cmbEmoji = new ComboBox();
        lblCategory = new Label();
        cmbCategory = new ComboBox();
        lblDays = new Label();
        numDays = new NumericUpDown();
        chkHours = new CheckBox();
        numHours = new NumericUpDown();
        lblProgress = new Label();
        btnSave = new Button();
        btnCancel = new Button();
        pnlTemplate.SuspendLayout();
        pnlCustom.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numDays).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numHours).BeginInit();
        SuspendLayout();

        lblHeader.AutoSize = true;
        lblHeader.Font = new Font("Segoe UI Semibold", 14F);
        lblHeader.Location = new Point(20, 18);
        lblHeader.Name = "lblHeader";
        lblHeader.TabIndex = 0;
        lblHeader.Text = "Новая привычка";

        rbTemplate.AutoSize = true;
        rbTemplate.Checked = true;
        rbTemplate.Location = new Point(26, 66);
        rbTemplate.Name = "rbTemplate";
        rbTemplate.TabIndex = 1;
        rbTemplate.TabStop = true;
        rbTemplate.Text = "Из списка";
        rbTemplate.CheckedChanged += rbTemplate_CheckedChanged;

        rbCustom.AutoSize = true;
        rbCustom.Location = new Point(150, 66);
        rbCustom.Name = "rbCustom";
        rbCustom.TabIndex = 2;
        rbCustom.Text = "Своя";

        pnlTemplate.Controls.Add(lblTemplate);
        pnlTemplate.Controls.Add(cmbTemplate);
        pnlTemplate.Controls.Add(lblDescription);
        pnlTemplate.Location = new Point(24, 104);
        pnlTemplate.Name = "pnlTemplate";
        pnlTemplate.Size = new Size(372, 132);
        pnlTemplate.TabIndex = 3;

        lblTemplate.AutoSize = true;
        lblTemplate.Location = new Point(-3, 0);
        lblTemplate.Name = "lblTemplate";
        lblTemplate.TabIndex = 0;
        lblTemplate.Tag = "muted";
        lblTemplate.Text = "Привычка";

        cmbTemplate.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbTemplate.Location = new Point(0, 24);
        cmbTemplate.Name = "cmbTemplate";
        cmbTemplate.Size = new Size(372, 25);
        cmbTemplate.TabIndex = 1;
        cmbTemplate.SelectedIndexChanged += cmbTemplate_SelectedIndexChanged;

        lblDescription.Location = new Point(0, 62);
        lblDescription.Name = "lblDescription";
        lblDescription.Size = new Size(372, 62);
        lblDescription.TabIndex = 2;
        lblDescription.Tag = "muted";

        pnlCustom.Controls.Add(lblTitle);
        pnlCustom.Controls.Add(txtTitle);
        pnlCustom.Controls.Add(lblEmoji);
        pnlCustom.Controls.Add(cmbEmoji);
        pnlCustom.Controls.Add(lblCategory);
        pnlCustom.Controls.Add(cmbCategory);
        pnlCustom.Location = new Point(24, 104);
        pnlCustom.Name = "pnlCustom";
        pnlCustom.Size = new Size(372, 132);
        pnlCustom.TabIndex = 4;
        pnlCustom.Visible = false;

        lblTitle.AutoSize = true;
        lblTitle.Location = new Point(-3, 0);
        lblTitle.Name = "lblTitle";
        lblTitle.TabIndex = 0;
        lblTitle.Tag = "muted";
        lblTitle.Text = "Название";

        txtTitle.Location = new Point(0, 24);
        txtTitle.Name = "txtTitle";
        txtTitle.Size = new Size(372, 25);
        txtTitle.TabIndex = 1;

        lblEmoji.AutoSize = true;
        lblEmoji.Location = new Point(-3, 66);
        lblEmoji.Name = "lblEmoji";
        lblEmoji.TabIndex = 2;
        lblEmoji.Tag = "muted";
        lblEmoji.Text = "Смайл";

        cmbEmoji.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbEmoji.Location = new Point(0, 90);
        cmbEmoji.Name = "cmbEmoji";
        cmbEmoji.Size = new Size(90, 25);
        cmbEmoji.TabIndex = 3;

        lblCategory.AutoSize = true;
        lblCategory.Location = new Point(117, 66);
        lblCategory.Name = "lblCategory";
        lblCategory.TabIndex = 4;
        lblCategory.Tag = "muted";
        lblCategory.Text = "Категория";

        cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbCategory.Location = new Point(120, 90);
        cmbCategory.Name = "cmbCategory";
        cmbCategory.Size = new Size(252, 25);
        cmbCategory.TabIndex = 5;

        lblDays.AutoSize = true;
        lblDays.Location = new Point(21, 248);
        lblDays.Name = "lblDays";
        lblDays.TabIndex = 5;
        lblDays.Tag = "muted";
        lblDays.Text = "Сколько дней подряд";

        numDays.Location = new Point(24, 274);
        numDays.Maximum = new decimal(new int[] { 365, 0, 0, 0 });
        numDays.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numDays.Name = "numDays";
        numDays.Size = new Size(120, 25);
        numDays.TabIndex = 6;
        numDays.Value = new decimal(new int[] { 21, 0, 0, 0 });

        chkHours.AutoSize = true;
        chkHours.Location = new Point(204, 246);
        chkHours.Name = "chkHours";
        chkHours.TabIndex = 7;
        chkHours.Text = "Указать часы в день";
        chkHours.CheckedChanged += chkHours_CheckedChanged;

        numHours.DecimalPlaces = 1;
        numHours.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
        numHours.Location = new Point(204, 274);
        numHours.Maximum = new decimal(new int[] { 24, 0, 0, 0 });
        numHours.Minimum = new decimal(new int[] { 5, 0, 0, 65536 });
        numHours.Name = "numHours";
        numHours.Size = new Size(120, 25);
        numHours.TabIndex = 8;
        numHours.Value = new decimal(new int[] { 1, 0, 0, 0 });
        numHours.Visible = false;

        lblProgress.Location = new Point(24, 316);
        lblProgress.Name = "lblProgress";
        lblProgress.Size = new Size(372, 44);
        lblProgress.TabIndex = 9;
        lblProgress.Tag = "muted";
        lblProgress.Visible = false;

        btnSave.Location = new Point(176, 376);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(110, 40);
        btnSave.TabIndex = 10;
        btnSave.Tag = "primary";
        btnSave.Text = "Создать";
        btnSave.Click += btnSave_Click;

        btnCancel.DialogResult = DialogResult.Cancel;
        btnCancel.Location = new Point(296, 376);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(100, 40);
        btnCancel.TabIndex = 11;
        btnCancel.Text = "Отмена";

        AcceptButton = btnSave;
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        CancelButton = btnCancel;
        ClientSize = new Size(420, 438);
        Controls.Add(lblHeader);
        Controls.Add(rbTemplate);
        Controls.Add(rbCustom);
        Controls.Add(pnlTemplate);
        Controls.Add(pnlCustom);
        Controls.Add(lblDays);
        Controls.Add(numDays);
        Controls.Add(chkHours);
        Controls.Add(numHours);
        Controls.Add(lblProgress);
        Controls.Add(btnSave);
        Controls.Add(btnCancel);
        Font = new Font("Segoe UI", 10F);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "HabitForm";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Новая привычка";
        pnlTemplate.ResumeLayout(false);
        pnlTemplate.PerformLayout();
        pnlCustom.ResumeLayout(false);
        pnlCustom.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numDays).EndInit();
        ((System.ComponentModel.ISupportInitialize)numHours).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    private Label lblHeader;
    private RadioButton rbTemplate;
    private RadioButton rbCustom;
    private Panel pnlTemplate;
    private Label lblTemplate;
    private ComboBox cmbTemplate;
    private Label lblDescription;
    private Panel pnlCustom;
    private Label lblTitle;
    private TextBox txtTitle;
    private Label lblEmoji;
    private ComboBox cmbEmoji;
    private Label lblCategory;
    private ComboBox cmbCategory;
    private Label lblDays;
    private NumericUpDown numDays;
    private CheckBox chkHours;
    private NumericUpDown numHours;
    private Label lblProgress;
    private Button btnSave;
    private Button btnCancel;
}
