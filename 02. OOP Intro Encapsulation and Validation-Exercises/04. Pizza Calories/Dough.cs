using System;

public class Dough
{
    private const int MinimumWeight = 1;
    private const int MaximumWeight = 200;

    private string flourType;
    private string bakingTechnique;
    private double weight;
    private double caloriesPerGram;

    public Dough(string flour, string baking, double weight)
    {
        this.FlourType = flour;
        this.BakingTechnique = baking;
        this.Weight = weight;
        this.Calories = this.CalculateCalories(flour, baking);
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

    private string FlourType
    {
        get
        {
            return this.flourType;
        }

        set
        {
            if (value.ToLower() == "white" || value.ToLower() == "wholegrain")
            {
                this.flourType = value;
            }
            else
            {
                throw new ArgumentException("Invalid type of dough.");
            }
        }
    }

    private string BakingTechnique
    {
        get
        {
            return this.bakingTechnique;
        }

        set
        {
            if (value.ToLower() == "crispy" || value.ToLower() == "chewy" || value.ToLower() == "homemade")
            {
                this.bakingTechnique = value;
            }
            else
            {
                throw new ArgumentException("Invalid type of dough.");
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
            if (value < MinimumWeight || value > MaximumWeight)
            {
                throw new ArgumentException("Dough weight should be in the range [1..200].");
            }

            this.weight = value;
        }
    }

    private double CalculateCalories(string flour, string baking)
    {
        double doughModifier = 0;
        double bakingModifier = 0;

        switch (flour.ToLower())
        {
            case "white": doughModifier = 1.5; break;
            case "wholegrain": doughModifier = 1.0; break;
        }

        switch (baking.ToLower())
        {
            case "crispy": bakingModifier = 0.9; break;
            case "chewy": bakingModifier = 1.1; break;
            case "homemade": bakingModifier = 1.0; break;
        }

        return 2 * this.Weight * doughModifier * bakingModifier;
    }
}