using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.DTOs
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
