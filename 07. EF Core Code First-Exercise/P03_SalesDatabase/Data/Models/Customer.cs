namespace P03_SalesDatabase.Data.Models
{
    using System.Collections.Generic;

    public class Customer
    {
        public Customer()
        {

        }

        public Customer(string name, string email, string ccNumber)
        {
            this.Name = name;
            this.Email = email;
            this.CreditCardNumber = ccNumber;
        }

        public int CustomerId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string CreditCardNumber { get; set; }

        public ICollection<Sale> Sales { get; set; } = new List<Sale>();
    }
}