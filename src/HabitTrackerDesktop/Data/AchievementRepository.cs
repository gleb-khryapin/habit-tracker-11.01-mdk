using HabitTrackerDesktop.Models;
using Microsoft.Data.Sqlite;

namespace HabitTrackerDesktop.Data;

public static class AchievementRepository
{
    // Все достижения; у полученных пользователем заполнена дата
    public static List<Achievement> GetByUser(int userId)
    {
        List<Achievement> achievements = new List<Achievement>();

        using (SqliteConnection connection = Database.Open())
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"
                SELECT a.id, a.code, a.title, a.emoji, a.description, ua.earned_date
                FROM achievements a
                LEFT JOIN user_achievements ua ON ua.achievement_id = a.id AND ua.user_id = @userId
                ORDER BY a.id";
            command.Parameters.AddWithValue("@userId", userId);

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Achievement achievement = new Achievement();
                    achievement.Id = reader.GetInt32(0);
                    achievement.Code = reader.GetString(1);
                    achievement.Title = reader.GetString(2);
                    achievement.Emoji = reader.GetString(3);
                    achievement.Description = reader.GetString(4);
                    if (!reader.IsDBNull(5))
                    {
                        achievement.EarnedDate = Database.TextToDate(reader.GetString(5));
                    }
                    achievements.Add(achievement);
                }
            }
        }

        return achievements;
    }

    public static void Add(Achievement achievement)
    {
        using (SqliteConnection connection = Database.Open())
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO achievements (code, title, emoji, description)
                VALUES (@code, @title, @emoji, @description);
                SELECT last_insert_rowid();";
            command.Parameters.AddWithValue("@code", achievement.Code);
            command.Parameters.AddWithValue("@title", achievement.Title);
            command.Parameters.AddWithValue("@emoji", achievement.Emoji);
            command.Parameters.AddWithValue("@description", achievement.Description);
            achievement.Id = Convert.ToInt32(command.ExecuteScalar());
        }
    }

    public static void Give(int userId, int achievementId, DateTime date)
    {
        using (SqliteConnection connection = Database.Open())
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO user_achievements (user_id, achievement_id, earned_date)
                VALUES (@userId, @achievementId, @date)";
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@achievementId", achievementId);
            command.Parameters.AddWithValue("@date", Database.DateToText(date));
            command.ExecuteNonQuery();
        }
    }
}
