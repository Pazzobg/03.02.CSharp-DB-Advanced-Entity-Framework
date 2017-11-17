namespace P03_FootballBetting.Data.Models.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    class PlayerStatisticConfiguration : IEntityTypeConfiguration<PlayerStatistic>
    {
        public void Configure(EntityTypeBuilder<PlayerStatistic> builder)
        {
            builder
                .ToTable("PlayerStatistics")
                .HasKey(ps => new { ps.PlayerId, ps.GameId });

            builder
                .HasOne(ps => ps.Player)
                .WithMany(pl => pl.PlayerStatistics)
                .HasForeignKey(ps => ps.PlayerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(ps => ps.Game)
                .WithMany(g => g.PlayerStatistics)
                .HasForeignKey(ps => ps.GameId);
        }
    }
}
