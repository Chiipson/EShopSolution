using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.DTOs.User
{
    public record AddUserDto(
        string Email,
        string password,
        string FirstName,
        string LastName
        );
}
