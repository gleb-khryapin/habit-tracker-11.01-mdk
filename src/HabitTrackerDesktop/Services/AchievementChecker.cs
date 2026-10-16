using HabitTrackerDesktop.Data;
using HabitTrackerDesktop.Models;

namespace HabitTrackerDesktop.Services;

public static class AchievementChecker
{
    // Выдаёт пользователю достижения, условия которых уже выполнены, и возвращает только что выданные
    public static List<Achievement> GiveNew(int userId)
    {
        List<Habit> habits = HabitRepository.GetByUser(userId);
        int totalCheckins = 0;
        int bestProgress = 0;
        int completedHabits = 0;

        foreach (Habit habit in habits)
        {
            totalCheckins += habit.Progress;
            if (habit.Progress > bestProgress)
            {
                bestProgress = habit.Progress;
            }
            if (habit.Progress >= habit.TargetDays)
            {
                completedHabits++;
            }
        }

        List<Achievement> newAchievements = new List<Achievement>();
        DateTime today = Clock.Today;

        foreach (Achievement achievement in AchievementRepository.GetByUser(userId))
        {
            if (achievement.EarnedDate != null)
            {
                continue;
            }

            if (IsEarned(achievement.Code, habits.Count, totalCheckins, bestProgress, completedHabits))
            {
                AchievementRepository.Give(userId, achievement.Id, today);
                achievement.EarnedDate = today;
                newAchievements.Add(achievement);
            }
        }

        return newAchievements;
    }

    private static bool IsEarned(string code, int habitCount, int totalCheckins, int bestProgress, int completedHabits)
    {
        switch (code)
        {
            case "first_habit":
                return habitCount >= 1;
            case "first_checkin":
                return totalCheckins >= 1;
            case "streak_7":
                return bestProgress >= 7;
            case "streak_30":
                return bestProgress >= 30;
            case "first_completed":
                return completedHabits >= 1;
            case "completed_3":
                return completedHabits >= 3;
            default:
                return false;
        }
    }
}
