using System;

public class StartUp
{
    public static void Main()
    {
        string date1 = Console.ReadLine();
        string date2 = Console.ReadLine();

        var dateMod = new DateModifier(date1, date2);

        Console.WriteLine(dateMod.CalculateDifferenceBetweenDates());
    }
}
