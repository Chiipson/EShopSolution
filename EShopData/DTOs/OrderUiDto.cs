using EShopData.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.DTOs
{
    public record OrderUiDto(
        int Id,
        DateTime CreatedAt,
        OrderStatus OrderStatus
        );
}
