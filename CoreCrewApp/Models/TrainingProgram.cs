using System.ComponentModel.DataAnnotations;

namespace CoreCrewApp.Models
{
    public class TrainingProgram
    {
        [Key]
        public int TrainingProgramID { get; set; }

        [Required]
        [StringLength(100)]
        public string ProgramName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        public int TrainerID { get; set; } // Assuming the trainer is an employee

        // Navigation properties
        public Employee Trainer { get; set; }
        public ICollection<EmployeeTrainingProgram> EmployeeTrainingPrograms { get; set; }
        // Navigation property for the employees enrolled in this training program
        public ICollection<EmployeeTraining> EmployeeTrainings { get; set; }

    }
}
