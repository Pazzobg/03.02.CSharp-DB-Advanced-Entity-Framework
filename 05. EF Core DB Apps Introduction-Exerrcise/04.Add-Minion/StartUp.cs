namespace _4.Add_Minion
{
    using System;
    using System.Data.SqlClient;

    public class StartUp
    {
        public static void Main()
        {
            var minion = Console.ReadLine().Split();
            var name = minion[1];
            var age = int.Parse(minion[2]);
            var town = minion[3];
            var vilian = Console.ReadLine().Split();
            var villainName = vilian[1];

            var connection = new SqlConnection("Server=.;Database=MinionsDB;Integrated Security=True");

            connection.Open();
            using (connection)
            {
                int townId = 0;
                int villainId = 0;

                try
                {
                    townId = FindTownId(connection, town);
                }
                catch (Exception)
                {
                    townId = 0;
                }

                try
                {
                    villainId = FindVillainId(connection, villainName);
                }
                catch (Exception)
                {
                    villainId = 0;
                }

                if (villainId != 0 && townId != 0)
                {
                    InsertIntoMinions(connection, name, age, townId, villainName);
                    var minionId = FindMinionId(connection, name);
                    InsertIntoMinionsVillains(connection, minionId, villainId);
                }
                else if (villainId == 0 && townId != 0)
                {
                    InsertIntoVillains(connection, villainName);
                    InsertIntoMinions(connection, name, age, townId, villainName);
                    var minionId = FindMinionId(connection, name);
                    InsertIntoMinionsVillains(connection, minionId, FindVillainId(connection, villainName));
                }
                else if (villainId != 0 && townId == 0)
                {
                    InsertIntoTowns(connection, town);
                    var townIdd = FindTownId(connection, town);
                    InsertIntoMinions(connection, name, age, townIdd, villainName);
                    var minionId = FindMinionId(connection, name);
                    InsertIntoMinionsVillains(connection, minionId, villainId);
                }
                else
                {
                    InsertIntoTowns(connection, town);
                    InsertIntoVillains(connection, villainName);
                    var vilainId = FindVillainId(connection, villainName);
                    var townIdd = FindTownId(connection, town);
                    InsertIntoMinions(connection, name, age, townIdd, villainName);
                    var minionId = FindMinionId(connection, name);
                    InsertIntoMinionsVillains(connection, minionId, vilainId);
                }
            }
        }

        private static int FindTownId(SqlConnection connection, string town)
        {
            string cmdText = "SELECT Id FROM Towns WHERE Name = @name";
            var findTown = new SqlCommand(cmdText, connection);
            findTown.Parameters.AddWithValue("@name", town);

            return (int)findTown.ExecuteScalar();
        }

        private static int FindVillainId(SqlConnection connection, string villainName)
        {
            string cmdText = "SELECT Id FROM Villains WHERE Name=@vname";
            var findVillain = new SqlCommand(cmdText, connection);
            findVillain.Parameters.AddWithValue("@vname", villainName);

            return (int)findVillain.ExecuteScalar();
        }

        private static int FindMinionId(SqlConnection connection, string name)
        {
            string cmdText = $"SELECT Id FROM Minions WHERE Name = '{name}'";
            var minionIdfind = new SqlCommand(cmdText, connection);

            return (int)minionIdfind.ExecuteScalar();
        }

        private static void InsertIntoMinions(SqlConnection connection, string name, int age, int townId, string villainName)
        {
            string cmdText = $"INSERT INTO Minions (Name, Age, TownId) VALUES ('{name}', {age}, {townId})";
            var insertM = new SqlCommand(cmdText, connection);
            insertM.ExecuteNonQuery();

            Console.WriteLine($"Successfully added {name} to be minion of {villainName}.");
        }

        private static void InsertIntoMinionsVillains(SqlConnection connection, int minionId, int villainId)
        {
            string cmdText = $"INSERT INTO MinionsVillains VALUES ({minionId}, {villainId})";
            var insertMV = new SqlCommand(cmdText, connection);
            insertMV.ExecuteNonQuery();
        }

        private static void InsertIntoVillains(SqlConnection connection, string villainName)
        {
            string cmdText = $"INSERT INTO Villains VALUES ('{villainName}', 'evil')";
            var insertV = new SqlCommand(cmdText, connection);
            insertV.ExecuteNonQuery();

            Console.WriteLine($"Villain {villainName} was added to the database.");
        }

        private static void InsertIntoTowns(SqlConnection connection, string town)
        {
            string cmdText = $"INSERT INTO Towns VALUES ('{town}','NoName-NoCountry')";
            var insertT = new SqlCommand(cmdText, connection);
            insertT.ExecuteNonQuery();

            Console.WriteLine($"Town {town} was added to the database.");
        }
    }
}