namespace P03_SalesDatabase
{
    using Microsoft.EntityFrameworkCore;
    using P03_SalesDatabase.Data;
    using P03_SalesDatabase.Data.Models;

    public class StartUp
    {
        public static void Main()
        {
            using (var db = new SalesContext())
            {
                ResetDatabase(db);





            }


        }

        private static void ResetDatabase(SalesContext db)
        {
            db.Database.EnsureDeleted();

            db.Database.Migrate();

            Seed(db);
        }

        private static void Seed(SalesContext db)
        {
            var products = new[]
            {
                new Product("Apples", 7.4, 2.40m),
                new Product("Pears", 6.0, 3.10m),
                new Product("Grapes", 250, 1.95m),
                new Product("Kiwi", 0.8, 3.5m),
            };

            db.Products.AddRange(products);

            var customers = new[]
            {
                new Customer("Iva", "iva@abv.bg", "545056485015120"),
                new Customer("Gergana", "gergana@abv.bg", "545056485015120"),
                new Customer("Rositsa", "rosi@abv.bg", "545056485015120"),
                new Customer("Valya", "valya@abv.bg", "545056485015120"),
            };

            db.Customers.AddRange(customers);

            var stores = new[]
            {
                new Store("CBA"), 
                new Store("Piccadilly"), 
                new Store("Kaufland"), 
                new Store("Metro"),
            };

            db.Stores.AddRange(stores);

            var sales = new[]
            {

                new Sale(products[0], customers[0], stores[0]),
                new Sale(products[1], customers[1], stores[1]),
                new Sale(products[2], customers[2], stores[2]),
                new Sale(products[3], customers[3], stores[3]),
            };

            db.Sales.AddRange(sales);

            db.SaveChanges();
        }
    }
}
