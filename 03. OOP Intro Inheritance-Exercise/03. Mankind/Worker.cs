using System;

public class Worker : Human
{
    private const int WeekSalaryMinimum = 10;
    private const int WorkHoursMin = 1;
    private const int WorkHoursMax = 12;
    private const int WorkDaysPerWeek = 5;

    private decimal weekSalary;
    private double workHoursPerDay;

    public Worker(string firstName, string lastName, decimal weekSalary, double workHrsDaily)
        : base(firstName, lastName)
    {
        this.WeekSalary = weekSalary;
        this.WorkHoursPerDay = workHrsDaily;
    }

    private decimal WeekSalary
    {
        get
        {
            return this.weekSalary;
        }

        set
        {
            if (value <= WeekSalaryMinimum)
            {
                throw new ArgumentException("Expected value mismatch! Argument: weekSalary");
            }

            this.weekSalary = value;
        }
    }

    private double WorkHoursPerDay
    {
        get
        {
            return this.workHoursPerDay;
        }

        set
        {
            if (value < WorkHoursMin || value > WorkHoursMax)
            {
                throw new ArgumentException("Expected value mismatch! Argument: workHoursPerDay");
            }

            this.workHoursPerDay = value;
        }
    }

    private decimal HourlySalary
    {
        get
        {
            return this.CalculateHourlySalary(this.WeekSalary, this.WorkHoursPerDay);
        }
    }

    public override string ToString()
    {
        return
            $"First Name: {this.FirstName}{Environment.NewLine}" +
            $"Last Name: {this.LastName}{Environment.NewLine}" +
            $"Week Salary: {this.WeekSalary:f2}{Environment.NewLine}" +
            $"Hours per day: {this.WorkHoursPerDay:f2}{Environment.NewLine}" +
            $"Salary per hour: {this.HourlySalary:f2}";
    }

    private decimal CalculateHourlySalary(decimal weekSalary, double workHoursPerDay)
    {
        return weekSalary / ((decimal)workHoursPerDay * WorkDaysPerWeek);
    }
}
