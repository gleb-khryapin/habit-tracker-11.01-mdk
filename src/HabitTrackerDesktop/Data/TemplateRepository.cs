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
            command.CommandText = @"
                SELECT t.id, t.title, t.emoji, t.description, t.completion_message, t.category_id, c.name
                FROM habit_templates t
                LEFT JOIN categories c ON c.id = t.category_id
                ORDER BY t.id";

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
                    if (!reader.IsDBNull(5))
                    {
                        template.CategoryId = reader.GetInt32(5);
                        template.CategoryName = reader.GetString(6);
                    }
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
                INSERT INTO habit_templates (category_id, title, emoji, description, completion_message)
                VALUES (@categoryId, @title, @emoji, @description, @message);
                SELECT last_insert_rowid();";
            command.Parameters.AddWithValue("@categoryId", (object)template.CategoryId ?? DBNull.Value);
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
