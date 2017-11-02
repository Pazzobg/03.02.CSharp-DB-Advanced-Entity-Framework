using System;
using System.Collections.Generic;
using System.Linq;

public class StartUp
{
    public static void Main()
    {
        var carPark = new List<Car>();

        int loopEnd = int.Parse(Console.ReadLine());

        for (int i = 0; i < loopEnd; i++)
        {
            string[] input = Console.ReadLine().Split(' ');

            string model = input[0];
            int engineSpeed = int.Parse(input[1]);
            int enginePower = int.Parse(input[2]);
            int cargoWeight = int.Parse(input[3]);
            string cargoType = input[4];
            double tyre1Pressure = double.Parse(input[5]);
            int tyre1Age = int.Parse(input[6]);
            double tyre2Pressure = double.Parse(input[7]);
            int tyre2Age = int.Parse(input[8]);
            double tyre3Pressure = double.Parse(input[9]);
            int tyre3Age = int.Parse(input[10]);
            double tyre4Pressure = double.Parse(input[11]);
            int tyre4Age = int.Parse(input[12]);

            var currentCar = new Car(
                model, engineSpeed, enginePower, cargoWeight, cargoType,
                tyre1Pressure, tyre1Age, tyre2Pressure, tyre2Age,
                tyre3Pressure, tyre3Age, tyre4Pressure, tyre4Age);

            carPark.Add(currentCar);
        }

        switch (Console.ReadLine().ToLower())
        {
            case "fragile":
                foreach (var car in carPark
                    .Where(c => c.CarCargo.CargoType == "fragile")
                    .Where(c => c.CarTyres.Any(t => t.TyrePressure < 1)))
                {
                    Console.WriteLine(car.CarModel);
                }

                break;

            case "flammable":
                foreach (var car in carPark
                    .Where(c => c.CarCargo.CargoType == "flammable")
                    .Where(e => e.CarEngine.EnginePower > 250))
                {
                    Console.WriteLine(car.CarModel);
                }

                break;
        }
    }
}
