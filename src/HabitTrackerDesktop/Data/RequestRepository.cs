using HabitTrackerDesktop.Models;
using Microsoft.Data.Sqlite;

namespace HabitTrackerDesktop.Data;

public static class RequestRepository
{
    private const string SelectSql = @"
        SELECT r.id, r.user_id, u.username, r.category_id, c.name, r.title, r.emoji, r.description,
               r.completion_message, r.status, r.reject_reason, r.created_date, r.reviewed_date
        FROM template_requests r
        JOIN users u ON u.id = r.user_id
        LEFT JOIN categories c ON c.id = r.category_id";

    public static List<TemplateRequest> GetByUser(int userId)
    {
        using (SqliteConnection connection = Database.Open())
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = SelectSql + " WHERE r.user_id = @userId ORDER BY r.id DESC";
            command.Parameters.AddWithValue("@userId", userId);
            return ReadAll(command);
        }
    }

    public static List<TemplateRequest> GetPending()
    {
        using (SqliteConnection connection = Database.Open())
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = SelectSql + " WHERE r.status = 'pending' ORDER BY r.id";
            return ReadAll(command);
        }
    }

    private static List<TemplateRequest> ReadAll(SqliteCommand command)
    {
        List<TemplateRequest> requests = new List<TemplateRequest>();

        using (SqliteDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                TemplateRequest request = new TemplateRequest();
                request.Id = reader.GetInt32(0);
                request.UserId = reader.GetInt32(1);
                request.Username = reader.GetString(2);
                if (!reader.IsDBNull(3))
                {
                    request.CategoryId = reader.GetInt32(3);
                    request.CategoryName = reader.GetString(4);
                }
                request.Title = reader.GetString(5);
                request.Emoji = reader.GetString(6);
                request.Description = reader.GetString(7);
                request.CompletionMessage = reader.GetString(8);
                request.Status = reader.GetString(9);
                if (!reader.IsDBNull(10))
                {
                    request.RejectReason = reader.GetString(10);
                }
                request.CreatedDate = Database.TextToDate(reader.GetString(11));
                if (!reader.IsDBNull(12))
                {
                    request.ReviewedDate = Database.TextToDate(reader.GetString(12));
                }
                requests.Add(request);
            }
        }

        return requests;
    }

    public static void Add(TemplateRequest request)
    {
        using (SqliteConnection connection = Database.Open())
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO template_requests (user_id, category_id, title, emoji, description, completion_message, created_date)
                VALUES (@userId, @categoryId, @title, @emoji, @description, @message, @date);
                SELECT last_insert_rowid();";
            command.Parameters.AddWithValue("@userId", request.UserId);
            command.Parameters.AddWithValue("@categoryId", (object)request.CategoryId ?? DBNull.Value);
            command.Parameters.AddWithValue("@title", request.Title);
            command.Parameters.AddWithValue("@emoji", request.Emoji);
            command.Parameters.AddWithValue("@description", request.Description);
            command.Parameters.AddWithValue("@message", request.CompletionMessage);
            command.Parameters.AddWithValue("@date", Database.DateToText(request.CreatedDate));
            request.Id = Convert.ToInt32(command.ExecuteScalar());
        }
    }

    public static void Withdraw(int id)
    {
        using (SqliteConnection connection = Database.Open())
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM template_requests WHERE id = @id AND status = 'pending'";
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }
    }

    public static void Approve(TemplateRequest request, DateTime date)
    {
        using (SqliteConnection connection = Database.Open())
        using (SqliteTransaction transaction = connection.BeginTransaction())
        {
            SqliteCommand insert = connection.CreateCommand();
            insert.Transaction = transaction;
            insert.CommandText = @"
                INSERT INTO habit_templates (category_id, title, emoji, description, completion_message)
                VALUES (@categoryId, @title, @emoji, @description, @message)";
            insert.Parameters.AddWithValue("@categoryId", (object)request.CategoryId ?? DBNull.Value);
            insert.Parameters.AddWithValue("@title", request.Title);
            insert.Parameters.AddWithValue("@emoji", request.Emoji);
            insert.Parameters.AddWithValue("@description", request.Description);
            insert.Parameters.AddWithValue("@message", request.CompletionMessage);
            insert.ExecuteNonQuery();

            SqliteCommand update = connection.CreateCommand();
            update.Transaction = transaction;
            update.CommandText = @"
                UPDATE template_requests
                SET status = 'approved', reviewed_date = @date
                WHERE id = @id";
            update.Parameters.AddWithValue("@date", Database.DateToText(date));
            update.Parameters.AddWithValue("@id", request.Id);
            update.ExecuteNonQuery();

            transaction.Commit();
        }
    }

    public static void Reject(int id, string reason, DateTime date)
    {
        using (SqliteConnection connection = Database.Open())
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"
                UPDATE template_requests
                SET status = 'rejected', reject_reason = @reason, reviewed_date = @date
                WHERE id = @id";
            command.Parameters.AddWithValue("@reason", reason);
            command.Parameters.AddWithValue("@date", Database.DateToText(date));
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }
    }
}
