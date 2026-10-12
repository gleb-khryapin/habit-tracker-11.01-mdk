using HabitTracker.Data;
using HabitTracker.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

namespace HabitTracker.Api;

public static class SystemEndpoints
{
    public static void MapMetaEndpoints(this IEndpointRouteBuilder app, bool testMode)
    {
        app.MapGet("/api/meta", (AppClock clock) =>
                Results.Ok(new MetaDto(clock.Today, clock.IsOverridden, testMode, Rules.AllowedEmojis)))
            .WithTags("Meta")
            .WithSummary("Сегодняшняя дата сервера, разрешённые смайлы, тестовый режим");
    }

    /// <summary>Только для окружения Development: управление датой и сброс БД.</summary>
    public static void MapTestEndpoints(this IEndpointRouteBuilder app)
    {
        var g = app.MapGroup("/api/test").WithTags("Test mode");

        g.MapPut("/clock", (ClockRequest req, AppClock clock) =>
        {
            if (!DateOnly.TryParseExact(req.Today, "yyyy-MM-dd", out var date))
                return Errors.BadRequest("Укажите дату в формате ГГГГ-ММ-ДД, например {\"today\": \"2026-10-30\"}");
            clock.Set(date);
            return Results.Ok(new { today = clock.Today, clockOverridden = clock.IsOverridden });
        }).WithSummary("Подменить «сегодня»");

        g.MapDelete("/clock", (AppClock clock) =>
        {
            clock.Reset();
            return Results.Ok(new { today = clock.Today, clockOverridden = clock.IsOverridden });
        }).WithSummary("Вернуть реальную дату");

        g.MapPost("/reset", async (AppDbContext db, AppClock clock, IPasswordHasher<User> hasher, HttpContext ctx) =>
        {
            clock.Reset();
            await db.Database.EnsureDeletedAsync();
            await db.Database.EnsureCreatedAsync();
            await Seeder.RunAsync(db, clock, hasher);
            await ctx.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Results.NoContent();
        }).WithSummary("Пересоздать БД с seed-данными и сбросить дату");
    }
}
