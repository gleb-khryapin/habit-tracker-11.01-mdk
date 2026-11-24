using System.Text;
using HabitTrackerDesktop.Data;
using HabitTrackerDesktop.Models;

namespace HabitTrackerDesktop.Services;

public static class ReportBuilder
{
    public static string GetStatusText(HabitStatus status)
    {
        if (status == HabitStatus.Completed)
        {
            return "Выполнена";
        }
        if (status == HabitStatus.Failed)
        {
            return "Прервана";
        }
        return "В процессе";
    }

    public static string BuildAdminReport(DateTime today, string author)
    {
        List<User> users = UserRepository.GetAll();
        List<string> rows = new List<string>();
        int active = 0;
        int completed = 0;
        int failed = 0;
        int checkins = 0;

        foreach (User user in users)
        {
            foreach (Habit habit in HabitRepository.GetByUser(user.Id))
            {
                HabitStatus status = habit.GetStatus(today);
                if (status == HabitStatus.Active)
                {
                    active++;
                }
                if (status == HabitStatus.Completed)
                {
                    completed++;
                }
                if (status == HabitStatus.Failed)
                {
                    failed++;
                }
                checkins += habit.Progress;

                rows.Add(Cell(user.Username, 14)
                    + Cell(habit.Title, 24)
                    + Cell(habit.CategoryName ?? "—", 16)
                    + Cell(habit.Progress + " / " + habit.TargetDays, 10)
                    + Cell(habit.StartDate.ToString("dd.MM.yyyy"), 12)
                    + GetStatusText(status));
            }
        }

        int total = active + completed + failed;
        int finished = completed + failed;
        string successRate = "—";
        if (finished > 0)
        {
            successRate = (completed * 100 / finished) + " %";
        }

        StringBuilder text = new StringBuilder();
        text.AppendLine("ОТЧЁТ ПО ПРИВЫЧКАМ ПОЛЬЗОВАТЕЛЕЙ");
        text.AppendLine("Трекер привычек");
        text.AppendLine();
        text.AppendLine("Дата формирования: " + today.ToString("dd.MM.yyyy"));
        text.AppendLine("Сформировал: " + author);
        text.AppendLine();
        text.AppendLine("ИТОГИ");
        text.AppendLine("Пользователей: " + users.Count);
        text.AppendLine("Привычек всего: " + total);
        text.AppendLine("  в процессе: " + active);
        text.AppendLine("  выполнено: " + completed);
        text.AppendLine("  прервано: " + failed);
        text.AppendLine("Доля выполненных среди завершённых: " + successRate);
        text.AppendLine("Отметок выполнения всего: " + checkins);
        text.AppendLine();
        text.AppendLine("ПРИВЫЧКИ");
        string header = Cell("Пользователь", 14) + Cell("Привычка", 24) + Cell("Категория", 16)
            + Cell("Прогресс", 10) + Cell("Начата", 12) + "Статус";
        text.AppendLine(header);
        text.AppendLine(new string('-', header.Length + 4));
        foreach (string row in rows)
        {
            text.AppendLine(row);
        }
        if (rows.Count == 0)
        {
            text.AppendLine("Привычек пока нет.");
        }

        return text.ToString();
    }

    public static string BuildCertificate(Habit habit, string username, DateTime today)
    {
        string hours = "не указано";
        if (habit.HoursPerDay != null)
        {
            hours = habit.HoursPerDay.Value.ToString("0.#");
        }

        StringBuilder text = new StringBuilder();
        text.AppendLine("СЕРТИФИКАТ О ВЫПОЛНЕНИИ ПРИВЫЧКИ");
        text.AppendLine("Трекер привычек");
        text.AppendLine();
        text.AppendLine("Пользователь: " + username);
        text.AppendLine("Привычка: " + habit.Title);
        text.AppendLine("Категория: " + (habit.CategoryName ?? "без категории"));
        text.AppendLine("Срок: " + habit.TargetDays + " дн. подряд");
        text.AppendLine("Часов в день: " + hours);
        text.AppendLine("Начата: " + habit.StartDate.ToString("dd.MM.yyyy"));
        text.AppendLine("Выполнена: " + habit.GetCompletedDate().ToString("dd.MM.yyyy"));
        text.AppendLine();
        text.AppendLine(habit.GetCompletionMessage());
        text.AppendLine();
        text.AppendLine("Дата выдачи: " + today.ToString("dd.MM.yyyy"));

        return text.ToString();
    }

    private static string Cell(string value, int width)
    {
        if (value.Length >= width)
        {
            value = value.Substring(0, width - 2) + "…";
        }
        return value.PadRight(width);
    }
}
