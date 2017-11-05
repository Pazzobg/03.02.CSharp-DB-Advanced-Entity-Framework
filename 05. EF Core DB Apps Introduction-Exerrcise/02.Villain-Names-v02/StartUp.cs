using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace _02.Villain_Names_v02
{
    class StartUp
    {
        static void Main(string[] args)
        {
            var connection = new SqlConnection("Server=.;Database=MinionsDB;Integrated Security=True");

            connection.Open();

            var villainsList = new List<Villain>();

            using (connection)
            {
                string cmdText = "SELECT v.Name, COUNT(mv.MinionId) [Minions Count] " +
                                   "FROM Villains v " +
                                   "JOIN MinionsVillains mv ON mv.VillainId = v.Id " +
                                  "GROUP BY v.Name ";

                var command = new SqlCommand(cmdText, connection);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string villainName = (string)reader["Name"];
                    int minionsCount = (int)reader["Minions Count"];
                    
                    villainsList.Add(new Villain(villainName, minionsCount));
                }

                foreach (var villain in villainsList
                    .Where(v => v.MinionsCount > 3)
                    .OrderByDescending(v => v.MinionsCount))
                {
                    Console.WriteLine(villain);
                }
            }
        }
    }
}
