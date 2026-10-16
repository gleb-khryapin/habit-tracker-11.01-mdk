using HabitTrackerDesktop.Data;
using HabitTrackerDesktop.Models;
using HabitTrackerDesktop.Services;

namespace HabitTrackerDesktop.Forms;

public partial class HabitForm : Form
{
    private Habit editedHabit;

    public HabitForm(Habit habit)
    {
        InitializeComponent();
        Theme.Apply(this);

        editedHabit = habit;
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

        if (editedHabit == null)
        {
            PrepareForAdd();
        }
        else
        {
            PrepareForEdit();
        }
    }

    private void PrepareForAdd()
    {
        Text = "Новая привычка";
        lblHeader.Text = "Новая привычка";
        btnSave.Text = "Создать";

        foreach (HabitTemplate template in TemplateRepository.GetAll())
        {
            cmbTemplate.Items.Add(template);
        }

        if (cmbTemplate.Items.Count > 0)
        {
            cmbTemplate.SelectedIndex = 0;
        }
        else
        {
            rbCustom.Checked = true;
            rbTemplate.Enabled = false;
        }

        ShowSelectedMode();
    }

    private void PrepareForEdit()
    {
        Text = "Изменить привычку";
        lblHeader.Text = "Изменить привычку";
        btnSave.Text = "Сохранить";

        rbTemplate.Visible = false;
        rbCustom.Visible = false;
        pnlTemplate.Visible = false;
        pnlCustom.Visible = true;

        txtTitle.Text = editedHabit.Title;
        cmbEmoji.SelectedItem = editedHabit.Emoji;
        numDays.Value = editedHabit.TargetDays;

        foreach (Category category in cmbCategory.Items)
        {
            if (category.Id == editedHabit.CategoryId)
            {
                cmbCategory.SelectedItem = category;
            }
        }

        if (editedHabit.HoursPerDay != null)
        {
            chkHours.Checked = true;
            numHours.Value = (decimal)editedHabit.HoursPerDay.Value;
        }

        if (editedHabit.Progress > 0)
        {
            lblProgress.Visible = true;
            lblProgress.Text = "Уже отмечено дней: " + editedHabit.Progress + ". Новый срок должен быть больше.";
        }
    }

    private void ShowSelectedMode()
    {
        pnlTemplate.Visible = rbTemplate.Checked;
        pnlCustom.Visible = rbCustom.Checked;
    }

    private void rbTemplate_CheckedChanged(object sender, EventArgs e)
    {
        ShowSelectedMode();
    }

    private void cmbTemplate_SelectedIndexChanged(object sender, EventArgs e)
    {
        HabitTemplate template = cmbTemplate.SelectedItem as HabitTemplate;
        if (template != null)
        {
            lblDescription.Text = template.Description;
            if (template.CategoryName != null)
            {
                lblDescription.Text += "
Категория: " + template.CategoryName;
            }
        }
    }

    private void chkHours_CheckedChanged(object sender, EventArgs e)
    {
        numHours.Visible = chkHours.Checked;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
        string title;
        string emoji;
        string message = null;
        int? templateId = null;
        int? categoryId = null;

        if (editedHabit == null && rbTemplate.Checked)
        {
            HabitTemplate template = cmbTemplate.SelectedItem as HabitTemplate;
            if (template == null)
            {
                Dialogs.Error("Выбери привычку из списка");
                return;
            }
            title = template.Title;
            emoji = template.Emoji;
            message = template.CompletionMessage;
            templateId = template.Id;
            categoryId = template.CategoryId;
        }
        else
        {
            title = txtTitle.Text.Trim();
            emoji = cmbEmoji.Text;

            Category category = cmbCategory.SelectedItem as Category;
            if (category != null && category.Id != 0)
            {
                categoryId = category.Id;
            }
        }

        int days = (int)numDays.Value;
        double? hours = null;
        if (chkHours.Checked)
        {
            hours = (double)numHours.Value;
        }

        string error = Rules.CheckTitle(title) ?? Rules.CheckDays(days) ?? Rules.CheckHours(hours);
        if (error == null && editedHabit != null && days <= editedHabit.Progress)
        {
            error = "Срок должен быть больше уже отмеченных дней (" + editedHabit.Progress + ")";
        }
        if (error == null && HasActiveDuplicate(title))
        {
            error = "У тебя уже есть активная привычка с таким названием";
        }
        if (error != null)
        {
            Dialogs.Error(error);
            return;
        }

        if (editedHabit == null)
        {
            Habit habit = new Habit();
            habit.UserId = Session.CurrentUser.Id;
            habit.TemplateId = templateId;
            habit.CategoryId = categoryId;
            habit.Title = title;
            habit.Emoji = emoji;
            habit.TargetDays = days;
            habit.HoursPerDay = hours;
            habit.StartDate = Clock.Today;
            habit.CompletionMessage = message;
            HabitRepository.Add(habit);
        }
        else
        {
            editedHabit.Title = title;
            editedHabit.Emoji = emoji;
            editedHabit.CategoryId = categoryId;
            editedHabit.TargetDays = days;
            editedHabit.HoursPerDay = hours;
            HabitRepository.Update(editedHabit);
        }

        DialogResult = DialogResult.OK;
    }

    private bool HasActiveDuplicate(string title)
    {
        DateTime today = Clock.Today;

        foreach (Habit habit in HabitRepository.GetByUser(Session.CurrentUser.Id))
        {
            bool isSameHabit = editedHabit != null && habit.Id == editedHabit.Id;
            bool isActive = habit.GetStatus(today) == HabitStatus.Active;
            bool hasSameTitle = string.Equals(habit.Title, title, StringComparison.OrdinalIgnoreCase);

            if (!isSameHabit && isActive && hasSameTitle)
            {
                return true;
            }
        }

        return false;
    }
}
