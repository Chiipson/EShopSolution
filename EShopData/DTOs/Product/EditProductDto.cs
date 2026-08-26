using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.DTOs.Product
{
    public record EditProductDto(
        string? Name,
        decimal? Price,
        int? CategoryId,
        int? ProducerId,
        List<int>? TagIds
        );
}
