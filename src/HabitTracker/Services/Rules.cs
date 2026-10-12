using System.Globalization;
using System.Text.RegularExpressions;

namespace HabitTracker.Services;

/// <summary>
/// Правила валидации из docs/requirements.md (раздел 3).
/// Каждый метод возвращает текст ошибки или null, если значение корректно.
/// </summary>
public static partial class Rules
{
    public const string DefaultEmoji = "🎯";

    public static readonly string[] AllowedEmojis =
        ["🎯", "💧", "🏃", "📚", "🧘", "😴", "🍎", "💪", "📝", "🎸", "🌱", "📵", "🧠", "🚶", "🎨", "💻"];

    /// <summary>Общий пул сообщений для своих привычек (раздел 7 требований).</summary>
    public static readonly string[] MessagePool =
    [
        "Финиш! Каждый из этих дней был маленькой победой над «потом» — и вместе они сложились в большую.",
        "Весь срок без единого пропуска. Это и есть дисциплина: не настроение, а решение, которое ты принимаешь каждый день.",
        "Привычка закреплена! Теперь это не усилие, а часть тебя. Дальше будет только легче.",
        "Готово! Теперь ты точно знаешь: если решить — получится. Запомни это чувство и бери новую цель.",
        "Регулярность сильнее таланта — и теперь у тебя есть доказательство. Можно гордиться, это заслуженно.",
    ];

    public static string PoolMessageFor(int habitId) => MessagePool[habitId % MessagePool.Length];

    /// <summary>Длина в символах так, как её видит человек (смайл = 1 символ).</summary>
    public static int VisibleLength(string s) => new StringInfo(s).LengthInTextElements;

    public static string? Clean(string? s) => s?.Trim();

    [GeneratedRegex("^[A-Za-z0-9_]{3,20}$")]
    private static partial Regex UsernameRegex();

    public static string? ValidateUsername(string? username)
    {
        if (string.IsNullOrEmpty(username)) return "Укажите логин";
        if (!UsernameRegex().IsMatch(username)) return "Логин: от 3 до 20 символов, только латиница, цифры и _";
        return null;
    }

    public static string? ValidatePassword(string? password)
    {
        if (string.IsNullOrEmpty(password)) return "Укажите пароль";
        if (password.Length < 6 || password.Length > 64) return "Пароль: от 6 до 64 символов";
        return null;
    }

    public static string? ValidateTitle(string? title)
    {
        if (string.IsNullOrEmpty(title)) return "Укажите название";
        if (VisibleLength(title) > 50) return "Название: не длиннее 50 символов";
        return null;
    }

    /// <summary>Пустой смайл заменяется на смайл по умолчанию.</summary>
    public static string? ValidateEmoji(string? emoji, out string normalized)
    {
        normalized = string.IsNullOrEmpty(emoji) ? DefaultEmoji : emoji;
        return AllowedEmojis.Contains(normalized) ? null : "Недопустимый смайл: выберите из списка /api/meta";
    }

    public static string? ValidateTargetDays(int? days)
    {
        if (days is null) return "Укажите срок в днях (targetDays)";
        if (days < 1 || days > 365) return "Срок: от 1 до 365 дней";
        return null;
    }

    public static string? ValidateHours(double? hours)
    {
        if (hours is null) return null;
        var doubled = hours.Value * 2;
        if (hours < 0.5 || hours > 24 || Math.Abs(doubled - Math.Round(doubled)) > 1e-9)
            return "Часы в день: от 0.5 до 24 с шагом 0.5";
        return null;
    }

    public static string? ValidateDescription(string? description)
    {
        if (description is not null && VisibleLength(description) > 200) return "Описание: не длиннее 200 символов";
        return null;
    }

    public static string? ValidateCompletionMessage(string? message)
    {
        if (string.IsNullOrEmpty(message)) return "Укажите сообщение после выполнения";
        var length = VisibleLength(message);
        if (length < 10 || length > 500) return "Сообщение после выполнения: от 10 до 500 символов";
        return null;
    }
}
