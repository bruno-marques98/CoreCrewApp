using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace CoreCrewApp.Models
{
    public class EmployeeRole
    {
        [Key]
        public int EmployeeID { get; set; }
        public Employee Employee { get; set; }

        [Key]
        public int RoleID { get; set; }
        public Role Role { get; set; }
    }
}
