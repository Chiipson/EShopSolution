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
        private readonly CartMenu cartMenu;
        private readonly UserService userService;
        private readonly UserSession session;
        private readonly OrderMenu orderMenu;

        public UserMenu(
            ConsoleHelper consoleHelper,
            ProductMenu productMenu,
            CartMenu cartMenu,
            UserService userService,
            UserSession session,
            OrderMenu orderMenu
            )
        {
            this.consoleHelper = consoleHelper;
            this.productMenu = productMenu;
            this.cartMenu = cartMenu;
            this.userService = userService;
            this.session = session;
            this.orderMenu = orderMenu;
        }

        public void Show()
        {
            var exit = false;

            var menu = new List<MenuItem>
            {
               new ("Show all products",productMenu.ShowAllProducts),
               new ("Search products",productMenu.SearchMenu),
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
                        new("Logout",()=>
                        {
                            userService.Logout();
                            exit = true;
                        }),
                     ]);
            }
            else
            {
                welcomeMessage = "Welcome, Guest";

                menu.Add(new("Back", () => exit = true));
            }

            while (!exit)
            {
                var selected = consoleHelper.ShowArrowMenu(welcomeMessage, menu.Select(mi => mi.Name).ToArray());
                menu[selected].Action();
            }
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
            var userInfo = userService.GetCurrentUserInfo();

            Console.Clear();

            var output =
                $"""
                Account informamation:

                First name: {userInfo.FirstName}
                LastName:   {userInfo.LastName}
                Email:      {userInfo.Email}
                """;

            consoleHelper.ShowArrowMenu(output, ["back"]);
        }
    }
}
