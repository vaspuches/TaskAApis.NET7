using Microsoft.EntityFrameworkCore;
using Tasks.Infrastructure.Data.Model;

namespace Tasks.Infrastructure.Persistence
{
    public class ToDoTaskDbContext : DbContext
    {   
        public DbSet<ToDoTask> ToDoTasks { get; set; }

        public ToDoTaskDbContext(DbContextOptions<ToDoTaskDbContext> options) : base(options)
        {            
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoTask>().ToTable("ToDoTasks");            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {            
            options.UseSqlite("Data Source=ToDoTaskDb.db;");            
        }
    }
}