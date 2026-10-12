using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using HabitTracker.Api;
using HabitTracker.Data;
using HabitTracker.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var testMode = builder.Configuration.GetValue<bool>("TestMode");
var dbPath = Path.Combine(builder.Environment.ContentRootPath, "habits.db");

builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlite($"Data Source={dbPath}"));
builder.Services.AddSingleton<AppClock>();
builder.Services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(AuthEndpoints.ConfigureCookie);
builder.Services.AddAuthorizationBuilder()
    .AddPolicy(AuthEndpoints.AdminPolicy, p => p.RequireRole(Roles.Admin));

builder.Services.ConfigureHttpJsonOptions(o =>
{
    // кириллица в JSON без \uXXXX — ответы читаемы в Swagger и консоли
    o.SerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
    // строгие типы: "targetDays": "21" — это ошибка 400, а не число 21
    o.SerializerOptions.NumberHandling = JsonNumberHandling.Strict;
});
// ошибки привязки тела (битый JSON, неверный тип поля) превращаем в единый формат ошибок ниже
builder.Services.Configure<RouteHandlerOptions>(o => o.ThrowOnBadRequest = true);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new() { Title = "Habit Tracker API", Version = "v1" }));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.EnsureCreatedAsync();
    await Seeder.RunAsync(db, app.Services.GetRequiredService<AppClock>(),
        app.Services.GetRequiredService<IPasswordHasher<User>>());
}

app.Use(async (ctx, next) =>
{
    try
    {
        await next();
    }
    catch (BadHttpRequestException ex) when (!ctx.Response.HasStarted)
    {
        var message = ex.StatusCode == StatusCodes.Status415UnsupportedMediaType
            ? "Тело запроса должно быть в формате JSON (Content-Type: application/json)"
            : "Некорректное тело запроса: проверьте JSON и типы полей";
        await Errors.Write(ctx.Response, ex.StatusCode, message);
    }
    catch (Exception ex) when (!ctx.Response.HasStarted)
    {
        app.Logger.LogError(ex, "Необработанная ошибка");
        await Errors.Write(ctx.Response, 500, "Внутренняя ошибка сервера");
    }
});

// ответы API без тела (нет маршрута, не тот HTTP-метод, не тот Content-Type) — тоже в едином формате
app.UseStatusCodePages(async status =>
{
    var ctx = status.HttpContext;
    if (!ctx.Request.Path.StartsWithSegments("/api")) return;
    var message = ctx.Response.StatusCode switch
    {
        404 => "Метод API не найден",
        405 => "HTTP-метод не поддерживается для этого адреса",
        415 => "Тело запроса должно быть в формате JSON (Content-Type: application/json)",
        _ => $"Ошибка {ctx.Response.StatusCode}",
    };
    await ctx.Response.WriteAsJsonAsync(new ApiError(message));
});

app.UseSwagger();
app.UseSwaggerUI();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapAuthEndpoints();
app.MapTemplateEndpoints();
app.MapHabitEndpoints();
app.MapMetaEndpoints(testMode);
if (testMode) app.MapTestEndpoints();

app.Run();
