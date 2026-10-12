using System.Text.RegularExpressions;

namespace HabitTrackerDesktop.Services;

public static class Rules
{
    public static readonly string[] Emojis =
    {
        "🎯", "💧", "🏃", "📚", "🧘", "😴", "🍎", "💪", "📝", "🎸", "🌱", "📵", "🧠", "🚶", "🎨", "💻"
    };

    public static readonly string[] MessagePool =
    {
        "Финиш! Каждый из этих дней был маленькой победой над «потом» — и вместе они сложились в большую.",
        "Весь срок без единого пропуска. Это и есть дисциплина: не настроение, а решение, которое ты принимаешь каждый день.",
        "Привычка закреплена! Теперь это не усилие, а часть тебя. Дальше будет только легче.",
        "Готово! Теперь ты точно знаешь: если решить — получится. Запомни это чувство и бери новую цель.",
        "Регулярность сильнее таланта — и теперь у тебя есть доказательство. Можно гордиться, это заслуженно."
    };

    public static string GetPoolMessage(int habitId)
    {
        return MessagePool[habitId % MessagePool.Length];
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
