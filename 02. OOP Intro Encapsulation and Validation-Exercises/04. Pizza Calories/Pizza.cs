using System;
using System.Collections.Generic;

public class Pizza
{
    private string name;
    private Dough dough;
    private List<Topping> toppings;

    public Pizza(string name)
    {
        this.Name = name;
        this.toppings = new List<Topping>();
    }

    public string Name
    {
        get
        {
            return this.name;
        }

        private set
        {
            if (string.IsNullOrEmpty(value) || value.Length > 15)
            {
                throw new ArgumentException("Pizza name should be between 1 and 15 symbols.");
            }

            this.name = value;
        }
    }

    public Dough Dough
    {
        private get
        {
            return this.dough;
        }

        set
        {
            this.dough = value;
        }
    }

    public void AddTopping(Topping toppingToAdd)
    {
        if (this.toppings.Count <= 10)
        {
            this.toppings.Add(toppingToAdd);
        }
        else
        {
            throw new ArgumentException("Number of toppings should be in range [0..10].");
        }
    }

    public double CalculateTotalCalories()
    {
        double totalCalories = this.Dough.Calories;

        foreach (var t in this.toppings)
        {
            totalCalories += t.Calories;
        }

        return totalCalories;
    }
}
