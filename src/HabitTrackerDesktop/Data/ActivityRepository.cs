using MySqlConnector;

namespace HabitTrackerDesktop.Data;

public static class ActivityRepository
{
    public static List<DateTime> GetDays(int userId)
    {
        List<DateTime> days = new List<DateTime>();

        using (MySqlConnection connection = Database.Open())
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT date FROM user_activity WHERE user_id = @userId ORDER BY date";
            command.Parameters.AddWithValue("@userId", userId);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    days.Add(reader.GetDateTime(0));
                }
            }
        }

        return days;
    }
}
