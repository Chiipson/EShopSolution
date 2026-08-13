using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.DTOs.Order
{
    public record OrderItemDetailsDto(
        string Productname,
        decimal UnitPrice,
        int Quantity
        );
}
