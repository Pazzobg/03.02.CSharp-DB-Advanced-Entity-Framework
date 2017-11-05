namespace _03.Minion_Names
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;

    public class StartUp
    {
        public static void Main()
        {
            var connection = new SqlConnection("Server=.;Database=MinionsDB;Integrated Security=True");

            connection.Open();

            int villainId = int.Parse(Console.ReadLine());
            var minionsDict = new Dictionary<string, int>();
            int counter = 1;

            using (connection)
            {
                // Get Villain's Name:
                string cmdText = "SELECT Name FROM Villains " +
                                  "WHERE Id = @villainId";
                var command = new SqlCommand(cmdText, connection);
                command.Parameters.AddWithValue("@villainId", villainId);

                var result = command.ExecuteScalar();

                if (string.IsNullOrWhiteSpace((string)result))
                {
                    Console.WriteLine($"No villain with ID {villainId} exists in the database.");
                    return;
                }

                Console.WriteLine($"Villain: {(string)result}");

                // Get Minions list: 
                cmdText = "SELECT v.Name [Villain], m.Name [Minion], m.Age [Age] " +
                            "FROM Minions m " +
                            "JOIN MinionsVillains mv ON mv.MinionId = m.Id " +
                            "JOIN Villains v ON v.Id = mv.VillainId " +
                           "WHERE mv.VillainId = @villainId " +
                           "GROUP BY v.Name, m.Name, m.Age";

                command = new SqlCommand(cmdText, connection);
                command.Parameters.AddWithValue("@villainId", villainId);

                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {


                    while (reader.Read())
                    {
                        string name = (string)reader["Minion"];
                        int minionAge = (int)reader["Age"];

                        if (!minionsDict.ContainsKey(name))
                        {
                            minionsDict[name] = 0;
                        }

                        minionsDict[name] = minionAge;
                    }

                    foreach (var minion in minionsDict.OrderBy(m => m.Key))
                    {
                        string name = minion.Key;
                        int age = minion.Value;
                        Console.WriteLine($"{counter}. {name} - {age}");
                        counter++;
                    }
                }
                else
                {
                    Console.WriteLine("(no minions)");
                }
            }
        }
    }
}
