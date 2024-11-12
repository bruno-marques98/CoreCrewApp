using System.ComponentModel.DataAnnotations;

namespace CoreCrewApi.Models
{
    public class Setting
    {
        [Key]
        public int SettingID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } // The name of the setting

        [Required]
        [StringLength(500)]
        public string Value { get; set; } // The value of the setting

        [StringLength(100)]
        public string Type { get; set; } // The type of the setting (e.g., string, int, boolean)

        [StringLength(1000)]
        public string Description { get; set; } // A description of what the setting does
    }
}
