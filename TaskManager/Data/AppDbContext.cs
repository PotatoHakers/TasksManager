using Microsoft.EntityFrameworkCore;
using TaskManager.Models;

namespace TaskManager.Data
{
    public class AppDbContext : DbContext
    //Чтобы содать миграцию для конкретной бд нужно использовать Add-Migration -Context {Навание бд} {Навание миграции}
    {
        public AppDbContext() { }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<TaskItem> Tasks { get; set; }  // Таблица задач
        public DbSet<LoginLog> LoginLogs { get; set; } // Логи залогиненных аккаунтов

    }
}
