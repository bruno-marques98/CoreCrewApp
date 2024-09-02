namespace CoreCrewApp.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        // Navigation Property
        public ICollection<Employee> Employees { get; set; }
    }
}
