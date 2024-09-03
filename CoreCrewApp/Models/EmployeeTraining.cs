using System.ComponentModel.DataAnnotations;

namespace CoreCrewApp.Models
{
    public class EmployeeTraining
    {
        [Key]
        public int EmployeeTrainingID { get; set; }

        [Required]
        public int EmployeeID { get; set; }

        [Required]
        public int TrainingProgramID { get; set; }

        [Required]
        public DateTime EnrollmentDate { get; set; }

        public DateTime? CompletionDate { get; set; }

        // Navigation properties
        public Employee Employee { get; set; }
        public TrainingProgram TrainingProgram { get; set; }
    }
}
