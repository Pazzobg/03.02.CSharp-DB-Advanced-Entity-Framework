namespace P01_ProductsShop.App
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using P01_ProductsShop.Data;
    using P01_ProductsShop.Models;
    using System.Linq;

    internal class DatabaseTools
    {
        internal static void FillInDataBase(User[] users, Category[] categories, Product[] products)
        {
            var random = new Random();

            var usersCount = users.Length;
            var categoriesCount = categories.Length;
            var productsCount = products.Length;

            using (var context = new ProductsDbContext())
            {
                // Reset Database
                ResetDatabase(context);

                // Seed Users
                foreach (var user in users)
                {
                    context.Users.Add(user);
                }

                // Seed Categories
                foreach (var category in categories)
                {
                    context.Categories.Add(category);
                }

                // Seed Products and CategoriesProducts
                for (int i = 0; i < productsCount; i++)
                {
                    var currentProduct = products[i];
                    var randomUser = random.Next(0, usersCount);
                    var randomCategory = random.Next(0, categoriesCount);

                    currentProduct.Seller = users[randomUser];

                    if (i % 2 == 0)
                    {
                        randomUser = random.Next(0, usersCount);
                        currentProduct.Buyer = users[randomUser];
                    }

                    context.Products.Add(currentProduct);

                    var currentMapping = new CategoryProduct
                    {
                        Category = categories[randomCategory],
                        Product = currentProduct
                    };

                    context.CategoryProducts.Add(currentMapping);
                }

                context.SaveChanges();

                var cpProdIds = context.CategoryProducts.Select(cp => cp.ProductId).ToList();
                var cpCatIds = context.CategoryProducts.Select(cp => cp.CategoryId).ToList();

                for (int i = 0; i < cpProdIds.Count; i++)
                {
                    if (i % 2 != 0)
                    {
                        var currentProdId = cpProdIds[i];
                        var currentCatid = cpCatIds[i];
                        var newCatId = 0;

                        var idAlter = random.Next(1, 6);

                        newCatId = (currentCatid + idAlter) <= categoriesCount ?
                            (currentCatid + idAlter) :
                            (currentCatid - idAlter);

                        var newMapping = new CategoryProduct
                        {
                            ProductId = currentProdId,
                            CategoryId = newCatId
                        };

                        context.CategoryProducts.Add(newMapping);
                    }
                }

                context.SaveChanges();
            }
        }

        protected static void ResetDatabase(DbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.Migrate();
        }
    }

}
