using System.Text.Json.Serialization;
using HabitTracker.Data;
using HabitTracker.Services;

namespace HabitTracker.Api;

// ---------- запросы ----------
// Поля nullable, чтобы отличать «не передано» от значения и отвечать понятной ошибкой.

public record AuthRequest(string? Username, string? Password);

public record CreateHabitRequest(int? TemplateId, string? Title, string? Emoji, int? TargetDays, double? HoursPerDay);

public record UpdateHabitRequest(string? Title, string? Emoji, int? TargetDays, double? HoursPerDay);

public record CreateTemplateRequest(string? Title, string? Emoji, string? Description, string? CompletionMessage);

public record ClockRequest(string? Today);

// ---------- ответы ----------

public record ApiError(string Error);

public record UserDto(int Id, string Username, string Role)
{
    public static UserDto From(User u) => new(u.Id, u.Username, u.Role);
}

public record TemplateDto(
    int Id,
    string Title,
    string Emoji,
    string Description,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? CompletionMessage)
{
    /// <summary>Сообщение видит только админ — для пользователя это сюрприз.</summary>
    public static TemplateDto From(HabitTemplate t, bool isAdmin) =>
        new(t.Id, t.Title, t.Emoji, t.Description, isAdmin ? t.CompletionMessage : null);
}

public record HabitDto(
    int Id,
    string Title,
    string Emoji,
    int TargetDays,
    double? HoursPerDay,
    int? TemplateId,
    DateOnly StartDate,
    int Progress,
    string Status,
    bool CheckedToday,
    DateOnly? FailedOn,
    DateOnly? CompletedOn,
    string? CompletionMessage,
    DateTime CreatedAt)
{
    public static HabitDto From(Habit h, DateOnly today)
    {
        var state = HabitState.Of(h, today);
        var completed = state.Status == HabitStatus.Completed;
        return new(
            h.Id, h.Title, h.Emoji, h.TargetDays, h.HoursPerDay, h.TemplateId, h.StartDate,
            state.Progress, state.Status, state.CheckedToday, state.FailedOn, h.CompletedOn,
            completed ? h.CompletionMessage : null,
            DateTime.SpecifyKind(h.CreatedAt, DateTimeKind.Utc));
    }
}

public record MetaDto(DateOnly Today, bool ClockOverridden, bool TestMode, string[] Emojis);
