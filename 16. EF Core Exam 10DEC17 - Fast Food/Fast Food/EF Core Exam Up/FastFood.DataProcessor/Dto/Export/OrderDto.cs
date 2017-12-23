﻿using System.Collections.Generic;

namespace FastFood.DataProcessor.Dto.Export
{
    public class OrderDto
    {
        public string Customer { get; set; }

        public ICollection<ItemDto> Items { get; set; } = new List<ItemDto>();

        public decimal TotalPrice { get; set; }
    }
}