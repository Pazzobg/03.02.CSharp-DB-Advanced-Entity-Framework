using System.Collections.Generic;

public class Car
{
    private string carModel;
    private Engine carEngine;
    private Cargo carCargo;
    private List<Tyre> carTyres;

    public Car(string model, int engineSpeed, int enginePower, int cargoWeight, string cargoType, 
        double tyre1Pressure, int tyre1Age, double tyre2Pressure, int tyre2Age, 
        double tyre3Pressure, int tyre3Age, double tyre4Pressure, int tyre4Age)
    {
        this.CarModel = model;

        this.CarEngine = new Engine(engineSpeed, enginePower);

        this.CarCargo = new Cargo(cargoWeight, cargoType);

        this.carTyres = new List<Tyre>();

        this.AddTyre(tyre1Pressure, tyre1Age);
        this.AddTyre(tyre3Pressure, tyre3Age);
        this.AddTyre(tyre4Pressure, tyre4Age);
        this.AddTyre(tyre2Pressure, tyre2Age);
    }

    public string CarModel
    {
        get
        {
            return this.carModel;
        }

        private set
        {
            this.carModel = value;
        }
    }

    public Engine CarEngine
    {
        get
        {
            return this.carEngine;
        }

        private set
        {
            this.carEngine = value;
        }
    }

    public Cargo CarCargo
    {
        get
        {
            return this.carCargo;
        }

        private set
        {
            this.carCargo = value;
        }
    }

    public List<Tyre> CarTyres
    {
        get
        {
            return this.carTyres;
        }
    }

    private void AddTyre(double pressure, int age)
    {
        if (this.carTyres.Count < 4)
        {
            var currentTyre = new Tyre(pressure, age);
            this.carTyres.Add(currentTyre);
        }
    }
}
