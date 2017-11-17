namespace P03_FootballBetting
{
    using System;
    using P03_FootballBetting.Data;

    public class StartUp
    {
        public static void Main()
        {
            using (var context = new FootballBettingContext())
            {
                ResetDatabase(context);

                //Seed(context);






            }
        }

        private static void ResetDatabase(FootballBettingContext context)
        {
            context.Database.EnsureDeleted();

            context.Database.EnsureCreated();
        }

        private static void Seed(FootballBettingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
