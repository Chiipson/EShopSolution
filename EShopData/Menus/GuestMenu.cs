using EShopData.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.Menus
{
    public class GuestMenu
    {
        private readonly ConsoleHelper consoleHelper;
        private readonly ProductMenu productMenu;
        private readonly CartMenu cartMenu;

        public GuestMenu(ConsoleHelper consoleHelper, ProductMenu productMenu, CartMenu cartMenu)
        {
            this.consoleHelper = consoleHelper;
            this.productMenu = productMenu;
            this.cartMenu = cartMenu;
        }

        public void Show()
        {
            var menuOptions = new string[]
            {
               "Show all products",
               "Search products",
               "View cart",
               "Back",
            };

            while (true)
            {
                var selected = consoleHelper.ShowArrowMenu("Welcome, Guest", menuOptions);

                switch (selected)
                {
                    case 0:
                        productMenu.ShowAllProducts();
                        break;
                    case 1:
                        //TODO: register menu
                        break;
                    case 2:
                        cartMenu.Show();
                        break;
                    case 3:
                        return;
                }
            }
        }
    }
}
