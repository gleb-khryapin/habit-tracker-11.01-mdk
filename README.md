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
| БД | MySQL (проверено на MariaDB 10.4 из XAMPP), база `habit_tracker` в кодировке `utf8mb4` |
| Доступ к БД | `MySqlConnector`, обычный SQL |

## Запуск с нуля

1. Установить [.NET SDK](https://dotnet.microsoft.com/download) версии 8 или новее и проверить: `dotnet --version`.
2. Запустить MySQL: в XAMPP Control Panel нажать **Start** в строке MySQL (или любой MySQL 8 / MariaDB на порту 3306).
3. Запустить приложение:

   ```bash
   cd src/HabitTrackerDesktop
   dotnet run
   ```

При первом запуске приложение само создаёт базу `habit_tracker`, таблицы, триггеры и начальные данные.

По умолчанию подключение — как в XAMPP: `localhost:3306`, пользователь `root` без пароля.
Если у MySQL другой пароль или порт, положи рядом с exe (`bin/Debug/net8.0-windows/`) файл `connection.txt` со строкой подключения:

```
Server=localhost;Port=3306;User ID=root;Password=твой_пароль;Database=habit_tracker;CharSet=utf8mb4
```

Если сервер не запущен, приложение покажет окно «Нет подключения к базе данных».

Чтобы открыть формы в дизайнере Visual Studio 2022, нужен компонент «Разработка классических приложений .NET».

### Тестовые аккаунты

| Логин | Пароль | Роль | Данные |
|---|---|---|---|
| `admin` | `admin123` | admin | Доступны кнопки «Заявки», «Журнал», «Предложенные привычки», «Категории», «Фразы» |
| `demo` | `demo123` | user | 3 привычки: в процессе, выполненная, прерванная; напоминание на 20:00; огонёк 10 дней; 5 достижений; 2 заявки (на рассмотрении и отклонённая) |

### Для тестирования

| Что нужно | Как |
|---|---|
| Вернуть начальные данные | `dotnet run -- --reset-db` (база `habit_tracker` удаляется и создаётся заново) |
| «Перемотать» дату | `dotnet run -- --today 2026-10-30` |

## Документация

- [docs/requirements.md](docs/requirements.md) — требования: сущности, правила валидации, жизненный цикл привычки.
- [docs/erd.png](docs/erd.png) и [docs/erd.drawio](docs/erd.drawio) — ER-диаграмма базы данных (`.drawio` открывается на [app.diagrams.net](https://app.diagrams.net) или в расширении Draw.io для VS Code).
- `docs/Курсовой проект - пояснительная записка.docx` — пояснительная записка к курсовому проекту по ГОСТ 7.32.
- `docs/Объяснение кода.docx` — разбор кода: что делает каждый файл, как устроены база данных, триггеры и CRUD.

## База данных (для SQL-проверок)

База `habit_tracker` открывается в phpMyAdmin (http://localhost/phpmyadmin), MySQL Workbench или DBeaver.

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

> Таблицы создаются командами `CREATE TABLE IF NOT EXISTS`, триггеры пересоздаются при каждом запуске. Если схема поменялась, базу нужно пересоздать: `dotnet run -- --reset-db`.
> Сравнение строк — `utf8mb4_unicode_520_ci`: регистр букв не различается (`Gleb` = `gleb`), а смайлы различаются.

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

