using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreCrewApi.Models
{
    public class Role
    {
        [Key]
        public int RoleID { get; set; }

        [Required]
        [StringLength(100)]
        public string RoleName { get; set; }

        // Navigation property for the EmployeeRoles associated with this Role
        public ICollection<EmployeeRole> EmployeeRoles { get; set; }

    }
}
