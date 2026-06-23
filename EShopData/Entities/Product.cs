using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; } 
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int ProducerId { get; set; }
        public Producer Producer { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public ICollection<Tag> Tags { get; set; }
    }
}
