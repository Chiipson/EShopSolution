using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.DTOs
{
    public record ProductDetailsDto(
        string Name,
        decimal Price,
        string CategoryName,
        string ProducerName,
        IEnumerable<string> Tags
        );
}
