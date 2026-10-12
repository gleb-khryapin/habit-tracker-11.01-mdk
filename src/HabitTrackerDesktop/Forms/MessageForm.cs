using HabitTrackerDesktop.Services;

namespace HabitTrackerDesktop.Forms;

public partial class MessageForm : Form
{
    public MessageForm(string title, string message)
    {
        InitializeComponent();
        Theme.Apply(this);

        lblTitle.Text = title;
        lblMessage.Text = message;
    }
}
