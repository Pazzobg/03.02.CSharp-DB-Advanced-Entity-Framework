namespace _07.Print_All_Minion_Names
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;

    public class StartUp
    {
        public static void Main()
        {
            var minions = new List<string>();

            var connection = new SqlConnection("Server=.;Database=MinionsDB;Integrated Security=True");
            connection.Open();

            using (connection)
            {
                string cmdText = "SELECT Name FROM Minions";
                var command = new SqlCommand(cmdText, connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string name = (string)reader["Name"];
                    minions.Add(name);
                }

                reader.Close();

                int count = minions.Count;
                int loopEnd = count / 2;

                for (int i = 0; i < loopEnd; i++)
                {
                    Console.WriteLine(minions[i]);
                    Console.WriteLine(minions[count - 1 - i]);
                }

                if (count % 2 == 1)
                {
                    Console.WriteLine(minions[count / 2]);
                }
            }
        }
    }
}
