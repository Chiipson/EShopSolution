using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.DTOs
{
    public record OrderItemDetailsDto(
        string Productname,
        decimal UnitPrice,
        int Quantity
        );
}
