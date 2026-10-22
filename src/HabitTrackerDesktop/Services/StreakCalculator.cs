namespace HabitTrackerDesktop.Services;

public static class StreakCalculator
{
    public static int GetCurrent(List<DateTime> days, DateTime today)
    {
        DateTime day = today;
        if (!days.Contains(day))
        {
            day = today.AddDays(-1);
        }

        int streak = 0;
        while (days.Contains(day))
        {
            streak++;
            day = day.AddDays(-1);
        }
        return streak;
    }

    public static int GetBest(List<DateTime> days)
    {
        int best = 0;
        int current = 0;
        DateTime previous = DateTime.MinValue;

        foreach (DateTime day in days)
        {
            if (current > 0 && day == previous.AddDays(1))
            {
                current++;
            }
            else
            {
                current = 1;
            }

            if (current > best)
            {
                best = current;
            }
            previous = day;
        }

        return best;
    }
}
