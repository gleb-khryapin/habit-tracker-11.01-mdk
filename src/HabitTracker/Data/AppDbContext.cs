using System.Text;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<HabitTemplate> HabitTemplates => Set<HabitTemplate>();
    public DbSet<Habit> Habits => Set<Habit>();
    public DbSet<Checkin> Checkins => Set<Checkin>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<User>(e =>
        {
            e.ToTable("users");
            // NOCASE: логины Gleb и gleb считаются одинаковыми на уровне БД
            e.Property(u => u.Username).HasMaxLength(20).UseCollation("NOCASE");
            e.HasIndex(u => u.Username).IsUnique();
        });

        b.Entity<HabitTemplate>(e => e.ToTable("habit_templates"));

        b.Entity<Habit>(e =>
        {
            e.ToTable("habits");
            e.HasOne(h => h.User).WithMany(u => u.Habits)
                .HasForeignKey(h => h.UserId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(h => h.Template).WithMany()
                .HasForeignKey(h => h.TemplateId).OnDelete(DeleteBehavior.SetNull);
        });

        b.Entity<Checkin>(e =>
        {
            e.ToTable("checkins");
            e.HasOne(c => c.Habit).WithMany(h => h.Checkins)
                .HasForeignKey(c => c.HabitId).OnDelete(DeleteBehavior.Cascade);
            // одна отметка на привычку в день — гарантия на уровне БД
            e.HasIndex(c => new { c.HabitId, c.Date }).IsUnique();
        });

        // snake_case для колонок, чтобы SQL-запросы при проверках читались привычно
        foreach (var entity in b.Model.GetEntityTypes())
            foreach (var property in entity.GetProperties())
                property.SetColumnName(ToSnakeCase(property.Name));
    }

    private static string ToSnakeCase(string name)
    {
        var sb = new StringBuilder();
        for (var i = 0; i < name.Length; i++)
        {
            if (char.IsUpper(name[i]) && i > 0) sb.Append('_');
            sb.Append(char.ToLowerInvariant(name[i]));
        }
        return sb.ToString();
    }
}
