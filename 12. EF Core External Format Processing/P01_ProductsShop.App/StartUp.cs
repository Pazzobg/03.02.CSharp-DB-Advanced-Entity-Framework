namespace P01_ProductsShop
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using P01_ProductsShop.Data;

    class StartUp
    {
        static void Main(string[] args)
        {
            ResetDatabase();
        }

        private static void ResetDatabase()
        {
            using (var context = new ProductsDbContext())
            {
                context.Database.EnsureDeleted();
                context.Database.Migrate();
            }
        }
    }
}
