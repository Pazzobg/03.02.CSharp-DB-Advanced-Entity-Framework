namespace PhotoShare.Client.Core.Commands
{
    using Microsoft.EntityFrameworkCore;
    using PhotoShare.Data;
    using System;
    using System.Linq;
    using System.Text;

    public class PrintFriendsListCommand
    {
        // PrintFriendsList <username>
        public static string Execute(string[] data)
        {
            if (data.Length != 2)
            {
                throw new InvalidOperationException($"Command {data[0]} not valid!");
            }

            string username = data[1];

            using (var context = new PhotoShareContext())
            {
                var user = context.Users
                    .Include(u => u.FriendsAdded)
                        .ThenInclude(fa => fa.Friend)
                    .Include(u => u.AddedAsFriendBy)
                        .ThenInclude(aaf => aaf.User)
                    .FirstOrDefault(u => u.Username == username);

                if (user == null)
                {
                    throw new ArgumentException($"User {username} not found!");
                }

                var friendsAddedList = user.FriendsAdded
                    .Select(fa => fa.Friend)
                    .ToList();
                var addedAsFriendList = user.AddedAsFriendBy
                    .Select(aaf => aaf.User)
                    .ToList();

                var sb = new StringBuilder();
                sb.AppendLine("Friends:");

                foreach (var friend in friendsAddedList.OrderBy(f => f.Username))
                {
                    if (addedAsFriendList.Any(af => af.Username == friend.Username))
                    {
                        sb.AppendLine($"-{friend.Username}");
                    }
                }

                if (sb.Length > 10)
                {
                    return sb.ToString();
                }
                else
                {
                    sb.Clear();
                    return "No friends for this user. :(";
                }
            }
        }
    }
}
