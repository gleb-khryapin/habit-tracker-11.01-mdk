namespace HabitTrackerDesktop.Services;

public static class Dialogs
{
    public static void Error(string text)
    {
        MessageBox.Show(text, "Проверь данные", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    public static bool Confirm(string text)
    {
        DialogResult result = MessageBox.Show(text, "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        return result == DialogResult.Yes;
    }
}
