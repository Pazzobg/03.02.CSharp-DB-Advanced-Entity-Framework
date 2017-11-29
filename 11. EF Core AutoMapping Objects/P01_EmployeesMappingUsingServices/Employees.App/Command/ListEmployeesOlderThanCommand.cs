namespace Employees.App.Command
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Employees.Services;

    public class ListEmployeesOlderThanCommand : ICommand
    {
        private readonly EmployeeService employeeService;

        public ListEmployeesOlderThanCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(params string[] args)
        {
            int age = int.Parse(args[0]);

            var olderEmployees = this.employeeService.ListEmployeesOlder(age);

            if (olderEmployees.Length > 0)
            {
                var employeesList = new List<string>();

                foreach (var e in olderEmployees.OrderByDescending(e => e.Salary))
                {
                    string manager = e.Manager == null ?
                        "[no manager]" :
                        $"{e.Manager.FirstName} {e.Manager.LastName}";

                    var employeeInfo = $"{e.FirstName} {e.LastName} - ${e.Salary} - Manager: {manager}";
                    employeesList.Add(employeeInfo);
                }

                return string.Join(Environment.NewLine, employeesList);
            }
            else
            {
                return "No employees found.";
            }
        }
    }
}