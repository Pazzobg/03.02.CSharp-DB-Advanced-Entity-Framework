using System;
using System.Globalization;

public class DateModifier
{
    private string date1;
    private string date2;

    public DateModifier(string dateOne, string dateTwo)
    {
        this.Date1 = dateOne;
        this.Date2 = dateTwo;
    }

    private string Date1
    {
        get
        {
            return this.date1;
        }

        set
        {
            this.date1 = value;
        }
    }

    private string Date2
    {
        get
        {
            return this.date2;
        }

        set
        {
            this.date2 = value;
        }
    }

    public double CalculateDifferenceBetweenDates()
    {
        var firstDate = DateTime.ParseExact(this.date1, "yyyy MM dd", CultureInfo.InvariantCulture);
        var secondDate = DateTime.ParseExact(this.date2, "yyyy MM dd", CultureInfo.InvariantCulture);

        var result = Math.Abs((secondDate - firstDate).TotalDays);

        return result;
    }
}
