namespace P03_FootballBetting.Data.Models.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    class TownConfiguration : IEntityTypeConfiguration<Town>
    {
        public void Configure(EntityTypeBuilder<Town> builder)
        {
            builder
                .Property(t => t.Name).IsRequired(true);

            builder
               .HasMany(t => t.Teams)
               .WithOne(tm => tm.Town)
               .HasForeignKey(tm => tm.TownId);

            builder
                .HasOne(t => t.Country)
                .WithMany(c => c.Towns)
                .HasForeignKey(t => t.CountryId);
        }
    }
}
