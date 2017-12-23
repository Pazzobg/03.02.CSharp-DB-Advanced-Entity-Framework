namespace FastFood.Models
{
    using System;
    using System.Collections.Generic;
    using FastFood.Models.Enums;
    using System.ComponentModel.DataAnnotations;

    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string Customer { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        public OrderType Type { get; set; } = OrderType.ForHere;

        [Required]
        public decimal TotalPrice
        {
            get; set;
        }

        public int EmployeeId { get; set; }
        [Required]
        public Employee Employee { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }

    //private static void Calculate(orderitems)
    //{

    //}
}
