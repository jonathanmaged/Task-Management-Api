using Microsoft.EntityFrameworkCore;
using Task_Management_Api.Models;
using Task = Task_Management_Api.Models.Task;


namespace Task_Management_Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskComment>().HasKey(tc =>
                new {tc.TaskId,tc.UserId }
                );
            modelBuilder.Entity<TaskComment>()
                .HasOne(tc => tc.Task)
                .WithMany(t => t.TaskComments)
                .HasForeignKey(tc => tc.TaskId);

            modelBuilder.Entity<TaskComment>()
                .HasOne(tc => tc.User)
                .WithMany(t => t.TaskComments)
                .HasForeignKey(tc => tc.UserId);

            modelBuilder.Entity<User>()
                .HasData(
                new User { Id=1, Name="John" },
                new User { Id=2, Name="Maged" },
                new User { Id=3, Name="Youssef" }
                );

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<TaskComment> TaskComments { get; set; }


    }
}
