using HabitTrackerDesktop.Data;
using HabitTrackerDesktop.Models;
using HabitTrackerDesktop.Services;

namespace HabitTrackerDesktop.Forms;

public partial class TemplatesForm : Form
{
    public TemplatesForm()
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

        LoadTemplates();
    }

    private void LoadTemplates()
    {
        grid.Rows.Clear();

        foreach (HabitTemplate template in TemplateRepository.GetAll())
        {
            int index = grid.Rows.Add(template.Emoji, template.Title, template.CategoryName ?? "—", template.Description);
            grid.Rows[index].Tag = template;
        }

        UpdateDetails();
    }

    private HabitTemplate GetSelectedTemplate()
    {
        if (grid.SelectedRows.Count == 0)
        {
            return null;
        }
        return grid.SelectedRows[0].Tag as HabitTemplate;
    }

    private void UpdateDetails()
    {
        HabitTemplate template = GetSelectedTemplate();
        btnDelete.Enabled = template != null;

        if (template == null)
        {
            lblMessage.Text = "Список пуст. Добавь первую привычку справа.";
        }
        else
        {
            lblMessage.Text = "🎉 " + template.CompletionMessage;
        }
    }

    private void grid_SelectionChanged(object sender, EventArgs e)
    {
        UpdateDetails();
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        string title = txtTitle.Text.Trim();
        string description = txtDescription.Text.Trim();
        string message = txtMessage.Text.Trim();

        string error = Rules.CheckTitle(title) ?? Rules.CheckDescription(description) ?? Rules.CheckCompletionMessage(message);
        if (error == null && TitleExists(title))
        {
            error = "Предложенная привычка с таким названием уже есть";
        }
        if (error != null)
        {
            Dialogs.Error(error);
            return;
        }

        HabitTemplate template = new HabitTemplate();
        template.Title = title;
        template.Emoji = cmbEmoji.Text;
        Category category = cmbCategory.SelectedItem as Category;
        if (category != null && category.Id != 0)
        {
            template.CategoryId = category.Id;
        }
        template.Description = description;
        template.CompletionMessage = message;
        TemplateRepository.Add(template);

        txtTitle.Clear();
        txtDescription.Clear();
        txtMessage.Clear();
        LoadTemplates();
    }

    private bool TitleExists(string title)
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

    private void btnDelete_Click(object sender, EventArgs e)
    {
        HabitTemplate template = GetSelectedTemplate();
        if (template == null)
        {
            return;
        }

        if (Dialogs.Confirm("Удалить «" + template.Title + "» из списка? Привычки, которые пользователи уже завели, останутся."))
        {
            TemplateRepository.Delete(template.Id);
            LoadTemplates();
        }
    }
}
