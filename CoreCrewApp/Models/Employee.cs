namespace CoreCrewApp.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime BirthTime { get; set;}
        // FK - Department
        public int DepartementId { get; set; }
        public Department Department { get; set;}
    }
}
