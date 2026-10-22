namespace HabitTrackerDesktop.Models;

public class AuditEntry
{
    public int Id { get; set; }
    public string CreatedAt { get; set; }
    public string Action { get; set; }
    public string TableName { get; set; }
    public int RecordId { get; set; }
    public string Details { get; set; }
}
