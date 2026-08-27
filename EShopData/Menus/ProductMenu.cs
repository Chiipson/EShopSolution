using EShopData.Common;
using EShopData.DTOs.Product;
using EShopData.Models;
using EShopData.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.Menus
{
    public class ProductMenu
    {
        private readonly CartService cartService;
        private readonly ConsoleHelper consoleHelper;

        public ProductMenu(
            ConsoleHelper consoleHelper,
            CartService cartService
            )
        {
            this.consoleHelper = consoleHelper;
            this.cartService = cartService;
        }

        public void ShowProductDetails(ProductDetailsDto product)
        {
            var availability = product.StockQuantity switch
            {
                <= 0 => "Out of stock",
                <= 5 => $"Only {product.StockQuantity} left",
                _ => "In stock"
            };

            var title =
                $"""
                Chosen product

                Name: {product.Name}
                Producer: {product.ProducerName}
                Category: {product.CategoryName}
                Tags: {string.Join(",", product.Tags)}
                Price: {product.Price}
                Availability: {availability}

                Options:
                """;

            var option = consoleHelper.ShowArrowMenu(title, ["Add to cart", "back"]);

            if (option == 0)
            {
                Console.Clear();

                var number = consoleHelper.GetNumber<int>("Enter number of product:");

                if (cartService.Add(product.id, number))
                {
                    consoleHelper.PrintUserMessage("The products were added to cart.", 3);
                }
                else
                {
                    consoleHelper.PrintUserMessage("The products weren't added to cart.\nNot enough items in stock", 3);
                }
            }
        }
    }
}
