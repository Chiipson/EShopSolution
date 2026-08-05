using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.DTOs
{
    public record AddUserDto(
        string Email,
        string password,
        string FirstName,
        string LastName
        );
}
