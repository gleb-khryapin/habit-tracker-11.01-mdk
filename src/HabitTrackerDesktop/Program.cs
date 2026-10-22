using HabitTrackerDesktop.Data;
using HabitTrackerDesktop.Forms;
using HabitTrackerDesktop.Services;
using MySqlConnector;

namespace HabitTrackerDesktop;

static class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        ApplicationConfiguration.Initialize();
        Clock.ReadArguments(args);
        Database.LoadSettings();

        try
        {
            if (args.Contains("--reset-db"))
            {
                Database.Reset();
            }
            else
            {
                Database.Initialize();
            }
        }
        catch (MySqlException ex)
        {
            MessageBox.Show(
                "Не удалось подключиться к базе данных MySQL.\n\n"
                + "Запусти MySQL в XAMPP Control Panel (кнопка Start в строке MySQL) "
                + "и проверь строку подключения в файле connection.txt рядом с программой.\n\n"
                + "Ошибка: " + ex.Message,
                "Нет подключения к базе данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        Application.Run(new LoginForm());
    }
}
