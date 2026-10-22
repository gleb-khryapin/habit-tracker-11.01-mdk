using HabitTrackerDesktop.Data;
using HabitTrackerDesktop.Models;
using HabitTrackerDesktop.Services;

namespace HabitTrackerDesktop.Forms;

public partial class MainForm : Form
{
    public bool LoggedOut;

    private string lastReminderTime;

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
        btnCategories.Visible = user.IsAdmin;
        btnPhrases.Visible = user.IsAdmin;
        btnAudit.Visible = user.IsAdmin;
        if (user.IsAdmin)
        {
            btnRequests.Text = "Заявки";
        }

        LoadCategories();
        LoadHabits();
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        timer.Stop();
        base.OnFormClosed(e);
    }

    private void LoadCategories()
    {
        Category selected = cmbCategory.SelectedItem as Category;

        Category all = new Category();
        all.Id = 0;
        all.Name = "Все категории";

        cmbCategory.Items.Clear();
        cmbCategory.Items.Add(all);
        cmbCategory.SelectedIndex = 0;

        foreach (Category category in CategoryRepository.GetAll())
        {
            cmbCategory.Items.Add(category);
            if (selected != null && category.Id == selected.Id)
            {
                cmbCategory.SelectedItem = category;
            }
        }
    }

    private void LoadHabits()
    {
        Habit selected = GetSelectedHabit();
        Category filter = cmbCategory.SelectedItem as Category;
        DateTime today = Clock.Today;
        List<Habit> habits = HabitRepository.GetByUser(Session.CurrentUser.Id);
        HabitStatus[] order = { HabitStatus.Active, HabitStatus.Completed, HabitStatus.Failed };

        grid.Rows.Clear();
        foreach (HabitStatus status in order)
        {
            foreach (Habit habit in habits)
            {
                bool inCategory = filter == null || filter.Id == 0 || habit.CategoryId == filter.Id;
                if (habit.GetStatus(today) == status && inCategory)
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
        UpdateStreak();
    }

    private void UpdateStreak()
    {
        DateTime today = Clock.Today;
        List<DateTime> days = ActivityRepository.GetDays(Session.CurrentUser.Id);
        int current = StreakCalculator.GetCurrent(days, today);
        int best = StreakCalculator.GetBest(days);

        if (current == 0 && best == 0)
        {
            lblStreak.Text = "🔥 Отметь привычку, чтобы зажечь огонёк";
            lblStreak.ForeColor = Theme.Muted;
        }
        else if (current == 0)
        {
            lblStreak.Text = "🔥 Огонёк погас · рекорд " + best + " дн.";
            lblStreak.ForeColor = Theme.Muted;
        }
        else if (days.Contains(today))
        {
            lblStreak.Text = "🔥 Огонёк: " + current + " дн. · рекорд " + best + " дн.";
            lblStreak.ForeColor = Theme.Flame;
        }
        else
        {
            lblStreak.Text = "🔥 Огонёк: " + current + " дн. · отметь сегодня, иначе погаснет";
            lblStreak.ForeColor = Theme.Muted;
        }
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
            habit.CategoryName ?? "—",
            habit.Progress + " / " + habit.TargetDays,
            hours,
            habit.StartDate.ToString("dd.MM.yyyy"),
            statusText);

        DataGridViewRow row = grid.Rows[index];
        row.Tag = habit;
        row.Cells[6].Style.ForeColor = statusColor;
        row.Cells[6].Style.SelectionForeColor = statusColor;
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
        btnReminders.Enabled = false;
        btnEdit.Enabled = false;
        btnDelete.Enabled = habit != null;

        if (habit == null)
        {
            if (cmbCategory.SelectedIndex > 0)
            {
                lblDetails.Text = "В этой категории привычек нет.";
            }
            else
            {
                lblDetails.Text = "Пока пусто. Нажми «Добавить» и начни с одной маленькой привычки 🌱";
            }
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
            btnReminders.Enabled = true;

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

    private void ShowNewAchievements()
    {
        foreach (Achievement achievement in AchievementChecker.GiveNew(Session.CurrentUser.Id))
        {
            string title = "Новое достижение: " + achievement.Emoji + " " + achievement.Title;
            MessageForm messageForm = new MessageForm(title, achievement.Description);
            messageForm.Text = "Новое достижение";
            messageForm.ShowDialog(this);
        }
    }

    private void grid_SelectionChanged(object sender, EventArgs e)
    {
        UpdateDetails();
    }

    private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadHabits();
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

        ShowNewAchievements();
    }

    private void btnReminders_Click(object sender, EventArgs e)
    {
        Habit habit = GetSelectedHabit();
        if (habit == null)
        {
            return;
        }

        RemindersForm remindersForm = new RemindersForm(habit);
        remindersForm.ShowDialog(this);
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        HabitForm habitForm = new HabitForm(null);
        if (habitForm.ShowDialog(this) == DialogResult.OK)
        {
            LoadHabits();
            ShowNewAchievements();
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

        if (Dialogs.Confirm("Удалить привычку «" + habit.Title + "»? Отметки и напоминания тоже удалятся, вернуть их нельзя."))
        {
            HabitRepository.Delete(habit.Id);
            LoadHabits();
        }
    }

    private void btnAchievements_Click(object sender, EventArgs e)
    {
        AchievementsForm achievementsForm = new AchievementsForm();
        achievementsForm.ShowDialog(this);
    }

    private void btnTemplates_Click(object sender, EventArgs e)
    {
        TemplatesForm templatesForm = new TemplatesForm();
        templatesForm.ShowDialog(this);
    }

    private void btnCategories_Click(object sender, EventArgs e)
    {
        CategoriesForm categoriesForm = new CategoriesForm();
        categoriesForm.ShowDialog(this);
        LoadCategories();
        LoadHabits();
    }

    private void btnPhrases_Click(object sender, EventArgs e)
    {
        PhrasesForm phrasesForm = new PhrasesForm();
        phrasesForm.ShowDialog(this);
        LoadHabits();
    }

    private void btnRequests_Click(object sender, EventArgs e)
    {
        if (Session.CurrentUser.IsAdmin)
        {
            ReviewForm reviewForm = new ReviewForm();
            reviewForm.ShowDialog(this);
        }
        else
        {
            ProposalsForm proposalsForm = new ProposalsForm();
            proposalsForm.ShowDialog(this);
        }
    }

    private void btnAudit_Click(object sender, EventArgs e)
    {
        AuditForm auditForm = new AuditForm();
        auditForm.ShowDialog(this);

        if (auditForm.Restored)
        {
            LoggedOut = true;
            Close();
        }
    }

    private void btnLogout_Click(object sender, EventArgs e)
    {
        LoggedOut = true;
        Close();
    }

    // Раз в минуту проверяем, не пора ли напомнить о неотмеченных привычках
    private void timer_Tick(object sender, EventArgs e)
    {
        string time = DateTime.Now.ToString("HH:mm");
        if (time == lastReminderTime)
        {
            return;
        }
        lastReminderTime = time;

        DateTime today = Clock.Today;
        List<int> dueHabitIds = ReminderRepository.GetDueHabitIds(Session.CurrentUser.Id, time);
        string titles = "";

        foreach (Habit habit in HabitRepository.GetByUser(Session.CurrentUser.Id))
        {
            bool isWaiting = habit.GetStatus(today) == HabitStatus.Active && !habit.IsCheckedToday(today);
            if (dueHabitIds.Contains(habit.Id) && isWaiting)
            {
                titles += "\n" + habit.Emoji + " " + habit.Title;
            }
        }

        if (titles != "")
        {
            timer.Stop();
            Dialogs.Info("Напоминание · " + time, "Пора отметить выполнение:" + titles);
            timer.Start();
        }
    }
}
