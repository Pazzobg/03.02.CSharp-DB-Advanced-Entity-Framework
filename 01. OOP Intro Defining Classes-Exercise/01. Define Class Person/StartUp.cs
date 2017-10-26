using System;
using System.Reflection;

public class StartUp
{
    public static void Main()
    {
        Type personType = typeof(Person);
        PropertyInfo[] properties = personType.GetProperties
            (BindingFlags.Public | BindingFlags.Instance);
        Console.WriteLine(properties.Length);

        //var first = new Person
        //{
        //    Name = "Ivan",
        //    Age = 20
        //};

        //Console.WriteLine(first.Name + " " + first.Age);
    }
}