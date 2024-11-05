using System.ComponentModel.DataAnnotations;
using CoreCrewApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
