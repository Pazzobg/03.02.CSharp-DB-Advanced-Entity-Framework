using System;
using System.Collections.Generic;
using System.Linq;

public class StartUp
{
    public static void Main()
    {
        var allCars = new Dictionary<string, Car>();

        int n = int.Parse(Console.ReadLine());

        for (int i = 0; i < n; i++)
        {
            string[] input = Console.ReadLine().Split(' ');
            string model = input[0];
            double fuel = double.Parse(input[1]);
            double consumption = double.Parse(input[2]);


            var currentCar = new Car(model, fuel, consumption);

            string carNameId = currentCar.Model;

            if (!allCars.ContainsKey(carNameId))
            {
                allCars.Add(carNameId, currentCar);
            }
        }

        string[] commands = Console.ReadLine().Split(' ');

        while (commands[0] != "End") 
        {
            string carToDriveId = commands[1];
            double wayToGo = double.Parse(commands[2]);

            var tempArr = allCars.Values.Where(c => c.Model == carToDriveId).ToArray();
            var currentCar = tempArr[0];

            bool isDrivePossible = currentCar.DriveCar(wayToGo);

            if (!isDrivePossible)
            {
                Console.WriteLine("Insufficient fuel for the drive");
            }
            
            commands = Console.ReadLine().Split(' ');
        }

        foreach (var car in allCars)
        {
            Console.WriteLine($"{car.Value.Model} {car.Value.FuelAmount:f2} {car.Value.DistanceTravelled}");
        }
    }
}