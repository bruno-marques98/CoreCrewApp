using System.ComponentModel.DataAnnotations;

namespace CoreCrewApp.Models
{
    public class Project
    {
        [Key]
        public int ProjectID { get; set; }

        [Required]
        [StringLength(100)]
        public string ProjectName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; } // Nullable, if the project is ongoing

        [Required]
        public int ManagerID { get; set; }

        // Navigation properties
        public Employee Manager { get; set; }
        public ICollection<EmployeeProject> EmployeeProjects { get; set; }
    }
}
