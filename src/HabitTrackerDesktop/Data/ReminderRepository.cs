using HabitTrackerDesktop.Models;
using MySqlConnector;

namespace HabitTrackerDesktop.Data;

public static class ReminderRepository
{
    public static List<Reminder> GetByHabit(int habitId)
    {
        List<Reminder> reminders = new List<Reminder>();

        using (MySqlConnection connection = Database.Open())
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT id, habit_id, time, is_enabled FROM reminders WHERE habit_id = @habitId ORDER BY time";
            command.Parameters.AddWithValue("@habitId", habitId);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Reminder reminder = new Reminder();
                    reminder.Id = reader.GetInt32(0);
                    reminder.HabitId = reader.GetInt32(1);
                    reminder.Time = reader.GetTimeSpan(2).ToString(@"hh\:mm");
                    reminder.IsEnabled = reader.GetBoolean(3);
                    reminders.Add(reminder);
                }
            }
        }

        return reminders;
    }

    // Привычки пользователя, у которых на это время стоит включённое напоминание
    public static List<int> GetDueHabitIds(int userId, string time)
    {
        List<int> habitIds = new List<int>();

        using (MySqlConnection connection = Database.Open())
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = @"
                SELECT r.habit_id
                FROM reminders r
                JOIN habits h ON h.id = r.habit_id
                WHERE h.user_id = @userId AND r.time = @time AND r.is_enabled = 1";
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@time", time);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    habitIds.Add(reader.GetInt32(0));
                }
            }
        }

        return habitIds;
    }

    public static void Add(Reminder reminder)
    {
        using (MySqlConnection connection = Database.Open())
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO reminders (habit_id, time, is_enabled)
                VALUES (@habitId, @time, @isEnabled);
                SELECT LAST_INSERT_ID();";
            command.Parameters.AddWithValue("@habitId", reminder.HabitId);
            command.Parameters.AddWithValue("@time", reminder.Time);
            command.Parameters.AddWithValue("@isEnabled", reminder.IsEnabled ? 1 : 0);
            reminder.Id = Convert.ToInt32(command.ExecuteScalar());
        }
    }

    public static void SetEnabled(int id, bool isEnabled)
    {
        using (MySqlConnection connection = Database.Open())
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE reminders SET is_enabled = @isEnabled WHERE id = @id";
            command.Parameters.AddWithValue("@isEnabled", isEnabled ? 1 : 0);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }
    }

    public static void Delete(int id)
    {
        using (MySqlConnection connection = Database.Open())
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM reminders WHERE id = @id";
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }
    }
}
