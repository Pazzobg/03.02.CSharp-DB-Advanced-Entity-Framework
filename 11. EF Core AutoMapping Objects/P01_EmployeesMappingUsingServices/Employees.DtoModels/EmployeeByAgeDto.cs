namespace Employees.DtoModels
{
    using Employees.Models;

    public class EmployeeByAgeDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public decimal Salary { get; set; }

        public Employee Manager { get; set; }
    }
}
