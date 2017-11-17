namespace P03_FootballBetting.Data.Models
{
    using System.Collections.Generic;

    public class Town
    {
        public Town()
        {

        }

        public Town(string name, Country country)
        {
            this.Name = name;
            this.Country = country;
        }

        public int TownId { get; set; }

        public string Name { get; set; }

        public int CountryId { get; set; }
        public Country Country { get; set; }

        public ICollection<Team> Teams { get; set; } = new List<Team>();    // ignore?
        // Town shto nqma ICollection ot Teams?!?
    }
}
