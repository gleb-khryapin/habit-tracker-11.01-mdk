namespace HabitTrackerDesktop.Models;

public class Reminder
{
    public int Id { get; set; }
    public int HabitId { get; set; }
    public string Time { get; set; }
    public bool IsEnabled { get; set; }
}
