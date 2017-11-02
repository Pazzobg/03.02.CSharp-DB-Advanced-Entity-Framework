public class Cargo
{
    private int cargoWeight;
    private string cargoType;

    public Cargo(int kgs, string type)
    {
        this.CargoWeight = kgs;
        this.CargoType = type;
    }

    public int CargoWeight
    {
        get
        {
            return this.cargoWeight;
        }

        private set
        {
            this.cargoWeight = value;
        }
    }

    public string CargoType
    {
        get
        {
            return this.cargoType;
        }

        private set
        {
            this.cargoType = value;
        }
    }
}
