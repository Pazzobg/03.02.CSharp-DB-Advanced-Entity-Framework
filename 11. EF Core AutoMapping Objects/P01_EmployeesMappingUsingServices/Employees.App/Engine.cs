namespace Employees.App
{
    using System;
    using System.Linq;

    internal class Engine
    {
        private readonly IServiceProvider serviceProvider;

        public Engine(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        internal void Run()
        {
            while (true)
            {
                try
                {
                    string input = Console.ReadLine().Trim();
                    string[] commandTokens = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    string commandName = commandTokens[0];
                    string[] commandArgs = commandTokens.Skip(1).ToArray();

                    var command = CommandParser.Parse(serviceProvider, commandName);

                    var result = command.Execute(commandArgs);

                    Console.WriteLine(result);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}