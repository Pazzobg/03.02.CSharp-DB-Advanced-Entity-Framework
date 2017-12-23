namespace FastFood.DataProcessor
{
    using FastFood.Data;
    using System.Linq;

    public static class Bonus
    {
        public static string UpdatePrice(FastFoodDbContext context, string itemName, decimal newPrice)
        {
            var item = context.Items.FirstOrDefault(i => i.Name == itemName);

            if (item == null)
            {
                string errorMsg = $"Item {itemName} not found!";
                return errorMsg;
            }

            var oldPrice = item.Price;
            item.Price = newPrice;
            context.SaveChanges();

            string successMsg = $"{item.Name} Price updated from ${oldPrice:F2} to ${newPrice:F2}";
            return successMsg;
        }
    }
}
