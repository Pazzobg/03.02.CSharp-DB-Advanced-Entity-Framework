namespace P01_ProductsShop.App
{
    using System.IO;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using P01_ProductsShop.Data;
    using P01_ProductsShop.Models;

    internal class JsonProcessing
    {
        /*========================     Pr.01     ========================*/

        internal static void ImportData()
        {
            var pathUsers = "Resources/users.json";
            var users = ImportJson<User>(pathUsers);

            var pathCategories = "Resources/categories.json";
            var categories = ImportJson<Category>(pathCategories);

            var pathProducts = "Resources/products.json";
            var products = ImportJson<Product>(pathProducts);

            DatabaseTools.FillInDataBase(users, categories, products);
        }

        /*========================     Pr.02     ========================*/

        /*Query 1 - Products In Range*/
        internal static void GetProductsInRange()
        {
            using (var context = new ProductsDbContext())
            {
                var result = context.Products
                    .Where(p => p.Price >= 500 && p.Price <= 1000)
                    .OrderBy(p => p.Price)
                    .Select(p => new
                    {
                        name = p.Name,
                        price = p.Price,
                        seller = $"{p.Seller.FirstName} {p.Seller.LastName}"
                    })
                    .ToArray();

                string path = "CompletedTasksJSONs/Q2.1_ProductsInRange.json";
                DeserializeResult(result, path);
            }
        }

        /*Query 2 - Successfully Sold Products*/
        internal static void GetSuccessfullySoldProducts()
        {
            using (var context = new ProductsDbContext())
            {
                var result = context.Users
                    .Where(u => u.ProductsSold.Count > 0)
                    .Include(u => u.ProductsSold)
                    .OrderBy(u => u.LastName)
                    .ThenBy(u => u.FirstName)
                    .Select(u => new
                    {
                        firstname = u.FirstName,
                        lastName = u.LastName,
                        soldProducts = u.ProductsSold.Select(ps => new
                        {
                            name = ps.Name,
                            price = ps.Price,
                            buyerFirstName = ps.Buyer.FirstName,
                            buyerLastName = ps.Buyer.LastName
                        })
                    })
                    .ToArray();

                string path = "CompletedTasksJSONs/Q2.2_SuccessfullySoldProducts.json";
                DeserializeResult(result, path);
            }
        }

        /*Query 3 - Categories By Products Count*/
        internal static void GetCategoriesByProductCount()
        {
            using (var context = new ProductsDbContext())
            {
                var result = context.Categories
                    .OrderBy(c => c.Name)
                    .Include(c => c.CategoryProducts)
                    .Select(c => new
                    {
                        category = c.Name,
                        productsCount = c.CategoryProducts
                            .Where(cp => cp.CategoryId == c.Id)
                            .Count(),
                        averagePrice = c.CategoryProducts
                            .Where(cp => cp.CategoryId == c.Id)
                            .Average(cp => cp.Product.Price),
                        totalRevenue = c.CategoryProducts
                            .Where(cp => cp.CategoryId == c.Id)
                            .Sum(cp => cp.Product.Price)
                    })
                    .ToArray();

                string path = "CompletedTasksJSONs/Q2.3_CategoriesByProdCount.json";
                DeserializeResult(result, path);
            }
        }

        /*Query 4 - Users and Products*/
        internal static void GetUsersAndProducts()
        {
            using (var context = new ProductsDbContext())
            {
                var sellers = context.Users
                    .Include(u => u.ProductsSold)
                    .Where(u => u.ProductsSold.Count > 0 && u.ProductsSold.Any(p => p.BuyerId != null))
                    .OrderByDescending(u => u.ProductsSold.Count)
                    .ThenBy(u => u.LastName)
                    .ToArray();

                var count = sellers.Length;

                var result = new
                {
                    usersCount = count,
                    users = sellers.Select(s => new
                    {
                        firstName = s.FirstName,
                        lastName = s.LastName,
                        age = s.Age ?? 0,
                        soldProducts = new
                        {
                            count = s.ProductsSold.Count,
                            products = s.ProductsSold.Select(p => new
                            {
                                name = p.Name,
                                price = p.Price
                            })
                            .ToArray()
                        }
                    })
                    .ToArray()
                };

                string jsonString = JsonConvert.SerializeObject(result, Formatting.Indented);
                File.WriteAllText("CompletedTasksJSONs/Q2.4_UsersProducts.json", jsonString);
            }
        }

        private static T[] ImportJson<T>(string resourcePath)
        {
            var jsonString = File.ReadAllText(resourcePath);
            var objects = JsonConvert.DeserializeObject<T[]>(jsonString);

            return objects;
        }

        private static void DeserializeResult(object[] result, string path)
        {
            var serializedProducts = JsonConvert.SerializeObject(result, Formatting.Indented);
            File.WriteAllText(path, serializedProducts);
        }

        // Parsing and adding Users, using the generic method ImportJson
        //internal static void ImportUsersFromJson()
        //{
        //    User[] users = ImportJson<User>("Resources/users.json");

        //    using (var context = new ProductsDbContext())
        //    {
        //        context.Users.AddRange(users);
        //        context.SaveChanges();
        //    }
        //}
    }
}
