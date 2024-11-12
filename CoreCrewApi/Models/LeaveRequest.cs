using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CoreCrewApi.Models
{
    public class LeaveRequest
    {
        [Key]
        public int LeaveRequestID { get; set; }

        [Required]
        public int EmployeeID { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [StringLength(500)]
        public string Reason { get; set; }

        public LeaveStatus Status { get; set; }

        [ForeignKey("EmployeeID")]
        public Employee Employee { get; set; }
    }
    public enum LeaveStatus
    {
        Pending,
        Approved,
        Rejected
    }
}
