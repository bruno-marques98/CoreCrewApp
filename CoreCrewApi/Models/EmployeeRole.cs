using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace CoreCrewApi.Models
{
    [PrimaryKey(nameof(EmployeeID), nameof(RoleID))]
    public class EmployeeRole
    {
        public int EmployeeID { get; set; }
        public Employee Employee { get; set; }
        public int RoleID { get; set; }
        public Role Role { get; set; }
        public DateTime AssignDate { get; set; }
    }
}
