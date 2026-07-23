using EShopData.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.Menus
{
    public class MainMenu
    {
        private readonly ConsoleHelper consoleHelper;
        private readonly GuestMenu guestMenu;

        public MainMenu(ConsoleHelper consoleHelper, GuestMenu guestMenu)
        {
            this.consoleHelper = consoleHelper;
            this.guestMenu = guestMenu;
        }
        public void Show()
        {
            var menuOptions = new string[]
            {
               "Login",
               "Register",
               "Continue as a Guest",
               "Exit",
            };
            while (true)
            {
                var selected = consoleHelper.ShowArrowMenu("Eshop Appliccation", menuOptions);

                switch (selected)
                {
                    case 0:
                        //TODO: login menu
                        break;
                    case 1:
                        //TODO: register menu
                        break;
                    case 2:
                        //TODO: Guest menu
                        break;
                    case 3:
                        Console.WriteLine("Exit");
                        return;
                }
            }

        }
    }
}
