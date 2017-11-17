namespace P03_FootballBetting.Data.Models
{
    using System.Collections.Generic;

    public class Team
    {
        public Team()
        {

        }

        public Team(string name, string logoUrl, string initials, decimal budget, Color primaryJersey, Color secondaryJersey, Town town)
        {
            this.Name = name;
            this.LogoUrl = logoUrl;
            this.Initials = initials;
            this.Budget = budget;
            this.PrimaryKitColor = primaryJersey;
            this.SecondaryKitColor = secondaryJersey;
            this.Town = town;
        }

        public int TeamId { get; set; }

        public string Name { get; set; }

        public string LogoUrl { get; set; }

        public string Initials { get; set; } // CHAR(3)???

        public decimal Budget { get; set; }

        public int PrimaryKitColorId { get; set; }
        public Color PrimaryKitColor { get; set; }

        public int SecondaryKitColorId { get; set; }
        public Color SecondaryKitColor { get; set; }

        public int TownId { get; set; }     // v Town ICollection ot Teams?!?
        public Town Town { get; set; }

        public ICollection<Player> Players { get; set; } = new List<Player>();

        public ICollection<Game> HomeGames { get; set; } = new List<Game>();

        public ICollection<Game> AwayGames { get; set; } = new List<Game>();
    }
}
