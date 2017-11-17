namespace P03_FootballBetting.Data.Models
{
    using System.Collections.Generic;

    public class Color
    {
        public Color()
        {

        }

        public Color(string name)
        {
            this.Name = name;
        }

        public int ColorId { get; set; }

        public string Name { get; set; }

        public ICollection<Team> PrimaryKitTeams { get; set; } = new List<Team>();

        public ICollection<Team> SecondaryKitTeams { get; set; } = new List<Team>();
    }
}
