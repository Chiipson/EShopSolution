using EShopData.Common;
using EShopData.Menus.Admin;
using EShopData.Models;
using EShopData.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.Menus
{
    public class AdministratorMenu
    {
        private readonly ConsoleHelper consoleHelper;
        private readonly UserService userService;
        private readonly AdminProductMenu adminProductMenu;

        public AdministratorMenu(
            ConsoleHelper consoleHelper,
            UserService userService,
            AdminProductMenu adminProductMenu
            )
        {
            this.consoleHelper = consoleHelper;
            this.userService = userService;
            this.adminProductMenu = adminProductMenu;
        }

        public void Show()
        {
            var exit = false;

            var menu = new List<MenuItem>
            {
                new("Product", adminProductMenu.Show),
                new("Categories",()=>{ }),
                new("Producer",()=>{ }),
                new("Tags",()=>{ }),
                new("Users",()=>{ }),
                new("Logout",()=>
                {
                    exit = true;
                    userService.Logout();
                }),
            };

            while(!exit)
            {
                var selected = consoleHelper.ShowArrowMenu("Administrator", menu.Select(m => m.Name).ToArray());

                menu[selected].Action();
            }
        }
    }
}
