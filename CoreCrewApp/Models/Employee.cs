using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreCrewApp.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeID { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        public DateTime HireDate { get; set; }

        public int DepartmentID { get; set; }
        public Department Department { get; set; }

        // Navigation properties
        public ICollection<EmployeeRole> EmployeeRoles { get; set; }
        public ICollection<LeaveRequest> LeaveRequests { get; set; }
        public ICollection<PerformanceReview> PerformanceReviews { get; set; }
        public ICollection<Salary> Salaries { get; set; }
        public ICollection<Benefit> Benefits { get; set; }
        public ICollection<EmployeeBenefit> EmployeeBenefits { get; set; }
        public ICollection<Project> ManagedProjects { get; set; }
        public ICollection<EmployeeProject> EmployeeProjects { get; set; }

        public ICollection<TrainingProgram> TrainingPrograms { get; set; }
        public ICollection<EmployeeTraining> EmployeeTrainings { get; set; }

        public ICollection<Notification> Notifications { get; set; }
        public ICollection<Attendance> Attendances { get; set; }

    }
}
