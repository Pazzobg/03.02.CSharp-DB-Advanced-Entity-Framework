using System;

public class StartUp
{
    public static void Main()
    {
        try
        {
            string author = Console.ReadLine();
            string title = Console.ReadLine();
            decimal price = decimal.Parse(Console.ReadLine());

            var book = new Book(title, author, price);
            var goldenEdition = new GoldenEditionBook(title, author, price);

            Console.WriteLine(book + Environment.NewLine);
            Console.WriteLine(goldenEdition);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}