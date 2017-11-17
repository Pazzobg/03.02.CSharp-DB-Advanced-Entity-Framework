namespace P03_FootballBetting.Data
{
    using Microsoft.EntityFrameworkCore;
    using P03_FootballBetting.Data.Models;
    using P03_FootballBetting.Data.Models.Configurations;

    public class FootballBettingContext : DbContext
    {
        public FootballBettingContext()
        {

        }

        public FootballBettingContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<Bet> Bets { get; set; }

        public DbSet<Color> Colors { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<PlayerStatistic> PlayerStatistics { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Town> Towns { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);

            if (!builder.IsConfigured)
            {
                builder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Country>(en =>
            {
                en.Property(c => c.Name).IsRequired(true);
            });

            builder.Entity<Color>(en =>
            {
                en.Property(c => c.Name).IsRequired(true);
            });

            builder.Entity<Position>(en =>
            {
                en.Property(p => p.Name).IsRequired(true);
            });

            builder.Entity<User>(en =>
            {
                en.Property(u => u.Username).IsRequired(true);
                en.Property(u => u.Name).IsRequired(true);
                en.Property(u => u.Password).IsRequired(true);
            });

            // Team config 
            builder.ApplyConfiguration(new TeamConfiguration());

            // Town config 
            builder.ApplyConfiguration(new TownConfiguration());

            // Playa Config
            builder.ApplyConfiguration(new PlayerConfiguration());

            // PlayerStats
            builder.ApplyConfiguration(new PlayerStatisticConfiguration());

            // Game Config
            builder.ApplyConfiguration(new GameConfiguration());

            // Bet Config
            builder.ApplyConfiguration(new BetConfiguration());
        }
    }
}
