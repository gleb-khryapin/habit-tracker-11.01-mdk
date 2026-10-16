using HabitTrackerDesktop.Models;
using Microsoft.Data.Sqlite;

namespace HabitTrackerDesktop.Data;

public static class UserRepository
{
    public static int Count()
    {
        using (SqliteConnection connection = Database.Open())
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM users";
            return Convert.ToInt32(command.ExecuteScalar());
        }
    }

    public static User FindByUsername(string username)
    {
        using (SqliteConnection connection = Database.Open())
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"
                SELECT u.id, u.username, u.password_hash, r.name
                FROM users u
                JOIN roles r ON r.id = u.role_id
                WHERE u.username = @username";
            command.Parameters.AddWithValue("@username", username);

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (!reader.Read())
                {
                    return null;
                }

                User user = new User();
                user.Id = reader.GetInt32(0);
                user.Username = reader.GetString(1);
                user.PasswordHash = reader.GetString(2);
                user.Role = reader.GetString(3);
                return user;
            }
        }
    }

    public static void Add(User user)
    {
        using (SqliteConnection connection = Database.Open())
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO users (username, password_hash, role_id)
                VALUES (@username, @passwordHash, (SELECT id FROM roles WHERE name = @role));
                SELECT last_insert_rowid();";
            command.Parameters.AddWithValue("@username", user.Username);
            command.Parameters.AddWithValue("@passwordHash", user.PasswordHash);
            command.Parameters.AddWithValue("@role", user.Role);
            user.Id = Convert.ToInt32(command.ExecuteScalar());
        }
    }
}
