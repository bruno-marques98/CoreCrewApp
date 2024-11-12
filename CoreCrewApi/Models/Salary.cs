using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CoreCrewApi.Models
{
    public class Salary
    {
        [Key]
        public int SalaryID { get; set; }

        [Required]
        public int EmployeeID { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime EffectiveDate { get; set; }

        public DateTime? EndDate { get; set; } 

        [ForeignKey("EmployeeID")]
        public Employee Employee { get; set; }
    }
}
