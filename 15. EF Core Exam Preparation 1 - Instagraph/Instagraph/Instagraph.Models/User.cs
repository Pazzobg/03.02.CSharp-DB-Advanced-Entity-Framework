namespace Instagraph.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Username { get; set; }           

        [Required]
        [MaxLength(20)]
        public string Password { get; set; }

        public int ProfilePictureId { get; set; }
        [Required]
        public Picture ProfilePicture { get; set; }

        public ICollection<UserFollower> Followers { get; set; } = new List<UserFollower>();

        public ICollection<UserFollower> UsersFollowing { get; set; } = new List<UserFollower>();

        public ICollection<Post> Posts { get; set; } = new List<Post>();

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
