using System.Globalization;
using Microsoft.Data.Sqlite;

namespace HabitTrackerDesktop.Data;

public static class Database
{
    public static string FilePath = Path.Combine(AppContext.BaseDirectory, "habits.db");

    // Версия схемы по ERD (docs/erd.drawio). Меняется вместе с PRAGMA user_version в Initialize
    private const int SchemaVersion = 3;

    public static SqliteConnection Open()
    {
        SqliteConnection connection = new SqliteConnection("Data Source=" + FilePath + ";Foreign Keys=True");
        connection.Open();
        return connection;
    }

    public static void Initialize()
    {
        // Файл от старой версии: его таблицы не совпадают с ERD, а начальные данные в него не попадут.
        // Пересоздаём базу с нуля — в ней только тестовые данные.
        if (File.Exists(FilePath) && ReadSchemaVersion() < SchemaVersion)
        {
            SqliteConnection.ClearAllPools();
            File.Delete(FilePath);
        }

        using (SqliteConnection connection = Open())
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS roles (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT NOT NULL UNIQUE
                );

                CREATE TABLE IF NOT EXISTS users (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    username TEXT NOT NULL UNIQUE COLLATE NOCASE,
                    password_hash TEXT NOT NULL,
                    role_id INTEGER NOT NULL REFERENCES roles (id)
                );

                CREATE TABLE IF NOT EXISTS categories (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT NOT NULL UNIQUE
                );

                CREATE TABLE IF NOT EXISTS completion_messages (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    text TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS habit_templates (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    category_id INTEGER REFERENCES categories (id) ON DELETE SET NULL,
                    title TEXT NOT NULL,
                    emoji TEXT NOT NULL,
                    description TEXT NOT NULL,
                    completion_message TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS habits (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    user_id INTEGER NOT NULL REFERENCES users (id) ON DELETE CASCADE,
                    template_id INTEGER REFERENCES habit_templates (id) ON DELETE SET NULL,
                    category_id INTEGER REFERENCES categories (id) ON DELETE SET NULL,
                    message_id INTEGER REFERENCES completion_messages (id) ON DELETE SET NULL,
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
                );

                CREATE TABLE IF NOT EXISTS reminders (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    habit_id INTEGER NOT NULL REFERENCES habits (id) ON DELETE CASCADE,
                    time TEXT NOT NULL,
                    is_enabled INTEGER NOT NULL DEFAULT 1,
                    UNIQUE (habit_id, time)
                );

                CREATE TABLE IF NOT EXISTS achievements (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    code TEXT NOT NULL UNIQUE,
                    title TEXT NOT NULL,
                    emoji TEXT NOT NULL,
                    description TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS user_achievements (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    user_id INTEGER NOT NULL REFERENCES users (id) ON DELETE CASCADE,
                    achievement_id INTEGER NOT NULL REFERENCES achievements (id) ON DELETE CASCADE,
                    earned_date TEXT NOT NULL,
                    UNIQUE (user_id, achievement_id)
                );

                INSERT OR IGNORE INTO roles (name) VALUES ('user'), ('admin');

                PRAGMA user_version = 3;";
            command.ExecuteNonQuery();
        }

        if (UserRepository.Count() == 0)
        {
            SeedData.Fill();
        }
    }

    private static int ReadSchemaVersion()
    {
        using (SqliteConnection connection = Open())
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = "PRAGMA user_version";
            return Convert.ToInt32(command.ExecuteScalar());
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
