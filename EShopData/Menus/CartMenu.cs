using EShopData.Common;
using EShopData.Models;
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

        public CartMenu(CartService cartService, ConsoleHelper consoleHelper)
        {
            this.cartService = cartService;
            this.consoleHelper = consoleHelper;
        }

        public void Show()
        {
            while (true)
            {
                var cartItems = cartService.GetAll();

                var output = new StringBuilder();
                output.AppendLine("Produscts in cart:");
                for (int i = 0; i < cartItems.Count; i++)
                {
                    output.AppendLine($"{i + 1}.{FormatCartItem(cartItems[i])}");
                }

                string[] options = cartItems.Count != 0 ? ["Check out", "Remove product", "Clear cart", "Back"] : ["Back"];

                var result = consoleHelper.ShowArrowMenu(output.ToString(), options);

                switch (result)
                {
                    case 0 when cartItems.Count > 0:
                        Checkout();
                        break;
                    case 1:
                        RemoveProduct();
                        break;
                    case 2:
                        ClearCart();
                        break;
                    default:
                        return;
                }
            }
        }

        public void Checkout()
        {
            //TODO:check out
        }

        public void RemoveProduct()
        {
            var cartItems = cartService.GetAll();

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
        private static string FormatCartItem(CartItem item)
        {
            return $"{item.ProductName} x{item.Quantity}  Price: {item.Quantity * item.Price:C}";
        }
    }
}
