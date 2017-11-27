namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using PhotoShare.Client.Utilities;
    using PhotoShare.Data;
    using PhotoShare.Models;

    public class AddTagToCommand
    {
        // AddTagTo <albumName> <tag>
        public static string Execute(string[] data)
        {
            if (data.Length != 3)
            {
                throw new InvalidOperationException($"Command {data[0]} not valid!");
            }

            AuthenticationCheck.CheckLogin();

            string albumTitle = data[1];
            string tagName = TagUtilities.ValidateOrTransform(data[2]);

            using (var context = new PhotoShareContext())
            {
                var album = context.Albums
                    .Include(a => a.AlbumTags)
                        .ThenInclude(at => at.Tag)
                    .FirstOrDefault(a => a.Name.ToLower() == albumTitle.ToLower());

                var tag = context.Tags
                    .FirstOrDefault(t => t.Name.ToLower() == tagName.ToLower());

                if (album == null || tag == null)
                {
                    throw new ArgumentException("Either tag or album do not exist!");
                }

                if (album.AlbumTags.Any(at => at.Tag.Name.ToLower() == tagName.ToLower()))
                {
                    throw new ArgumentException($"Tag {tagName} already present in album {albumTitle}");
                }

                var userId = Session.User.Id;

                AuthenticationCheck.CheckAlbumOwnership(userId, album.Id, context);

                context.AlbumTags.Add(
                    new AlbumTag()
                    {
                        Album = album,
                        Tag = tag
                    });

                context.SaveChanges();
            }

            return $"Tag {tagName} added to {albumTitle}!";
        }
    }
}
