using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.DTOs.User
{
    public record UserInfoDto(
        string Email,
        string FirstName,
        string LastName
        );
}
