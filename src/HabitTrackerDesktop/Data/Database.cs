using System.Globalization;
using System.Text;
using MySqlConnector;

namespace HabitTrackerDesktop.Data;

public static class Database
{
    public static string ConnectionString = "Server=localhost;Port=3306;User ID=root;Password=;Database=habit_tracker;CharSet=utf8mb4";

    private const string BackupHeader = "-- habit_tracker backup";

    private static readonly string[] Tables =
    {
        "roles", "users", "categories", "completion_messages", "habit_templates", "habits", "checkins",
        "reminders", "achievements", "user_achievements", "user_activity", "template_requests", "audit_log"
    };

    private static readonly string[] TableSql =
    {
        @"CREATE TABLE IF NOT EXISTS roles (
            id INT AUTO_INCREMENT PRIMARY KEY,
            name VARCHAR(20) NOT NULL UNIQUE
        )",
        @"CREATE TABLE IF NOT EXISTS users (
            id INT AUTO_INCREMENT PRIMARY KEY,
            username VARCHAR(20) NOT NULL UNIQUE,
            password_hash VARCHAR(100) NOT NULL,
            role_id INT NOT NULL,
            FOREIGN KEY (role_id) REFERENCES roles (id)
        )",
        @"CREATE TABLE IF NOT EXISTS categories (
            id INT AUTO_INCREMENT PRIMARY KEY,
            name VARCHAR(30) NOT NULL UNIQUE
        )",
        @"CREATE TABLE IF NOT EXISTS completion_messages (
            id INT AUTO_INCREMENT PRIMARY KEY,
            text VARCHAR(500) NOT NULL
        )",
        @"CREATE TABLE IF NOT EXISTS habit_templates (
            id INT AUTO_INCREMENT PRIMARY KEY,
            category_id INT NULL,
            title VARCHAR(50) NOT NULL,
            emoji VARCHAR(10) NOT NULL,
            description VARCHAR(200) NOT NULL DEFAULT '',
            completion_message VARCHAR(500) NOT NULL,
            FOREIGN KEY (category_id) REFERENCES categories (id) ON DELETE SET NULL
        )",
        @"CREATE TABLE IF NOT EXISTS habits (
            id INT AUTO_INCREMENT PRIMARY KEY,
            user_id INT NOT NULL,
            template_id INT NULL,
            category_id INT NULL,
            message_id INT NULL,
            title VARCHAR(50) NOT NULL,
            emoji VARCHAR(10) NOT NULL,
            target_days INT NOT NULL CHECK (target_days BETWEEN 1 AND 365),
            hours_per_day DECIMAL(3,1) NULL CHECK (hours_per_day IS NULL OR hours_per_day BETWEEN 0.5 AND 24),
            start_date DATE NOT NULL,
            completion_message VARCHAR(500) NULL,
            FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE CASCADE,
            FOREIGN KEY (template_id) REFERENCES habit_templates (id) ON DELETE SET NULL,
            FOREIGN KEY (category_id) REFERENCES categories (id) ON DELETE SET NULL,
            FOREIGN KEY (message_id) REFERENCES completion_messages (id) ON DELETE SET NULL
        )",
        @"CREATE TABLE IF NOT EXISTS checkins (
            id INT AUTO_INCREMENT PRIMARY KEY,
            habit_id INT NOT NULL,
            date DATE NOT NULL,
            UNIQUE (habit_id, date),
            FOREIGN KEY (habit_id) REFERENCES habits (id) ON DELETE CASCADE
        )",
        @"CREATE TABLE IF NOT EXISTS reminders (
            id INT AUTO_INCREMENT PRIMARY KEY,
            habit_id INT NOT NULL,
            time TIME NOT NULL,
            is_enabled BOOLEAN NOT NULL DEFAULT TRUE,
            UNIQUE (habit_id, time),
            FOREIGN KEY (habit_id) REFERENCES habits (id) ON DELETE CASCADE
        )",
        @"CREATE TABLE IF NOT EXISTS achievements (
            id INT AUTO_INCREMENT PRIMARY KEY,
            code VARCHAR(30) NOT NULL UNIQUE,
            title VARCHAR(50) NOT NULL,
            emoji VARCHAR(10) NOT NULL,
            description VARCHAR(200) NOT NULL
        )",
        @"CREATE TABLE IF NOT EXISTS user_achievements (
            id INT AUTO_INCREMENT PRIMARY KEY,
            user_id INT NOT NULL,
            achievement_id INT NOT NULL,
            earned_date DATE NOT NULL,
            UNIQUE (user_id, achievement_id),
            FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE CASCADE,
            FOREIGN KEY (achievement_id) REFERENCES achievements (id) ON DELETE CASCADE
        )",
        @"CREATE TABLE IF NOT EXISTS user_activity (
            id INT AUTO_INCREMENT PRIMARY KEY,
            user_id INT NOT NULL,
            date DATE NOT NULL,
            UNIQUE (user_id, date),
            FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE CASCADE
        )",
        @"CREATE TABLE IF NOT EXISTS template_requests (
            id INT AUTO_INCREMENT PRIMARY KEY,
            user_id INT NOT NULL,
            category_id INT NULL,
            title VARCHAR(50) NOT NULL,
            emoji VARCHAR(10) NOT NULL,
            description VARCHAR(200) NOT NULL DEFAULT '',
            completion_message VARCHAR(500) NOT NULL,
            status ENUM('pending', 'approved', 'rejected') NOT NULL DEFAULT 'pending',
            reject_reason VARCHAR(200) NULL,
            created_date DATE NOT NULL,
            reviewed_date DATE NULL,
            FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE CASCADE,
            FOREIGN KEY (category_id) REFERENCES categories (id) ON DELETE SET NULL
        )",
        @"CREATE TABLE IF NOT EXISTS audit_log (
            id INT AUTO_INCREMENT PRIMARY KEY,
            created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
            action VARCHAR(20) NOT NULL,
            table_name VARCHAR(50) NOT NULL,
            record_id INT NOT NULL,
            details VARCHAR(1000) NOT NULL
        )",
    };

    private static readonly string[] TriggerSql =
    {
        @"CREATE TRIGGER trg_checkins_activity AFTER INSERT ON checkins FOR EACH ROW
            INSERT IGNORE INTO user_activity (user_id, date)
            SELECT user_id, NEW.date FROM habits WHERE id = NEW.habit_id",
        @"CREATE TRIGGER trg_checkins_audit AFTER INSERT ON checkins FOR EACH ROW
            INSERT INTO audit_log (action, table_name, record_id, details)
            SELECT 'добавление', 'checkins', NEW.id,
                CONCAT(u.username, ': отметка «', h.title, '» за ', DATE_FORMAT(NEW.date, '%Y-%m-%d'))
            FROM habits h JOIN users u ON u.id = h.user_id
            WHERE h.id = NEW.habit_id",
        @"CREATE TRIGGER trg_users_insert AFTER INSERT ON users FOR EACH ROW
            INSERT INTO audit_log (action, table_name, record_id, details)
            VALUES ('добавление', 'users', NEW.id, CONCAT('Зарегистрирован пользователь ', NEW.username))",
        @"CREATE TRIGGER trg_habits_insert AFTER INSERT ON habits FOR EACH ROW
            INSERT INTO audit_log (action, table_name, record_id, details)
            VALUES ('добавление', 'habits', NEW.id,
                CONCAT((SELECT username FROM users WHERE id = NEW.user_id),
                    ': создана привычка «', NEW.title, '» на ', NEW.target_days, ' дн.'))",
        @"CREATE TRIGGER trg_habits_update AFTER UPDATE ON habits FOR EACH ROW
            INSERT INTO audit_log (action, table_name, record_id, details)
            SELECT 'изменение', 'habits', NEW.id,
                CONCAT((SELECT username FROM users WHERE id = NEW.user_id), ': изменена привычка «', OLD.title, '»',
                    IF(BINARY OLD.title <> BINARY NEW.title, CONCAT(', новое название «', NEW.title, '»'), ''),
                    IF(OLD.target_days <> NEW.target_days, CONCAT(', срок ', OLD.target_days, ' → ', NEW.target_days), ''))
            FROM DUAL
            WHERE NOT (BINARY OLD.title <=> BINARY NEW.title AND BINARY OLD.emoji <=> BINARY NEW.emoji
                AND OLD.category_id <=> NEW.category_id AND OLD.target_days <=> NEW.target_days
                AND OLD.hours_per_day <=> NEW.hours_per_day)",
        @"CREATE TRIGGER trg_habits_delete AFTER DELETE ON habits FOR EACH ROW
            INSERT INTO audit_log (action, table_name, record_id, details)
            VALUES ('удаление', 'habits', OLD.id,
                CONCAT(COALESCE((SELECT username FROM users WHERE id = OLD.user_id), CONCAT('пользователь #', OLD.user_id)),
                    ': удалена привычка «', OLD.title, '»'))",
        @"CREATE TRIGGER trg_templates_insert AFTER INSERT ON habit_templates FOR EACH ROW
            INSERT INTO audit_log (action, table_name, record_id, details)
            VALUES ('добавление', 'habit_templates', NEW.id, CONCAT('В список добавлена привычка «', NEW.title, '»'))",
        @"CREATE TRIGGER trg_templates_delete AFTER DELETE ON habit_templates FOR EACH ROW
            INSERT INTO audit_log (action, table_name, record_id, details)
            VALUES ('удаление', 'habit_templates', OLD.id, CONCAT('Из списка удалена привычка «', OLD.title, '»'))",
        @"CREATE TRIGGER trg_requests_insert AFTER INSERT ON template_requests FOR EACH ROW
            INSERT INTO audit_log (action, table_name, record_id, details)
            VALUES ('добавление', 'template_requests', NEW.id,
                CONCAT((SELECT username FROM users WHERE id = NEW.user_id), ': заявка на привычку «', NEW.title, '»'))",
        @"CREATE TRIGGER trg_requests_review AFTER UPDATE ON template_requests FOR EACH ROW
            INSERT INTO audit_log (action, table_name, record_id, details)
            SELECT 'изменение', 'template_requests', NEW.id,
                CONCAT('Заявка «', NEW.title, '» ',
                    CASE NEW.status
                        WHEN 'approved' THEN 'одобрена'
                        WHEN 'rejected' THEN CONCAT('отклонена: ', COALESCE(NEW.reject_reason, ''))
                        ELSE NEW.status
                    END)
            FROM DUAL
            WHERE NOT (OLD.status <=> NEW.status)",
        @"CREATE TRIGGER trg_requests_delete AFTER DELETE ON template_requests FOR EACH ROW
            INSERT INTO audit_log (action, table_name, record_id, details)
            VALUES ('удаление', 'template_requests', OLD.id,
                CONCAT(COALESCE((SELECT username FROM users WHERE id = OLD.user_id), CONCAT('пользователь #', OLD.user_id)),
                    ': отозвана заявка «', OLD.title, '»'))",
    };

    public static void LoadSettings()
    {
        string path = Path.Combine(AppContext.BaseDirectory, "connection.txt");
        if (File.Exists(path))
        {
            ConnectionString = File.ReadAllText(path).Trim();
        }
    }

    public static MySqlConnection Open()
    {
        MySqlConnection connection = new MySqlConnection(ConnectionString);
        connection.Open();
        return connection;
    }

    public static void Initialize()
    {
        CreateDatabase();

        using (MySqlConnection connection = Open())
        {
            foreach (string sql in TableSql)
            {
                Execute(connection, sql);
            }

            foreach (string sql in TriggerSql)
            {
                string name = sql.Split(' ')[2];
                Execute(connection, "DROP TRIGGER IF EXISTS " + name);
                Execute(connection, sql);
            }

            Execute(connection, "INSERT IGNORE INTO roles (name) VALUES ('user'), ('admin')");
        }

        if (UserRepository.Count() == 0)
        {
            SeedData.Fill();
        }
    }

    public static void Reset()
    {
        MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder(ConnectionString);
        string name = builder.Database;
        builder.Database = "";

        using (MySqlConnection connection = new MySqlConnection(builder.ConnectionString))
        {
            connection.Open();
            Execute(connection, "DROP DATABASE IF EXISTS `" + name + "`");
        }

        MySqlConnection.ClearAllPools();
        Initialize();
    }

    private static void CreateDatabase()
    {
        MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder(ConnectionString);
        string name = builder.Database;
        builder.Database = "";

        using (MySqlConnection connection = new MySqlConnection(builder.ConnectionString))
        {
            connection.Open();
            Execute(connection, "CREATE DATABASE IF NOT EXISTS `" + name + "` CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_520_ci");
        }
    }

    private static void Execute(MySqlConnection connection, string sql, MySqlTransaction transaction = null)
    {
        MySqlCommand command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = sql;
        command.ExecuteNonQuery();
    }

    public static void Backup(string targetPath)
    {
        using (MySqlConnection connection = Open())
        using (StreamWriter writer = new StreamWriter(targetPath, false, new UTF8Encoding(false)))
        {
            writer.WriteLine(BackupHeader);
            writer.WriteLine("-- " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            foreach (string table in Tables)
            {
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM `" + table + "`";

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    List<string> columns = new List<string>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        columns.Add("`" + reader.GetName(i) + "`");
                    }

                    while (reader.Read())
                    {
                        List<string> values = new List<string>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            values.Add(ToSqlValue(reader.GetValue(i)));
                        }
                        writer.WriteLine("INSERT INTO `" + table + "` (" + string.Join(", ", columns) + ") VALUES (" + string.Join(", ", values) + ");");
                    }
                }
            }
        }
    }

    private static string ToSqlValue(object value)
    {
        if (value is DBNull)
        {
            return "NULL";
        }
        if (value is string)
        {
            string text = (string)value;
            text = text.Replace("\\", "\\\\").Replace("'", "\\'").Replace("\r", "\\r").Replace("\n", "\\n").Replace("\0", "\\0");
            return "'" + text + "'";
        }
        if (value is DateTime)
        {
            DateTime date = (DateTime)value;
            if (date.TimeOfDay == TimeSpan.Zero)
            {
                return "'" + date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "'";
            }
            return "'" + date.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + "'";
        }
        if (value is TimeSpan)
        {
            return "'" + ((TimeSpan)value).ToString(@"hh\:mm\:ss") + "'";
        }
        if (value is bool)
        {
            return (bool)value ? "1" : "0";
        }
        return Convert.ToString(value, CultureInfo.InvariantCulture);
    }

    public static string CheckBackupFile(string path)
    {
        try
        {
            using (StreamReader reader = new StreamReader(path))
            {
                if (reader.ReadLine() != BackupHeader)
                {
                    return "Файл не является резервной копией приложения";
                }
            }
        }
        catch (IOException)
        {
            return "Не удалось прочитать файл резервной копии";
        }
        return null;
    }

    public static void Restore(string sourcePath)
    {
        string[] lines = File.ReadAllLines(sourcePath);
        Backup(Path.Combine(AppContext.BaseDirectory, "habits-before-restore.sql"));

        using (MySqlConnection connection = Open())
        using (MySqlTransaction transaction = connection.BeginTransaction())
        {
            Execute(connection, "SET FOREIGN_KEY_CHECKS = 0", transaction);

            foreach (string table in Tables)
            {
                Execute(connection, "DELETE FROM `" + table + "`", transaction);
            }

            foreach (string line in lines)
            {
                if (line.StartsWith("INSERT INTO") && !IsFilledByTriggers(line))
                {
                    Execute(connection, line, transaction);
                }
            }

            Execute(connection, "DELETE FROM audit_log", transaction);
            Execute(connection, "DELETE FROM user_activity", transaction);

            foreach (string line in lines)
            {
                if (IsFilledByTriggers(line))
                {
                    Execute(connection, line, transaction);
                }
            }

            Execute(connection, "SET FOREIGN_KEY_CHECKS = 1", transaction);
            transaction.Commit();
        }
    }

    private static bool IsFilledByTriggers(string line)
    {
        return line.StartsWith("INSERT INTO `audit_log`") || line.StartsWith("INSERT INTO `user_activity`");
    }
}
