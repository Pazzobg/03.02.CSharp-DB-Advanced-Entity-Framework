using System;

public class StartUp
{
    public static void Main()
    {
        try
        {
            string[] inputPizza = Console.ReadLine().Split(' ');
            var pizza = new Pizza(inputPizza[1]);

            string[] inputDough = Console.ReadLine().Split(' ');
            var dough = new Dough(inputDough[1], inputDough[2], double.Parse(inputDough[3]));
            pizza.Dough = dough;

            string[] inputToppings = Console.ReadLine().Split(' ');
            while (inputToppings[0] != "END")
            {
                var currentTopping = new Topping(inputToppings[1], double.Parse(inputToppings[2]));

                pizza.AddTopping(currentTopping);

                inputToppings = Console.ReadLine().Split(' ');
            }

            Console.WriteLine($"{pizza.Name} - {pizza.CalculateTotalCalories():f2} Calories.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        /* Testing Steps 1 - 4

        string[] input = Console.ReadLine().Split(' ');

        while (input[0] != "END")
        {
            switch (input[0])
            {
                case "Dough":
                    try
                    {
                        var somedough = new Dough(input[1], input[2], double.Parse(input[3]));
                        Console.WriteLine($"{somedough.CaloriesPerGram:f2}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;

                case "Topping":
                    try
                    {
                        var someTopping = new Topping(input[1], double.Parse(input[2]));
                        Console.WriteLine($"{someTopping.CaloriesPerGram:f2}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;
            }

            input = Console.ReadLine().Split(' ');
        } */
    }
}