using Microsoft.EntityFrameworkCore;

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

            modelBuilder.Entity<User>().HasData(new User { Id = 1, Username = "admin@gmail.com", Password = "12345", Role = "Admin" });

            base.OnModelCreating(modelBuilder);
        }
    }

}

