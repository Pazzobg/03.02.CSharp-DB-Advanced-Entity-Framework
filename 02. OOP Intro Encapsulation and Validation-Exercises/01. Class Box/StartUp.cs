using System;
using System.Linq;
using System.Reflection;

public class StartUp
{
    public static void Main()
    {
        Type boxType = typeof(Box);
        FieldInfo[] fields = boxType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
        Console.WriteLine(fields.Count());

        double length = double.Parse(Console.ReadLine());
        double width = double.Parse(Console.ReadLine());
        double height = double.Parse(Console.ReadLine());

        var box = new Box(length, width, height);

        Console.WriteLine($"Surface Area - {box.GetSurfaceArea():f2}");
        Console.WriteLine($"Lateral Surface Area - {box.GetLateralSurfaceArea():f2}");
        Console.WriteLine($"Volume - {box.GetVolume():f2}");
    }
}