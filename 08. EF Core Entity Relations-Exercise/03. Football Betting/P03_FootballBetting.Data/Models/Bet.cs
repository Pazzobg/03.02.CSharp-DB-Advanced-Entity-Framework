namespace P03_FootballBetting.Data.Models
{
    using System;

    public class Bet
    {
        public Bet()
        {

        }

        public Bet(decimal amount, string prediction, User user, Game game)
        {
            this.Amount = amount;
            this.Prediction = prediction;
            this.User = user;
            this.Game = game;
        }

        public int BetId { get; set; }

        public decimal Amount { get; set; }

        public string Prediction { get; set; }  //string / int ?!?

        public DateTime DateTime { get; set; } // isRequired(fallse)

        public int UserId { get; set; }
        public User User { get; set; }

        public int GameId { get; set; }
        public Game Game { get; set; }
    }
}
