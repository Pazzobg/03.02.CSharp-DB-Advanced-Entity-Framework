namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using PhotoShare.Data;
    using PhotoShare.Models;
    using PhotoShare.Client.Utilities;

    public class CreateAlbumCommand
    {
        // CreateAlbum <username> <albumTitle> <BgColor> <tag1> <tag2>...<tagN>
        public static string Execute(string[] data)
        {
            if (data.Length < 4)
            {
                throw new InvalidOperationException($"Command {data[0]} not valid!");
            }

            AuthenticationCheck.CheckLogin();

            string username = data[1];
            string albumTitle = data[2];
            string bgColor = ConvertToTitleCase(data[3].ToLower());
            var tags = data
                .Skip(4)
                .Select(t => TagUtilities.ValidateOrTransform(t))
                .ToArray();

            AuthenticationCheck.CheckUserCredentials(username);

            using (var context = new PhotoShareContext())
            {
                var user = context.Users
                    .FirstOrDefault(u => u.Username == username);

                if (user == null)
                {
                    throw new ArgumentException($"User {username} not found!");
                }

                if (context.Albums.Any(a => a.Name == albumTitle))
                {
                    throw new ArgumentException($"Album {albumTitle} exists!");
                }

                bool bgColorIsValid = Enum.TryParse(bgColor, out Color color);
                if (!bgColorIsValid)
                {
                    throw new ArgumentException($"Color {bgColor} not found!");
                }

                var album = new Album()
                {
                    Name = albumTitle,
                    IsPublic = false,
                    BackgroundColor = color,

                };

                context.Albums.Add(album);

                var currentAlbumRole = new AlbumRole
                {
                    Album = album,
                    User = user,
                    Role = Role.Owner
                };

                context.AlbumRoles.Add(currentAlbumRole);

                for (int i = 0; i < tags.Length; i++)
                {
                    var currentTag = context.Tags.FirstOrDefault(t => t.Name.ToLower() == tags[i].ToLower());

                    if (currentTag == null)
                    {
                        throw new ArgumentException("Invalid tags!");
                    }

                    context.AlbumTags.Add(
                        new AlbumTag()
                        {
                            TagId = currentTag.Id,
                            AlbumId = album.Id
                        });
                }

                context.SaveChanges();
            }

            return $"Album {albumTitle} successfully created!";
        }

        private static string ConvertToTitleCase(string str)
        {
            char converted = char.ToUpper(str[0]);
            return converted + str.Substring(1, str.Length - 1);
        }
    }
}
