namespace P03_SalesDatabase.Data.Models
{
    using System.Collections.Generic;

    public class Store
    {
        public Store()
        {

        }

        public Store(string name)
        {
            this.Name = name;
        }

        public int StoreId { get; set; }

        public string Name { get; set; }

        public ICollection<Sale> Sales { get; set; } = new List<Sale>();
    }
}