using HabitTrackerDesktop.Services;

namespace HabitTrackerDesktop.Models;

public class Habit
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; }
    public string Emoji { get; set; }
    public int TargetDays { get; set; }
    public double? HoursPerDay { get; set; }
    public DateTime StartDate { get; set; }
    public string CompletionMessage { get; set; }
    public int Progress { get; set; }

    public HabitStatus GetStatus(DateTime today)
    {
        if (Progress >= TargetDays)
        {
            return HabitStatus.Completed;
        }
        if (today > StartDate.AddDays(Progress))
        {
            return HabitStatus.Failed;
        }
        return HabitStatus.Active;
    }

    public bool IsCheckedToday(DateTime today)
    {
        return Progress > 0 && StartDate.AddDays(Progress - 1) == today;
    }

    public DateTime GetFailedDate()
    {
        return StartDate.AddDays(Progress);
    }

    public DateTime GetCompletedDate()
    {
        return StartDate.AddDays(TargetDays - 1);
    }

    public string GetCompletionMessage()
    {
        if (string.IsNullOrEmpty(CompletionMessage))
        {
            return Rules.GetPoolMessage(Id);
        }
        return CompletionMessage;
    }
}
