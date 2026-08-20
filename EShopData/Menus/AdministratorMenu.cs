using EShopData.Common;
using EShopData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.Menus
{
    public class AdministratorMenu
    {
        private readonly ConsoleHelper consoleHelper;

        public AdministratorMenu(ConsoleHelper consoleHelper)
        {
            this.consoleHelper = consoleHelper;
        }

        public void Show()
        {
            var exit = false;

            var menu = new List<MenuItem>
            {
                new("Product",()=>{ }),
                new("Categories",()=>{ }),
                new("Producer",()=>{ }),
                new("Tags",()=>{ }),
                new("Users",()=>{ }),
                new("Logout",()=>{ }),
            };

            while(!exit)
            {
                var selected = consoleHelper.ShowArrowMenu("Administrator", menu.Select(m => m.Name).ToArray());

                menu[selected].Action();
            }
        }
    }
}
