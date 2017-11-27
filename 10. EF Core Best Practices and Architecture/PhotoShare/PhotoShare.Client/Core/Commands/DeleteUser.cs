namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Data;
    using PhotoShare.Client.Utilities;

    public class DeleteUser
    {
        // DeleteUser <username>
        public static string Execute(string[] data)
        {
            if (data.Length != 2)
            {
                throw new InvalidOperationException($"Command {data[0]} not valid!");
            }

            string username = data[1];

            AuthenticationCheck.CheckUserCredentials(username);

            using (var context = new PhotoShareContext())
            {
                var user = context.Users
                    .FirstOrDefault(u => u.Username == username);

                if (user == null)
                {
                    throw new ArgumentException($"User {username} not found!");
                }

                if (user.IsDeleted.Value == true)
                {
                    throw new InvalidOperationException($"User {username} is already deleted!");
                }

                user.IsDeleted = true;

                context.SaveChanges();
            }

            return $"User {username} was deleted successfully!";
        }
    }
}