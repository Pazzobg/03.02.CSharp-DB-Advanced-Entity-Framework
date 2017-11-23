namespace BookShop.Data.Configurations
{
    using BookShop.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            /*•	Book:
                o	BookId
                o	Title (up to 50 characters, unicode)
                o	Description (up to 1000 characters, unicode)
                o	ReleaseDate (not required)
                o	Copies (an integer)
                o	Price
                o	EditionType – enum (Normal, Promo, Gold)
                o	AgeRestriction – enum (Minor, Teen, Adult)
                o	Author
                o	BookCategories*/

            builder.HasKey(b => b.BookId);

            builder.Property(b => b.Title).IsUnicode().HasMaxLength(50).IsRequired();
            builder.Property(b => b.Description).IsUnicode().HasMaxLength(1000).IsRequired();
            builder.Property(b => b.ReleaseDate).IsRequired(false);
        }
    }
}
