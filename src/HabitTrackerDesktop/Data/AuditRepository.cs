using HabitTrackerDesktop.Models;
using Microsoft.Data.Sqlite;

namespace HabitTrackerDesktop.Data;

public static class AuditRepository
{
    public static List<AuditEntry> GetLatest(int count)
    {
        List<AuditEntry> entries = new List<AuditEntry>();

        using (SqliteConnection connection = Database.Open())
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"
                SELECT id, created_at, action, table_name, record_id, details
                FROM audit_log
                ORDER BY id DESC
                LIMIT @count";
            command.Parameters.AddWithValue("@count", count);

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    AuditEntry entry = new AuditEntry();
                    entry.Id = reader.GetInt32(0);
                    entry.CreatedAt = reader.GetString(1);
                    entry.Action = reader.GetString(2);
                    entry.TableName = reader.GetString(3);
                    entry.RecordId = reader.GetInt32(4);
                    entry.Details = reader.GetString(5);
                    entries.Add(entry);
                }
            }
        }

        return entries;
    }

    public static void Clear()
    {
        using (SqliteConnection connection = Database.Open())
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM audit_log";
            command.ExecuteNonQuery();
        }
    }
}
