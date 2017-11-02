using System;

public class Player
{
    private const int StatMinValue = 0;
    private const int StatMaxValue = 100;

    private string name;
    private int endurance;
    private int sprint;
    private int dribble;
    private int passing;
    private int shooting;
    private double skill;

    public Player(string name, int endurance, int sprint, int dribble, int passing, int shooting)
    {
        this.Name = name;
        this.Endurance = endurance;
        this.Sprint = sprint;
        this.Dribble = dribble;
        this.Passing = passing;
        this.Shooting = shooting;
        this.Skill = this.skill;
    }

    public string Name
    {
        get
        {
            return this.name;
        }

        private set
        {
            if (string.IsNullOrWhiteSpace(value) || value == " ")
            {
                throw new ArgumentException("A name should not be empty.");
            }

            this.name = value;
        }
    }

    public int Endurance
    {
        get
        {
            return this.endurance;
        }

        private set
        {
            this.CheckValidityOfValue(value, "Endurance");

            this.endurance = value;
        }
    }

    public int Sprint
    {
        get
        {
            return this.sprint;
        }

        private set
        {
            this.CheckValidityOfValue(value, "Sprint");

            this.sprint = value;
        }
    }

    public int Dribble
    {
        get
        {
            return this.dribble;
        }

        private set
        {
            this.CheckValidityOfValue(value, "Dribble");

            this.dribble = value;
        }
    }

    public int Passing
    {
        get
        {
            return this.passing;
        }

        private set
        {
            this.CheckValidityOfValue(value, "Passing");

            this.passing = value;
        }
    }

    public int Shooting
    {
        get
        {
            return this.shooting;
        }

        private set
        {
            this.CheckValidityOfValue(value, "Shooting");

            this.shooting = value;
        }
    }

    public double Skill
    {
        get
        {
            return this.skill;
        }

        private set
        {
            this.skill = (this.Endurance + this.Sprint + this.Dribble + this.Passing + this.Shooting) / 5.0;
        }
    }

    private void CheckValidityOfValue(int value, string statName)
    {
        if (value < StatMinValue || value > StatMaxValue)
        {
            throw new ArgumentException($"{statName} should be between 0 and 100.");
        }
    }
}
