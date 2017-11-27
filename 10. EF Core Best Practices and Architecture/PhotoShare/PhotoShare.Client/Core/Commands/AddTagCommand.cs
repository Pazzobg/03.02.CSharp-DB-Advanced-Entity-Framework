namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Data;
    using Models;
    using Utilities;

    public class AddTagCommand
    {
        // AddTag <tag>
        public static string Execute(string[] data)
        {
            if (data.Length != 2)
            {
                throw new InvalidOperationException($"Command {data[0]} not valid!");
            }

            AuthenticationCheck.CheckLogin();

            string tagName = data[1].ValidateOrTransform();

            using (PhotoShareContext context = new PhotoShareContext())
            {
                if (context.Tags.Any(t => t.Name == tagName))
                {
                    throw new ArgumentException($"Tag {tagName} exists!");
                }
                
                context.Tags.Add(new Tag
                {
                    Name = tagName
                });

                context.SaveChanges();
            }

            return $"Tag {tagName} was added successfully!";
        }
    }
}
