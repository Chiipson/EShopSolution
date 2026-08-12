using EShopData.Common;
using EShopData.DTOs;
using EShopData.Models;
using EShopData.Security;
using EShopData.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.Menus
{
    public class CartMenu
    {
        private readonly CartService cartService;
        private readonly ConsoleHelper consoleHelper;
        private readonly CheckoutService checkoutService;
        private readonly UserSession session;

        public CartMenu(CartService cartService, 
            ConsoleHelper consoleHelper, 
            CheckoutService checkoutService, 
            UserSession session)
        {
            this.cartService = cartService;
            this.consoleHelper = consoleHelper;
            this.checkoutService = checkoutService;
            this.session = session;
        }

        public void Show()
        {
            var exit = false;

            while (!exit)
            {
                var cartItems = cartService.GetCartItemsDetails();

                var output = new StringBuilder();
                output.AppendLine("Produscts in cart:");
                for (int i = 0; i < cartItems.Count; i++)
                {
                    output.AppendLine($"{i + 1}.{FormatCartItem(cartItems[i])}");
                }

                var menu = new List<MenuItem>();

                if(cartItems.Count != 0)
                {
                    menu.Add(new MenuItem("Check out", Checkout));
                    menu.Add(new MenuItem("Remove product", RemoveProduct));
                    menu.Add(new MenuItem("Clear cart", ClearCart));
                }
                menu.Add(new MenuItem("Back", ()=> exit=true));

                var selected = consoleHelper.ShowArrowMenu(output.ToString(), menu.Select(mi=>mi.Name).ToArray());

                menu[selected].Action();
            }
        }

        public void Checkout()
        {
            if(session.IsLoggedIn())
            {
                checkoutService.Checkout();

                Console.Clear();
                Console.WriteLine("Order was successfully added");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("You must log in before checking out");
            }

            Thread.Sleep(2000);
        }

        public void RemoveProduct()
        {
            var cartItems = cartService.GetCartItemsDetails();

            var result = consoleHelper.ShowArrowMenu("Chose product to remove:",
                cartItems.Select(ci => FormatCartItem(ci)).ToArray());

            //TODO: behavior, if more then 1 item per product
            cartService.RemoveProduct(cartItems[result].ProductId);
        }

        public void ClearCart()
        {
            var result = consoleHelper.ShowArrowMenu("Are you sure:", ["yes", "no"]);

            if (result == 0)
            {
                cartService.Clear();
            }
        }
        private static string FormatCartItem(CartItemDetailsDto item)
        {
            return $"{item.ProductName} x{item.Quantity}  Price: {item.Quantity * item.Price:C}";
        }
    }
}
