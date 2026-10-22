using HabitTrackerDesktop.Models;
using MySqlConnector;

namespace HabitTrackerDesktop.Data;

public static class MessageRepository
{
    public static List<CompletionMessage> GetAll()
    {
        List<CompletionMessage> messages = new List<CompletionMessage>();

        using (MySqlConnection connection = Database.Open())
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT id, text FROM completion_messages ORDER BY id";

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    CompletionMessage message = new CompletionMessage();
                    message.Id = reader.GetInt32(0);
                    message.Text = reader.GetString(1);
                    messages.Add(message);
                }
            }
        }

        return messages;
    }

    public static void Add(CompletionMessage message)
    {
        using (MySqlConnection connection = Database.Open())
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO completion_messages (text) VALUES (@text);
                SELECT LAST_INSERT_ID();";
            command.Parameters.AddWithValue("@text", message.Text);
            message.Id = Convert.ToInt32(command.ExecuteScalar());
        }
    }

    public static void Delete(int id)
    {
        using (MySqlConnection connection = Database.Open())
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM completion_messages WHERE id = @id";
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }
    }
}
