namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using PhotoShare.Client.Utilities;
    using PhotoShare.Data;
    using PhotoShare.Models;

    public class AddFriendCommand
    {
        // AddFriend <username1> <username2>
        public static string Execute(string[] data)
        {
            if (data.Length != 3)
            {
                throw new InvalidOperationException($"Command {data[0]} not valid!");
            }

            string rqSenderUsername = data[1];
            string rqAccepterUsername = data[2];

            using (var context = new PhotoShareContext())
            {
                var rqSender = context.Users
                    .Include(u => u.FriendsAdded)
                        .ThenInclude(fa => fa.Friend)
                    .FirstOrDefault(u => u.Username == rqSenderUsername);

                var rqAccepter = context.Users
                    .Include(u => u.FriendsAdded)
                        .ThenInclude(fa => fa.Friend)
                    .FirstOrDefault(u => u.Username == rqAccepterUsername);

                if (rqSender == null || rqAccepter == null)
                {
                    string userDoesNotExist = rqSender == null ? rqSenderUsername : rqAccepterUsername;

                    throw new ArgumentException($"{userDoesNotExist} not found!");
                }

                AuthenticationCheck.CheckUserCredentials(rqSenderUsername);

                bool rqAlreadySent = rqSender.FriendsAdded.Any(u => u.Friend == rqAccepter);
                bool oppositeRqAlreadySent = rqAccepter.FriendsAdded.Any(u => u.Friend == rqSender);

                if (rqAlreadySent && !oppositeRqAlreadySent)
                {
                    throw new InvalidOperationException("Friend request already sent!");
                }

                if (!rqAlreadySent && oppositeRqAlreadySent)
                {
                    throw new InvalidOperationException($"{rqSenderUsername} has already an invitation from {rqAccepterUsername}");
                }

                if (rqAlreadySent && oppositeRqAlreadySent)
                {
                    throw new InvalidOperationException($"{rqAccepterUsername} is already a friend to {rqSenderUsername}");
                }

                rqSender.FriendsAdded.Add(
                    new Friendship()
                    {
                        User = rqSender,
                        Friend = rqAccepter
                    });

                context.SaveChanges();
            }

            return $"Friend {rqAccepterUsername} added to {rqSenderUsername}";
        }
    }
}
