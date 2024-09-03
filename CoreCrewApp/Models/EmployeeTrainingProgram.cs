using System.ComponentModel.DataAnnotations;

namespace CoreCrewApp.Models
{
    public class EmployeeTrainingProgram
    {
        [Key]
        public int EmployeeTrainingProgramID { get; set; }

        [Required]
        public int EmployeeID { get; set; }

        [Required]
        public int TrainingProgramID { get; set; }

        public DateTime EnrollmentDate { get; set; }

        public Employee Employee { get; set; }
        public TrainingProgram TrainingProgram { get; set; }
    }
}
