namespace Employees.App.Command
{
    using System;

    public class ExitCommand : ICommand
    {
        public string Execute(params string[] args)
        {
            Console.WriteLine("Good Bye!");
            Environment.Exit(0);

            return string.Empty;
        }
    }
}
