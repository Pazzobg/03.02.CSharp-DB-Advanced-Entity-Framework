using System;
using System.Collections.Generic;
using System.Linq;

public class OpinionPoll
{
    public static void Main()
    {
        var names = new List<Person>();

        int n = int.Parse(Console.ReadLine());

        for (int i = 0; i < n; i++)
        {
            string[] input = Console.ReadLine().Split(' ');

            var currentPerson = new Person
            {
                Name = input[0],
                Age = int.Parse(input[1])
            };

            if (currentPerson.Age > 30)
            {
                names.Add(currentPerson);
            }
        }

        foreach (var person in names.OrderBy(x => x.Name))
        {
            Console.WriteLine($"{person.Name} - {person.Age}");
        }
    }
}