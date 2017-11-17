namespace P03_FootballBetting.Data.Models
{
    using System.Collections.Generic;

    public class Position
    {
        public Position()
        {

        }

        public Position(string name)
        {
            this.Name = name;
        }

        public int PositionId { get; set; }

        public string Name { get; set; }

        public ICollection<Player> Players { get; set; } = new List<Player>();
    }
}
