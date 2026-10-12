using HabitTrackerDesktop.Data;
using HabitTrackerDesktop.Models;
using HabitTrackerDesktop.Services;

namespace HabitTrackerDesktop.Forms;

public partial class MainForm : Form
{
    public bool LoggedOut;

    public MainForm()
    {
        InitializeComponent();
        Theme.Apply(this);

        User user = Session.CurrentUser;
        lblInfo.Text = user.Username + " · сегодня " + Clock.Today.ToString("dd.MM.yyyy");
        if (Clock.TestDate != null)
        {
            lblInfo.Text += " (тестовая дата)";
        }
        btnTemplates.Visible = user.IsAdmin;

        LoadHabits();
    }

    private void LoadHabits()
    {
        Habit selected = GetSelectedHabit();
        DateTime today = Clock.Today;
        List<Habit> habits = HabitRepository.GetByUser(Session.CurrentUser.Id);
        HabitStatus[] order = { HabitStatus.Active, HabitStatus.Completed, HabitStatus.Failed };

        grid.Rows.Clear();
        foreach (HabitStatus status in order)
        {
            foreach (Habit habit in habits)
            {
                if (habit.GetStatus(today) == status)
                {
                    AddRow(habit, status);
                }
            }
        }

        if (selected != null)
        {
            foreach (DataGridViewRow row in grid.Rows)
            {
                Habit habit = (Habit)row.Tag;
                if (habit.Id == selected.Id)
                {
                    grid.CurrentCell = row.Cells[1];
                }
            }
        }

        UpdateDetails();
    }

    private void AddRow(Habit habit, HabitStatus status)
    {
        string hours = "—";
        if (habit.HoursPerDay != null)
        {
            hours = habit.HoursPerDay.Value.ToString("0.#") + " ч";
        }

        string statusText = "В процессе";
        Color statusColor = Theme.Accent;
        if (status == HabitStatus.Completed)
        {
            statusText = "Выполнена";
            statusColor = Theme.Success;
        }
        if (status == HabitStatus.Failed)
        {
            statusText = "Прервана";
            statusColor = Theme.Danger;
        }

        int index = grid.Rows.Add(
            habit.Emoji,
            habit.Title,
            habit.Progress + " / " + habit.TargetDays,
            hours,
            habit.StartDate.ToString("dd.MM.yyyy"),
            statusText);

        DataGridViewRow row = grid.Rows[index];
        row.Tag = habit;
        row.Cells[5].Style.ForeColor = statusColor;
        row.Cells[5].Style.SelectionForeColor = statusColor;
    }

    private Habit GetSelectedHabit()
    {
        if (grid.SelectedRows.Count == 0)
        {
            return null;
        }
        return grid.SelectedRows[0].Tag as Habit;
    }

    private void UpdateDetails()
    {
        Habit habit = GetSelectedHabit();
        DateTime today = Clock.Today;

        btnCheckIn.Text = "Отметить сегодня";
        btnCheckIn.Enabled = false;
        btnEdit.Enabled = false;
        btnDelete.Enabled = habit != null;

        if (habit == null)
        {
            lblDetails.Text = "Пока пусто. Нажми «Добавить» и начни с одной маленькой привычки 🌱";
        }
        else if (habit.GetStatus(today) == HabitStatus.Completed)
        {
            lblDetails.Text = "🎉 Выполнена " + habit.GetCompletedDate().ToString("dd.MM.yyyy") + ". " + habit.GetCompletionMessage();
        }
        else if (habit.GetStatus(today) == HabitStatus.Failed)
        {
            lblDetails.Text = "Пропущен день " + habit.GetFailedDate().ToString("dd.MM.yyyy")
                + " — серия прервалась. Такую привычку можно только удалить и начать заново.";
        }
        else
        {
            int daysLeft = habit.TargetDays - habit.Progress;
            btnEdit.Enabled = true;

            if (habit.IsCheckedToday(today))
            {
                btnCheckIn.Text = "✓ Сегодня отмечено";
                lblDetails.Text = "Сегодня отмечено. Осталось дней: " + daysLeft + ". Возвращайся завтра!";
            }
            else
            {
                btnCheckIn.Enabled = true;
                lblDetails.Text = "Отметь выполнение до конца дня, иначе серия прервётся. Осталось дней: " + daysLeft + ".";
            }
        }

        if (btnCheckIn.Enabled)
        {
            btnCheckIn.BackColor = Theme.Accent;
        }
        else
        {
            btnCheckIn.BackColor = Theme.Input;
        }
    }

    private void grid_SelectionChanged(object sender, EventArgs e)
    {
        UpdateDetails();
    }

    private void btnCheckIn_Click(object sender, EventArgs e)
    {
        Habit habit = GetSelectedHabit();
        DateTime today = Clock.Today;

        if (habit == null || habit.GetStatus(today) != HabitStatus.Active || habit.IsCheckedToday(today))
        {
            LoadHabits();
            return;
        }

        HabitRepository.AddCheckin(habit.Id, today);
        habit.Progress++;
        LoadHabits();

        if (habit.GetStatus(today) == HabitStatus.Completed)
        {
            string title = "«" + habit.Title + "» — дней подряд: " + habit.TargetDays + "!";
            MessageForm messageForm = new MessageForm(title, habit.GetCompletionMessage());
            messageForm.ShowDialog(this);
        }
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        HabitForm habitForm = new HabitForm(null);
        if (habitForm.ShowDialog(this) == DialogResult.OK)
        {
            LoadHabits();
        }
    }

    private void btnEdit_Click(object sender, EventArgs e)
    {
        Habit habit = GetSelectedHabit();
        if (habit == null)
        {
            return;
        }

        HabitForm habitForm = new HabitForm(habit);
        if (habitForm.ShowDialog(this) == DialogResult.OK)
        {
            LoadHabits();
        }
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
        Habit habit = GetSelectedHabit();
        if (habit == null)
        {
            return;
        }

        if (Dialogs.Confirm("Удалить привычку «" + habit.Title + "»? Отметки тоже удалятся, вернуть их нельзя."))
        {
            HabitRepository.Delete(habit.Id);
            LoadHabits();
        }
    }

    private void btnTemplates_Click(object sender, EventArgs e)
    {
        TemplatesForm templatesForm = new TemplatesForm();
        templatesForm.ShowDialog(this);
    }

    private void btnLogout_Click(object sender, EventArgs e)
    {
        LoggedOut = true;
        Close();
    }
}
