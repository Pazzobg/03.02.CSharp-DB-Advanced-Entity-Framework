namespace _08.Increase_Minion_Age
{
    using System;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading;

    public class Program
    {
        public static void Main()
        {
            int[] ids = Console.ReadLine().Split().Select(int.Parse).ToArray();

            var connection = new SqlConnection("Server=.;Database=MinionsDB;Integrated Security=True");
            connection.Open();

            using (connection)
            {
                for (int i = 0; i < ids.Length; i++)
                {
                    string cmdText = $"SELECT * FROM Minions WHERE Id = @id";
                    var command = new SqlCommand(cmdText, connection);
                    command.Parameters.AddWithValue("@id", ids[i]);
                    var reader = command.ExecuteReader();
                    reader.Read();
                    string name = Convert.ToString(reader["Name"]);
                    reader.Close();

                    var cultureInfo = Thread.CurrentThread.CurrentCulture;
                    var textInfo = cultureInfo.TextInfo;
                    string convertedName = textInfo.ToTitleCase(name);

                    var updateCmd = $"UPDATE Minions SET Name = '{convertedName}', Age += 1 WHERE Id = {ids[i]}";
                    var updateCommand = new SqlCommand(updateCmd, connection);
                    updateCommand.ExecuteNonQuery();
                }

                string printQuery = "SELECT Name, Age FROM Minions";
                var printCommand = new SqlCommand(printQuery, connection);
                var printer = printCommand.ExecuteReader();
                while (printer.Read())
                {
                    string minionName = (string)printer["Name"];
                    int age = (int)printer["Age"];

                    Console.WriteLine($"{minionName} {age}");
                }
            }
        }
    }
}
