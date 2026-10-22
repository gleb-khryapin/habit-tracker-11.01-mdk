using HabitTrackerDesktop.Models;
using MySqlConnector;

namespace HabitTrackerDesktop.Data;

public static class CategoryRepository
{
    public static List<Category> GetAll()
    {
        List<Category> categories = new List<Category>();

        using (MySqlConnection connection = Database.Open())
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT id, name FROM categories ORDER BY name";

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Category category = new Category();
                    category.Id = reader.GetInt32(0);
                    category.Name = reader.GetString(1);
                    categories.Add(category);
                }
            }
        }

        return categories;
    }

    public static void Add(Category category)
    {
        using (MySqlConnection connection = Database.Open())
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO categories (name) VALUES (@name);
                SELECT LAST_INSERT_ID();";
            command.Parameters.AddWithValue("@name", category.Name);
            category.Id = Convert.ToInt32(command.ExecuteScalar());
        }
    }

    public static void Delete(int id)
    {
        using (MySqlConnection connection = Database.Open())
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM categories WHERE id = @id";
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }
    }
}
