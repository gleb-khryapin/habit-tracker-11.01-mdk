using HabitTrackerDesktop.Data;
using HabitTrackerDesktop.Models;
using HabitTrackerDesktop.Services;

namespace HabitTrackerDesktop.Forms;

public partial class AchievementsForm : Form
{
    public AchievementsForm()
    {
        InitializeComponent();
        Theme.Apply(this);

        List<Achievement> achievements = AchievementRepository.GetByUser(Session.CurrentUser.Id);
        int earnedCount = 0;

        foreach (Achievement achievement in achievements)
        {
            string date = "—";
            if (achievement.EarnedDate != null)
            {
                date = achievement.EarnedDate.Value.ToString("dd.MM.yyyy");
                earnedCount++;
            }

            int index = grid.Rows.Add(achievement.Emoji, achievement.Title, achievement.Description, date);

            if (achievement.EarnedDate == null)
            {
                grid.Rows[index].DefaultCellStyle.ForeColor = Theme.Muted;
                grid.Rows[index].DefaultCellStyle.SelectionForeColor = Theme.Muted;
            }
        }

        lblSubtitle.Text = "Получено: " + earnedCount + " из " + achievements.Count;
    }
}
