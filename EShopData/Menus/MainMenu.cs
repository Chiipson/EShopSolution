using EShopData.Common;
using EShopData.Models;
using EShopData.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.Menus
{
    public class MainMenu
    {
        private readonly ConsoleHelper consoleHelper;
        private readonly UserMenu userMenu;
        private readonly AdministratorMenu administratorMenu;
        private readonly UserSession userSession;

        public MainMenu(
            ConsoleHelper consoleHelper, 
            UserMenu userMenu,
            AdministratorMenu administratorMenu, 
            UserSession userSession
            )
        {
            this.consoleHelper = consoleHelper;
            this.userMenu = userMenu;
            this.administratorMenu = administratorMenu;
            this.userSession = userSession;
        }
        public void Show()
        {
            var exit = false;

            var menu = new List<MenuItem>
            {
               new MenuItem("Login", ()=>{
                    if(userMenu.Login())
                    {
                       if(userSession.User.Role == Enums.Role.User)
                       {
                           userMenu.Show();
                       }
                       else
                       {
                           administratorMenu.Show();
                       }
                    }
                    else
                    {
                       Thread.Sleep(1000);
                       Console.WriteLine("Fail");
                    }
                   }),
               new MenuItem("Register", userMenu.Register),
               new MenuItem("Continue as a Guest", userMenu.Show),
               new MenuItem("Exit", ()=> exit=true)
            };

            while (!exit)
            {
                var selected = consoleHelper.ShowArrowMenu("Eshop Appliccation", menu.Select(mi => mi.Name).ToArray());

                menu[selected].Action();
            }
        }
    }
}
