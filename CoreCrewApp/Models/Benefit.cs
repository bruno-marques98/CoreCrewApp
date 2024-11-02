using System.ComponentModel.DataAnnotations;

namespace CoreCrewApp.Models
{
    public class Benefit
    {
        [Key]
        public int BenefitID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Cost { get; set; }

        // Navigation property for the EmployeeBenefits associated with this Benefit
        public ICollection<EmployeeBenefit> EmployeeBenefits { get; set; }
    }
}
