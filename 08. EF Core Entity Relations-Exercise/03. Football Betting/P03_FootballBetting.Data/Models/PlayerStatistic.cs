namespace P03_FootballBetting.Data.Models
{
    public class PlayerStatistic
    {
        public PlayerStatistic()
        {

        }

        public PlayerStatistic(Player player, Game game, int minutes, int goals)
        {
            this.Player = player;
            this.Game = game;
            this.MinutesPlayed = minutes;
            this.ScoredGoals = goals;
        }
        public int PlayerId { get; set; }
        public Player Player { get; set; }

        public int GameId { get; set; }
        public Game Game { get; set; }

        public int Assists { get; set; }

        public int MinutesPlayed { get; set; }

        public int ScoredGoals { get; set; }
    }
}
