using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.DTOs.Product
{
    public class AddProductDto
    {
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
        public int ProducerId { get; set; }
        public List<int> TagIds { get; set; } = new();
    }
}
