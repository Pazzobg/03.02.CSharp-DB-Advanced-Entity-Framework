namespace _06.Remove_Villain
{
    using System;
    using System.Data.SqlClient;

    public class Program
    {
        public static void Main()
        {
            int villainId = int.Parse(Console.ReadLine());

            var connection = new SqlConnection("Server=.;Database=MinionsDB;Integrated Security=True");
            connection.Open();

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
                    Console.WriteLine("No such villain was found.");
                    return;
                }

                cmdText = "DELETE FROM MinionsVillains WHERE VillainId = @villainId";
                command = new SqlCommand(cmdText, connection);
                command.Parameters.AddWithValue("@VillainId", villainId);
                int freedMinions = command.ExecuteNonQuery();

                cmdText = "DELETE FROM Villains WHERE Id = @villainId";
                command = new SqlCommand(cmdText, connection);
                command.Parameters.AddWithValue("@VillainId", villainId);
                command.ExecuteNonQuery();

                Console.WriteLine($"{(string)result} was deleted.");
                Console.WriteLine($"{freedMinions} minions were released.");
            }
        }
    }
}
