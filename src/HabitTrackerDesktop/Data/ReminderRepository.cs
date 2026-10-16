using HabitTrackerDesktop.Models;
using Microsoft.Data.Sqlite;

namespace HabitTrackerDesktop.Data;

public static class ReminderRepository
{
    public static List<Reminder> GetByHabit(int habitId)
    {
        List<Reminder> reminders = new List<Reminder>();

        using (SqliteConnection connection = Database.Open())
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT id, habit_id, time, is_enabled FROM reminders WHERE habit_id = @habitId ORDER BY time";
            command.Parameters.AddWithValue("@habitId", habitId);

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Reminder reminder = new Reminder();
                    reminder.Id = reader.GetInt32(0);
                    reminder.HabitId = reader.GetInt32(1);
                    reminder.Time = reader.GetString(2);
                    reminder.IsEnabled = reader.GetInt32(3) == 1;
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

        using (SqliteConnection connection = Database.Open())
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"
                SELECT r.habit_id
                FROM reminders r
                JOIN habits h ON h.id = r.habit_id
                WHERE h.user_id = @userId AND r.time = @time AND r.is_enabled = 1";
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@time", time);

            using (SqliteDataReader reader = command.ExecuteReader())
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
        using (SqliteConnection connection = Database.Open())
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO reminders (habit_id, time, is_enabled)
                VALUES (@habitId, @time, @isEnabled);
                SELECT last_insert_rowid();";
            command.Parameters.AddWithValue("@habitId", reminder.HabitId);
            command.Parameters.AddWithValue("@time", reminder.Time);
            command.Parameters.AddWithValue("@isEnabled", reminder.IsEnabled ? 1 : 0);
            reminder.Id = Convert.ToInt32(command.ExecuteScalar());
        }
    }

    public static void SetEnabled(int id, bool isEnabled)
    {
        using (SqliteConnection connection = Database.Open())
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE reminders SET is_enabled = @isEnabled WHERE id = @id";
            command.Parameters.AddWithValue("@isEnabled", isEnabled ? 1 : 0);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }
    }

    public static void Delete(int id)
    {
        using (SqliteConnection connection = Database.Open())
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM reminders WHERE id = @id";
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }
    }
}
