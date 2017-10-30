using System;
using System.Collections.Generic;

public class Person
{
    private const int MinValue = 0;

    private string name;
    private decimal money;
    private List<Product> bagOfProducts;

    public Person(string name, decimal money)
    {
        this.Name = name;
        this.Money = money;
        this.bagOfProducts = new List<Product>();
    }

    public string Name
    {
        get
        {
            return this.name;
        }

        private set
        {
            if (string.IsNullOrEmpty(value) || value == " ")
            {
                throw new ArgumentException("Name cannot be empty");
            }

            this.name = value;
        }
    }

    public decimal Money
    {
        get
        {
            return this.money;
        }

        private set
        {
            if (value < MinValue)
            {
                throw new ArgumentException("Money cannot be negative");
            }

            this.money = value;
        }
    }

    public List<Product> BagOfProducts
    {
        get
        {
            return this.bagOfProducts;
        }
    }

    public void BuyProduct(Product product)
    {
        if (product.Price <= this.money)
        {
            this.money -= product.Price;
            this.bagOfProducts.Add(product);
        }
        else
        {
            throw new InvalidOperationException($"{this.Name} can't afford {product.Name}");
        }
    }

    public override string ToString()
    {
        if (this.bagOfProducts.Count > 0)
        {
            return $"{this.Name} - {string.Join(", ", this.bagOfProducts)}";
        }
        else
        {
            return $"{this.Name} - Nothing bought";
        }
    }
}
