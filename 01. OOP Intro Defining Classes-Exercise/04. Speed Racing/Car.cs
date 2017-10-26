using System;

public class Car
{
    public string Model { get; set; }
    public double FuelAmount { get; set; }
    public double ConsumptionPerKm { get; set; }
    public double DistanceTravelled { get; set; }

    public Car(string model, double fuel, double consumption)
    {
        this.Model = model;
        this.FuelAmount = fuel;
        this.ConsumptionPerKm = consumption;
        this.DistanceTravelled = 0;
    }

    public bool DriveCar(double distanceToGo) 
    {
        var totalFuelNeeded = distanceToGo * this.ConsumptionPerKm;

        if (totalFuelNeeded > FuelAmount)
        {
            return false;
        }

        this.FuelAmount -= totalFuelNeeded;
        this.DistanceTravelled += distanceToGo;

        return true;
    }

    //public void DriveCar(double distanceToGo) //static?!?!?!?
    //{
    //    var totalFuelNeeded = distanceToGo * this.ConsumptionPerKm;

    //    if (totalFuelNeeded > FuelAmount)
    //    {
    //        Console.WriteLine("Insufficient fuel for the drive");
    //    }
    //    else
    //    {
    //        this.FuelAmount -= totalFuelNeeded;
    //        this.DistanceTravelled += distanceToGo;
    //    }
    //}
}