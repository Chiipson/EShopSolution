using EShopData.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.DTOs.User
{
    public record UserSessionDto(
        int Id,
        string FirstName,
        string LastName,
        Role Role
        );
}
