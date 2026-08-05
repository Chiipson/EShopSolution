using System;
using System.Collections.Generic;
using System.Text;
using BCrypt.Net;

namespace EShopData.Security
{
    public class PasswordHasher
    {
        public bool Verify(string password, string userPasswordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password,userPasswordHash);
        }

        public string Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
