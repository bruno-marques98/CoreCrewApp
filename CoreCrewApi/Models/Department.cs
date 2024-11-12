using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreCrewApi.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }
        [Required]
        [Column("DepartmentName")]
        [Display(Name = "Department")]
        public string DepartmentName { get; set; }

        // Navigation Property
        public ICollection<Employee> Employees { get; set; }
    }
}
