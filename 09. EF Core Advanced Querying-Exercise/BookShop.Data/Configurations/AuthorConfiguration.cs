namespace BookShop.Data.Configurations
{
    using BookShop.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            /*•	Author:
                o	AuthorId
                o	FirstName (up to 50 characters, unicode, not required)
                o	LastName (up to 50 characters, unicode)
                */
            builder.HasKey(a => a.AuthorId);    

            builder.Property(a => a.FirstName).IsUnicode().HasMaxLength(50).IsRequired(false);
            builder.Property(a => a.LastName).IsUnicode().HasMaxLength(50).IsRequired();

            builder
                .HasMany(a => a.Books)
                .WithOne(b => b.Author)
                .HasForeignKey(b => b.AuthorId);
        }
    }
}
