namespace Employees.Models
{
    using System;
    using System.Collections.Generic;

    public class Employee
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public decimal Salary { get; set; }

        public DateTime? Birthday { get; set; }

        public int Age => (int)Math.Floor((DateTime.Now - this.Birthday.Value).Days / 365.25);
        /*Get the difference between the dates in Timespan.Days -> divide by avg year length ->
          -> Round the result down -> cast to int*/

        public string Address { get; set; }

        public int? ManagerId { get; set; }
        public Employee Manager { get; set; }

        public ICollection<Employee> ManagedEmployees { get; set; } = new List<Employee>();
    }
}