public class GoldenEditionBook : Book
{
    public GoldenEditionBook(string title, string author, decimal price)
        : base(title, author, price)
    {
        this.Price *= 1.3m;
    }
}
