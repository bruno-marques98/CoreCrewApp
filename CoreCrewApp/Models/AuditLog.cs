using System.ComponentModel.DataAnnotations;

namespace CoreCrewApp.Models
{
    public class AuditLog
    {
        [Key]
        public int AuditLogID { get; set; }

        [Required]
        [StringLength(100)]
        public string Action { get; set; }

        [Required]
        [StringLength(100)]
        public string TableName { get; set; }

        public int? RecordID { get; set; } // ID of the record affected, if applicable

        [Required]
        [StringLength(100)]
        public string UserName { get; set; } // User who performed the action

        [Required]
        public DateTime Timestamp { get; set; }
        [Required]
        [StringLength(1000)]
        public string Details { get; set; } // Additional details about the action
    }
}
