namespace HabitTracker.Services;

/// <summary>
/// Источник «сегодняшней» даты. В тестовом режиме дату можно подменить через /api/test/clock,
/// чтобы проверять сценарии «прошло N дней» без ожидания.
/// </summary>
public class AppClock
{
    private readonly Lock _lock = new();
    private DateOnly? _override;

    public DateOnly Today
    {
        get { lock (_lock) return _override ?? DateOnly.FromDateTime(DateTime.Now); }
    }

    public bool IsOverridden
    {
        get { lock (_lock) return _override.HasValue; }
    }

    public void Set(DateOnly date)
    {
        lock (_lock) _override = date;
    }

    public void Reset()
    {
        lock (_lock) _override = null;
    }
}
