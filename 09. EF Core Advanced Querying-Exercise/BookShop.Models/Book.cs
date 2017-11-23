namespace BookShop.Models
{
    using System;
    using System.Collections.Generic;

    public class Book
    {
        public Book()
        {
        }

        public Book(string title, string descr, int copies, EditionType editionType, AgeRestriction ageRestr, Author author)
        {
            this.Title = title;
            this.Description = descr;
            this.Copies = copies;
            this.EditionType = editionType;
            this.AgeRestriction = ageRestr;
            this.Author = author;
        }

        public int BookId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public int Copies { get; set; }

        public decimal Price { get; set; }

        public EditionType EditionType { get; set; }

        public AgeRestriction AgeRestriction { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; }

        public ICollection<BookCategory> BookCategories { get; set; } = new HashSet<BookCategory>();
    }
}