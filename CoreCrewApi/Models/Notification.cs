using System.ComponentModel.DataAnnotations;

namespace CoreCrewApi.Models
{
    public class Notification
    {
        [Key]
        public int NotificationID { get; set; }

        [Required]
        public int EmployeeID { get; set; } // Recipient of the notification

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(1000)]
        public string Message { get; set; }

        public bool IsRead { get; set; } = false; // To track if the notification has been read

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        // Navigation property
        public Employee Employee { get; set; } // Link to the employee who received the notification
    }
}
