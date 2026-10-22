# ✦ Привычки — трекер привычек

Десктопное приложение для Windows: заводишь привычку на N дней, каждый день отмечаешь выполнение, а после N дней подряд
получаешь мотивирующее сообщение о пользе сделанного. Пропустил день — привычка прерывается.
Есть категории привычек, напоминания по времени и достижения.

Бизнес-процессы:

| Процесс | Суть |
|---|---|
| 🔥 Огонёк | Серия дней подряд, в которые отмечена хотя бы одна привычка, и рекорд за всё время |
| 📋 Заявки | Пользователь предлагает привычку в общий список, администратор одобряет её или отклоняет с причиной |
| 🛡 Журнал и резервная копия | Триггеры БД записывают изменения данных в журнал; администратор делает копию базы и восстанавливает из неё |


## Стек

| Часть | Технология |
|---|---|
| Язык | C# (.NET 8, запускается и на более новых версиях) |
| Окна | Windows Forms |
| БД | SQLite, один файл `habits.db` |
| Доступ к БД | `Microsoft.Data.Sqlite`, обычный SQL |

## Запуск с нуля

1. Установить [.NET SDK](https://dotnet.microsoft.com/download) версии 8 или новее и проверить: `dotnet --version`.
2. Запустить:

   ```bash
   cd src/HabitTrackerDesktop
   dotnet run
   ```

При первом запуске рядом с exe (`bin/Debug/net8.0-windows/`) создаётся файл БД `habits.db` с начальными данными.

Чтобы открыть формы в дизайнере Visual Studio 2022, нужен компонент «Разработка классических приложений .NET».

### Тестовые аккаунты

| Логин | Пароль | Роль | Данные |
|---|---|---|---|
| `admin` | `admin123` | admin | Доступны кнопки «Заявки», «Журнал», «Предложенные привычки», «Категории», «Фразы» |
| `demo` | `demo123` | user | 3 привычки: в процессе, выполненная, прерванная; напоминание на 20:00; огонёк 10 дней; 5 достижений; 2 заявки (на рассмотрении и отклонённая) |

### Для тестирования

| Что нужно | Как |
|---|---|
| Вернуть начальные данные | Закрыть приложение и удалить `habits.db` |
| «Перемотать» дату | `dotnet run -- --today 2026-10-30` |

## Документация

- [docs/requirements.md](docs/requirements.md) — требования: сущности, правила валидации, жизненный цикл привычки.
- [docs/erd.drawio](docs/erd.drawio) — ER-диаграмма базы данных (открывается на [app.diagrams.net](https://app.diagrams.net) или в расширении Draw.io для VS Code).
- `docs/Пояснительная записка.docx` — пояснительная записка к проекту.
- `docs/Объяснение кода.docx` — разбор кода: что делает каждый файл и как работает CRUD (написан до версии 3.0, новые таблицы и окна в нём не описаны).

ER-диаграмма и пояснительная записка описывают схему версии 3.0: таблиц `user_activity`, `template_requests`, `audit_log` и триггеров в них пока нет.

## База данных (для SQL-проверок)

Файл `habits.db` открывается в DB Browser for SQLite или DBeaver.

| Таблица | Колонки |
|---|---|
| `roles` | `id`, `name` — справочник ролей: `user`, `admin` |
| `users` | `id`, `username`, `password_hash`, `role_id` → `roles` |
| `categories` | `id`, `name` |
| `completion_messages` | `id`, `text` — общий пул мотивирующих фраз |
| `habit_templates` | `id`, `category_id` → `categories`, `title`, `emoji`, `description`, `completion_message` |
| `habits` | `id`, `user_id` → `users`, `template_id` → `habit_templates`, `category_id` → `categories`, `message_id` → `completion_messages`, `title`, `emoji`, `target_days`, `hours_per_day`, `start_date`, `completion_message` |
| `checkins` | `id`, `habit_id` → `habits`, `date` — уникальная пара (`habit_id`, `date`) |
| `reminders` | `id`, `habit_id` → `habits`, `time`, `is_enabled` — уникальная пара (`habit_id`, `time`) |
| `achievements` | `id`, `code`, `title`, `emoji`, `description` |
| `user_achievements` | `id`, `user_id` → `users`, `achievement_id` → `achievements`, `earned_date` — уникальная пара (`user_id`, `achievement_id`) |
| `user_activity` | `id`, `user_id` → `users`, `date` — активные дни для огонька, уникальная пара (`user_id`, `date`) |
| `template_requests` | `id`, `user_id` → `users`, `category_id` → `categories`, `title`, `emoji`, `description`, `completion_message`, `status` (`pending` / `approved` / `rejected`), `reject_reason`, `created_date`, `reviewed_date` |
| `audit_log` | `id`, `created_at`, `action`, `table_name`, `record_id`, `details` — журнал действий |

Триггеры:

| Триггер | Когда | Что делает |
|---|---|---|
| `trg_checkins_activity` | после отметки | Записывает активный день в `user_activity` |
| `trg_checkins_audit`, `trg_users_insert`, `trg_habits_insert`, `trg_habits_update`, `trg_habits_delete`, `trg_templates_insert`, `trg_templates_delete`, `trg_requests_insert`, `trg_requests_review`, `trg_requests_delete` | после изменения данных | Пишут запись в `audit_log` |

Статус привычки в БД не хранится — он вычисляется по отметкам и дате (раздел 4 требований).

> Версия схемы хранится в `PRAGMA user_version` (сейчас 4). Файл `habits.db` от более старой версии приложение при запуске удаляет и создаёт заново с начальными данными.

## Структура

```
docs/                         требования, ER-диаграмма, пояснительная записка, объяснение кода
src/HabitTrackerDesktop/      десктопное приложение
  Program.cs                  точка входа
  Models/                     классы данных
  Data/                       таблицы, SQL-запросы, начальные данные
  Services/                   правила проверки, дата, сессия, оформление
  Forms/                      окна
```

