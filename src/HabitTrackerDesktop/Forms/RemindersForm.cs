using HabitTrackerDesktop.Data;
using HabitTrackerDesktop.Models;
using HabitTrackerDesktop.Services;

namespace HabitTrackerDesktop.Forms;

public partial class RemindersForm : Form
{
    private Habit habit;

    public RemindersForm(Habit habit)
    {
        InitializeComponent();
        Theme.Apply(this);

        this.habit = habit;
        lblSubtitle.Text = habit.Emoji + " " + habit.Title;

        LoadReminders();
    }

    private void LoadReminders()
    {
        Reminder selected = GetSelectedReminder();

        grid.Rows.Clear();
        foreach (Reminder reminder in ReminderRepository.GetByHabit(habit.Id))
        {
            string state = "Включено";
            if (!reminder.IsEnabled)
            {
                state = "Выключено";
            }

            int index = grid.Rows.Add(reminder.Time, state);
            grid.Rows[index].Tag = reminder;

            if (selected != null && reminder.Id == selected.Id)
            {
                grid.CurrentCell = grid.Rows[index].Cells[0];
            }
        }

        UpdateButtons();
    }

    private Reminder GetSelectedReminder()
    {
        if (grid.SelectedRows.Count == 0)
        {
            return null;
        }
        return grid.SelectedRows[0].Tag as Reminder;
    }

    private void UpdateButtons()
    {
        Reminder reminder = GetSelectedReminder();
        btnToggle.Enabled = reminder != null;
        btnDelete.Enabled = reminder != null;

        if (reminder != null && !reminder.IsEnabled)
        {
            btnToggle.Text = "Включить";
        }
        else
        {
            btnToggle.Text = "Выключить";
        }
    }

    private void grid_SelectionChanged(object sender, EventArgs e)
    {
        UpdateButtons();
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        string time = ((int)numHour.Value).ToString("00") + ":" + ((int)numMinute.Value).ToString("00");
        List<Reminder> reminders = ReminderRepository.GetByHabit(habit.Id);

        if (reminders.Count >= Rules.MaxReminders)
        {
            Dialogs.Error("У привычки может быть не больше " + Rules.MaxReminders + " напоминаний");
            return;
        }

        foreach (Reminder existing in reminders)
        {
            if (existing.Time == time)
            {
                Dialogs.Error("Напоминание на " + time + " уже есть");
                return;
            }
        }

        Reminder reminder = new Reminder();
        reminder.HabitId = habit.Id;
        reminder.Time = time;
        reminder.IsEnabled = true;
        ReminderRepository.Add(reminder);

        LoadReminders();
    }

    private void btnToggle_Click(object sender, EventArgs e)
    {
        Reminder reminder = GetSelectedReminder();
        if (reminder == null)
        {
            return;
        }

        ReminderRepository.SetEnabled(reminder.Id, !reminder.IsEnabled);
        LoadReminders();
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
        Reminder reminder = GetSelectedReminder();
        if (reminder == null)
        {
            return;
        }

        if (Dialogs.Confirm("Удалить напоминание на " + reminder.Time + "?"))
        {
            ReminderRepository.Delete(reminder.Id);
            LoadReminders();
        }
    }
}
