using EShopData.Data;
using EShopData.DTOs;
using EShopData.DTOs.User;
using EShopData.Entities;
using EShopData.Enums;
using EShopData.Security;
using Microsoft.EntityFrameworkCore;
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
        private readonly CartService cartService;

        public UserService(
            EShopDbContext context, 
            PasswordHasher passwordHasher, 
            UserSession session, 
            CartService cartService
            )
        {
            this.context = context;
            this.passwordHasher = passwordHasher;
            this.session = session;
            this.cartService = cartService;
        }

        public bool LogIn(string email, string password)
        {
            var user = context.Users
                .FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                return false;
            }

            if (!passwordHasher.Verify(password, user.PasswordHash))
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

            cartService.MergeUserAndGuestcarts();

            return true;
        }

        public void Logout()
        {
            session.Logout();
        }

        public bool Register(AddUserDto userData)
        {
            if (context.Users.FirstOrDefault(u => u.Email == userData.Email) != null)
            {
                return false;
            }

            var user = new User()
            {
                Email = userData.Email,
                PasswordHash = passwordHasher.Hash(userData.password),
                Role = Role.User,
                CreatedAt = DateTime.UtcNow,
                UserInfo = new UserInfo
                {
                    FirstName = userData.FirstName,
                    LastName = userData.LastName
                }
            };

            context.Users.Add(user);
            context.SaveChanges();

            return true;
        }

        public UserInfoDto GetCurrentUserInfo()
        {
            if(!session.IsLoggedIn())
            {
                throw new InvalidOperationException("User isn't logged in.");
            }

            var user = context.Users
                .Where(u => u.Id == session.User.Id)
                .Select(u => new UserInfoDto(
                    u.Email,
                    u.UserInfo.FirstName,
                    u.UserInfo.LastName
                ))
                .FirstOrDefault();

            return user == null ? throw new InvalidOperationException("User not found.") : user;
        }
    }
}
