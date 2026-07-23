using EShopData.Common;
using EShopData.DTOs;
using EShopData.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.Menus
{
    public class ProductMenu
    {
        private readonly ProductService productService;
        private readonly CartService cartService;
        private readonly ConsoleHelper consoleHelper;

        public ProductMenu(ProductService productService, ConsoleHelper consoleHelper, CartService cartService)
        {
            this.productService = productService;
            this.consoleHelper = consoleHelper;
            this.cartService = cartService;
        }

        public void ShowAllProducts()
        {
            var products = productService.GetProductList();

            var position = consoleHelper.ShowArrowMenu(
                "Products",
                products
                    .Select(p => $"{p.Id}.{p.Name}")
                    .Append("Go back")
                    .ToArray()
                );

            if(position>=products.Count)
            {
                return;
            }

            ShowProductDetails(productService.GetProductDetails(products[position].Id));
        }

        public void ShowProductDetails(ProductDetailsDto product)
        {
            var title =
                $"""
                Chosen product

                Name: {product.Name}
                Producer: {product.ProducerName}
                Category: {product.CategoryName}
                Tags: {string.Join(",", product.Tags)}
                Price: {product.Price}

                Add to cart:
                """;

            var option = consoleHelper.ShowArrowMenu(title, ["yes", "no"]);

            if(option == 0)
            {
                cartService.Add(new AddToCartDto(product.id, product.Name, product.Price));
            }
        }
    }
}
