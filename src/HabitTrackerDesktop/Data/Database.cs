using System.Globalization;
using Microsoft.Data.Sqlite;

namespace HabitTrackerDesktop.Data;

public static class Database
{
    public static string FilePath = Path.Combine(AppContext.BaseDirectory, "habits.db");

    // Версия схемы. При изменении таблиц увеличить на 1: старый файл БД пересоздастся при запуске
    private const int SchemaVersion = 4;

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

                CREATE TABLE IF NOT EXISTS user_activity (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    user_id INTEGER NOT NULL REFERENCES users (id) ON DELETE CASCADE,
                    date TEXT NOT NULL,
                    UNIQUE (user_id, date)
                );

                CREATE TABLE IF NOT EXISTS template_requests (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    user_id INTEGER NOT NULL REFERENCES users (id) ON DELETE CASCADE,
                    category_id INTEGER REFERENCES categories (id) ON DELETE SET NULL,
                    title TEXT NOT NULL,
                    emoji TEXT NOT NULL,
                    description TEXT NOT NULL,
                    completion_message TEXT NOT NULL,
                    status TEXT NOT NULL DEFAULT 'pending' CHECK (status IN ('pending', 'approved', 'rejected')),
                    reject_reason TEXT,
                    created_date TEXT NOT NULL,
                    reviewed_date TEXT
                );

                CREATE TABLE IF NOT EXISTS audit_log (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    created_at TEXT NOT NULL DEFAULT (datetime('now', 'localtime')),
                    action TEXT NOT NULL,
                    table_name TEXT NOT NULL,
                    record_id INTEGER NOT NULL,
                    details TEXT NOT NULL
                );

                CREATE TRIGGER IF NOT EXISTS trg_checkins_activity AFTER INSERT ON checkins
                BEGIN
                    INSERT OR IGNORE INTO user_activity (user_id, date)
                    SELECT user_id, NEW.date FROM habits WHERE id = NEW.habit_id;
                END;

                CREATE TRIGGER IF NOT EXISTS trg_checkins_audit AFTER INSERT ON checkins
                BEGIN
                    INSERT INTO audit_log (action, table_name, record_id, details)
                    SELECT 'добавление', 'checkins', NEW.id, u.username || ': отметка «' || h.title || '» за ' || NEW.date
                    FROM habits h JOIN users u ON u.id = h.user_id
                    WHERE h.id = NEW.habit_id;
                END;

                CREATE TRIGGER IF NOT EXISTS trg_users_insert AFTER INSERT ON users
                BEGIN
                    INSERT INTO audit_log (action, table_name, record_id, details)
                    VALUES ('добавление', 'users', NEW.id, 'Зарегистрирован пользователь ' || NEW.username);
                END;

                CREATE TRIGGER IF NOT EXISTS trg_habits_insert AFTER INSERT ON habits
                BEGIN
                    INSERT INTO audit_log (action, table_name, record_id, details)
                    VALUES ('добавление', 'habits', NEW.id,
                        (SELECT username FROM users WHERE id = NEW.user_id)
                        || ': создана привычка «' || NEW.title || '» на ' || NEW.target_days || ' дн.');
                END;

                CREATE TRIGGER IF NOT EXISTS trg_habits_update
                AFTER UPDATE OF title, emoji, category_id, target_days, hours_per_day ON habits
                BEGIN
                    INSERT INTO audit_log (action, table_name, record_id, details)
                    VALUES ('изменение', 'habits', NEW.id,
                        (SELECT username FROM users WHERE id = NEW.user_id)
                        || ': изменена привычка «' || OLD.title || '»'
                        || CASE WHEN OLD.title <> NEW.title THEN ', новое название «' || NEW.title || '»' ELSE '' END
                        || CASE WHEN OLD.target_days <> NEW.target_days
                                THEN ', срок ' || OLD.target_days || ' → ' || NEW.target_days ELSE '' END);
                END;

                CREATE TRIGGER IF NOT EXISTS trg_habits_delete AFTER DELETE ON habits
                BEGIN
                    INSERT INTO audit_log (action, table_name, record_id, details)
                    VALUES ('удаление', 'habits', OLD.id,
                        COALESCE((SELECT username FROM users WHERE id = OLD.user_id), 'пользователь #' || OLD.user_id)
                        || ': удалена привычка «' || OLD.title || '»');
                END;

                CREATE TRIGGER IF NOT EXISTS trg_templates_insert AFTER INSERT ON habit_templates
                BEGIN
                    INSERT INTO audit_log (action, table_name, record_id, details)
                    VALUES ('добавление', 'habit_templates', NEW.id, 'В список добавлена привычка «' || NEW.title || '»');
                END;

                CREATE TRIGGER IF NOT EXISTS trg_templates_delete AFTER DELETE ON habit_templates
                BEGIN
                    INSERT INTO audit_log (action, table_name, record_id, details)
                    VALUES ('удаление', 'habit_templates', OLD.id, 'Из списка удалена привычка «' || OLD.title || '»');
                END;

                CREATE TRIGGER IF NOT EXISTS trg_requests_insert AFTER INSERT ON template_requests
                BEGIN
                    INSERT INTO audit_log (action, table_name, record_id, details)
                    VALUES ('добавление', 'template_requests', NEW.id,
                        (SELECT username FROM users WHERE id = NEW.user_id) || ': заявка на привычку «' || NEW.title || '»');
                END;

                CREATE TRIGGER IF NOT EXISTS trg_requests_review AFTER UPDATE OF status ON template_requests
                BEGIN
                    INSERT INTO audit_log (action, table_name, record_id, details)
                    VALUES ('изменение', 'template_requests', NEW.id,
                        'Заявка «' || NEW.title || '» '
                        || CASE NEW.status WHEN 'approved' THEN 'одобрена'
                                           WHEN 'rejected' THEN 'отклонена: ' || NEW.reject_reason
                                           ELSE NEW.status END);
                END;

                CREATE TRIGGER IF NOT EXISTS trg_requests_delete AFTER DELETE ON template_requests
                BEGIN
                    INSERT INTO audit_log (action, table_name, record_id, details)
                    VALUES ('удаление', 'template_requests', OLD.id,
                        COALESCE((SELECT username FROM users WHERE id = OLD.user_id), 'пользователь #' || OLD.user_id)
                        || ': отозвана заявка «' || OLD.title || '»');
                END;

                INSERT OR IGNORE INTO roles (name) VALUES ('user'), ('admin');

                PRAGMA user_version = " + SchemaVersion + ";";
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

    public static void Backup(string targetPath)
    {
        if (File.Exists(targetPath))
        {
            File.Delete(targetPath);
        }

        using (SqliteConnection connection = Open())
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = "VACUUM INTO @path";
            command.Parameters.AddWithValue("@path", targetPath);
            command.ExecuteNonQuery();
        }
    }

    public static string CheckBackupFile(string path)
    {
        try
        {
            using (SqliteConnection connection = new SqliteConnection("Data Source=" + path + ";Mode=ReadOnly"))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "PRAGMA user_version";
                int version = Convert.ToInt32(command.ExecuteScalar());
                if (version != SchemaVersion)
                {
                    return "Копия от другой версии приложения (версия схемы " + version + ", нужна " + SchemaVersion + ")";
                }
            }
        }
        catch (SqliteException)
        {
            return "Файл не является базой данных приложения";
        }
        finally
        {
            SqliteConnection.ClearAllPools();
        }
        return null;
    }

    public static void Restore(string sourcePath)
    {
        byte[] backup = File.ReadAllBytes(sourcePath);
        Backup(Path.Combine(AppContext.BaseDirectory, "habits-before-restore.db"));
        SqliteConnection.ClearAllPools();
        File.WriteAllBytes(FilePath, backup);
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
