using CoreCrewApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CoreCrewApp.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
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
        public DbSet<Attendance> Attendances { get; set; }
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

            // Attendance Configuration
            modelBuilder.Entity<Attendance>(entity =>
            {
                // Setting up the primary key
                entity.HasKey(a => a.AttendanceId);

                // Configuring the relationship between Attendance and Employee
                entity.HasOne(a => a.Employee)
                      .WithMany(e => e.Attendances)
                      .HasForeignKey("EmployeeId")
                      .OnDelete(DeleteBehavior.Restrict); // Or use Cascade, depending on your business rules

                // Configuring the AttendanceStatus enum
                entity.Property(a => a.AttendanceStatus)
                      .IsRequired()
                      .HasConversion<int>(); // This will store the enum as an integer in the database

                // Configuring other properties
                entity.Property(a => a.CheckInTime)
                      .IsRequired();

                entity.Property(a => a.CheckOutTime)
                      .IsRequired();

                entity.Property(a => a.CreatedAt)
                      .IsRequired();

                entity.Property(a => a.UpdatedAt)
                      .IsRequired();
            });

            // Configure Notification entity
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Employee)
                .WithMany(e => e.Notifications)
                .HasForeignKey(n => n.EmployeeID)
                .OnDelete(DeleteBehavior.Restrict);

            #region Seed Data
            // Seed Departments
            modelBuilder.Entity<Department>().HasData(
                new Department { DepartmentId = 1, DepartmentName = "Human Resources" },
                new Department { DepartmentId = 2, DepartmentName = "IT" },
                new Department { DepartmentId = 3, DepartmentName = "Marketing" }
            );
            // Seed Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleID = 1, RoleName = "Manager" },
                new Role { RoleID = 2, RoleName = "Developer" },
                new Role { RoleID = 3, RoleName = "Designer" }
            );

            // Seed Employees
            modelBuilder.Entity<Employee>().HasData(
                new Employee { EmployeeID = 1, DepartmentID = 1, FirstName = "John", LastName = "Doe", Email = "test@test.com", HireDate = new DateTime(1985, 1, 1) },
                new Employee { EmployeeID = 2, DepartmentID = 1, FirstName = "Jane", LastName = "Smith", Email = "test@test.com", HireDate = new DateTime(1990, 2, 15) }
            );

            // Seed Benefits
            modelBuilder.Entity<Benefit>().HasData(
                new Benefit { BenefitID = 1, Name = "Health Insurance", Description = "Basic health insurance", Cost = 200 },
                new Benefit { BenefitID = 2, Name = "Retirement Plan", Description = "Company matching retirement plan", Cost = 150 }
            );

            // Seed Projects
            modelBuilder.Entity<Project>().HasData(
                new Project { ProjectID = 1, ProjectName = "Project Alpha", Description = "First project", StartDate = new DateTime(2024, 1, 1), EndDate = new DateTime(2024, 6, 30), ManagerID = 1 },
                new Project { ProjectID = 2, ProjectName = "Project Beta", Description = "Second project", StartDate = new DateTime(2024, 2, 1), EndDate = new DateTime(2024, 7, 31), ManagerID = 2 }
            );

            // Seed TrainingPrograms
            modelBuilder.Entity<TrainingProgram>().HasData(
                new TrainingProgram { TrainingProgramID = 1, ProgramName = "Leadership Training", Description = "Develop leadership skills", TrainerID = 1 },
                new TrainingProgram { TrainingProgramID = 2, ProgramName = "Software Development", Description = "Advanced software development techniques", TrainerID = 2 }
            );

            // Seed EmployeeBenefits
            modelBuilder.Entity<EmployeeBenefit>().HasData(
                new EmployeeBenefit { EmployeeID = 1, BenefitID = 1, EnrollmentDate = DateTime.Now },
                new EmployeeBenefit { EmployeeID = 2, BenefitID = 2, EnrollmentDate = DateTime.Now }
            );

            // Seed EmployeeTrainings
            modelBuilder.Entity<EmployeeTraining>().HasData(
                new EmployeeTraining { EmployeeID = 1, TrainingProgramID = 1 },
                new EmployeeTraining { EmployeeID = 2, TrainingProgramID = 2 }
            );

            // Seed EmployeeRoles
            modelBuilder.Entity<EmployeeRole>().HasData(
                new EmployeeRole { EmployeeID = 1, RoleID = 1 },
                new EmployeeRole { EmployeeID = 2, RoleID = 2 }
            );

            // Seed EmployeeProjects
            modelBuilder.Entity<EmployeeProject>().HasData(
                new EmployeeProject { EmployeeID = 1, ProjectID = 1 },
                new EmployeeProject { EmployeeID = 2, ProjectID = 2 }
            );

            // Seed Notifications
            modelBuilder.Entity<Notification>().HasData(
                new Notification { NotificationID = 1, EmployeeID = 1, Title = "Welcome", Message = "Welcome to the company!" },
                new Notification { NotificationID = 2, EmployeeID = 2, Title = "Benefits", Message = "Please review your benefits options." }
            );

            // Seed AuditLogs
            modelBuilder.Entity<AuditLog>().HasData(
                new AuditLog { AuditLogID = 1, Action = "Created", TableName = "Employee", UserName = "Bruno Marquês", Timestamp = DateTime.Now, Details = "Initial setup of the application." },
                new AuditLog { AuditLogID = 2, Action = "Updated", TableName = "Employee", UserName = "Bruno Marquês", Timestamp = DateTime.Now, Details = "Updated employee records." }
            );

            // Seed Settings
            modelBuilder.Entity<Setting>().HasData(
                new Setting { SettingID = 1, Name = "AppName", Value = "Employee Management System", Type = "Naming", Description = "Application Naming" },
                new Setting { SettingID = 2, Name = "Version", Value = "1.0.0", Type = "Version", Description = "Version Number" }
            );
            #endregion
            // Ensure the configuration of all entities
            base.OnModelCreating(modelBuilder);
        }
    }
}
