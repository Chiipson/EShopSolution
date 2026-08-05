using EShopData.Common;
using EShopData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.Menus
{
    public class MainMenu
    {
        private readonly ConsoleHelper consoleHelper;
        private readonly UserMenu userMenu;

        public MainMenu(ConsoleHelper consoleHelper, UserMenu userMenu)
        {
            this.consoleHelper = consoleHelper;
            this.userMenu = userMenu;
        }
        public void Show()
        {
            var exit = false;

            var menu = new List<MenuItem>
            {
               new MenuItem("Login", userMenu.Login),
               new MenuItem("Register", userMenu.Register),
               new MenuItem("Continue as a Guest", userMenu.Show),
               new MenuItem("Exit", ()=> exit=true)
            };

            while (!exit)
            {
                var selected = consoleHelper.ShowArrowMenu("Eshop Appliccation", menu.Select(mi=>mi.Name).ToArray());

                menu[selected].Action();
            }
        }
    }
}
