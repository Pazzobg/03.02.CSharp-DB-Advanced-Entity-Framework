namespace Instagraph.DataProcessor
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;

    using Instagraph.Data;
    using Instagraph.DataProcessor.Dto.Export;
    using System.Collections.Generic;

    public class Serializer
    {
        public static string ExportUncommentedPosts(InstagraphContext context)
        {
            var uncommentedPosts = context.Posts
                .Include(p => p.Comments)
                .Where(p => p.Comments.Count == 0)
                .Select(p => new PostDto
                {
                    Id = p.Id, 
                    Picture = p.Picture.Path,
                    User = p.User.Username
                })
                .OrderBy(p => p.Id)
                .ToArray();
            
            var jsonString = JsonConvert.SerializeObject(uncommentedPosts, Newtonsoft.Json.Formatting.Indented);
            return jsonString;
        }

        public static string ExportPopularUsers(InstagraphContext context)
        {
            var users = context.Users
                .Include(u => u.Followers)
                .Include(u => u.Posts)
                .ThenInclude(p => p.Comments)
                .OrderBy(u => u.Id)
                .ToArray();

            var popularUsers = new List<UserDto>();

            foreach (var user in users)
            {
                var posts = user.Posts.ToArray();
                bool followerCommentsExist = false;
                var followersCount = user.Followers.Count;

                foreach (var post in posts)
                {
                    var comments = post.Comments.ToArray();

                    foreach (var comment in comments)
                    {
                        if (user.Followers.Any(f => f.FollowerId == comment.UserId))  
                        {
                            followerCommentsExist = true;
                            break;
                        }
                    }

                    if (followerCommentsExist)
                    {
                        break;
                    }
                }

                if (!followerCommentsExist)
                {
                    continue;
                }

                var userDto = new UserDto
                {
                    Username = user.Username,
                    Followers = followersCount
                };

                popularUsers.Add(userDto);
            }
            
            var jsonString = JsonConvert.SerializeObject(popularUsers, Newtonsoft.Json.Formatting.Indented);
            return jsonString;
        }

        public static string ExportCommentsOnPosts(InstagraphContext context)
        {
            var users = context.Users
                .Include(u => u.Posts)
                .ThenInclude(p => p.Comments)
                .Select(u => new UserCommentDto
                {
                    Username = u.Username,
                    MostComments = u.Posts.Any(p => p.Comments.Count > 0) ? u.Posts.Max(p => p.Comments.Count) : 0
                })
                .OrderByDescending(x => x.MostComments)
                .ThenBy(x => x.Username)
                .ToArray();

            var sb = new StringBuilder();
            var serializer = new XmlSerializer(typeof(UserCommentDto[]), new XmlRootAttribute("users"));
            serializer.Serialize(new StringWriter(sb), users, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));

            string result = sb.ToString();
            return result;
        }
    }
}
