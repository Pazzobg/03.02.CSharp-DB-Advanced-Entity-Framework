namespace Instagraph.DataProcessor
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    using Newtonsoft.Json;

    using Instagraph.Data;
    using Instagraph.DataProcessor.Dto.Import;
    using Instagraph.Models;

    public class Deserializer
    {
        private const string SuccessMsg = "Successfully imported {0} {1}.";
        private const string SuccessUserFollwerMsg = "Successfully imported Follower {0} to User {1}.";
        private const string ErrorMsg = "Error: Invalid data.";

        public static string ImportPictures(InstagraphContext context, string jsonString)
        {
            var pictures = JsonConvert.DeserializeObject<PictureDto[]>(jsonString);

            var sb = new StringBuilder();
            var validPics = new List<Picture>();

            foreach (var picDto in pictures)
            {
                if (!IsValid(picDto) || 
                    picDto.Size <= 0 || 
                    string.IsNullOrWhiteSpace(picDto.Path) || 
                    validPics.Any(p => p.Path == picDto.Path))
                {
                    sb.AppendLine(ErrorMsg);
                    continue;
                }

                var pic = new Picture
                {
                    Path = picDto.Path,
                    Size = picDto.Size
                };

                validPics.Add(pic);
                sb.AppendLine(string.Format(SuccessMsg, "Picture", pic.Path));
            }

            context.Pictures.AddRange(validPics);
            context.SaveChanges();

            var result = sb.ToString().TrimEnd();
            return result;
        }

        public static string ImportUsers(InstagraphContext context, string jsonString)
        {
            var users = JsonConvert.DeserializeObject<UserDto[]>(jsonString);

            var sb = new StringBuilder();
            var validUsers = new List<User>();

            foreach (var userDto in users)
            {
                if (!IsValid(userDto))
                {
                    sb.AppendLine(ErrorMsg);
                    continue;
                }

                var profilePic = context.Pictures.FirstOrDefault(p => p.Path == userDto.ProfilePicture);

                if (profilePic == null)
                {
                    sb.AppendLine(ErrorMsg);
                    continue;
                }

                var user = new User
                {
                    Username = userDto.Username,
                    Password = userDto.Password,
                    ProfilePicture = profilePic
                };

                validUsers.Add(user);
                sb.AppendLine(string.Format(SuccessMsg, "User", user.Username));
            }

            context.Users.AddRange(validUsers);
            context.SaveChanges();

            var result = sb.ToString().TrimEnd();
            return result;
        }

        public static string ImportFollowers(InstagraphContext context, string jsonString)
        {
            var userFollowers = JsonConvert.DeserializeObject<FollowerDto[]>(jsonString);

            var sb = new StringBuilder();
            var validFollowers = new List<UserFollower>();

            foreach (var userFollDto in userFollowers)
            {
                if (!IsValid(userFollDto))
                {
                    sb.AppendLine(ErrorMsg);
                    continue;
                }

                var user = context.Users.FirstOrDefault(u => u.Username == userFollDto.User);
                var follower = context.Users.FirstOrDefault(u => u.Username == userFollDto.Follower);

                if (user == null || follower == null)
                {
                    sb.AppendLine(ErrorMsg);
                    continue;
                }

                var userFollower = new UserFollower
                {
                    User = user,
                    Follower = follower
                };

                var alreadyFollowers = validFollowers
                    .Where(vf => vf.User == user)
                    .ToArray();
                if (alreadyFollowers.Any(af => af.Follower == follower))
                {
                    sb.AppendLine(ErrorMsg);
                    continue;
                }
                
                validFollowers.Add(userFollower);
                sb.AppendLine(string.Format(SuccessUserFollwerMsg, follower.Username, user.Username));
            }

            context.UsersFollowers.AddRange(validFollowers);
            context.SaveChanges();

            var result = sb.ToString().TrimEnd();
            return result;
        }

        public static string ImportPosts(InstagraphContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(PostDto[]), new XmlRootAttribute("posts"));
            var deserializedPosts = (PostDto[])serializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(xmlString)));

            var sb = new StringBuilder();
            var validPosts = new List<Post>();

            foreach (var postDto in deserializedPosts)
            {
                if (!IsValid(postDto))
                {
                    sb.AppendLine(ErrorMsg);
                    continue;
                }

                var user = context.Users.FirstOrDefault(u => u.Username == postDto.User);
                var picture = context.Pictures.FirstOrDefault(p => p.Path == postDto.PicPath);

                if (user == null || picture == null)
                {
                    sb.AppendLine(ErrorMsg);
                    continue;
                }

                var post = new Post
                {
                    Caption = postDto.Caption,
                    User = user,
                    Picture = picture
                };
                
                validPosts.Add(post);
                sb.AppendLine(string.Format(SuccessMsg, "Post", post.Caption));
            }

            context.Posts.AddRange(validPosts);
            context.SaveChanges();

            var result = sb.ToString();
            return result;
        }

        public static string ImportComments(InstagraphContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(CommentDto[]), new XmlRootAttribute("comments"));
            var deserializedComments = (CommentDto[])serializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(xmlString)));

            var sb = new StringBuilder();
            var validComments = new List<Comment>();

            foreach (var commentDto in deserializedComments)
            {
                if (!IsValid(commentDto))
                {
                    sb.AppendLine(ErrorMsg);
                    continue;
                }

                var user = context.Users.FirstOrDefault(u => u.Username == commentDto.User);
                var post = context.Posts.FirstOrDefault(p => p.Id == commentDto.Post.Id);

                if (user == null || post == null)
                {
                    sb.AppendLine(ErrorMsg);
                    continue;
                }

                var comment = new Comment
                {
                    User = user,
                    Post = post,
                    Content = commentDto.Content
                };

                validComments.Add(comment);
                sb.AppendLine(string.Format(SuccessMsg, "Comment", comment.Content));
            }

            context.Comments.AddRange(validComments);
            context.SaveChanges();

            var result = sb.ToString();
            return result;
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);

            return isValid;
        }
    }
}
