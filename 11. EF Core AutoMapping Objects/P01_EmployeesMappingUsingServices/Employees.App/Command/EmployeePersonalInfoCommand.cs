namespace Employees.App.Command
{
    using System;
    using Employees.Services;

    public class EmployeePersonalInfoCommand : ICommand
    {
        private readonly EmployeeService employeeService;

        public EmployeePersonalInfoCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        // <employeeId>
        public string Execute(params string[] args)
        {
            int employeeId = int.Parse(args[0]);

            var emplDto = this.employeeService.FullPersonalInfoById(employeeId);

            string birthdayNullCheck = emplDto.Birthday == null ? 
                "[No birthday provided]" : 
                emplDto.Birthday.Value.ToString("dd-MM-yyyy");
            string addressNullCheck = emplDto.Address ?? "[No address provided]";

            string result = $"ID: {employeeId} - {emplDto.FirstName} {emplDto.LastName} - ${emplDto.Salary:f2}{Environment.NewLine}" +
                $"Birthday: {birthdayNullCheck}{Environment.NewLine}" +
                $"Address: {addressNullCheck}";

            return result;
        }
    }
}