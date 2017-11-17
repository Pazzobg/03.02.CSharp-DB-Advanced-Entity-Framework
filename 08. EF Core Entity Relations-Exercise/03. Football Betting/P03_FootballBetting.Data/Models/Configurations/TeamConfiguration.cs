namespace P03_FootballBetting.Data.Models.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.Property(t => t.Name).IsRequired(true);
            builder.Property(t => t.Initials).IsRequired(true);
            
            builder
                .HasOne(t => t.PrimaryKitColor)
                .WithMany(c => c.PrimaryKitTeams)
                .HasForeignKey(t => t.PrimaryKitColorId);

            builder
                .HasOne(t => t.SecondaryKitColor)
                .WithMany(c => c.SecondaryKitTeams)
                .HasForeignKey(t => t.SecondaryKitColorId)
                .OnDelete(DeleteBehavior.Restrict);

            //builder.Entity<Team>()
            //    .HasOne(t => t.Town)
            //    .WithMany(tw => tw.Teams)
            //    .HasForeignKey(t => t.TownId);
        }
    }
}
