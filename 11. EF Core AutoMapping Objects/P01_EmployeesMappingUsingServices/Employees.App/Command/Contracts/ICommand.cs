namespace Employees.App.Command
{
    internal interface ICommand
    {
        string Execute(params string[] args);
    }
}
