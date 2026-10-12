namespace HabitTrackerDesktop.Forms;

partial class LoginForm
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
        lblLogo = new Label();
        lblTitle = new Label();
        lblSubtitle = new Label();
        lblUsername = new Label();
        txtUsername = new TextBox();
        lblPassword = new Label();
        txtPassword = new TextBox();
        btnLogin = new Button();
        btnRegister = new Button();
        lblHint = new Label();
        SuspendLayout();

        lblLogo.Font = new Font("Segoe UI", 28F);
        lblLogo.Location = new Point(0, 36);
        lblLogo.Name = "lblLogo";
        lblLogo.Size = new Size(380, 60);
        lblLogo.TabIndex = 0;
        lblLogo.Tag = "accent";
        lblLogo.Text = "✦";
        lblLogo.TextAlign = ContentAlignment.MiddleCenter;

        lblTitle.Font = new Font("Segoe UI Semibold", 18F);
        lblTitle.Location = new Point(0, 100);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(380, 40);
        lblTitle.TabIndex = 1;
        lblTitle.Text = "Привычки";
        lblTitle.TextAlign = ContentAlignment.MiddleCenter;

        lblSubtitle.Location = new Point(0, 142);
        lblSubtitle.Name = "lblSubtitle";
        lblSubtitle.Size = new Size(380, 24);
        lblSubtitle.TabIndex = 2;
        lblSubtitle.Tag = "muted";
        lblSubtitle.Text = "Маленькие шаги каждый день";
        lblSubtitle.TextAlign = ContentAlignment.MiddleCenter;

        lblUsername.AutoSize = true;
        lblUsername.Location = new Point(47, 196);
        lblUsername.Name = "lblUsername";
        lblUsername.TabIndex = 3;
        lblUsername.Tag = "muted";
        lblUsername.Text = "Логин";

        txtUsername.Location = new Point(50, 220);
        txtUsername.Name = "txtUsername";
        txtUsername.Size = new Size(280, 25);
        txtUsername.TabIndex = 4;

        lblPassword.AutoSize = true;
        lblPassword.Location = new Point(47, 262);
        lblPassword.Name = "lblPassword";
        lblPassword.TabIndex = 5;
        lblPassword.Tag = "muted";
        lblPassword.Text = "Пароль";

        txtPassword.Location = new Point(50, 286);
        txtPassword.Name = "txtPassword";
        txtPassword.Size = new Size(280, 25);
        txtPassword.TabIndex = 6;
        txtPassword.UseSystemPasswordChar = true;

        btnLogin.Location = new Point(50, 336);
        btnLogin.Name = "btnLogin";
        btnLogin.Size = new Size(280, 40);
        btnLogin.TabIndex = 7;
        btnLogin.Tag = "primary";
        btnLogin.Text = "Войти";
        btnLogin.Click += btnLogin_Click;

        btnRegister.Location = new Point(50, 386);
        btnRegister.Name = "btnRegister";
        btnRegister.Size = new Size(280, 40);
        btnRegister.TabIndex = 8;
        btnRegister.Text = "Зарегистрироваться";
        btnRegister.Click += btnRegister_Click;

        lblHint.Location = new Point(0, 448);
        lblHint.Name = "lblHint";
        lblHint.Size = new Size(380, 24);
        lblHint.TabIndex = 9;
        lblHint.Tag = "muted";
        lblHint.Text = "Для проверки: demo / demo123 · admin / admin123";
        lblHint.TextAlign = ContentAlignment.MiddleCenter;

        AcceptButton = btnLogin;
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(380, 496);
        Controls.Add(lblLogo);
        Controls.Add(lblTitle);
        Controls.Add(lblSubtitle);
        Controls.Add(lblUsername);
        Controls.Add(txtUsername);
        Controls.Add(lblPassword);
        Controls.Add(txtPassword);
        Controls.Add(btnLogin);
        Controls.Add(btnRegister);
        Controls.Add(lblHint);
        Font = new Font("Segoe UI", 10F);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Name = "LoginForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Привычки — вход";
        ResumeLayout(false);
        PerformLayout();
    }

    private Label lblLogo;
    private Label lblTitle;
    private Label lblSubtitle;
    private Label lblUsername;
    private TextBox txtUsername;
    private Label lblPassword;
    private TextBox txtPassword;
    private Button btnLogin;
    private Button btnRegister;
    private Label lblHint;
}
