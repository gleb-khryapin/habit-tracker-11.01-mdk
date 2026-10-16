using HabitTrackerDesktop.Models;
using Microsoft.Data.Sqlite;

namespace HabitTrackerDesktop.Data;

public static class CategoryRepository
{
    public static List<Category> GetAll()
    {
        List<Category> categories = new List<Category>();

        using (SqliteConnection connection = Database.Open())
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT id, name FROM categories ORDER BY name";

            using (SqliteDataReader reader = command.ExecuteReader())
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
        using (SqliteConnection connection = Database.Open())
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO categories (name) VALUES (@name);
                SELECT last_insert_rowid();";
            command.Parameters.AddWithValue("@name", category.Name);
            category.Id = Convert.ToInt32(command.ExecuteScalar());
        }
    }

    public static void Delete(int id)
    {
        using (SqliteConnection connection = Database.Open())
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM categories WHERE id = @id";
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }
    }
}
