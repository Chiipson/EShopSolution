using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.DTOs.Cart
{
    public record CartItemDetailsDto(
        int ProductId,
        string ProductName,
        int Quantity,
        decimal Price
        );
}
