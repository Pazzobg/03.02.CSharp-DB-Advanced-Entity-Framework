using System;
using System.Collections.Generic;

public class StartUp
{
    public static void Main()
    {
        var population = new List<Animal>();

        string animal = Console.ReadLine();

        while (animal != "Beast!")
        {
            string[] tokens = Console.ReadLine().Split();
            string name = tokens[0];
            int age = int.Parse(tokens[1]);
            string gender = tokens[2];

            try
            {
                switch (animal)
                {
                    case "Dog": population.Add(new Dog(name, age, gender)); break;
                    case "Cat": population.Add(new Cat(name, age, gender)); break;
                    case "Frog": population.Add(new Frog(name, age, gender)); break;
                    case "Kitten": population.Add(new Kitten(name, age, gender)); break;
                    case "Tomcat": population.Add(new Tomcat(name, age, gender)); break;
                    default: throw new ArgumentException("Invalid input!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            animal = Console.ReadLine();
        }

        foreach (var creature in population)
        {
            Console.WriteLine(creature);
        }
    }
}