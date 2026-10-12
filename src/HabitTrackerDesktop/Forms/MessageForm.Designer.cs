namespace HabitTrackerDesktop.Forms;

partial class MessageForm
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
        lblEmoji = new Label();
        lblTitle = new Label();
        lblMessage = new Label();
        btnOk = new Button();
        SuspendLayout();

        lblEmoji.Font = new Font("Segoe UI Emoji", 30F);
        lblEmoji.Location = new Point(0, 20);
        lblEmoji.Name = "lblEmoji";
        lblEmoji.Size = new Size(460, 70);
        lblEmoji.TabIndex = 0;
        lblEmoji.Tag = "accent";
        lblEmoji.Text = "🎉";
        lblEmoji.TextAlign = ContentAlignment.MiddleCenter;

        lblTitle.Font = new Font("Segoe UI Semibold", 13F);
        lblTitle.Location = new Point(24, 96);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(412, 56);
        lblTitle.TabIndex = 1;
        lblTitle.Text = "Привычка выполнена!";
        lblTitle.TextAlign = ContentAlignment.MiddleCenter;

        lblMessage.Location = new Point(32, 158);
        lblMessage.Name = "lblMessage";
        lblMessage.Size = new Size(396, 110);
        lblMessage.TabIndex = 2;
        lblMessage.Tag = "muted";
        lblMessage.Text = "Сообщение";
        lblMessage.TextAlign = ContentAlignment.TopCenter;

        btnOk.DialogResult = DialogResult.OK;
        btnOk.Location = new Point(165, 282);
        btnOk.Name = "btnOk";
        btnOk.Size = new Size(130, 40);
        btnOk.TabIndex = 3;
        btnOk.Tag = "primary";
        btnOk.Text = "Спасибо!";

        AcceptButton = btnOk;
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(460, 346);
        Controls.Add(lblEmoji);
        Controls.Add(lblTitle);
        Controls.Add(lblMessage);
        Controls.Add(btnOk);
        Font = new Font("Segoe UI", 10F);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "MessageForm";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Привычка выполнена";
        ResumeLayout(false);
    }

    private Label lblEmoji;
    private Label lblTitle;
    private Label lblMessage;
    private Button btnOk;
}
