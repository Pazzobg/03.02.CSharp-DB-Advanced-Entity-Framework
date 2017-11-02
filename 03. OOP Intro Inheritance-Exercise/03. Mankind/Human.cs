using System;

public class Human
{
    private const int FirstNameMinLenght = 4;
    private const int LastNameMinLength = 3;

    private string firstName;
    private string lastName;

    public Human(string firstName, string lastName)
    {
        this.FirstName = firstName;
        this.LastName = lastName;
    }

    public string FirstName
    {
        get
        {
            return this.firstName;
        }

        protected set
        {
            if (value.Length < FirstNameMinLenght)
            {
                throw new ArgumentException("Expected length at least 4 symbols! Argument: firstName");
            }
            else if (!char.IsUpper(value[0]))
            {
                throw new ArgumentException("Expected upper case letter! Argument: firstName");
            }

            this.firstName = value;
        }
    }

    public string LastName
    {
        get
        {
            return this.lastName;
        }

        protected set
        {
            if (value.Length < LastNameMinLength)
            {
                throw new ArgumentException("Expected length at least 3 symbols! Argument: lastName");
            }
            else if (!char.IsUpper(value[0]))
            {
                throw new ArgumentException("Expected upper case letter! Argument: lastName");
            }

            this.lastName = value;
        }
    }

    public override string ToString()
    {
        return 
            $"First Name: {this.FirstName}{Environment.NewLine}" +
            $"Last Name: {this.LastName}";
    }
}
