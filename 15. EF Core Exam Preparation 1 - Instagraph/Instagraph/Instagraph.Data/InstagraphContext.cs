namespace Instagraph.Data
{
    using Instagraph.Models;
    using Microsoft.EntityFrameworkCore;

    public class InstagraphContext : DbContext
    {
        public InstagraphContext()
        {
        }

        public InstagraphContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Picture> Pictures { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<UserFollower> UsersFollowers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().HasAlternateKey(u => u.Username);

            builder.Entity<UserFollower>(e =>
            {
                e.HasKey(uf => new { uf.UserId, uf.FollowerId });

                e.HasOne(uf => uf.User)
                    .WithMany(u => u.Followers)
                    .HasForeignKey(uf => uf.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(uf => uf.Follower)
                    .WithMany(f => f.UsersFollowing)
                    .HasForeignKey(uf => uf.FollowerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Post>()
                .HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
