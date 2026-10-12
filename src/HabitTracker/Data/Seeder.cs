using HabitTracker.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Data;

/// <summary>Тестовые данные (раздел 9 требований). Заполняет только пустую БД.</summary>
public static class Seeder
{
    public static async Task RunAsync(AppDbContext db, AppClock clock, IPasswordHasher<User> hasher)
    {
        if (await db.Users.AnyAsync()) return;

        var now = DateTime.UtcNow;
        var today = clock.Today;

        var admin = new User { Username = "admin", Role = Roles.Admin, CreatedAt = now };
        admin.PasswordHash = hasher.HashPassword(admin, "admin123");
        var demo = new User { Username = "demo", Role = Roles.User, CreatedAt = now };
        demo.PasswordHash = hasher.HashPassword(demo, "demo123");
        db.Users.AddRange(admin, demo);

        var templates = new[]
        {
            Template("💧", "Пить воду", "6–8 стаканов чистой воды в течение дня",
                "Весь срок с водным балансом в порядке! Коже, суставам и голове это на пользу: меньше усталости, больше ясности. Тело точно говорит спасибо."),
            Template("💪", "Утренняя зарядка", "10–15 минут движения сразу после пробуждения",
                "Зарядка каждое утро — и так весь срок! Это лучший способ разбудить тело и голову: энергия с самого утра и меньше скованности в спине."),
            Template("📚", "Чтение", "Хотя бы 20 страниц в день",
                "Каждый день с книгой — это уже сотни страниц новых мыслей. Чтение тренирует концентрацию, словарный запас и воображение. Отличная работа!"),
            Template("🧘", "Медитация", "10 минут тишины и внимания к дыханию",
                "Целый срок практики осознанности! Регулярная медитация снижает стресс и помогает держать фокус. Теперь этот инструмент всегда с тобой."),
            Template("😴", "Сон до 23:00", "Ложиться спать не позже 23:00",
                "Режим сна выстроен! Ранний отбой — это восстановление, крепкий иммунитет и ясная голова по утрам. Лучшая инвестиция в самочувствие."),
            Template("📵", "Утро без соцсетей", "Первый час после пробуждения — без ленты",
                "Утро снова принадлежит тебе! Без ленты с первых минут меньше тревоги и больше внимания к своим делам. Ценный навык в мире бесконечных уведомлений."),
        };
        db.HabitTemplates.AddRange(templates);
        await db.SaveChangesAsync();

        var reading = templates[2];
        var workout = templates[1];

        // активная: начата 3 дня назад, отмечены 3 дня, сегодня ещё нет
        db.Habits.Add(DemoHabit(demo, reading, reading.Title, reading.Emoji, 21, 0.5,
            today.AddDays(-3), checkins: 3, completed: false));

        // прервана: начата 6 дней назад, отмечены 2 дня, третий пропущен
        db.Habits.Add(DemoHabit(demo, workout, workout.Title, workout.Emoji, 30, null,
            today.AddDays(-6), checkins: 2, completed: false));

        // выполнена: своя привычка на 7 дней, закончилась 4 дня назад
        var english = DemoHabit(demo, null, "Английский", "🧠", 7, 1,
            today.AddDays(-10), checkins: 7, completed: true);
        db.Habits.Add(english);
        await db.SaveChangesAsync();

        english.CompletionMessage = Rules.PoolMessageFor(english.Id);
        await db.SaveChangesAsync();
    }

    private static HabitTemplate Template(string emoji, string title, string description, string message) => new()
    {
        Emoji = emoji, Title = title, Description = description, CompletionMessage = message,
        CreatedAt = DateTime.UtcNow,
    };

    private static Habit DemoHabit(User user, HabitTemplate? template, string title, string emoji,
        int targetDays, double? hours, DateOnly start, int checkins, bool completed)
    {
        var habit = new Habit
        {
            User = user,
            TemplateId = template?.Id,
            Title = title,
            Emoji = emoji,
            TargetDays = targetDays,
            HoursPerDay = hours,
            StartDate = start,
            CompletionMessage = template?.CompletionMessage,
            CompletedOn = completed ? start.AddDays(checkins - 1) : null,
            CreatedAt = DateTime.UtcNow,
        };
        for (var i = 0; i < checkins; i++)
            habit.Checkins.Add(new Checkin { Date = start.AddDays(i), CreatedAt = DateTime.UtcNow });
        return habit;
    }
}
