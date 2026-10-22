using HabitTrackerDesktop.Data;
using HabitTrackerDesktop.Models;
using HabitTrackerDesktop.Services;

namespace HabitTrackerDesktop.Forms;

public partial class ReviewForm : Form
{
    public ReviewForm()
    {
        InitializeComponent();
        Theme.Apply(this);

        LoadRequests();
    }

    private void LoadRequests()
    {
        grid.Rows.Clear();

        foreach (TemplateRequest request in RequestRepository.GetPending())
        {
            int index = grid.Rows.Add(request.Emoji, request.Title, request.Username, request.CreatedDate.ToString("dd.MM.yyyy"));
            grid.Rows[index].Tag = request;
        }

        UpdateDetails();
    }

    private TemplateRequest GetSelectedRequest()
    {
        if (grid.SelectedRows.Count == 0)
        {
            return null;
        }
        return grid.SelectedRows[0].Tag as TemplateRequest;
    }

    private void UpdateDetails()
    {
        TemplateRequest request = GetSelectedRequest();
        btnApprove.Enabled = request != null;
        btnReject.Enabled = request != null;

        if (request == null)
        {
            lblDetails.Text = "Новых заявок нет.";
            return;
        }

        string text = "Автор: " + request.Username;
        text += "\nКатегория: " + (request.CategoryName ?? "без категории");
        if (request.Description != "")
        {
            text += "\nОписание: " + request.Description;
        }
        text += "\n\nСообщение после выполнения:\n«" + request.CompletionMessage + "»";

        lblDetails.Text = text;
    }

    private void grid_SelectionChanged(object sender, EventArgs e)
    {
        UpdateDetails();
    }

    private void btnApprove_Click(object sender, EventArgs e)
    {
        TemplateRequest request = GetSelectedRequest();
        if (request == null)
        {
            return;
        }

        foreach (HabitTemplate template in TemplateRepository.GetAll())
        {
            if (string.Equals(template.Title, request.Title, StringComparison.OrdinalIgnoreCase))
            {
                Dialogs.Error("В списке уже есть привычка «" + template.Title + "». Отклони заявку с этой причиной.");
                return;
            }
        }

        RequestRepository.Approve(request, Clock.Today);
        txtReason.Clear();
        LoadRequests();
    }

    private void btnReject_Click(object sender, EventArgs e)
    {
        TemplateRequest request = GetSelectedRequest();
        if (request == null)
        {
            return;
        }

        string reason = txtReason.Text.Trim();
        string error = Rules.CheckRejectReason(reason);
        if (error != null)
        {
            Dialogs.Error(error);
            return;
        }

        RequestRepository.Reject(request.Id, reason, Clock.Today);
        txtReason.Clear();
        LoadRequests();
    }
}
