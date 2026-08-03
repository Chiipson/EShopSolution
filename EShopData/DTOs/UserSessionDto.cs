using EShopData.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.DTOs
{
    public record UserSessionDto(
        int Id,
        string Name,
        string Surname,
        Role Role
        );
}
