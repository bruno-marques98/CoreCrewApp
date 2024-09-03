using System.ComponentModel.DataAnnotations;

namespace CoreCrewApp.Models
{
    public class EmployeeBenefit
    {
        [Key]
        public int EmployeeID { get; set; }
        public Employee Employee { get; set; }

        [Key]
        public int BenefitID { get; set; }
        public Benefit Benefit { get; set; }

        public DateTime EnrollmentDate { get; set; }
    }
}
