using HabitTrackerDesktop.Data;
using HabitTrackerDesktop.Forms;
using HabitTrackerDesktop.Services;

namespace HabitTrackerDesktop;

static class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        ApplicationConfiguration.Initialize();
        Clock.ReadArguments(args);
        Database.Initialize();
        Application.Run(new LoginForm());
    }
}
