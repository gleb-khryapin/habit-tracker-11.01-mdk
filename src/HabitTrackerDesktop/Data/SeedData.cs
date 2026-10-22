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

        Category health = AddCategory("Здоровье");
        Category sport = AddCategory("Спорт");
        Category study = AddCategory("Учёба");
        Category mindfulness = AddCategory("Осознанность");
        Category productivity = AddCategory("Продуктивность");

        AddPoolMessage("Финиш! Каждый из этих дней был маленькой победой над «потом» — и вместе они сложились в большую.");
        AddPoolMessage("Весь срок без единого пропуска. Это и есть дисциплина: не настроение, а решение, которое ты принимаешь каждый день.");
        AddPoolMessage("Привычка закреплена! Теперь это не усилие, а часть тебя. Дальше будет только легче.");
        AddPoolMessage("Готово! Теперь ты точно знаешь: если решить — получится. Запомни это чувство и бери новую цель.");
        AddPoolMessage("Регулярность сильнее таланта — и теперь у тебя есть доказательство. Можно гордиться, это заслуженно.");

        AddAchievement("first_habit", "🌱", "Первый шаг", "Создана первая привычка");
        AddAchievement("first_checkin", "🎯", "Начало положено", "Сделана первая отметка выполнения");
        AddAchievement("streak_7", "💪", "Неделя подряд", "7 отметок подряд в одной привычке");
        AddAchievement("streak_30", "🏃", "Марафонец", "30 отметок подряд в одной привычке");
        AddAchievement("first_completed", "📝", "Финишёр", "Выполнена первая привычка");
        AddAchievement("completed_3", "🧠", "Три победы", "Выполнено три привычки");
        AddAchievement("flame_7", "🔥", "Огонь недели", "Огонёк горит 7 дней подряд");

        AddTemplate(health, "💧", "Пить воду", "6–8 стаканов чистой воды в течение дня",
            "Весь срок с водным балансом в порядке! Коже, суставам и голове это на пользу: меньше усталости, больше ясности. Тело точно говорит спасибо.");
        HabitTemplate workout = AddTemplate(sport, "💪", "Утренняя зарядка", "10–15 минут движения сразу после пробуждения",
            "Зарядка каждое утро — и так весь срок! Это лучший способ разбудить тело и голову: энергия с самого утра и меньше скованности в спине.");
        HabitTemplate reading = AddTemplate(study, "📚", "Чтение", "Хотя бы 20 страниц в день",
            "Каждый день с книгой — это уже сотни страниц новых мыслей. Чтение тренирует концентрацию, словарный запас и воображение. Отличная работа!");
        AddTemplate(mindfulness, "🧘", "Медитация", "10 минут тишины и внимания к дыханию",
            "Целый срок практики осознанности! Регулярная медитация снижает стресс и помогает держать фокус. Теперь этот инструмент всегда с тобой.");
        AddTemplate(health, "😴", "Сон до 23:00", "Ложиться спать не позже 23:00",
            "Режим сна выстроен! Ранний отбой — это восстановление, крепкий иммунитет и ясная голова по утрам. Лучшая инвестиция в самочувствие.");
        AddTemplate(productivity, "📵", "Утро без соцсетей", "Первый час после пробуждения — без ленты",
            "Утро снова принадлежит тебе! Без ленты с первых минут меньше тревоги и больше внимания к своим делам. Ценный навык в мире бесконечных уведомлений.");

        DateTime today = Clock.Today;
        Habit readingHabit = AddHabit(demo.Id, reading, reading.Title, reading.Emoji, reading.CategoryId, 21, 0.5, today.AddDays(-3), 3);
        AddHabit(demo.Id, null, "Английский", "🧠", study.Id, 7, 1, today.AddDays(-10), 7);
        AddHabit(demo.Id, workout, workout.Title, workout.Emoji, workout.CategoryId, 30, null, today.AddDays(-6), 2);

        Reminder reminder = new Reminder();
        reminder.HabitId = readingHabit.Id;
        reminder.Time = "20:00";
        reminder.IsEnabled = true;
        ReminderRepository.Add(reminder);

        AchievementChecker.GiveNew(demo.Id);

        AddRequest(demo.Id, health, "🚶", "Прогулка 30 минут", "Полчаса пешком на свежем воздухе",
            "Каждый день на свежем воздухе — это бодрость, крепкий сон и ясная голова. Отличная привычка!", today.AddDays(-1));
        TemplateRequest rejected = AddRequest(demo.Id, null, "💻", "Без телефона утром", "Первый час после пробуждения без телефона",
            "Утро без телефона — это спокойствие и внимание к себе. Главный соблазн дня побеждён!", today.AddDays(-2));
        RequestRepository.Reject(rejected.Id, "Такая привычка уже есть в списке: «Утро без соцсетей»", today.AddDays(-1));

        AuditRepository.Clear();
    }

    private static TemplateRequest AddRequest(int userId, Category category, string emoji, string title,
        string description, string message, DateTime date)
    {
        TemplateRequest request = new TemplateRequest();
        request.UserId = userId;
        if (category != null)
        {
            request.CategoryId = category.Id;
        }
        request.Emoji = emoji;
        request.Title = title;
        request.Description = description;
        request.CompletionMessage = message;
        request.CreatedDate = date;
        RequestRepository.Add(request);
        return request;
    }

    private static Category AddCategory(string name)
    {
        Category category = new Category();
        category.Name = name;
        CategoryRepository.Add(category);
        return category;
    }

    private static void AddPoolMessage(string text)
    {
        CompletionMessage message = new CompletionMessage();
        message.Text = text;
        MessageRepository.Add(message);
    }

    private static void AddAchievement(string code, string emoji, string title, string description)
    {
        Achievement achievement = new Achievement();
        achievement.Code = code;
        achievement.Emoji = emoji;
        achievement.Title = title;
        achievement.Description = description;
        AchievementRepository.Add(achievement);
    }

    private static HabitTemplate AddTemplate(Category category, string emoji, string title, string description, string message)
    {
        HabitTemplate template = new HabitTemplate();
        template.CategoryId = category.Id;
        template.CategoryName = category.Name;
        template.Emoji = emoji;
        template.Title = title;
        template.Description = description;
        template.CompletionMessage = message;
        TemplateRepository.Add(template);
        return template;
    }

    private static Habit AddHabit(int userId, HabitTemplate template, string title, string emoji, int? categoryId,
        int targetDays, double? hours, DateTime startDate, int checkedDays)
    {
        Habit habit = new Habit();
        habit.UserId = userId;
        habit.Title = title;
        habit.Emoji = emoji;
        habit.CategoryId = categoryId;
        habit.TargetDays = targetDays;
        habit.HoursPerDay = hours;
        habit.StartDate = startDate;
        if (template != null)
        {
            habit.TemplateId = template.Id;
            habit.CompletionMessage = template.CompletionMessage;
        }
        HabitRepository.Add(habit);

        for (int day = 0; day < checkedDays; day++)
        {
            HabitRepository.AddCheckin(habit.Id, startDate.AddDays(day));
        }

        return habit;
    }
}
