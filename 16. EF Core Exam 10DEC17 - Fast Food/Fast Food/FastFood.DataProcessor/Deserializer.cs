namespace FastFood.DataProcessor
{
    using System;
    using FastFood.Data;
    using FastFood.DataProcessor.Dto.Import;
    using Newtonsoft.Json;
    using System.Text;
    using System.Collections.Generic;
    using FastFood.Models;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Xml.Serialization;
    using System.IO;
    using System.Globalization;
    using FastFood.Models.Enums;

    public static class Deserializer
    {
        private const string FailureMessage = "Invalid data format.";
        private const string SuccessMessage = "Record {0} successfully imported.";

        public static string ImportEmployees(FastFoodDbContext context, string jsonString)
        {
            var employees = JsonConvert.DeserializeObject<EmployeeDto[]>(jsonString);

            var sb = new StringBuilder();
            var validEmployees = new List<Employee>();

            foreach (var emplDto in employees)
            {
                if (!IsValid(emplDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                bool positionExists = context.Positions.Any(p => p.Name == emplDto.Position);
                if (!positionExists)
                {
                    context.Positions.Add(new Position
                    {
                        Name = emplDto.Position
                    });

                    context.SaveChanges();
                }

                var employee = new Employee
                {
                    Name = emplDto.Name,
                    Age = emplDto.Age,
                    PositionId = context.Positions.FirstOrDefault(p => p.Name == emplDto.Position).Id
                };

                validEmployees.Add(employee);
                sb.AppendLine(string.Format(SuccessMessage, emplDto.Name));
            }

            context.Employees.AddRange(validEmployees);
            context.SaveChanges();

            var result = sb.ToString().TrimEnd();
            return result;
        }

        public static string ImportItems(FastFoodDbContext context, string jsonString)
        {
            var items = JsonConvert.DeserializeObject<ItemDto[]>(jsonString);

            var sb = new StringBuilder();
            var validItems = new List<Item>();

            foreach (var itemDto in items)
            {
                if (!IsValid(itemDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                if (validItems.Any(i => i.Name == itemDto.Name))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                bool categoryExists = context.Categories.Any(c => c.Name == itemDto.Category);
                if (!categoryExists)
                {
                    context.Categories.Add(new Category
                    {
                        Name = itemDto.Category
                    });

                    context.SaveChanges();
                }

                var item = new Item
                {
                    Name = itemDto.Name,
                    Price = itemDto.Price,
                    CategoryId = context.Categories.FirstOrDefault(c => c.Name == itemDto.Category).Id
                };

                validItems.Add(item);
                sb.AppendLine(string.Format(SuccessMessage, item.Name));
            }

            context.Items.AddRange(validItems);
            context.SaveChanges();

            var result = sb.ToString().TrimEnd();
            return result;
        }

        public static string ImportOrders(FastFoodDbContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(OrderDto[]), new XmlRootAttribute("Orders"));
            var deserializedOrders = (OrderDto[])serializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(xmlString)));

            var sb = new StringBuilder();
            var validOrders = new List<Order>();

            foreach (var orderDto in deserializedOrders)
            {
                if (!IsValid(orderDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var orderItemsValid = orderDto.Items.All(IsValid);
                if (!orderItemsValid)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var propperOrder = orderDto.Items.All(oi => context.Items.Any(i => i.Name == oi.Name));
                var employee = context.Employees.FirstOrDefault(e => e.Name == orderDto.Employee);
                if (!propperOrder || employee == null)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }
                
                // validaciq dali vyobshte ima datetime i enum-a
                var orderTime = DateTime.ParseExact(orderDto.DateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                var type = Enum.Parse<OrderType>(orderDto.Type);

                var order = new Order
                {
                    Customer = orderDto.Customer,
                    Employee = employee,
                    DateTime = orderTime,
                    Type = type                    
                };

                var items = orderDto.Items
                    .Select(oi => new OrderItem
                    {
                        ItemId = context.Items.FirstOrDefault(i => i.Name == oi.Name).Id,
                        Quantity = oi.Quantity,
                        Order = order
                    })
                    .ToArray();

                foreach (var item in items)
                {
                    order.OrderItems.Add(item);
                }

                validOrders.Add(order);
                sb.AppendLine($"Order for {order.Customer} on {order.DateTime.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)} added");
            }

            context.Orders.AddRange(validOrders);
            context.SaveChanges();

            var result = sb.ToString().TrimEnd();
            return result;
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);

            return isValid;
        }
    }
}