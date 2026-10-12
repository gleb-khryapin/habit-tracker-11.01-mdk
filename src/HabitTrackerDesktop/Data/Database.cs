using System.Globalization;
using Microsoft.Data.Sqlite;

namespace HabitTrackerDesktop.Data;

public static class Database
{
    public static string FilePath = Path.Combine(AppContext.BaseDirectory, "habits.db");

    public static SqliteConnection Open()
    {
        SqliteConnection connection = new SqliteConnection("Data Source=" + FilePath + ";Foreign Keys=True");
        connection.Open();
        return connection;
    }

    public static void Initialize()
    {
        using (SqliteConnection connection = Open())
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS users (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    username TEXT NOT NULL UNIQUE COLLATE NOCASE,
                    password_hash TEXT NOT NULL,
                    role TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS habit_templates (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    title TEXT NOT NULL,
                    emoji TEXT NOT NULL,
                    description TEXT NOT NULL,
                    completion_message TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS habits (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    user_id INTEGER NOT NULL REFERENCES users (id) ON DELETE CASCADE,
                    title TEXT NOT NULL,
                    emoji TEXT NOT NULL,
                    target_days INTEGER NOT NULL,
                    hours_per_day REAL,
                    start_date TEXT NOT NULL,
                    completion_message TEXT
                );

                CREATE TABLE IF NOT EXISTS checkins (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    habit_id INTEGER NOT NULL REFERENCES habits (id) ON DELETE CASCADE,
                    date TEXT NOT NULL,
                    UNIQUE (habit_id, date)
                );";
            command.ExecuteNonQuery();
        }

        if (UserRepository.Count() == 0)
        {
            SeedData.Fill();
        }
    }

    public static string DateToText(DateTime date)
    {
        return date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
    }

    public static DateTime TextToDate(string text)
    {
        return DateTime.ParseExact(text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
    }
}
