using Microsoft.EntityFrameworkCore;
using TodoList.Models;

namespace TodoList.Entity
{
	public class AppDbContext : DbContext
	{
		public DbSet<User> Users { get; set; }
        public DbSet<TaskToDo> Tasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=app.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskToDo>()
            .Property(e => e.CreatedDate)
            .HasDefaultValueSql("datetime('now')");


            base.OnModelCreating(modelBuilder);
        }
    }

}

