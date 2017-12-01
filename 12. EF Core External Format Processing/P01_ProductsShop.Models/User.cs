namespace P01_ProductsShop.Models
{
    using System.Collections.Generic;

    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? Age { get; set; }

        public ICollection<Product> ProductsSold { get; set; } = new List<Product>();

        public ICollection<Product> ProductsBought { get; set; } = new List<Product>();

        public ICollection<User> Friends { get; set; } = new List<User>();
    }
}