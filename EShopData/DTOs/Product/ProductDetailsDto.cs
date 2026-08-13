using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.DTOs.Product
{
    public record ProductDetailsDto(
        int id,
        string Name,
        decimal Price,
        string CategoryName,
        string ProducerName,
        IEnumerable<string> Tags
        );
}
