namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using PhotoShare.Data;
    using PhotoShare.Models;
    using PhotoShare.Client.Utilities;

    public class AcceptFriendCommand
    {
        // AcceptFriend <username1> <username2>
        public static string Execute(string[] data)
        {
            if (data.Length != 3)
            {
                throw new InvalidOperationException($"Command {data[0]} not valid!");
            }

            AuthenticationCheck.CheckLogin();

            string rqAccepterUsername = data[1];
            string rqSenderUsername = data[2];

            using (var context = new PhotoShareContext())
            {
                var rqAccepter = context.Users
                    .Include(u => u.FriendsAdded)
                        .ThenInclude(fa => fa.Friend)
                    .FirstOrDefault(u => u.Username == rqAccepterUsername);
                var rqSender = context.Users
                    .Include(u => u.FriendsAdded)
                        .ThenInclude(fa => fa.Friend)
                    .FirstOrDefault(u => u.Username == rqSenderUsername);

                if (rqAccepter == null || rqSender == null)
                {
                    string userDoesNotExist = rqAccepter == null ? rqAccepterUsername : rqSenderUsername;

                    throw new ArgumentException($"{userDoesNotExist} not found!");
                }

                AuthenticationCheck.CheckUserCredentials(rqAccepterUsername);

                bool alreadyFriends = (rqAccepter.FriendsAdded.Any(u => u.Friend == rqSender) &&
                                        (rqSender.FriendsAdded.Any(u => u.Friend == rqAccepter)));
                bool isInvititationSent = rqSender.FriendsAdded.Any(u => u.Friend == rqAccepter);

                if (alreadyFriends)
                {
                    throw new InvalidOperationException($"{rqSenderUsername} is already a friend to {rqAccepterUsername}");
                }

                if (!isInvititationSent)
                {
                    throw new InvalidOperationException($"{rqSenderUsername} has not added {rqAccepterUsername} as a friend");
                }

                rqAccepter.FriendsAdded.Add(
                    new Friendship()
                    {
                        User = rqAccepter,
                        Friend = rqSender
                    });

                context.SaveChanges();
            }

            return $"{rqAccepterUsername} accepted {rqSenderUsername} as a friend";
        }
    }
}
