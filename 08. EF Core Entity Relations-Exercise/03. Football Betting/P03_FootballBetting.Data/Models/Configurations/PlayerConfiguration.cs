namespace P03_FootballBetting.Data.Models.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.Property(p => p.PlayerId).IsRequired(true);
            builder.Property(p => p.Name).IsRequired(true);

            builder
               .HasOne(p => p.Team)
               .WithMany(t => t.Players)
               .HasForeignKey(p => p.TeamId);

            builder
                .HasOne(p => p.Position)
                .WithMany(pos => pos.Players)
                .HasForeignKey(p => p.PositionId);
        }
    }
}
