namespace Employees.App.Command
{
    using Employees.DtoModels;
    using Employees.Services;

    public class AddEmployeeCommand : ICommand
    {
        private readonly EmployeeService employeeService;

        public AddEmployeeCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        // <first name> <last name> <salary>
        public string Execute(params string[] args)
        {
            string firstName = args[0];
            string lastName = args[1];
            decimal salary = decimal.Parse(args[2]);

            var employeeDto = new EmployeeDto(firstName, lastName, salary);

            this.employeeService.AddEmployee(employeeDto);

            return $"Employee {firstName} {lastName} successfully added.";
        }
    }
}
