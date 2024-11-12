using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreCrewApi.Models
{
    [PrimaryKey(nameof(EmployeeID),nameof(BenefitID))]
    public class EmployeeBenefit
    {
        public int EmployeeID { get; set; }
        public Employee Employee { get; set; }
        public int BenefitID { get; set; }
        public Benefit Benefit { get; set; }

        public DateTime EnrollmentDate { get; set; }
    }
}
