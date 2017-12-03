namespace P01_ProductsShop.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using P01_ProductsShop.Models;

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.LastName).IsRequired();
            builder.Property(u => u.Age).IsRequired(false);

            builder
                .HasMany(u => u.ProductsSold)
                .WithOne(ps => ps.Seller)
                .HasForeignKey(ps => ps.SellerId);

            builder
                .HasMany(u => u.ProductsBought)
                .WithOne(pb => pb.Buyer)
                .HasForeignKey(pb => pb.BuyerId);

            //builder.Ignore(u => u.CommonFriendId);         

            //builder
            //    .HasMany(u => u.Friends)
            //    .WithOne(f => f.CommonFriend)
            //    .HasForeignKey(f => f.CommonFriendId);    
        }
    }
}
