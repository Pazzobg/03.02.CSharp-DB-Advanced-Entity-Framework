using System;
using System.Collections.Generic;
using System.Linq;

public class StartUp
{
    public static void Main()
    {
        var people = new List<Person>();
        var products = new List<Product>();

        string[] allPeople = CustomSplit(Console.ReadLine(), ';');
        string[] allProducts = CustomSplit(Console.ReadLine(), ';');

        for (int i = 0; i < allPeople.Length; i++)
        {
            try
            {
                var currentPersonArgs = allPeople[i].Split('=');
                string currentName = currentPersonArgs[0];
                decimal currentMoney = decimal.Parse(currentPersonArgs[1]);

                var person = new Person(currentName, currentMoney);
                people.Add(person);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(0);
            }
        }

        for (int i = 0; i < allProducts.Length; i++)
        {
            var currentProductArgs = allProducts[i].Split('=');
            string currentProduct = currentProductArgs[0];
            decimal currentPrice = decimal.Parse(currentProductArgs[1]);

            try
            {
                var product = new Product(currentProduct, currentPrice);
                products.Add(product);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(0);
            }
        }

        string[] command = CustomSplit(Console.ReadLine(), ' ');

        while (command[0] != "END")
        {
            string currentName = command[0];
            var currentItem = command[1];
            Person buyer = people.FirstOrDefault(p => p.Name == currentName);
            Product item = products.FirstOrDefault(p => p.Name == currentItem);

            try
            {
                buyer.BuyProduct(item);
                Console.WriteLine($"{buyer.Name} bought {item.Name}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            command = CustomSplit(Console.ReadLine(), ' ');
        }

        foreach (var person in people)
        {
            Console.WriteLine(person);
        }
    }

    private static string[] CustomSplit(string input, char delimiter)
    {
        return input.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
    }
}