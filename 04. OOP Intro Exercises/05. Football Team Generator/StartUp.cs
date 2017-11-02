using System;
using System.Collections.Generic;

public class StartUp
{
    public static void Main()
    {
        var league = new Dictionary<string, Team>();

        string[] input = Console.ReadLine().Split(';');

        while (input[0] != "END")
        {
            try
            {
                string command = input[0];
                string teamName = input[1];

                if (!league.ContainsKey(teamName) && command != "Team")
                {
                    throw new ArgumentException($"Team { teamName } does not exist.");
                }

                switch (command)
                {
                    case "Team":
                        var team = new Team(teamName);

                        league.Add(team.Name, team);
                        break;

                    case "Add":
                        string playerName = input[2];
                        int endurance = int.Parse(input[3]);
                        int sprint = int.Parse(input[4]);
                        int dribble = int.Parse(input[5]);
                        int passing = int.Parse(input[6]);
                        int shooting = int.Parse(input[7]);

                        var player = new Player(playerName, endurance, sprint, dribble, passing, shooting);

                        league[teamName].AddPlayer(player);

                        break;

                    case "Remove":
                        string removeName = input[2];
                        league[teamName].RemovePlayer(removeName);
                        break;

                    case "Rating":
                        Console.WriteLine(league[teamName]);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            input = Console.ReadLine().Split(';');
        }
    }
}