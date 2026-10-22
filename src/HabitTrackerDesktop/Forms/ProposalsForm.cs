using HabitTrackerDesktop.Data;
using HabitTrackerDesktop.Models;
using HabitTrackerDesktop.Services;

namespace HabitTrackerDesktop.Forms;

public partial class ProposalsForm : Form
{
    public ProposalsForm()
    {
        InitializeComponent();
        Theme.Apply(this);

        cmbEmoji.Items.AddRange(Rules.Emojis);
        cmbEmoji.SelectedIndex = 0;

        Category none = new Category();
        none.Id = 0;
        none.Name = "Без категории";
        cmbCategory.Items.Add(none);
        cmbCategory.SelectedIndex = 0;
        foreach (Category category in CategoryRepository.GetAll())
        {
            cmbCategory.Items.Add(category);
        }

        LoadRequests();
    }

    private void LoadRequests()
    {
        grid.Rows.Clear();

        foreach (TemplateRequest request in RequestRepository.GetByUser(Session.CurrentUser.Id))
        {
            int index = grid.Rows.Add(request.Emoji, request.Title, request.GetStatusText(), request.CreatedDate.ToString("dd.MM.yyyy"));
            DataGridViewRow row = grid.Rows[index];
            row.Tag = request;

            Color statusColor = Theme.Accent;
            if (request.Status == TemplateRequest.Approved)
            {
                statusColor = Theme.Success;
            }
            if (request.Status == TemplateRequest.Rejected)
            {
                statusColor = Theme.Danger;
            }
            row.Cells[2].Style.ForeColor = statusColor;
            row.Cells[2].Style.SelectionForeColor = statusColor;
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
        btnWithdraw.Enabled = request != null && request.Status == TemplateRequest.Pending;

        if (request == null)
        {
            lblDetails.Text = "Заявок пока нет. Заполни форму справа — администратор рассмотрит привычку и добавит её в общий список.";
            return;
        }

        string text = "Категория: " + (request.CategoryName ?? "без категории");
        if (request.Description != "")
        {
            text += "\n" + request.Description;
        }

        if (request.Status == TemplateRequest.Approved)
        {
            text += "\n\nОдобрена " + request.ReviewedDate.Value.ToString("dd.MM.yyyy") + ". Привычка уже в общем списке.";
        }
        else if (request.Status == TemplateRequest.Rejected)
        {
            text += "\n\nОтклонена " + request.ReviewedDate.Value.ToString("dd.MM.yyyy") + ". Причина: " + request.RejectReason;
        }
        else
        {
            text += "\n\nЖдёт рассмотрения администратором.";
        }

        lblDetails.Text = text;
    }

    private void grid_SelectionChanged(object sender, EventArgs e)
    {
        UpdateDetails();
    }

    private void btnSend_Click(object sender, EventArgs e)
    {
        string title = txtTitle.Text.Trim();
        string description = txtDescription.Text.Trim();
        string message = txtMessage.Text.Trim();

        string error = Rules.CheckTitle(title) ?? Rules.CheckDescription(description) ?? Rules.CheckCompletionMessage(message);
        if (error == null && CountMyPending() >= Rules.MaxPendingRequests)
        {
            error = "Можно отправить не больше " + Rules.MaxPendingRequests + " заявок, которые ждут рассмотрения";
        }
        if (error == null && TemplateExists(title))
        {
            error = "Такая привычка уже есть в общем списке";
        }
        if (error == null && PendingExists(title))
        {
            error = "Заявка на такую привычку уже ждёт рассмотрения";
        }
        if (error != null)
        {
            Dialogs.Error(error);
            return;
        }

        TemplateRequest request = new TemplateRequest();
        request.UserId = Session.CurrentUser.Id;
        request.Title = title;
        request.Emoji = cmbEmoji.Text;
        request.Description = description;
        request.CompletionMessage = message;
        request.CreatedDate = Clock.Today;

        Category category = cmbCategory.SelectedItem as Category;
        if (category != null && category.Id != 0)
        {
            request.CategoryId = category.Id;
        }

        RequestRepository.Add(request);

        txtTitle.Clear();
        txtDescription.Clear();
        txtMessage.Clear();
        LoadRequests();
    }

    private int CountMyPending()
    {
        int count = 0;
        foreach (TemplateRequest request in RequestRepository.GetByUser(Session.CurrentUser.Id))
        {
            if (request.Status == TemplateRequest.Pending)
            {
                count++;
            }
        }
        return count;
    }

    private bool TemplateExists(string title)
    {
        foreach (HabitTemplate template in TemplateRepository.GetAll())
        {
            if (string.Equals(template.Title, title, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }

    private bool PendingExists(string title)
    {
        foreach (TemplateRequest request in RequestRepository.GetPending())
        {
            if (string.Equals(request.Title, title, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }

    private void btnWithdraw_Click(object sender, EventArgs e)
    {
        TemplateRequest request = GetSelectedRequest();
        if (request == null || request.Status != TemplateRequest.Pending)
        {
            return;
        }

        if (Dialogs.Confirm("Отозвать заявку «" + request.Title + "»?"))
        {
            RequestRepository.Withdraw(request.Id);
            LoadRequests();
        }
    }
}
