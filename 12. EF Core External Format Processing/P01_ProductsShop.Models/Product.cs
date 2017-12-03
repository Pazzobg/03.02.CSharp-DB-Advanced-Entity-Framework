namespace P01_ProductsShop.Models
{
    using System.Collections.Generic;

    public class Product
    {
        public Product()
        {

        }

        public Product(string name, decimal price)
        {
            this.Name = name;
            this.Price = price;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int? BuyerId { get; set; }
        public User Buyer { get; set; }

        public int SellerId { get; set; }
        public User Seller { get; set; }

        public ICollection<CategoryProduct> CategoryProducts { get; set; } = new HashSet<CategoryProduct>();
    }
}
