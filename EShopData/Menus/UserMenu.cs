using EShopData.Common;
using EShopData.Models;
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

        public UserMenu(ConsoleHelper consoleHelper, ProductMenu productMenu, CartMenu cartMenu)
        {
            this.consoleHelper = consoleHelper;
            this.productMenu = productMenu;
            this.cartMenu = cartMenu;
        }

        public void Show()
        {
            var exit = false;

            var menu = new List<MenuItem>
            {
               new MenuItem("Show all products",productMenu.ShowAllProducts),
               new MenuItem("Search products",()=>Console.WriteLine("Todo:Search")),
               new MenuItem("View cart",cartMenu.Show),
               new MenuItem("Back",()=>exit=true)
            };

            while (!exit)
            {
                var selected = consoleHelper.ShowArrowMenu("Welcome, Guest", menu.Select(mi => mi.Name).ToArray());
                menu[selected].Action();
            }
        }
    }
}
