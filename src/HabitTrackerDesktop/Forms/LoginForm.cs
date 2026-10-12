using HabitTrackerDesktop.Data;
using HabitTrackerDesktop.Models;
using HabitTrackerDesktop.Services;

namespace HabitTrackerDesktop.Forms;

public partial class LoginForm : Form
{
    public LoginForm()
    {
        InitializeComponent();
        Theme.Apply(this);
    }

    private void btnLogin_Click(object sender, EventArgs e)
    {
        string username = txtUsername.Text.Trim();
        string password = txtPassword.Text;

        if (username == "" || password == "")
        {
            Dialogs.Error("Укажи логин и пароль");
            return;
        }

        User user = UserRepository.FindByUsername(username);
        if (user == null || !PasswordHasher.Verify(password, user.PasswordHash))
        {
            Dialogs.Error("Неверный логин или пароль");
            return;
        }

        OpenMainForm(user);
    }

    private void btnRegister_Click(object sender, EventArgs e)
    {
        string username = txtUsername.Text.Trim();
        string password = txtPassword.Text;

        string error = Rules.CheckUsername(username) ?? Rules.CheckPassword(password);
        if (error == null && UserRepository.FindByUsername(username) != null)
        {
            error = "Этот логин уже занят";
        }
        if (error != null)
        {
            Dialogs.Error(error);
            return;
        }

        User user = new User();
        user.Username = username;
        user.PasswordHash = PasswordHasher.Hash(password);
        user.Role = "user";
        UserRepository.Add(user);

        OpenMainForm(user);
    }

    private void OpenMainForm(User user)
    {
        Session.CurrentUser = user;
        txtPassword.Clear();
        Hide();

        MainForm mainForm = new MainForm();
        mainForm.ShowDialog();

        if (mainForm.LoggedOut)
        {
            Session.CurrentUser = null;
            Show();
            txtUsername.Focus();
        }
        else
        {
            Close();
        }
    }
}
