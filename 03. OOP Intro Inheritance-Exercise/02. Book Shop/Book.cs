using System;

public class Book
{
    private const int NameMinLength = 3;

    private string title;
    private string author;
    private decimal price;

    public Book(string title, string author, decimal price)
    {
        this.Title = title;
        this.Author = author;
        this.Price = price;
    }

    public string Title
    {
        get
        {
            return this.title;
        }

        protected set
        {
            if (value.Length < NameMinLength)
            {
                throw new ArgumentException("Title not valid!");
            }

            this.title = value;
        }
    }

    public string Author
    {
        get
        {
            return this.author;
        }

        protected set
        {
            int lastNameStart = value.IndexOf(" ") + 1;

            if (char.IsDigit(value[lastNameStart]))
            {
                throw new ArgumentException("Author not valid!");
            }

            this.author = value;
        }
    }

    public virtual decimal Price
    {
        get
        {
            return this.price;
        }

        protected set
        {
            if (value <= 0)
            {
                throw new ArgumentException("Price not valid!");
            }

            this.price = value;
        }
    }

    public override string ToString()
    {
        return 
            $"Type: {this.GetType().Name}{Environment.NewLine}" +
            $"Title: {this.Title}{Environment.NewLine}" +
            $"Author: {this.author}{Environment.NewLine}" +
            $"Price: {this.Price:f2}";
    }
}
