using HabitTrackerDesktop.Models;
using Microsoft.Data.Sqlite;

namespace HabitTrackerDesktop.Data;

public static class HabitRepository
{
    public static List<Habit> GetByUser(int userId)
    {
        List<Habit> habits = new List<Habit>();

        using (SqliteConnection connection = Database.Open())
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"
                SELECT h.id, h.user_id, h.title, h.emoji, h.target_days, h.hours_per_day, h.start_date, h.completion_message,
                       (SELECT COUNT(*) FROM checkins c WHERE c.habit_id = h.id) AS progress
                FROM habits h
                WHERE h.user_id = @userId
                ORDER BY h.id";
            command.Parameters.AddWithValue("@userId", userId);

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Habit habit = new Habit();
                    habit.Id = reader.GetInt32(0);
                    habit.UserId = reader.GetInt32(1);
                    habit.Title = reader.GetString(2);
                    habit.Emoji = reader.GetString(3);
                    habit.TargetDays = reader.GetInt32(4);
                    if (!reader.IsDBNull(5))
                    {
                        habit.HoursPerDay = reader.GetDouble(5);
                    }
                    habit.StartDate = Database.TextToDate(reader.GetString(6));
                    if (!reader.IsDBNull(7))
                    {
                        habit.CompletionMessage = reader.GetString(7);
                    }
                    habit.Progress = reader.GetInt32(8);
                    habits.Add(habit);
                }
            }
        }

        return habits;
    }

    public static void Add(Habit habit)
    {
        using (SqliteConnection connection = Database.Open())
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO habits (user_id, title, emoji, target_days, hours_per_day, start_date, completion_message)
                VALUES (@userId, @title, @emoji, @targetDays, @hours, @startDate, @message);
                SELECT last_insert_rowid();";
            command.Parameters.AddWithValue("@userId", habit.UserId);
            command.Parameters.AddWithValue("@title", habit.Title);
            command.Parameters.AddWithValue("@emoji", habit.Emoji);
            command.Parameters.AddWithValue("@targetDays", habit.TargetDays);
            command.Parameters.AddWithValue("@hours", (object)habit.HoursPerDay ?? DBNull.Value);
            command.Parameters.AddWithValue("@startDate", Database.DateToText(habit.StartDate));
            command.Parameters.AddWithValue("@message", (object)habit.CompletionMessage ?? DBNull.Value);
            habit.Id = Convert.ToInt32(command.ExecuteScalar());
        }
    }

    public static void Update(Habit habit)
    {
        using (SqliteConnection connection = Database.Open())
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"
                UPDATE habits
                SET title = @title, emoji = @emoji, target_days = @targetDays, hours_per_day = @hours
                WHERE id = @id";
            command.Parameters.AddWithValue("@title", habit.Title);
            command.Parameters.AddWithValue("@emoji", habit.Emoji);
            command.Parameters.AddWithValue("@targetDays", habit.TargetDays);
            command.Parameters.AddWithValue("@hours", (object)habit.HoursPerDay ?? DBNull.Value);
            command.Parameters.AddWithValue("@id", habit.Id);
            command.ExecuteNonQuery();
        }
    }

    public static void Delete(int id)
    {
        using (SqliteConnection connection = Database.Open())
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM habits WHERE id = @id";
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }
    }

    public static void AddCheckin(int habitId, DateTime date)
    {
        using (SqliteConnection connection = Database.Open())
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO checkins (habit_id, date) VALUES (@habitId, @date)";
            command.Parameters.AddWithValue("@habitId", habitId);
            command.Parameters.AddWithValue("@date", Database.DateToText(date));
            command.ExecuteNonQuery();
        }
    }
}
