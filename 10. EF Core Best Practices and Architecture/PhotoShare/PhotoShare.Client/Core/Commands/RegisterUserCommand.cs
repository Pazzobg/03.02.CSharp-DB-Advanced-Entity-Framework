namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Data;
    using Models;
    using PhotoShare.Client.Utilities;

    public class RegisterUserCommand
    {
        // RegisterUser <username> <password> <repeat-password> <email>
        public static string Execute(string[] data)
        {
            if (data.Length != 5)
            {
                throw new InvalidOperationException($"Command {data[0]} not valid!");
            }

            AuthenticationCheck.CheckLogout();
            
            string username = data[1];
            string password = data[2];
            string repeatPassword = data[3];
            string email = data[4];

            using (var context = new PhotoShareContext())
            {
                if (context.Users.Any(u => u.Username == username))
                {
                    throw new InvalidOperationException($"Username {username} is already taken!");
                }
            }

            if (password != repeatPassword)
            {
                throw new ArgumentException("Passwords do not match!");
            }

            User user = new User
            {
                Username = username,
                Password = password,
                Email = email,
                IsDeleted = false,
                RegisteredOn = DateTime.Now,
                LastTimeLoggedIn = DateTime.Now
            };

            using (PhotoShareContext context = new PhotoShareContext())
            {
                context.Users.Add(user);
                context.SaveChanges();
            }

            return $"User {user.Username} was registered successfully!";
        }
    }
}
