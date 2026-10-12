namespace HabitTracker.Data;

public static class Roles
{
    public const string User = "user";
    public const string Admin = "admin";
}

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public string Role { get; set; } = Roles.User;
    public DateTime CreatedAt { get; set; }

    public List<Habit> Habits { get; set; } = new();
}

/// <summary>Предложенная привычка, которую добавляет админ.</summary>
public class HabitTemplate
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Emoji { get; set; } = "";
    public string Description { get; set; } = "";
    public string CompletionMessage { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}

public class Habit
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public int? TemplateId { get; set; }
    public HabitTemplate? Template { get; set; }

    public string Title { get; set; } = "";
    public string Emoji { get; set; } = "";
    public int TargetDays { get; set; }
    public double? HoursPerDay { get; set; }

    /// <summary>День 1 привычки — дата создания.</summary>
    public DateOnly StartDate { get; set; }
    public DateOnly? CompletedOn { get; set; }

    /// <summary>
    /// Для привычки из шаблона копируется при создании.
    /// Для своей — заполняется фразой из общего пула в момент выполнения.
    /// Пользователю отдаётся только у выполненной привычки.
    /// </summary>
    public string? CompletionMessage { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<Checkin> Checkins { get; set; } = new();
}

/// <summary>Отметка выполнения привычки за конкретный день.</summary>
public class Checkin
{
    public int Id { get; set; }
    public int HabitId { get; set; }
    public Habit? Habit { get; set; }
    public DateOnly Date { get; set; }
    public DateTime CreatedAt { get; set; }
}
