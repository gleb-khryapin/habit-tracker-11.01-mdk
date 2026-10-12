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
        }
        else
        {
            title = txtTitle.Text.Trim();
            emoji = cmbEmoji.Text;
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
