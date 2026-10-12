using HabitTrackerDesktop.Models;
using HabitTrackerDesktop.Services;

namespace HabitTrackerDesktop.Data;

public static class SeedData
{
    public static void Fill()
    {
        User admin = new User();
        admin.Username = "admin";
        admin.PasswordHash = PasswordHasher.Hash("admin123");
        admin.Role = "admin";
        UserRepository.Add(admin);

        User demo = new User();
        demo.Username = "demo";
        demo.PasswordHash = PasswordHasher.Hash("demo123");
        demo.Role = "user";
        UserRepository.Add(demo);

        AddTemplate("💧", "Пить воду", "6–8 стаканов чистой воды в течение дня",
            "Весь срок с водным балансом в порядке! Коже, суставам и голове это на пользу: меньше усталости, больше ясности. Тело точно говорит спасибо.");
        HabitTemplate workout = AddTemplate("💪", "Утренняя зарядка", "10–15 минут движения сразу после пробуждения",
            "Зарядка каждое утро — и так весь срок! Это лучший способ разбудить тело и голову: энергия с самого утра и меньше скованности в спине.");
        HabitTemplate reading = AddTemplate("📚", "Чтение", "Хотя бы 20 страниц в день",
            "Каждый день с книгой — это уже сотни страниц новых мыслей. Чтение тренирует концентрацию, словарный запас и воображение. Отличная работа!");
        AddTemplate("🧘", "Медитация", "10 минут тишины и внимания к дыханию",
            "Целый срок практики осознанности! Регулярная медитация снижает стресс и помогает держать фокус. Теперь этот инструмент всегда с тобой.");
        AddTemplate("😴", "Сон до 23:00", "Ложиться спать не позже 23:00",
            "Режим сна выстроен! Ранний отбой — это восстановление, крепкий иммунитет и ясная голова по утрам. Лучшая инвестиция в самочувствие.");
        AddTemplate("📵", "Утро без соцсетей", "Первый час после пробуждения — без ленты",
            "Утро снова принадлежит тебе! Без ленты с первых минут меньше тревоги и больше внимания к своим делам. Ценный навык в мире бесконечных уведомлений.");

        DateTime today = Clock.Today;
        AddHabit(demo.Id, reading.Title, reading.Emoji, 21, 0.5, today.AddDays(-3), 3, reading.CompletionMessage);
        AddHabit(demo.Id, "Английский", "🧠", 7, 1, today.AddDays(-10), 7, null);
        AddHabit(demo.Id, workout.Title, workout.Emoji, 30, null, today.AddDays(-6), 2, workout.CompletionMessage);
    }

    private static HabitTemplate AddTemplate(string emoji, string title, string description, string message)
    {
        HabitTemplate template = new HabitTemplate();
        template.Emoji = emoji;
        template.Title = title;
        template.Description = description;
        template.CompletionMessage = message;
        TemplateRepository.Add(template);
        return template;
    }

    private static void AddHabit(int userId, string title, string emoji, int targetDays, double? hours,
        DateTime startDate, int checkedDays, string message)
    {
        Habit habit = new Habit();
        habit.UserId = userId;
        habit.Title = title;
        habit.Emoji = emoji;
        habit.TargetDays = targetDays;
        habit.HoursPerDay = hours;
        habit.StartDate = startDate;
        habit.CompletionMessage = message;
        HabitRepository.Add(habit);

        for (int day = 0; day < checkedDays; day++)
        {
            HabitRepository.AddCheckin(habit.Id, startDate.AddDays(day));
        }
    }
}
