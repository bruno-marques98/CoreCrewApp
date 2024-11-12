using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CoreCrewApi.Models
{
    public class PerformanceReview
    {
        [Key]
        public int PerformanceReviewID { get; set; }

        [Required]
        public int EmployeeID { get; set; }

        [Required]
        public DateTime ReviewDate { get; set; }

        [Required]
        [StringLength(500)]
        public string ReviewComments { get; set; }

        [Required]
        public int Rating { get; set; }

        [ForeignKey("EmployeeID")]
        public Employee Employee { get; set; }
    }
}
