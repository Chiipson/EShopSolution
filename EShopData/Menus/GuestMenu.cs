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

        public GuestMenu(ConsoleHelper consoleHelper, ProductMenu productMenu)
        {
            this.consoleHelper = consoleHelper;
            this.productMenu = productMenu;
        }

        public void Show()
        {
            var menuOptions = new string[]
            {
               "Show all products",
               "Search products",
               "View cart",
               "Go back",
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
                        break;
                    case 3:
                        return;
                }
            }
        }
    }
}
