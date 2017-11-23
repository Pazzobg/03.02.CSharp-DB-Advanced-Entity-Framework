namespace BookShop
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using BookShop.Data;
    using BookShop.Initializer;
    using BookShop.Models;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main()
        {
            using (var db = new BookShopContext())
            {
                // Problem 0
                // DbInitializer.ResetDatabase(db);

                // Problem 1
                // var input = Console.ReadLine();
                // Console.WriteLine(GetBooksByAgeRestriction(db, input));

                // Problem 2
                // Console.WriteLine(GetGoldenBooks(db));

                // Problem 3
                // Console.WriteLine(GetBooksByPrice(db));

                // Problem 4
                // int year = int.Parse(Console.ReadLine());
                // Console.WriteLine(GetBooksNotRealeasedIn(db, year));

                // Problem 5
                // string input = Console.ReadLine();
                // Console.WriteLine(GetBooksByCategory(db, input));

                // Problem 6
                // Console.WriteLine(GetBooksReleasedBefore(db, input));

                // Problem 7
                // string input = Console.ReadLine();
                // Console.WriteLine(GetAuthorNamesEndingIn(db, input));

                // Problem 8
                // string input = Console.ReadLine();
                // Console.WriteLine(GetBookTitlesContaining(db, input));

                // Problem 9
                // string input = Console.ReadLine();
                // Console.WriteLine(GetBooksByAuthor(db, input));

                // Problem 10
                // int lengthCheck = int.Parse(Console.ReadLine());
                // Console.WriteLine(CountBooks(db, lengthCheck));

                // Problem 11
                // Console.WriteLine(CountCopiesByAuthor(db));

                // Problem 12
                // Console.WriteLine(GetTotalProfitByCategory(db));

                // Problem 13
                 Console.WriteLine(GetMostRecentBooks(db));

                // Problem 14
                // IncreasePrices(db);

                // Problem 15
                // Console.WriteLine(RemoveBooks(db));
            }
        }

        // Pr. 01
        public static string GetBooksByAgeRestriction(BookShopContext db, string input)
        {
            var books = db.Books
                .Where(b => b.AgeRestriction
                                .ToString()
                                .Equals(input, StringComparison.InvariantCultureIgnoreCase))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToList();

            return string.Join(Environment.NewLine, books);
        }

        // Pr. 02
        public static string GetGoldenBooks(BookShopContext db)
        {
            var books = db.Books
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToList();

            return string.Join(Environment.NewLine, books);
        }

        // Pr.03
        public static string GetBooksByPrice(BookShopContext db)
        {
            var books = db.Books
                .Select(e => new
                {
                    Title = e.Title,
                    Price = e.Price
                })
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .ToList();

            var resultList = new List<string>();

            foreach (var book in books)
            {
                string currentBookDisplay = $"{book.Title} - ${book.Price:f2}";
                resultList.Add(currentBookDisplay);
            }

            return string.Join(Environment.NewLine, resultList);

            /*var sb = new StringBuilder();
            for (int i = 0; i < books.Count - 1; i++) an alternative, using StringBuilder.Also works.
            {
                var book = books[i];
                sb.Append(book.Title);
                sb.Append(" - $");
                sb.AppendFormat("{0:f2}", book.Price);
                sb.AppendLine();
            }
            sb.Append(books.Last().Title);
            sb.Append(" - $");
            sb.AppendFormat("{0:f2}", books.Last().Price);
            return sb.ToString();*/
        }

        // Pr. 04
        public static string GetBooksNotRealeasedIn(BookShopContext db, int year)
        {
            var books = db.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToList();

            return string.Join(Environment.NewLine, books);
        }

        // Pr. 05
        public static string GetBooksByCategory(BookShopContext db, string input)
        {
            string[] categories = input.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var titles = db.Books
                .Where(b => b.BookCategories.Any(c => categories.Contains(c.Category.Name.ToLower())))
                .Select(e => e.Title)
                .OrderBy(t => t)
                .ToList();

            return string.Join(Environment.NewLine, titles);    
        }

        // Pr. 06
        public static string GetBooksReleasedBefore(BookShopContext db, string input)
        {
            DateTime date = DateTime.ParseExact(input, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var books = db.Books
                .Where(b => b.ReleaseDate.HasValue && b.ReleaseDate < date)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => $"{b.Title} - {b.EditionType} - ${b.Price:f2}")
                .ToList();

            return string.Join(Environment.NewLine, books);
        }

        // Pr. 07
        public static string GetAuthorNamesEndingIn(BookShopContext db, string input)
        {
            var names = db.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(e => $"{e.FirstName} {e.LastName}")
                .OrderBy(n => n)
                .ToList();

            return string.Join(Environment.NewLine, names);
        }

        // Pr. 08
        public static string GetBookTitlesContaining(BookShopContext db, string input)
        {
            var titles = db.Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .Select(b => b.Title)
                .OrderBy(t => t)
                .ToList();

            return string.Join(Environment.NewLine, titles);
        }

        // Pr. 09
        public static string GetBooksByAuthor(BookShopContext db, string input)
        {
            var books = db.Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .Select(b => $"{b.Title} ({b.Author.FirstName} {b.Author.LastName})")
                .ToList();
            
            return string.Join(Environment.NewLine, books);
        }
        
        // Pr. 10
        public static int CountBooks(BookShopContext db, int lengthCheck)
        {
            var books = db.Books
                .Where(b => b.Title.Length > lengthCheck)
                .ToList();

            return books.Count;
        }

        // Pr. 11
        public static string CountCopiesByAuthor(BookShopContext db)
        {
            var authorsCopies = db.Authors
                .Select(e => new
                {
                    Name = $"{e.FirstName} {e.LastName}",
                    CopiesCount = e.Books.Sum(b => b.Copies)
                })
                .OrderByDescending(ac => ac.CopiesCount)
                .ToList();

            var sb = new StringBuilder();

            foreach (var author in authorsCopies)
            {
                sb.AppendLine($"{author.Name} - {author.CopiesCount}");
            }

            return sb.ToString();
        }

        // Pr. 12
        public static string GetTotalProfitByCategory(BookShopContext db)
        {
            var categoriesProfits = db.Categories
                .Include(c => c.CategoryBooks)
                .Select(e => new
                {
                    Name = e.Name,
                    Profit = e.CategoryBooks.Sum(cb => cb.Book.Copies * cb.Book.Price)
                })
                .OrderByDescending(cp => cp.Profit)
                .ThenBy(cp => cp.Name)
                .ToList();

            var sb = new StringBuilder();

            foreach (var cat in categoriesProfits)
            {
                sb.AppendLine($"{cat.Name} ${cat.Profit}");
            }

            return sb.ToString();

            /*also works but quite slower - 1 second difference than the other one
            var categories = db.Categories
                .Select(c => c.Name)
                .ToList();

            var categoriesProfits = new Dictionary<string, decimal>();
            var sb = new StringBuilder();

            foreach (var categoryName in categories)
            {
                var profitPerCat = db.Books
                    .Where(b => b.BookCategories.Any(c => c.Category.Name.Equals(categoryName)))
                    .Sum(c => c.Copies * c.Price);

                categoriesProfits.Add(categoryName, profitPerCat);
            }

            foreach (var kvp in categoriesProfits.OrderByDescending(cp => cp.Value).ThenBy(cp => cp.Key))
            {
                sb.AppendLine($"{kvp.Key} ${kvp.Value:f2}");
            }

            return sb.ToString();*/
        }

        // Pr. 13
        public static string GetMostRecentBooks(BookShopContext db)
        {
            var categories = db.Categories
                .OrderBy(c => c.Name)
                .Select(c => new
                {
                    Name = c.Name,
                    Books = c.CategoryBooks
                                .Select(cb => cb.Book)
                                .OrderByDescending(b => b.ReleaseDate)
                                .Take(3)
                                .ToList()
                })
                .ToList();

            var sb = new StringBuilder();

            foreach (var cat in categories)
            {
                sb.AppendLine($"--{cat.Name}");

                foreach (var book in cat.Books)
                {
                    sb.AppendLine($"{book.Title} ({(book.ReleaseDate == null ? "N/A" : book.ReleaseDate.Value.Year.ToString())})");
                }
            }
            //var categories = db.Categories
            //    .Include(c => c.CategoryBooks)   // alternative solution
            //    .ThenInclude(cb => cb.Book)
            //    .ToList();

            //var sb = new StringBuilder();

            //foreach (var cat in categories.OrderBy(c => c.Name))
            //{
            //    sb.AppendLine($"--{cat.Name}");

            //    foreach (var catBook in cat.CategoryBooks.OrderByDescending(cb => cb.Book.ReleaseDate).Take(3))
            //    {
            //        sb.AppendLine($"{catBook.Book.Title} ({catBook.Book.ReleaseDate.Value.Year})");
            //    }
            //}

            return sb.ToString();
        }

        // Pr. 14
        public static void IncreasePrices(BookShopContext db)
        {
            var books = db.Books
                .Where(b => b.ReleaseDate.HasValue && b.ReleaseDate.Value.Year < 2010)
                .ToList();

            foreach (var book in books)
            {
                book.Price += 5m;
            }

            db.SaveChanges();
        }

        // Pr. 15
        public static int RemoveBooks(BookShopContext db)
        {
            var books = db.Books
                .Where(b => b.Copies < 4200)
                .ToList();

            int count = books.Count;

            db.Books.RemoveRange(books);
            db.SaveChanges();

            return count;
        }
    }
}
