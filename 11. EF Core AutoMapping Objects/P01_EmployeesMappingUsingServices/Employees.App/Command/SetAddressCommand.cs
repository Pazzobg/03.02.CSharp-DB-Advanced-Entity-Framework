namespace Employees.App.Command
{
    using System.Linq;
    using Employees.Services;

    public class SetAddressCommand : ICommand
    {
        private readonly EmployeeService employeeService;

        public SetAddressCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        // SetAddress <employeeId> <address> 
        public string Execute(params string[] args)
        {
            int employeeId = int.Parse(args[0]);
            string address = string.Join(" ", args.Skip(1));

            string employeeName = this.employeeService.SetAddress(employeeId, address);

            return $"{employeeName}'s address successfully set.";
        }
    }
}
