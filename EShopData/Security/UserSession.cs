using EShopData.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.Security
{
    public class UserSession
    {
        public UserSessionDto? User { get; private set; }
        public bool IsLoggenIn() => User != null;

        public void Login(UserSessionDto userSessionDto)
        {
            this.User = userSessionDto;
        }

        public void Logout()
        {
            this.User = null;
        }
    }
}
