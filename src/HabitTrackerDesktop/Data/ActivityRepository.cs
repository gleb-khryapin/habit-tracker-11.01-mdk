using Microsoft.Data.Sqlite;

namespace HabitTrackerDesktop.Data;

public static class ActivityRepository
{
    public static List<DateTime> GetDays(int userId)
    {
        List<DateTime> days = new List<DateTime>();

        using (SqliteConnection connection = Database.Open())
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT date FROM user_activity WHERE user_id = @userId ORDER BY date";
            command.Parameters.AddWithValue("@userId", userId);

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    days.Add(Database.TextToDate(reader.GetString(0)));
                }
            }
        }

        return days;
    }
}
