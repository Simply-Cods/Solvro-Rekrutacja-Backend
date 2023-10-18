using Microsoft.EntityFrameworkCore;
using Solvro_Backend.Models.Database;
using STask = Solvro_Backend.Models.Database.Task;

namespace Solvro_Backend.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectMemberMapping> ProjectMemberMappings { get; set; }
        public DbSet<STask> Tasks { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Project>()
                .HasOne(p => p.Owner)
                .WithMany(u => u.OwnedProjects)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            

            modelBuilder.Entity<ProjectMemberMapping>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<ProjectMemberMapping>()
                .HasOne(p => p.Project)
                .WithMany(p => p.ProjectMemberMappings)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProjectMemberMapping>()
                .HasOne(p => p.User)
                .WithMany(u => u.ProjectMemberMappings)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<STask>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<STask>()
                .HasOne(t => t.Project)
                .WithMany(p => p.Tasks)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<STask>()
                .HasOne(t => t.Creator)
                .WithMany(u => u.CreatedTasks)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<STask>()
                .HasOne(t => t.AssignedUser)
                .WithMany(u => u.AssignedTasks)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);
        }
    }
}
