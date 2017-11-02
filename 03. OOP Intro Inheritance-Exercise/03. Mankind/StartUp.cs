using System;

public class StartUp
{
    public static void Main()
    {
        try
        {
            string[] studentParams = Console.ReadLine().Split(' ');
            string[] workerParams = Console.ReadLine().Split(' ');

            var student = new Student(studentParams[0], studentParams[1], studentParams[2]);
            var worker = new Worker(
                workerParams[0],
                workerParams[1],
                decimal.Parse(workerParams[2]),
                double.Parse(workerParams[3]));

            Console.WriteLine(student + Environment.NewLine);
            Console.WriteLine(worker);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
