using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.Models
{
    public class FilterOptions
    {
        public decimal? PriceLowerBound { get; set; }
        public decimal? PriceUpperBound { get; set; }
        public List<int> CategoryIds {  get; set; } = new();
        public List<int> TagIds { get; set; } = new();
        public List<int> ProducerIds { get; set; } = new();
    }
}
