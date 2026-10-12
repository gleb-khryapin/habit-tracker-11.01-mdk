using HabitTrackerDesktop.Models;
using Microsoft.Data.Sqlite;

namespace HabitTrackerDesktop.Data;

public static class TemplateRepository
{
    public static List<HabitTemplate> GetAll()
    {
        List<HabitTemplate> templates = new List<HabitTemplate>();

        using (SqliteConnection connection = Database.Open())
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT id, title, emoji, description, completion_message FROM habit_templates ORDER BY id";

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    HabitTemplate template = new HabitTemplate();
                    template.Id = reader.GetInt32(0);
                    template.Title = reader.GetString(1);
                    template.Emoji = reader.GetString(2);
                    template.Description = reader.GetString(3);
                    template.CompletionMessage = reader.GetString(4);
                    templates.Add(template);
                }
            }
        }

        return templates;
    }

    public static void Add(HabitTemplate template)
    {
        using (SqliteConnection connection = Database.Open())
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO habit_templates (title, emoji, description, completion_message)
                VALUES (@title, @emoji, @description, @message);
                SELECT last_insert_rowid();";
            command.Parameters.AddWithValue("@title", template.Title);
            command.Parameters.AddWithValue("@emoji", template.Emoji);
            command.Parameters.AddWithValue("@description", template.Description);
            command.Parameters.AddWithValue("@message", template.CompletionMessage);
            template.Id = Convert.ToInt32(command.ExecuteScalar());
        }
    }

    public static void Delete(int id)
    {
        using (SqliteConnection connection = Database.Open())
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM habit_templates WHERE id = @id";
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }
    }
}
