using Microsoft.EntityFrameworkCore;

namespace TodoList.Entity
{
	public class AppDbContext : DbContext
	{
		public DbSet<User> Users { get; set; }
        public DbSet<TaskToDo> Tasks { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=app.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskToDo>()
            .Property(e => e.CreatedDate)
            .HasDefaultValueSql("datetime('now')");

            modelBuilder.Entity<User>().HasIndex(o => o.Username)
                  .IsUnique();

            modelBuilder.Entity<User>().HasMany(e => e.Roles)
                   .WithMany(e => e.Users)
                   .UsingEntity<UserRole>();

            modelBuilder.Entity<User>().HasData(new User { Id = 1, Username = "admin@gmail.com", Password = "12345" });

            modelBuilder.Entity<Role>().HasData(new Role { Id = 1, Name = "Admin" });
            modelBuilder.Entity<Role>().HasData(new Role { Id = 2, Name = "User" });

            modelBuilder.Entity<UserRole>().HasData(new UserRole() { UserId = 1, RoleId = 1 });

            base.OnModelCreating(modelBuilder);
        }
    }

}

