namespace FastFood.DataProcessor
{
    using System;
    using System.IO;
    using FastFood.Data;
    using System.Linq;
    using FastFood.Models.Enums;
    using Microsoft.EntityFrameworkCore;
    using FastFood.DataProcessor.Dto.Export;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using FastFood.Models;
    using System.Xml.Linq;

    public class Serializer
    {
        public static string ExportOrdersByEmployee(FastFoodDbContext context, string employeeName, string orderType)
        {
            var employee = context.Employees.FirstOrDefault(e => e.Name == employeeName);
            var type = Enum.Parse<OrderType>(orderType);
            var totalMoneyMade = 0m;

            var orders = context.Orders
                .Include(o => o.Employee)
                .Include(o => o.OrderItems)
                .Where(o => o.Type == type && o.Employee.Name == employeeName)
                .ToArray();

            var dtosToExport = new List<OrderDto>();

            foreach (var order in orders)
            {
                var dto = new OrderDto
                {
                    Customer = order.Customer
                };

                var orderPrice = 0m;

                foreach (var oi in order.OrderItems)
                {
                    orderPrice += oi.Item.Price * oi.Quantity;
                    dto.Items.Add(new ItemDto
                    {
                        Name = oi.Item.Name,
                        Price = oi.Item.Price,
                        Quantity = oi.Quantity
                    });
                }

                dto.TotalPrice = orderPrice;
                totalMoneyMade += orderPrice;

                dtosToExport.Add(dto);
            }

            var orderedDtos = dtosToExport
                .OrderByDescending(d => d.TotalPrice)
                .ThenByDescending(d => d.Items.Count)
                .ToList();

            var result = new
            {
                Name = employeeName, 
                Orders = orderedDtos.Select(d => new
                {
                    Customer = d.Customer, 
                    Items = d.Items.Select( i => new
                    {
                        Name = i.Name, 
                        Price = i.Price, 
                        Quantity = i.Quantity
                    }).ToArray(), 
                    TotalPrice = d.TotalPrice
                }).ToArray(), 
                TotalMade = totalMoneyMade                
            };

            string jsonString = JsonConvert.SerializeObject(result, Formatting.Indented);
            return jsonString;
        }

        public static string ExportCategoryStatistics(FastFoodDbContext context, string categoriesString)
        {
            var inputCategories = categoriesString.Split(',');

            var cats = context.Categories
                .Include(c => c.Items)
                .ThenInclude(i => i.OrderItems)
                .Where(c => inputCategories.Any(ic => ic == c.Name))
                .ToArray();

            var categList = new List<CategoryDto>();

            foreach (var c in cats)
            {
                var name = c.Name;
                var itemName = c.Items
                    .OrderByDescending(i => i.OrderItems.Sum(oi => i.Price * oi.Quantity))
                    .First()
                    .Name;

                var totalMade = c.Items
                    .OrderByDescending(i => i.OrderItems.Sum(oi => i.Price * oi.Quantity)).First()
                    .OrderItems.Sum(oi => oi.Quantity * oi.Item.Price);

                var timesSold = c.Items
                    .OrderByDescending(i => i.OrderItems.Sum(oi => oi.Quantity * i.Price)).First()
                    .OrderItems.Sum(oi => oi.Quantity);

                categList.Add(new CategoryDto
                {
                    Name = name,
                    ItemName = itemName,
                    TotalMade = totalMade,
                    TimesSold = timesSold
                });
            }

            categList = categList
                .OrderByDescending(c => c.TimesSold)
                .ThenByDescending(c => c.TotalMade)
                .ToList();

            var xDoc = new XDocument();
            xDoc.Add(new XElement("Categories"));

            foreach (var c in categList)
            {
                var category = new XElement("Category");

                category.Add(new XElement("Name", c.Name));
                var mostPopular = new XElement("MostPopularItem");

                mostPopular.Add(new XElement("Name", c.ItemName));
                mostPopular.Add(new XElement("TotalMade", c.TotalMade));
                mostPopular.Add(new XElement("TimesSold", c.TimesSold));

                category.Add(mostPopular);
                xDoc.Element("Categories").Add(category);
            }

            var result = xDoc.ToString();
            return result;
        }
    }
}