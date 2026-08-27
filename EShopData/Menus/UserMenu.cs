using EShopData.Common;
using EShopData.DTOs;
using EShopData.DTOs.User;
using EShopData.Models;
using EShopData.Security;
using EShopData.Services;
using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Text;

namespace EShopData.Menus
{
    public class UserMenu
    {
        private readonly ConsoleHelper consoleHelper;
        private readonly ProductMenu productMenu;
        private readonly ProductBrowser productBrowser;
        private readonly CartMenu cartMenu;
        private readonly UserService userService;
        private readonly UserSession session;
        private readonly OrderMenu orderMenu;

        private bool exitFromUserMenu = false;

        public UserMenu(
            ConsoleHelper consoleHelper,
            ProductMenu productMenu,
            ProductBrowser productBrowser,
            CartMenu cartMenu,
            UserService userService,
            UserSession session,
            OrderMenu orderMenu
            )
        {
            this.consoleHelper = consoleHelper;
            this.productMenu = productMenu;
            this.productBrowser = productBrowser;
            this.cartMenu = cartMenu;
            this.userService = userService;
            this.session = session;
            this.orderMenu = orderMenu;
        }

        public void Show()
        {
            exitFromUserMenu = false;

            var menu = new List<MenuItem>
            {
               new ("Show all products",()=>productBrowser.ShowAllProducts(productMenu.ShowProductDetails)),
               new ("Search products",()=>productBrowser.SearchMenu(productMenu.ShowProductDetails)),
               new("Show filter products",()=>productBrowser.ShowFilteredProducts(productMenu.ShowProductDetails)),
               new ("View cart",cartMenu.Show)
            };

            string welcomeMessage;

            if (session.IsLoggedIn())
            {
                welcomeMessage = $"Welcome, {session.User.FirstName} {session.User.LastName}";

                menu.AddRange
                    ([
                        new("Account info", ShowAccountInfo),
                        new("Order history", orderMenu.ShowOrderHistory),
                        new("Logout", Logout),
                     ]);
            }
            else
            {
                welcomeMessage = "Welcome, Guest";

                menu.Add(new("Back", () => exitFromUserMenu = true));
            }

            while (!exitFromUserMenu)
            {
                var selected = consoleHelper.ShowArrowMenu(welcomeMessage, menu.Select(mi => mi.Name).ToArray());
                menu[selected].Action();
            }
        }

        public void Logout()
        {
            userService.Logout();
            exitFromUserMenu = true;
        }

        public bool Login()
        {
            Console.Clear();

            Console.WriteLine("Login window:\n");

            var email = consoleHelper.GetString("Enter email:");
            var password = consoleHelper.GetString("Enter password:");

            return userService.LogIn(email, password);
        }

        public void Register()
        {
            Console.Clear();

            Console.WriteLine("Registration window:\n");

            var email = consoleHelper.GetString("Enter email:");
            var password = consoleHelper.GetString("Enter password:");
            var firstName = consoleHelper.GetString("Enter first name:");
            var lastName = consoleHelper.GetString("Enter last name:");

            var state = userService.Register(new AddUserDto(
                email,
                password,
                firstName,
                lastName
                ));

            Console.Clear();

            if (state)
            {
                Console.WriteLine("Registration success");
            }
            else
            {
                Console.WriteLine("Registration fails");
            }
            Thread.Sleep(1000);
        }

        public void ShowAccountInfo()
        {
            var exit = false;

            var menu = new List<MenuItem>
            {
                new("Change profile info", ()=>
                {
                    Console.Clear();

                    var newFirstName = consoleHelper.GetString("New first name:");
                    var newLastName = consoleHelper.GetString("New last name:");

                    userService.EditUserInfo(new EditUserInfoDto(newFirstName, newLastName));
                }),
                new("Change Password", ()=>
                {
                    Console.Clear();

                    var newPassword = consoleHelper.GetString("Enter new password:");

                    userService.ChangePassword(newPassword);
                }),
                new("Delete profile", ()=>
                {
                    userService.DeleteUser(session.User.Id);
                    Logout();
                    exit = true;
                }),
                new("Back", ()=>{exit = true;}),
            };


            while (!exit)
            {
                var userInfo = userService.GetCurrentUserInfo();

                var output =
                    $"""
                Account informamation:

                First name: {userInfo.FirstName}
                LastName:   {userInfo.LastName}
                Email:      {userInfo.Email}

                Options:
                """;

                var chose = consoleHelper.ShowArrowMenu(output, menu.Select(m => m.Name).ToArray());

                menu[chose].Action();
            }
        }
    }
}
