namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using PhotoShare.Client.Utilities;
    using PhotoShare.Data;

    public class ModifyUserCommand
    {
        // ModifyUser <username> <property> <new value>
        // For example:
        // ModifyUser <username> Password <NewPassword>
        // ModifyUser <username> BornTown <newBornTownName>
        // ModifyUser <username> CurrentTown <newCurrentTownName>
        // !!! Cannot change username
        public static string Execute(string[] data)
        {
            if (data.Length != 4)
            {
                throw new InvalidOperationException($"Command {data[0]} not valid!");
            }

            string username = data[1];
            string property = data[2];
            string newValue = data[3];

            string exceptionMessage = $"Value {newValue} not valid.{Environment.NewLine}";

            using (var context = new PhotoShareContext())
            {
                var currentUser = context.Users
                    .Where(u => u.Username == username)
                    .FirstOrDefault();

                if (currentUser == null)
                {
                    throw new ArgumentException($"User {username} not found!");
                }

                AuthenticationCheck.CheckUserCredentials(username);

                switch (property.ToLower())
                {
                    case "password":

                        if (!newValue.Any(char.IsLower) || !newValue.Any(char.IsDigit))
                        {
                            throw new ArgumentException( $"{exceptionMessage}Invalid Password");
                        }

                        currentUser.Password = newValue;
                        break;

                    case "borntown":
                        var bornTownValue = context.Towns
                            .Where(t => t.Name == newValue)
                            .FirstOrDefault();

                        if (bornTownValue == null)
                        {
                            throw new ArgumentException($"{exceptionMessage}Town {newValue} not found!");
                        }

                        currentUser.BornTown = bornTownValue;
                        break;

                    case "currenttown":
                        var currentTownValue = context.Towns
                            .Where(t => t.Name == newValue)
                            .FirstOrDefault();

                        if (currentTownValue == null)
                        {
                            throw new ArgumentException($"{exceptionMessage}Town {newValue} not found!");
                        }

                        currentUser.CurrentTown = currentTownValue;
                        break;

                    default: throw new ArgumentException ($"Property {property} not supported!");
                }

                context.SaveChanges();
            }

            return $"User {username} {property} is {newValue}.";
        }
    }
}