namespace PhotoShare.Client.Utilities
{
    using System;
    using System.Linq;
    using PhotoShare.Client.Core;
    using PhotoShare.Data;
    using PhotoShare.Models;

    public class AuthenticationCheck
    {
        public static void CheckUserCredentials(string userName)
        {
            if (Session.User.Username != userName)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }
        }

        public static void CheckLogin()
        {
            if (Session.User == null)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }
        }

        public static void CheckLogout()
        {
            if (Session.User != null)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }
        }

        public static void CheckAlbumOwnership(int userId, int albumId, PhotoShareContext context)
        {
            var correctRole = context
                .AlbumRoles
                .FirstOrDefault(ar => ar.UserId == userId && ar.AlbumId == albumId && ar.Role == Role.Owner);

            if (correctRole == null)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }
        }
    }
}