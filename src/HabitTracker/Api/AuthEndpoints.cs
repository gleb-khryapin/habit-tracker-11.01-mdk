using System.Security.Claims;
using HabitTracker.Data;
using HabitTracker.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Api;

public static class AuthEndpoints
{
    public const string CookieName = "habits_auth";
    public const string AdminPolicy = "admin";

    /// <summary>
    /// Метка «версии» пользователя в cookie. После сброса БД пользователь с тем же id
    /// будет другим (другая дата создания) — и старая cookie перестанет действовать.
    /// </summary>
    private const string CreatedClaim = "created";

    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var g = app.MapGroup("/api/auth").WithTags("Auth");

        g.MapPost("/register", Register)
            .WithSummary("Регистрация. После неё вход выполняется автоматически");
        g.MapPost("/login", Login).WithSummary("Вход: выдаёт cookie habits_auth");
        // приведение к Delegate: иначе метод с одним HttpContext считается RequestDelegate и 204 теряется
        g.MapPost("/logout", (Delegate)Logout).WithSummary("Выход");
        g.MapGet("/me", Me).RequireAuthorization().WithSummary("Текущий пользователь");
    }

    public static int UserId(this ClaimsPrincipal principal) =>
        int.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public static bool IsAdmin(this ClaimsPrincipal principal) => principal.IsInRole(Roles.Admin);

    private static async Task<IResult> Register(
        AuthRequest req, AppDbContext db, IPasswordHasher<User> hasher, HttpContext ctx)
    {
        var username = Rules.Clean(req.Username);
        var error = Errors.First(Rules.ValidateUsername(username), Rules.ValidatePassword(req.Password));
        if (error is not null) return Errors.BadRequest(error);

        if (await db.Users.AnyAsync(u => u.Username == username))
            return Errors.Conflict("Этот логин уже занят");

        var user = new User { Username = username!, Role = Roles.User, CreatedAt = DateTime.UtcNow };
        user.PasswordHash = hasher.HashPassword(user, req.Password!);
        db.Users.Add(user);
        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            // одновременная регистрация с тем же логином — сработал уникальный индекс
            return Errors.Conflict("Этот логин уже занят");
        }

        await SignInAsync(ctx, user);
        return Results.Created("/api/auth/me", UserDto.From(user));
    }

    private static async Task<IResult> Login(
        AuthRequest req, AppDbContext db, IPasswordHasher<User> hasher, HttpContext ctx)
    {
        var username = Rules.Clean(req.Username);
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(req.Password))
            return Errors.BadRequest("Укажите логин и пароль");

        var user = await db.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user is null ||
            hasher.VerifyHashedPassword(user, user.PasswordHash, req.Password) == PasswordVerificationResult.Failed)
            return Errors.Unauthorized("Неверный логин или пароль");

        await SignInAsync(ctx, user);
        return Results.Ok(UserDto.From(user));
    }

    private static async Task<IResult> Logout(HttpContext ctx)
    {
        await ctx.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Results.NoContent();
    }

    private static async Task<IResult> Me(AppDbContext db, ClaimsPrincipal principal)
    {
        var user = await db.Users.FindAsync(principal.UserId());
        return user is null ? Errors.Unauthorized("Требуется вход") : Results.Ok(UserDto.From(user));
    }

    private static Task SignInAsync(HttpContext ctx, User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(CreatedClaim, user.CreatedAt.Ticks.ToString()),
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        return ctx.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity),
            new AuthenticationProperties { IsPersistent = true });
    }

    /// <summary>
    /// Настройка cookie: API отвечает 401/403 в формате JSON вместо редиректа на страницу входа,
    /// а при каждом запросе проверяет, что пользователь из cookie всё ещё существует.
    /// </summary>
    public static void ConfigureCookie(CookieAuthenticationOptions o)
    {
        o.Cookie.Name = CookieName;
        o.Cookie.HttpOnly = true;
        o.Cookie.SameSite = SameSiteMode.Lax;
        o.ExpireTimeSpan = TimeSpan.FromDays(7);
        o.SlidingExpiration = true;

        o.Events.OnRedirectToLogin = ctx => Errors.Write(ctx.Response, 401, "Требуется вход");
        o.Events.OnRedirectToAccessDenied = ctx => Errors.Write(ctx.Response, 403, "Доступ только для администратора");

        o.Events.OnValidatePrincipal = async ctx =>
        {
            var principal = ctx.Principal ?? new ClaimsPrincipal();
            var db = ctx.HttpContext.RequestServices.GetRequiredService<AppDbContext>();
            User? user = null;
            if (int.TryParse(principal.FindFirstValue(ClaimTypes.NameIdentifier), out var id))
                user = await db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

            var valid = user is not null
                        && user.CreatedAt.Ticks.ToString() == principal.FindFirstValue(CreatedClaim)
                        && user.Role == principal.FindFirstValue(ClaimTypes.Role);
            if (!valid)
            {
                ctx.RejectPrincipal();
                await ctx.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
        };
    }
}
