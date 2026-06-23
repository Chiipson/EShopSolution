using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.Entities
{
    public class Producer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
