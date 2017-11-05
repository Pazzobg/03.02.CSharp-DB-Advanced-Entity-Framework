namespace _05.Change_Town_Name_Casing
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;

    public class Program
    {
        public static void Main()
        {
            var connection = new SqlConnection("Server=.;Database=MinionsDB;Integrated Security=True");

            connection.Open();

            string countryName = Console.ReadLine();
            int countryId = 0;
            var townsInCountry = new List<string>();

            using (connection)
            {
                string cmdText = "SELECT Id FROM Countries WHERE Name = @countryName";
                var command = new SqlCommand(cmdText, connection);
                command.Parameters.AddWithValue("@countryName", countryName);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    countryId = (int)reader["Id"];
                }

                reader.Close();

                cmdText = $"UPDATE Towns SET Name = UPPER(Name) WHERE CountryId = @countryId";
                command = new SqlCommand(cmdText, connection);
                command.Parameters.AddWithValue("@countryId", countryId);
                int rowsAffected = command.ExecuteNonQuery();

                cmdText = "SELECT Name FROM Towns WHERE CountryId = @countryId";
                command = new SqlCommand(cmdText, connection);
                command.Parameters.AddWithValue("@countryId", countryId);
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string town = (string)reader["Name"];
                    townsInCountry.Add(town);
                }

                reader.Close();

                if (townsInCountry.Count > 0)
                {
                    Console.WriteLine($"{rowsAffected} town names were affected.");
                    Console.WriteLine($"[{String.Join(", ", townsInCountry)}]");
                }
                else
                {
                    Console.WriteLine("No town names were affected.");
                }
            }
        }
    }
}
