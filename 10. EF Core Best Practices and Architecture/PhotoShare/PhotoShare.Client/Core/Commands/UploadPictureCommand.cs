namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using PhotoShare.Data;
    using PhotoShare.Models;
    using PhotoShare.Client.Utilities;

    public class UploadPictureCommand
    {
        // UploadPicture <albumName> <pictureTitle> <pictureFilePath>
        public static string Execute(string[] data)
        {
            if (data.Length != 4)
            {
                throw new InvalidOperationException($"Command {data[0]} not valid!");
            }

            AuthenticationCheck.CheckLogin();

            string albumTitle = data[1];
            string pictureTitle = data[2];
            string filePath = data[3];

            using (var context = new PhotoShareContext())
            {
                var album = context.Albums
                    .Include(a => a.Pictures)
                    .SingleOrDefault(a => a.Name == albumTitle);

                if (album == null)
                {
                    throw new ArgumentException($"Album {albumTitle} not found!");
                }

                int currentlyLoggedinUserId = Session.User.Id;
                AuthenticationCheck.CheckAlbumOwnership(currentlyLoggedinUserId, album.Id, context);

                if (album.Pictures.Any(p => p.Title.ToLower() == pictureTitle.ToLower()))
                {
                    throw new InvalidOperationException(
                        $"A picture with the name {pictureTitle} is already present in album {albumTitle}");
                }

                album.Pictures.Add(
                    new Picture()
                    {
                        Title = pictureTitle, 
                        Path = filePath, 
                        Album = album
                    });

                context.SaveChanges();
            }

            return $"Picture {pictureTitle} added to {albumTitle}!";
        }
    }
}
