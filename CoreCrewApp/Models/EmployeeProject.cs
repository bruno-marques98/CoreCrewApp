using System.ComponentModel.DataAnnotations;

namespace CoreCrewApp.Models
{
    public class EmployeeProject
    {
        [Key]
        public int EmployeeProjectID { get; set; }

        [Required]
        public int EmployeeID { get; set; }

        [Required]
        public int ProjectID { get; set; }

        [Required]
        public DateTime AssignmentDate { get; set; }

        public Employee Employee { get; set; }
        public Project Project { get; set; }
    }
}
