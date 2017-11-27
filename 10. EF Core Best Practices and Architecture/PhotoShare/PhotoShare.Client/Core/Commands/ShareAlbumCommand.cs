namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using PhotoShare.Data;
    using PhotoShare.Models;
    using Microsoft.EntityFrameworkCore;
    using PhotoShare.Client.Utilities;

    public class ShareAlbumCommand
    {
        // ShareAlbum <albumId> <username> <permission>
        // For example:
        // ShareAlbum 4 dragon321 Owner
        // ShareAlbum 4 dragon11 Viewer
        public static string Execute(string[] data)
        {
            if (data.Length != 4)
            {
                throw new InvalidOperationException($"Command {data[0]} not valid!");
            }

            int albumId = int.Parse(data[1]);
            string username = data[2];
            string permissionType = ConvertToTitleCase(data[3].ToLower());
            string albumTitle = string.Empty;

            using (var context = new PhotoShareContext())
            {
                var album = context.Albums
                    .Include(a => a.AlbumRoles)
                    .FirstOrDefault(a => a.Id == albumId);

                if (album == null)
                {
                    throw new ArgumentException($"Album {albumId} not found!");
                }

                albumTitle = album.Name;

                var user = context.Users
                    .Include(a => a.AlbumRoles)
                    .FirstOrDefault(u => u.Username == username);

                if (user == null)
                {
                    throw new ArgumentException($"User {username} not found!");
                }

                bool permissionTypeIsValid = Enum.TryParse(permissionType, out Role role);

                if (!permissionTypeIsValid)
                {
                    throw new ArgumentException("Permission must be either “Owner” or “Viewer”!");
                }

                AuthenticationCheck.CheckAlbumOwnership(Session.User.Id, albumId, context);

                context.AlbumRoles.Add(
                    new AlbumRole()
                    {
                        AlbumId = albumId,
                        UserId = user.Id,
                        Role = role
                    });

                context.SaveChanges();
            }

            return $"Username {username} added to album {albumTitle} ({permissionType})";
        }

        private static string ConvertToTitleCase(string str)
        {
            char converted = char.ToUpper(str[0]);
            return converted + str.Substring(1, str.Length - 1);
        }
    }
}
