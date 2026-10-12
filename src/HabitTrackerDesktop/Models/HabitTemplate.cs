namespace HabitTrackerDesktop.Models;

public class HabitTemplate
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Emoji { get; set; }
    public string Description { get; set; }
    public string CompletionMessage { get; set; }

    public override string ToString()
    {
        return Emoji + "  " + Title;
    }
}
