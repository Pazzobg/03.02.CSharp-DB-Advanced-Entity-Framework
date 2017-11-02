using System;
using System.Linq;

public class Student : Human
{
    private const int FacNumMinLength = 5;
    private const int FacNumMaxLength = 10;

    private string facultyNumber;

    public Student(string firstName, string lastName, string facNumber)
        : base(firstName, lastName)
    {
        this.FacultyNumber = facNumber;
    }

    private string FacultyNumber
    {
        get
        {
            return this.facultyNumber;
        }

        set
        {
            if (value.All(char.IsLetterOrDigit) && 
                value.Length >= FacNumMinLength && 
                value.Length <= FacNumMaxLength)
            {
                this.facultyNumber = value;
            }
            else
            {
                throw new ArgumentException("Invalid faculty number!");
            }
        }
    }

    public override string ToString()
    {
        return
            $"First Name: {this.FirstName}{Environment.NewLine}" +
            $"Last Name: {this.LastName}{Environment.NewLine}" +
            $"Faculty number: {this.FacultyNumber}";
    }
}
