using HabitTrackerDesktop.Models;
using MySqlConnector;

namespace HabitTrackerDesktop.Data;

public static class UserRepository
{
    public static int Count()
    {
        using (MySqlConnection connection = Database.Open())
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM users";
            return Convert.ToInt32(command.ExecuteScalar());
        }
    }

    public static User FindByUsername(string username)
    {
        using (MySqlConnection connection = Database.Open())
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = @"
                SELECT u.id, u.username, u.password_hash, r.name
                FROM users u
                JOIN roles r ON r.id = u.role_id
                WHERE u.username = @username";
            command.Parameters.AddWithValue("@username", username);

            using (MySqlDataReader reader = command.ExecuteReader())
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

    public static List<User> GetAll()
    {
        List<User> users = new List<User>();

        using (MySqlConnection connection = Database.Open())
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = @"
                SELECT u.id, u.username, r.name
                FROM users u
                JOIN roles r ON r.id = u.role_id
                ORDER BY u.username";

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    User user = new User();
                    user.Id = reader.GetInt32(0);
                    user.Username = reader.GetString(1);
                    user.Role = reader.GetString(2);
                    users.Add(user);
                }
            }
        }

        return users;
    }

    public static void Add(User user)
    {
        using (MySqlConnection connection = Database.Open())
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO users (username, password_hash, role_id)
                VALUES (@username, @passwordHash, (SELECT id FROM roles WHERE name = @role));
                SELECT LAST_INSERT_ID();";
            command.Parameters.AddWithValue("@username", user.Username);
            command.Parameters.AddWithValue("@passwordHash", user.PasswordHash);
            command.Parameters.AddWithValue("@role", user.Role);
            user.Id = Convert.ToInt32(command.ExecuteScalar());
        }
    }
}
