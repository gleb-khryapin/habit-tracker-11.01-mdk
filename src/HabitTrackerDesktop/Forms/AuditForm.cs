using HabitTrackerDesktop.Data;
using HabitTrackerDesktop.Models;
using HabitTrackerDesktop.Services;

namespace HabitTrackerDesktop.Forms;

public partial class AuditForm : Form
{
    private const int MaxEntries = 500;

    public bool Restored;

    public AuditForm()
    {
        InitializeComponent();
        Theme.Apply(this);

        LoadEntries();
    }

    private void LoadEntries()
    {
        grid.Rows.Clear();

        List<AuditEntry> entries = AuditRepository.GetLatest(MaxEntries);
        foreach (AuditEntry entry in entries)
        {
            int index = grid.Rows.Add(entry.CreatedAt, entry.Action, entry.TableName, entry.Details);
            if (entry.Action == "удаление")
            {
                grid.Rows[index].Cells[1].Style.ForeColor = Theme.Danger;
                grid.Rows[index].Cells[1].Style.SelectionForeColor = Theme.Danger;
            }
        }

        lblSubtitle.Text = "Записи создают триггеры базы данных. Показано последних: " + entries.Count + " (не больше " + MaxEntries + ")";
    }

    private void btnBackup_Click(object sender, EventArgs e)
    {
        SaveFileDialog dialog = new SaveFileDialog();
        dialog.Title = "Сохранить резервную копию";
        dialog.Filter = "Резервная копия SQL (*.sql)|*.sql";
        dialog.FileName = "habits-backup-" + DateTime.Now.ToString("yyyy-MM-dd-HHmm") + ".sql";

        if (dialog.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        Database.Backup(dialog.FileName);
        Dialogs.Info("Резервная копия", "Копия сохранена:\n" + dialog.FileName);
    }

    private void btnRestore_Click(object sender, EventArgs e)
    {
        OpenFileDialog dialog = new OpenFileDialog();
        dialog.Title = "Выбрать резервную копию";
        dialog.Filter = "Резервная копия SQL (*.sql)|*.sql";

        if (dialog.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        string error = Database.CheckBackupFile(dialog.FileName);
        if (error != null)
        {
            Dialogs.Error(error);
            return;
        }

        if (!Dialogs.Confirm("Заменить текущие данные данными из копии? Всё, что сделано после её создания, пропадёт. "
            + "Текущие данные сохранятся в файл habits-before-restore.sql рядом с программой."))
        {
            return;
        }

        Database.Restore(dialog.FileName);
        Restored = true;
        Dialogs.Info("Восстановление", "Данные восстановлены из копии. Войди в приложение заново.");
        Close();
    }
}
