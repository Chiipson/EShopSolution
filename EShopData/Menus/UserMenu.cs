using EShopData.Common;
using EShopData.DTOs;
using EShopData.Models;
using EShopData.Security;
using EShopData.Services;
using System;
using System.Collections.Generic;
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

        public UserMenu(
            ConsoleHelper consoleHelper,
            ProductMenu productMenu,
            CartMenu cartMenu,
            UserService userService,
            UserSession session
            )
        {
            this.consoleHelper = consoleHelper;
            this.productMenu = productMenu;
            this.cartMenu = cartMenu;
            this.userService = userService;
            this.session = session;
        }

        public void Show()
        {
            var exit = false;

            var menu = new List<MenuItem>
            {
               new ("Show all products",productMenu.ShowAllProducts),
               new ("Search products",()=>Console.WriteLine("Todo:Search")),
               new ("View cart",cartMenu.Show)
            };

            string welcomeMessage;

            if (session.IsLoggenIn())
            {
                welcomeMessage = $"Welcome, {session.User.FirstName} {session.User.LastName}";

                menu.AddRange
                    ([
                        new("Account info", () => { }),
                        new("Logout",()=>{ }),
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

        public void Login()
        {
            Console.Clear();

            Console.WriteLine("Login window:\n");

            var email = consoleHelper.GetString("Enter email:");
            var password = consoleHelper.GetString("Enter password:");

            Console.Clear();
            if (userService.LogIn(email, password))
            {
                Console.WriteLine("Login success");
                Show();
            }
            else
            {
                Console.WriteLine("Login fails");
            }
            Thread.Sleep(1000);
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
    }
}
