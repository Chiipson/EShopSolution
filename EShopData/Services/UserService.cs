using EShopData.Data;
using EShopData.DTOs;
using EShopData.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.Services
{
    public class UserService
    {
        private readonly EShopDbContext context;
        private readonly PasswordHasher passwordHasher;
        private readonly UserSession session;

        public UserService(EShopDbContext context, PasswordHasher passwordHasher, UserSession session)
        {
            this.context = context;
            this.passwordHasher = passwordHasher;
            this.session = session;
        }

        public bool LogIn(string email, string password)
        {
            var user = context.Users
                .FirstOrDefault(u=>u.Email== email);

            if (user == null)
            {
                return false;
            }

            if(!passwordHasher.Verify(password, user.PasswordHash))
            {
                return false;
            }

            var userInfo = context.UserInfos.Find(user.Id);

            session.Login(new UserSessionDto(
                user.Id,
                userInfo.FirstName,
                userInfo.LastName,
                user.Role
                ));

            return true;
        }

        public void LogOut()
        {
            session.Logout();
        }

        public void Register()
        {

        }
    }
}
