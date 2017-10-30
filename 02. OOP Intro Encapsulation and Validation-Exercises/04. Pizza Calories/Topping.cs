using System;

public class Topping
{
    private const int ToppinMinValue = 1;
    private const int ToppingMaxValue = 50;

    private string toppingType;
    private double weight;
    private double caloriesPerGram;

    public Topping(string topping, double weight)
    {
        this.ToppingType = topping;
        this.Weight = weight;
        this.Calories = this.CalculateCalories(topping);
    }

    public double Calories
    {
        get
        {
            return this.caloriesPerGram;
        }

        private set
        {
            this.caloriesPerGram = value;
        }
    }

    private string ToppingType
    {
        get
        {
            return this.toppingType;
        }

        set
        {
            if (value.ToLower() == "meat" || 
                value.ToLower() == "veggies" || 
                value.ToLower() == "cheese" || 
                value.ToLower() == "sauce")
            {
                this.toppingType = value;
            }
            else
            {
                throw new ArgumentException($"Cannot place {value} on top of your pizza.");
            }
        }
    }

    private double Weight
    {
        get
        {
            return this.weight;
        }

        set
        {
            if (value < ToppinMinValue || value > ToppingMaxValue)
            {
                throw new ArgumentException($"{this.ToppingType} weight should be in the range [1..50].");
            }

            this.weight = value;
        }
    }

    private double CalculateCalories(string topping)
    {
        double toppingModifier = 0;

        switch (topping.ToLower())
        {
            case "meat": toppingModifier = 1.2; break;
            case "veggies": toppingModifier = 0.8; break;
            case "cheese": toppingModifier = 1.1; break;
            case "sauce": toppingModifier = 0.9; break;
        }

        return 2 * this.Weight * toppingModifier;
    }
}
