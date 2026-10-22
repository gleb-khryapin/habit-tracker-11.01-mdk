using System.Text.RegularExpressions;

namespace HabitTrackerDesktop.Services;

public static class Rules
{
    public static readonly string[] Emojis =
    {
        "🎯", "💧", "🏃", "📚", "🧘", "😴", "🍎", "💪", "📝", "🎸", "🌱", "📵", "🧠", "🚶", "🎨", "💻"
    };

    // Показывается, если за привычкой не закреплена фраза из пула (пул пуст или фразу удалили)
    public const string DefaultMessage = "Привычка выполнена! Весь срок без единого пропуска — отличная работа.";

    public const int MaxReminders = 5;

    public const int MaxPendingRequests = 3;

    public static string CheckRejectReason(string reason)
    {
        if (reason == "")
        {
            return "Укажи причину отказа";
        }
        if (reason.Length < 5 || reason.Length > 200)
        {
            return "Причина отказа: от 5 до 200 символов";
        }
        return null;
    }

    public static string CheckCategoryName(string name)
    {
        if (name == "")
        {
            return "Укажи название категории";
        }
        if (name.Length > 30)
        {
            return "Название категории: не длиннее 30 символов";
        }
        return null;
    }

    public static string CheckPoolMessage(string message)
    {
        if (message == "")
        {
            return "Укажи текст фразы";
        }
        if (message.Length < 10 || message.Length > 500)
        {
            return "Фраза: от 10 до 500 символов";
        }
        return null;
    }

    public static string CheckUsername(string username)
    {
        if (username == "")
        {
            return "Укажи логин";
        }
        if (!Regex.IsMatch(username, "^[A-Za-z0-9_]{3,20}$"))
        {
            return "Логин: от 3 до 20 символов, только латиница, цифры и _";
        }
        return null;
    }

    public static string CheckPassword(string password)
    {
        if (password == "")
        {
            return "Укажи пароль";
        }
        if (password.Length < 6 || password.Length > 64)
        {
            return "Пароль: от 6 до 64 символов";
        }
        return null;
    }

    public static string CheckTitle(string title)
    {
        if (title == "")
        {
            return "Укажи название";
        }
        if (title.Length > 50)
        {
            return "Название: не длиннее 50 символов";
        }
        return null;
    }

    public static string CheckDays(int days)
    {
        if (days < 1 || days > 365)
        {
            return "Срок: от 1 до 365 дней";
        }
        return null;
    }

    public static string CheckHours(double? hours)
    {
        if (hours == null)
        {
            return null;
        }
        double doubled = hours.Value * 2;
        if (hours < 0.5 || hours > 24 || doubled != Math.Round(doubled))
        {
            return "Часы в день: от 0.5 до 24 с шагом 0.5";
        }
        return null;
    }

    public static string CheckDescription(string description)
    {
        if (description.Length > 200)
        {
            return "Описание: не длиннее 200 символов";
        }
        return null;
    }

    public static string CheckCompletionMessage(string message)
    {
        if (message == "")
        {
            return "Укажи сообщение после выполнения";
        }
        if (message.Length < 10 || message.Length > 500)
        {
            return "Сообщение после выполнения: от 10 до 500 символов";
        }
        return null;
    }
}
