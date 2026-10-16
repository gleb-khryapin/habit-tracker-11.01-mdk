using HabitTrackerDesktop.Data;
using HabitTrackerDesktop.Models;
using HabitTrackerDesktop.Services;

namespace HabitTrackerDesktop.Forms;

public partial class PhrasesForm : Form
{
    public PhrasesForm()
    {
        InitializeComponent();
        Theme.Apply(this);

        LoadPhrases();
    }

    private void LoadPhrases()
    {
        grid.Rows.Clear();

        foreach (CompletionMessage message in MessageRepository.GetAll())
        {
            int index = grid.Rows.Add(message.Text);
            grid.Rows[index].Tag = message;
        }

        UpdateDetails();
    }

    private CompletionMessage GetSelectedPhrase()
    {
        if (grid.SelectedRows.Count == 0)
        {
            return null;
        }
        return grid.SelectedRows[0].Tag as CompletionMessage;
    }

    private void UpdateDetails()
    {
        CompletionMessage message = GetSelectedPhrase();
        btnDelete.Enabled = message != null;

        if (message == null)
        {
            lblPhrase.Text = "Пул пуст. Пока фраз нет, своим привычкам показывается стандартное сообщение.";
        }
        else
        {
            lblPhrase.Text = "🎉 " + message.Text;
        }
    }

    private void grid_SelectionChanged(object sender, EventArgs e)
    {
        UpdateDetails();
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        string text = txtText.Text.Trim();

        string error = Rules.CheckPoolMessage(text);
        if (error != null)
        {
            Dialogs.Error(error);
            return;
        }

        CompletionMessage message = new CompletionMessage();
        message.Text = text;
        MessageRepository.Add(message);

        txtText.Clear();
        LoadPhrases();
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
        CompletionMessage message = GetSelectedPhrase();
        if (message == null)
        {
            return;
        }

        if (Dialogs.Confirm("Удалить фразу из пула? Привычки, за которыми она закреплена, получат стандартное сообщение."))
        {
            MessageRepository.Delete(message.Id);
            LoadPhrases();
        }
    }
}
