namespace PhotoShare.Client.Core.Commands
{
    using System;

    public class LogoutCommand
    {
        // Logout
        public static string Execute(string[] data)
        {
            if (data.Length > 1)
            {
                throw new InvalidOperationException($"Command {data[0]} not valid!");
            }

            var currentUser = Session.User;

            if (currentUser == null)
            {
                throw new InvalidOperationException("You should log in first in order to logout.");
            }

            string username = currentUser.Username;

            Session.User = null;

            return $"User {username} successfully logged out!";
        }
    }
}
