using CoreCrewApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreCrewApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<EmployeeRole> EmployeeRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<PerformanceReview> PerformanceReviews { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<Benefit> Benefits { get; set; }
        public DbSet<EmployeeBenefit> EmployeeBenefits { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<EmployeeProject> EmployeeProjects { get; set; }
        public DbSet<TrainingProgram> TrainingPrograms { get; set; }
        public DbSet<EmployeeTraining> EmployeeTrainings { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Setting> Settings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure EmployeeRole entity
            modelBuilder.Entity<EmployeeRole>()
                .HasKey(er => new { er.EmployeeID, er.RoleID });

            modelBuilder.Entity<EmployeeRole>()
                .HasOne(er => er.Employee)
                .WithMany(e => e.EmployeeRoles)
                .HasForeignKey(er => er.EmployeeID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EmployeeRole>()
                .HasOne(er => er.Role)
                .WithMany(r => r.EmployeeRoles)
                .HasForeignKey(er => er.RoleID)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure LeaveRequest entity
            modelBuilder.Entity<LeaveRequest>()
                .HasOne(lr => lr.Employee)
                .WithMany(e => e.LeaveRequests)
                .HasForeignKey(lr => lr.EmployeeID)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure PerformanceReview entity
            modelBuilder.Entity<PerformanceReview>()
                .HasOne(pr => pr.Employee)
                .WithMany(e => e.PerformanceReviews)
                .HasForeignKey(pr => pr.EmployeeID)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Salary entity
            modelBuilder.Entity<Salary>()
                .HasOne(s => s.Employee)
                .WithMany(e => e.Salaries)
                .HasForeignKey(s => s.EmployeeID)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Benefit entity
            modelBuilder.Entity<Benefit>()
                .HasKey(b => b.BenefitID);

            // Configure EmployeeBenefit entity
            modelBuilder.Entity<EmployeeBenefit>()
                .HasKey(eb => new { eb.EmployeeID, eb.BenefitID });

            modelBuilder.Entity<EmployeeBenefit>()
                .HasOne(eb => eb.Employee)
                .WithMany(e => e.EmployeeBenefits)
                .HasForeignKey(eb => eb.EmployeeID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EmployeeBenefit>()
                .HasOne(eb => eb.Benefit)
                .WithMany(b => b.EmployeeBenefits)
                .HasForeignKey(eb => eb.BenefitID)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Project entity
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Manager)
                .WithMany(e => e.ManagedProjects)
                .HasForeignKey(p => p.ManagerID)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure EmployeeProject entity
            modelBuilder.Entity<EmployeeProject>()
                .HasKey(ep => new { ep.EmployeeID, ep.ProjectID });

            modelBuilder.Entity<EmployeeProject>()
                .HasOne(ep => ep.Employee)
                .WithMany(e => e.EmployeeProjects)
                .HasForeignKey(ep => ep.EmployeeID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EmployeeProject>()
                .HasOne(ep => ep.Project)
                .WithMany(p => p.EmployeeProjects)
                .HasForeignKey(ep => ep.ProjectID)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure TrainingProgram entity
            modelBuilder.Entity<TrainingProgram>()
                .HasOne(tp => tp.Trainer)
                .WithMany(e => e.TrainingPrograms)
                .HasForeignKey(tp => tp.TrainerID)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure EmployeeTraining entity
            modelBuilder.Entity<EmployeeTraining>()
                .HasKey(et => new { et.EmployeeID, et.TrainingProgramID });

            modelBuilder.Entity<EmployeeTraining>()
                .HasOne(et => et.Employee)
                .WithMany(e => e.EmployeeTrainings)
                .HasForeignKey(et => et.EmployeeID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EmployeeTraining>()
                .HasOne(et => et.TrainingProgram)
                .WithMany(tp => tp.EmployeeTrainings)
                .HasForeignKey(et => et.TrainingProgramID)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Notification entity
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Employee)
                .WithMany(e => e.Notifications)
                .HasForeignKey(n => n.EmployeeID)
                .OnDelete(DeleteBehavior.Restrict);

            // Ensure the configuration of all entities
            base.OnModelCreating(modelBuilder);
        }
    }
}
