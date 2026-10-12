using System.Security.Claims;
using HabitTracker.Data;
using HabitTracker.Services;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Api;

public static class TemplateEndpoints
{
    public static void MapTemplateEndpoints(this IEndpointRouteBuilder app)
    {
        var g = app.MapGroup("/api/templates").WithTags("Templates").RequireAuthorization();

        g.MapGet("/", List)
            .WithSummary("Список предложенных привычек (completionMessage видит только админ)");
        g.MapPost("/", Create).RequireAuthorization(AuthEndpoints.AdminPolicy)
            .WithSummary("Добавить предложенную привычку (админ)");
        g.MapDelete("/{id:int}", Delete).RequireAuthorization(AuthEndpoints.AdminPolicy)
            .WithSummary("Удалить предложенную привычку (админ)");
    }

    private static async Task<IResult> List(AppDbContext db, ClaimsPrincipal principal)
    {
        var isAdmin = principal.IsAdmin();
        var templates = await db.HabitTemplates.OrderBy(t => t.Id).ToListAsync();
        return Results.Ok(templates.Select(t => TemplateDto.From(t, isAdmin)));
    }

    private static async Task<IResult> Create(CreateTemplateRequest req, AppDbContext db)
    {
        var title = Rules.Clean(req.Title);
        var description = Rules.Clean(req.Description) ?? "";
        var message = Rules.Clean(req.CompletionMessage);

        var error = Errors.First(
            Rules.ValidateTitle(title),
            Rules.ValidateEmoji(Rules.Clean(req.Emoji), out var emoji),
            Rules.ValidateDescription(description),
            Rules.ValidateCompletionMessage(message));
        if (error is not null) return Errors.BadRequest(error);

        var titles = await db.HabitTemplates.Select(t => t.Title).ToListAsync();
        if (titles.Any(t => string.Equals(t, title, StringComparison.OrdinalIgnoreCase)))
            return Errors.Conflict("Предложенная привычка с таким названием уже есть");

        var template = new HabitTemplate
        {
            Title = title!,
            Emoji = emoji,
            Description = description,
            CompletionMessage = message!,
            CreatedAt = DateTime.UtcNow,
        };
        db.HabitTemplates.Add(template);
        await db.SaveChangesAsync();
        return Results.Created($"/api/templates/{template.Id}", TemplateDto.From(template, isAdmin: true));
    }

    private static async Task<IResult> Delete(int id, AppDbContext db)
    {
        var template = await db.HabitTemplates.FindAsync(id);
        if (template is null) return Errors.NotFound("Предложенная привычка не найдена");

        // привычки пользователей остаются: в БД у них template_id станет NULL (ON DELETE SET NULL)
        db.HabitTemplates.Remove(template);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
}
