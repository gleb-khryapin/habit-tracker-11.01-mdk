using System.Security.Claims;
using HabitTracker.Data;
using HabitTracker.Services;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Api;

/// <summary>
/// Привычки текущего пользователя.
/// Порядок проверок: 401 → 404 → 409 (выполнена/прервана) → 400 (валидация) → 409 (дубль названия).
/// Чужая привычка отдаётся как 404, чтобы не раскрывать, что она существует.
/// </summary>
public static class HabitEndpoints
{
    private const string NotFoundMessage = "Привычка не найдена";

    public static void MapHabitEndpoints(this IEndpointRouteBuilder app)
    {
        var g = app.MapGroup("/api/habits").WithTags("Habits").RequireAuthorization();

        g.MapGet("/", List).WithSummary("Мои привычки");
        g.MapGet("/{id:int}", Get).WithSummary("Привычка по id");
        g.MapPost("/", Create)
            .WithSummary("Создать привычку: из шаблона (templateId) или свою (title)");
        g.MapPut("/{id:int}", Update)
            .WithSummary("Изменить привычку: передаются все поля title, emoji, targetDays, hoursPerDay");
        g.MapDelete("/{id:int}", Delete).WithSummary("Удалить привычку вместе с отметками");
        g.MapPost("/{id:int}/checkins", CheckIn).WithSummary("Отметить выполнение за сегодня");
    }

    private static Task<Habit?> FindOwn(AppDbContext db, int id, ClaimsPrincipal principal)
    {
        var userId = principal.UserId();
        return db.Habits.Include(h => h.Checkins).FirstOrDefaultAsync(h => h.Id == id && h.UserId == userId);
    }

    private static async Task<IResult> List(AppDbContext db, ClaimsPrincipal principal, AppClock clock)
    {
        var userId = principal.UserId();
        var habits = await db.Habits.Include(h => h.Checkins)
            .Where(h => h.UserId == userId).OrderBy(h => h.Id).ToListAsync();
        var today = clock.Today;
        return Results.Ok(habits.Select(h => HabitDto.From(h, today)));
    }

    private static async Task<IResult> Get(int id, AppDbContext db, ClaimsPrincipal principal, AppClock clock)
    {
        var habit = await FindOwn(db, id, principal);
        return habit is null ? Errors.NotFound(NotFoundMessage) : Results.Ok(HabitDto.From(habit, clock.Today));
    }

    private static async Task<IResult> Create(
        CreateHabitRequest req, AppDbContext db, ClaimsPrincipal principal, AppClock clock)
    {
        var userId = principal.UserId();
        var today = clock.Today;

        string? title;
        string emoji;
        string? templateMessage = null;
        string? error;

        if (req.TemplateId is not null)
        {
            // из шаблона: title и emoji запроса игнорируются
            var template = await db.HabitTemplates.FindAsync(req.TemplateId.Value);
            if (template is null) return Errors.BadRequest("Предложенная привычка не найдена");
            title = template.Title;
            emoji = template.Emoji;
            templateMessage = template.CompletionMessage;
            error = null;
        }
        else
        {
            title = Rules.Clean(req.Title);
            error = Errors.First(Rules.ValidateTitle(title), Rules.ValidateEmoji(Rules.Clean(req.Emoji), out emoji));
        }

        error ??= Errors.First(Rules.ValidateTargetDays(req.TargetDays), Rules.ValidateHours(req.HoursPerDay));
        if (error is not null) return Errors.BadRequest(error);

        if (await HasActiveDuplicate(db, userId, title!, today, exceptId: null))
            return Errors.Conflict("У тебя уже есть активная привычка с таким названием");

        var habit = new Habit
        {
            UserId = userId,
            TemplateId = req.TemplateId,
            Title = title!,
            Emoji = emoji,
            TargetDays = req.TargetDays!.Value,
            HoursPerDay = req.HoursPerDay,
            StartDate = today,
            CompletionMessage = templateMessage,
            CreatedAt = DateTime.UtcNow,
        };
        db.Habits.Add(habit);
        await db.SaveChangesAsync();
        return Results.Created($"/api/habits/{habit.Id}", HabitDto.From(habit, today));
    }

    private static async Task<IResult> Update(
        int id, UpdateHabitRequest req, AppDbContext db, ClaimsPrincipal principal, AppClock clock)
    {
        var habit = await FindOwn(db, id, principal);
        if (habit is null) return Errors.NotFound(NotFoundMessage);

        var today = clock.Today;
        var state = HabitState.Of(habit, today);
        var stateError = FinalStateError(state);
        if (stateError is not null) return Errors.Conflict(stateError);

        var title = Rules.Clean(req.Title);
        var error = Errors.First(
            Rules.ValidateTitle(title),
            Rules.ValidateEmoji(Rules.Clean(req.Emoji), out var emoji),
            Rules.ValidateTargetDays(req.TargetDays),
            Rules.ValidateHours(req.HoursPerDay));
        if (error is null && req.TargetDays <= state.Progress)
            error = $"Срок должен быть больше уже выполненных дней ({state.Progress})";
        if (error is not null) return Errors.BadRequest(error);

        if (await HasActiveDuplicate(db, principal.UserId(), title!, today, exceptId: habit.Id))
            return Errors.Conflict("У тебя уже есть активная привычка с таким названием");

        habit.Title = title!;
        habit.Emoji = emoji;
        habit.TargetDays = req.TargetDays!.Value;
        habit.HoursPerDay = req.HoursPerDay;
        await db.SaveChangesAsync();
        return Results.Ok(HabitDto.From(habit, today));
    }

    private static async Task<IResult> Delete(int id, AppDbContext db, ClaimsPrincipal principal)
    {
        var userId = principal.UserId();
        var habit = await db.Habits.FirstOrDefaultAsync(h => h.Id == id && h.UserId == userId);
        if (habit is null) return Errors.NotFound(NotFoundMessage);

        db.Habits.Remove(habit); // отметки удалятся каскадно
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    private static async Task<IResult> CheckIn(int id, AppDbContext db, ClaimsPrincipal principal, AppClock clock)
    {
        var habit = await FindOwn(db, id, principal);
        if (habit is null) return Errors.NotFound(NotFoundMessage);

        var today = clock.Today;
        var state = HabitState.Of(habit, today);

        var stateError = FinalStateError(state);
        if (stateError is not null) return Errors.Conflict(stateError);
        if (today < state.NextDay) return Errors.Conflict("Сегодня уже отмечено");

        habit.Checkins.Add(new Checkin { Date = today, CreatedAt = DateTime.UtcNow });
        if (habit.Checkins.Count >= habit.TargetDays)
        {
            habit.CompletedOn = today;
            habit.CompletionMessage ??= Rules.PoolMessageFor(habit.Id);
        }

        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            // двойной клик: параллельный запрос уже записал отметку (уникальный индекс habit_id + date)
            return Errors.Conflict("Сегодня уже отмечено");
        }

        return Results.Created($"/api/habits/{habit.Id}", HabitDto.From(habit, today));
    }

    /// <summary>Выполненную или прерванную привычку можно только удалить.</summary>
    private static string? FinalStateError(HabitState state) => state.Status switch
    {
        HabitStatus.Completed => "Привычка уже выполнена — её можно только удалить",
        HabitStatus.Failed => $"Привычка прервана: пропущен день {state.FailedOn:dd.MM.yyyy}. Её можно только удалить",
        _ => null,
    };

    private static async Task<bool> HasActiveDuplicate(
        AppDbContext db, int userId, string title, DateOnly today, int? exceptId)
    {
        var habits = await db.Habits.Include(h => h.Checkins)
            .Where(h => h.UserId == userId && h.Id != exceptId).ToListAsync();
        return habits.Any(h =>
            HabitState.Of(h, today).Status == HabitStatus.Active &&
            string.Equals(h.Title, title, StringComparison.OrdinalIgnoreCase));
    }
}
