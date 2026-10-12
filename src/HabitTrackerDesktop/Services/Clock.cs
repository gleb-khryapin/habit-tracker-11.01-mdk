using System.Globalization;

namespace HabitTrackerDesktop.Services;

public static class Clock
{
    public static DateTime? TestDate;

    public static DateTime Today
    {
        get
        {
            if (TestDate != null)
            {
                return TestDate.Value;
            }
            return DateTime.Today;
        }
    }

    public static void ReadArguments(string[] args)
    {
        for (int i = 0; i < args.Length - 1; i++)
        {
            if (args[i] == "--today")
            {
                DateTime date;
                if (DateTime.TryParseExact(args[i + 1], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                {
                    TestDate = date;
                }
            }
        }
    }
}
