using HabitTrackerDesktop.Models;
using MySqlConnector;

namespace HabitTrackerDesktop.Data;

public static class HabitRepository
{
    public static List<Habit> GetByUser(int userId)
    {
        List<Habit> habits = new List<Habit>();

        using (MySqlConnection connection = Database.Open())
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = @"
                SELECT h.id, h.user_id, h.title, h.emoji, h.target_days, h.hours_per_day, h.start_date, h.completion_message,
                       (SELECT COUNT(*) FROM checkins ch WHERE ch.habit_id = h.id) AS progress,
                       h.template_id, h.category_id, c.name, m.text
                FROM habits h
                LEFT JOIN categories c ON c.id = h.category_id
                LEFT JOIN completion_messages m ON m.id = h.message_id
                WHERE h.user_id = @userId
                ORDER BY h.id";
            command.Parameters.AddWithValue("@userId", userId);

            using (MySqlDataReader reader = command.ExecuteReader())
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
                        habit.HoursPerDay = Convert.ToDouble(reader.GetDecimal(5));
                    }
                    habit.StartDate = reader.GetDateTime(6);
                    if (!reader.IsDBNull(7))
                    {
                        habit.CompletionMessage = reader.GetString(7);
                    }
                    habit.Progress = Convert.ToInt32(reader.GetInt64(8));
                    if (!reader.IsDBNull(9))
                    {
                        habit.TemplateId = reader.GetInt32(9);
                    }
                    if (!reader.IsDBNull(10))
                    {
                        habit.CategoryId = reader.GetInt32(10);
                        habit.CategoryName = reader.GetString(11);
                    }
                    if (!reader.IsDBNull(12))
                    {
                        habit.PoolMessage = reader.GetString(12);
                    }
                    habits.Add(habit);
                }
            }
        }

        return habits;
    }

    public static void Add(Habit habit)
    {
        using (MySqlConnection connection = Database.Open())
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO habits (user_id, template_id, category_id, title, emoji, target_days, hours_per_day, start_date, completion_message)
                VALUES (@userId, @templateId, @categoryId, @title, @emoji, @targetDays, @hours, @startDate, @message);
                SELECT LAST_INSERT_ID();";
            command.Parameters.AddWithValue("@userId", habit.UserId);
            command.Parameters.AddWithValue("@templateId", (object)habit.TemplateId ?? DBNull.Value);
            command.Parameters.AddWithValue("@categoryId", (object)habit.CategoryId ?? DBNull.Value);
            command.Parameters.AddWithValue("@title", habit.Title);
            command.Parameters.AddWithValue("@emoji", habit.Emoji);
            command.Parameters.AddWithValue("@targetDays", habit.TargetDays);
            command.Parameters.AddWithValue("@hours", (object)habit.HoursPerDay ?? DBNull.Value);
            command.Parameters.AddWithValue("@startDate", habit.StartDate.Date);
            command.Parameters.AddWithValue("@message", (object)habit.CompletionMessage ?? DBNull.Value);
            habit.Id = Convert.ToInt32(command.ExecuteScalar());
        }

        // У своей привычки нет сообщения от предложенной — закрепляем за ней фразу из общего пула
        if (habit.CompletionMessage == null)
        {
            List<CompletionMessage> pool = MessageRepository.GetAll();
            if (pool.Count > 0)
            {
                CompletionMessage message = pool[habit.Id % pool.Count];
                SetMessage(habit.Id, message.Id);
                habit.PoolMessage = message.Text;
            }
        }
    }

    private static void SetMessage(int habitId, int messageId)
    {
        using (MySqlConnection connection = Database.Open())
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE habits SET message_id = @messageId WHERE id = @id";
            command.Parameters.AddWithValue("@messageId", messageId);
            command.Parameters.AddWithValue("@id", habitId);
            command.ExecuteNonQuery();
        }
    }

    public static void Update(Habit habit)
    {
        using (MySqlConnection connection = Database.Open())
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = @"
                UPDATE habits
                SET title = @title, emoji = @emoji, category_id = @categoryId, target_days = @targetDays, hours_per_day = @hours
                WHERE id = @id";
            command.Parameters.AddWithValue("@title", habit.Title);
            command.Parameters.AddWithValue("@emoji", habit.Emoji);
            command.Parameters.AddWithValue("@categoryId", (object)habit.CategoryId ?? DBNull.Value);
            command.Parameters.AddWithValue("@targetDays", habit.TargetDays);
            command.Parameters.AddWithValue("@hours", (object)habit.HoursPerDay ?? DBNull.Value);
            command.Parameters.AddWithValue("@id", habit.Id);
            command.ExecuteNonQuery();
        }
    }

    public static void Delete(int id)
    {
        using (MySqlConnection connection = Database.Open())
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM habits WHERE id = @id";
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }
    }

    public static void AddCheckin(int habitId, DateTime date)
    {
        using (MySqlConnection connection = Database.Open())
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO checkins (habit_id, date) VALUES (@habitId, @date)";
            command.Parameters.AddWithValue("@habitId", habitId);
            command.Parameters.AddWithValue("@date", date.Date);
            command.ExecuteNonQuery();
        }
    }
}
