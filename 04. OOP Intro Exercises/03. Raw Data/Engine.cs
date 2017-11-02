public class Engine
{
    private int engineSpeed;
    private int enginePower;

    public Engine(int maxSpeed, int hp)
    {
        this.EngineSpeed = maxSpeed;
        this.EnginePower = hp;
    }

    public int EngineSpeed
    {
        get
        {
            return this.engineSpeed;
        }

        private set
        {
            this.engineSpeed = value;
        }
    }

    public int EnginePower
    {
        get
        {
            return this.enginePower;
        }

        private set
        {
            this.enginePower = value;
        }
    }
}
