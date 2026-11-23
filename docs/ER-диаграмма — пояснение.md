# ER-диаграмма — пояснение

Схема: [erd.png](erd.png). База `habit_tracker`, 13 таблиц.

## Обозначения

| Знак | Значение |
|---|---|
| PK | первичный ключ, уникальный номер записи |
| FK | внешний ключ, ссылка на запись другой таблицы |
| UQ | значение не повторяется |
| 1 — ∞ | связь «один ко многим»: одной записи слева соответствует много записей справа |

## Таблицы

| Группа | Таблицы |
|---|---|
| Пользователи | `roles` → `users` |
| Привычки | `habits`, отметки `checkins`, напоминания `reminders` |
| Справочники | `categories`, каталог `habit_templates`, фразы `completion_messages` |
| Бизнес-процессы | огонёк `user_activity`, заявки `template_requests`, достижения `achievements` + `user_achievements` |
| Журнал | `audit_log`, связей нет, записи добавляют триггеры |

## Связи

Все связи «один ко многим»:

- `users` 1 — ∞ `habits`, `user_activity`, `user_achievements`, `template_requests`;
- `habits` 1 — ∞ `checkins`, `reminders`;
- `categories` 1 — ∞ `habits`, `habit_templates`, `template_requests`;
- `habit_templates` 1 — ∞ `habits`, `completion_messages` 1 — ∞ `habits`, `roles` 1 — ∞ `users`.

Связь «многие ко многим» между пользователями и достижениями сделана через таблицу `user_achievements`.

При удалении пользователя или привычки зависимые записи удаляются каскадно (`ON DELETE CASCADE`). При удалении категории, привычки каталога или фразы ссылка обнуляется (`ON DELETE SET NULL`). Роль нельзя удалить, пока у неё есть пользователи.
