using System;
using System.Reflection;

public class StartUp
{
    public static void Main()
    {
        MethodInfo oldestMemberMethod = typeof(Family).GetMethod("GetOldestMember");
        MethodInfo addMemberMethod = typeof(Family).GetMethod("AddMember");
        if (oldestMemberMethod == null || addMemberMethod == null)
        {
            throw new Exception();
        }

        int n = int.Parse(Console.ReadLine());
        var family = new Family();

        for (int i = 0; i < n; i++)
        {
            string[] input = Console.ReadLine().Split();
            string name = input[0];
            int age = int.Parse(input[1]);

            family.AddMember(new Person(name, age));
        }

        Console.WriteLine(family.GetOldestMember());
    }
}
