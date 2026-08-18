using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.DTOs.User
{
    public record EditUserInfoDto(
        string FirstName,
        string LastName
        );
}
