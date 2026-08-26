using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.DTOs.Product
{
    public record AddProductDto(
        string Name,
        decimal Price,
        int StockQuantity,
        int CategoryId,
        int ProducerId,
        List<int> TagIds
        );
}
