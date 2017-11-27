namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using PhotoShare.Data;

    public class LoginCommand
    {
        // Login <username> <password>
        public static string Execute(string[] data)
        {
            if (data.Length != 3)
            {
                throw new InvalidOperationException($"Command {data[0]} not valid!");
            }

            string loginUsername = data[1];
            string password = data[2];

            using (var context = new PhotoShareContext())
            {
                var loginUser = context.Users.SingleOrDefault(u => u.Username == loginUsername);

                if (loginUser == null || loginUser.IsDeleted.Value == true)
                {
                    throw new ArgumentException("Invalid username or password!");
                }

                if (password != loginUser.Password)
                {
                    throw new ArgumentException("Invalid username or password!");
                }

                var sessionUser = Session.User;

                if (sessionUser != null && sessionUser.Id == loginUser.Id)
                {
                    throw new ArgumentException("You should logout first!");
                }

                loginUser.LastTimeLoggedIn = DateTime.Now;
                Session.User = loginUser;

                context.SaveChanges();
            }

            return $"User {loginUsername} successfully logged in!";
        }
    }
}
