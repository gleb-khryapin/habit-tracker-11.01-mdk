using HabitTracker.Data;

namespace HabitTracker.Services;

public static class HabitStatus
{
    public const string Active = "active";
    public const string Completed = "completed";
    public const string Failed = "failed";
}

/// <summary>
/// Вычисляет статус привычки на дату today (раздел 4 требований).
/// Отметки бывают только «за сегодня» и только подряд, поэтому
/// следующая ожидаемая дата отметки = StartDate + количество отметок.
/// </summary>
public record HabitState(int Progress, string Status, bool CheckedToday, DateOnly? FailedOn, DateOnly NextDay)
{
    public static HabitState Of(Habit habit, DateOnly today)
    {
        var progress = habit.Checkins.Count;
        var nextDay = habit.StartDate.AddDays(progress);
        var checkedToday = habit.Checkins.Any(c => c.Date == today);

        if (habit.CompletedOn is not null)
            return new(progress, HabitStatus.Completed, checkedToday, null, nextDay);

        // день nextDay закончился без отметки → привычка прервана
        if (today > nextDay)
            return new(progress, HabitStatus.Failed, checkedToday, nextDay, nextDay);

        return new(progress, HabitStatus.Active, checkedToday, null, nextDay);
    }
}
