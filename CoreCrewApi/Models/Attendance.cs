using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreCrewApi.Models
{
    public class Attendance
    {
        [Key] 
        public int AttendanceId { get; set; }
        [Required]
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set;}

        [Required]
        public AttendanceStatus AttendanceStatus { get; set; }
        [Required]
        public DateTime CheckInTime { get; set; }
        [Required]
        public DateTime CheckOutTime { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime UpdatedAt { get; set;}

    }
    public enum AttendanceStatus
    {
        Present, 
        Absent, 
        OnLeave
    }
}
