namespace P01_ProductsShop.App
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Linq;
    using P01_ProductsShop.Models;
    using P01_ProductsShop.Data;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    internal class XmlProcessing
    {
        /*========================     Pr.01     ========================*/

        internal static void ImportDataXml()
        {
            var users = GetUsersList();
            var categories = GetCategoriesList();
            var products = GetProductsList();

            DatabaseTools.FillInDataBase(users, categories, products);
        }

        private static User[] GetUsersList()
        {
            var xmlString = File.ReadAllText("D:/SoftUni/3.2. CSharpDB-Xml-Resources/users.xml");
            var usersXml = XDocument.Parse(xmlString);

            var root = usersXml.Root.Elements();

            var users = new List<User>();

            foreach (var element in root)
            {
                string firstName = element.Attribute("firstName")?.Value;
                string lastName = element.Attribute("lastName").Value;
                string ageStr = element.Attribute("age")?.Value;
                int? age = null;

                if (ageStr != null)
                {
                    age = int.Parse(ageStr);
                }

                var user = new User(firstName, lastName, age);

                users.Add(user);
            }

            return users.ToArray();
        }

        private static Category[] GetCategoriesList()
        {
            var xmlString = File.ReadAllText("D:/SoftUni/3.2. CSharpDB-Xml-Resources/categories.xml");
            var categoriesXml = XDocument.Parse(xmlString);

            var categoriesList = new List<Category>();

            var root = categoriesXml.Root.Elements();

            foreach (var element in root)
            {
                string name = element.Element("name").Value;

                var category = new Category(name);
                categoriesList.Add(category);
            }

            return categoriesList.ToArray();
        }

        private static Product[] GetProductsList()
        {
            var xmlString = File.ReadAllText("D:/SoftUni/3.2. CSharpDB-Xml-Resources/products.xml");
            var productsXml = XDocument.Parse(xmlString);

            var productsList = new List<Product>();

            var root = productsXml.Root.Elements();

            foreach (var element in root)
            {
                string name = element.Element("name").Value;
                decimal price = decimal.Parse(element.Element("price").Value);

                var currentProduct = new Product(name, price);
                productsList.Add(currentProduct);
            }

            return productsList.ToArray();
        }

        /*========================     Pr.02     ========================*/

        /*Query 1 - Products In Range*/
        internal static void GetProductsInRange()
        {
            using (var context = new ProductsDbContext())
            {
                var products = context.Products
                    .Where(p => p.Buyer != null && (p.Price >= 1000 && p.Price <= 2000))
                    .OrderBy(p => p.Price)
                    .Select(p => new
                    {
                        name = p.Name,
                        price = p.Price,
                        buyer = $"{p.Buyer.FirstName} {p.Buyer.LastName}"
                    })
                    .ToArray();

                var doc = new XDocument();
                doc.Add(new XElement("products"));

                foreach (var p in products)
                {
                    doc.Element("products")
                        .Add(new XElement("product",
                                new XAttribute("name", p.name),
                                new XAttribute("price", p.price),
                                new XAttribute("buyer", p.buyer)));
                }

                doc.Save("CompletedTasksXMLs/Q2.1_ProductsInRange.xml");
            }
        }

        /*Query 2 - Sold Products*/
        internal static void GetSoldProducts()
        {
            using (var context = new ProductsDbContext())
            {

                var usersWithSoldItems = context.Users
                    .Where(u => u.ProductsSold.Count > 0)
                    .OrderBy(u => u.LastName)
                    .ThenBy(u => u.FirstName)
                    .Select(u => new
                    {
                        firstName = u.FirstName,
                        lastName = u.LastName,
                        soldProducts = u.ProductsSold.Select(ps => new
                        {
                            name = ps.Name,
                            price = ps.Price
                        })
                    })
                    .ToArray();

                var doc = new XDocument();
                doc.Add(new XElement("users"));

                foreach (var u in usersWithSoldItems)
                {
                    var saleDetails = new List<XElement>();
                    foreach (var sale in u.soldProducts)
                    {
                        var currentDetail = new XElement("product",
                                                new XElement("name", sale.name),
                                                new XElement("price", sale.price));

                        saleDetails.Add(currentDetail);
                    }

                    doc.Root
                        .Add(new XElement("user",
                                new XAttribute("first-name", u.firstName ?? "N/A"),
                                new XAttribute("last-name", u.lastName),
                                    new XElement("sold-products", saleDetails)));
                }

                doc.Save("CompletedTasksXMLs/Q2.2_SoldProducts.xml");
            }
        }

        /*Query 3 - Categories by Product Count*/
        internal static void GetCategoriesByProductCount()
        {
            using (var context = new ProductsDbContext())
            {
                var categories = context.Categories
                    .Include(c => c.CategoryProducts)
                    .OrderBy(c => c.CategoryProducts.Count)
                    .Select(c => new
                    {
                        name = c.Name,
                        numberOfPoducts = c.CategoryProducts.Count,
                        avgPriceInCategory = c.CategoryProducts.Average(cp => cp.Product.Price),
                        totalRevenue = c.CategoryProducts.Sum(cp => cp.Product.Price)
                    })
                    .ToArray();

                var doc = new XDocument(new XElement("categories"));

                foreach (var cat in categories)
                {
                    doc.Root
                        .Add(new XElement("category", new XAttribute("name", cat.name),
                                new XElement("products-count", cat.numberOfPoducts),
                                new XElement("average-price", cat.avgPriceInCategory),
                                new XElement("total-revenue", cat.totalRevenue)));
                }

                doc.Save("CompletedTasksXMLs/Q2.3_CategoriesByProdCount.xml");
            }
        }

        internal static void GetUsersAndProducts()
        {
            using (var context = new ProductsDbContext())
            {
                var usersWithSales = context.Users
                    .Include(u => u.ProductsSold)
                    .Where(u => u.ProductsSold.Count > 0 && u.ProductsSold.Any(p => p.BuyerId != null))
                    .OrderByDescending(u => u.ProductsSold.Count)
                    .ThenBy(u => u.LastName)
                    .ToArray();

                var usersCount = usersWithSales.Count();

                var resultUsers = usersWithSales
                    .Select(u => new
                    {
                        firstName = u.FirstName,
                        lastName = u.LastName,
                        age = u.Age,
                        productsSold = new
                        {
                            count = u.ProductsSold.Count,
                            products = u.ProductsSold.Select(ps => new
                            {
                                name = ps.Name,
                                price = ps.Price
                            })
                            .ToArray()
                        }
                    })
                    .ToArray();

                var doc = new XDocument(new XElement("users", new XAttribute("count", usersCount)));

                foreach (var user in resultUsers)
                {
                    var currentUserX =
                        new XElement("user");

                    if (user.firstName != null)
                    {
                        currentUserX.Add(new XAttribute("first-name", user.firstName));
                    }

                    currentUserX.Add(new XAttribute("last-name", user.lastName));

                    if (user.age != null)
                    {
                        currentUserX.Add(new XAttribute("age", user.age ?? 0));
                    }

                    var sales = new XElement("sold-products", new XAttribute("count", user.productsSold.count));
                    foreach (var sale in user.productsSold.products)
                    {
                        sales.Add(new XElement("product",
                                    new XAttribute("name", sale.name),
                                    new XAttribute("price", sale.price)));
                    }

                    currentUserX.Add(sales);

                    doc.Root.Add(currentUserX);
                }

                doc.Save("CompletedTasksXMLs/Q2.4_UsersProducts.xml");
            }
        }
    }
}
