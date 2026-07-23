using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.DTOs
{
    public record AddToCartDto(
        int ProductId,
        string Name,
        decimal Price
        );
}
