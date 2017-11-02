public class Tyre
{
    private double tyrePressure;
    private int tyreAge; 

    public Tyre(double pressure, int age)
    {
        this.TyrePressure = pressure;
        this.TyreAge = age;
    }

    public double TyrePressure
    {
        get
        {
            return this.tyrePressure;
        }

        private set
        {
            this.tyrePressure = value;
        }
    }

    public int TyreAge
    {
        get
        {
            return this.tyreAge;
        }

        private set
        {
            this.tyreAge = value;
        }
    }
}
