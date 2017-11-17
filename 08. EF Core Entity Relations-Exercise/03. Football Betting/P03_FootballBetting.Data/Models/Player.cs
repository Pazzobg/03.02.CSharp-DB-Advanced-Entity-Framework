namespace P03_FootballBetting.Data.Models
{
    using System.Collections.Generic;

    public class Player
    {
        public Player()
        {

        }

        public Player(string name, int squadNumber, bool isInjured, Position position, Team team)
        {
            this.Name = name;
            this.SquadNumber = squadNumber;
            this.IsInjured = IsInjured;
            this.Position = position;
            this.Team = team;
        }

        public int PlayerId { get; set; }

        public string Name { get; set; }

        public bool IsInjured { get; set; }

        public int SquadNumber { get; set; }

        public int PositionId { get; set; }
        public Position Position { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }

        public ICollection<PlayerStatistic> PlayerStatistics { get; set; } = new HashSet<PlayerStatistic>();
    }
}
