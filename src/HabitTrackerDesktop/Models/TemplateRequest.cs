namespace HabitTrackerDesktop.Models;

public class TemplateRequest
{
    public const string Pending = "pending";
    public const string Approved = "approved";
    public const string Rejected = "rejected";

    public int Id { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; }
    public int? CategoryId { get; set; }
    public string CategoryName { get; set; }
    public string Title { get; set; }
    public string Emoji { get; set; }
    public string Description { get; set; }
    public string CompletionMessage { get; set; }
    public string Status { get; set; }
    public string RejectReason { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ReviewedDate { get; set; }

    public string GetStatusText()
    {
        if (Status == Approved)
        {
            return "Одобрена";
        }
        if (Status == Rejected)
        {
            return "Отклонена";
        }
        return "На рассмотрении";
    }
}
