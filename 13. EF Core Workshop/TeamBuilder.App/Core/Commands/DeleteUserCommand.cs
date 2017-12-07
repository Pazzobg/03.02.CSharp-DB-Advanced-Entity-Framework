namespace TeamBuilder.App.Core.Commands
{
    using System.Linq;
    using TeamBuilder.App.Utilities;
    using TeamBuilder.Data;

    // • DeleteUser
    public class DeleteUserCommand
    {
        public string Execute(string[] commandParams)
        {
            Check.CheckLength(0, commandParams);
            AuthenticationManager.Authorize();

            var user = AuthenticationManager.GetCurrentUser();

            using (var context = new TeamBuilderContext())
            {
                context.Users.FirstOrDefault(u => u.Id == user.Id).IsDeleted = true;
                context.SaveChanges();
            }

            AuthenticationManager.Logout();

            return $"User {user.Username} was deleted successfully!"; 
        }
    }
}