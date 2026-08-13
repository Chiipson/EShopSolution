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
        private readonly ProductService productService;
        private readonly CartService cartService;
        private readonly CategoryService categoryService;
        private readonly ConsoleHelper consoleHelper;

        public ProductMenu(
            ProductService productService, 
            ConsoleHelper consoleHelper, 
            CartService cartService, 
            CategoryService categoryService
            )
        {
            this.productService = productService;
            this.consoleHelper = consoleHelper;
            this.cartService = cartService;
            this.categoryService = categoryService;
        }

        public void ShowAllProducts()
        {
            var products = productService.GetProductList();

            ShowProducts(products);
        }

        public void SearchMenu()
        {
            var exit = false;

            var menu = new List<MenuItem>
            {
                new("By name", SearchByName),
                new("By category", SearchByCategory),
                new("Back", ()=> exit=true)
            };

            while (!exit)
            {
                var choice = consoleHelper.ShowArrowMenu("Search options:", menu.Select(m => m.Name).ToArray());

                menu[choice].Action();
            }
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

            if (option == 0)
            {
                cartService.Add(product.id, 1);
            }
        }

        private void ShowProducts(List<ProductsNamesListDto> products)
        {
            var position = consoleHelper.ShowArrowMenu(
                "Products",
                products
                    .Select(p => $"{p.Name}")
                    .Append("Back")
                    .ToArray()
                );

            if (position >= products.Count)
            {
                return;
            }

            ShowProductDetails(productService.GetProductDetails(products[position].Id));
        }

        private void SearchByName()
        {
            Console.Clear();

            var partOfProductName = consoleHelper.GetString("Enter product name:");

            var products = productService.GetProductListByName(partOfProductName);

            ShowProducts(products);
        }

        private void SearchByCategory()
        {
            var categories = categoryService.GetCategoryList();

            var choice = consoleHelper.ShowArrowMenu("Choose category:", categories.Select(c => c.Name).ToArray());

            var products = productService.GetProductListByCategory(categories[choice].Id);

            ShowProducts(products);
        }
    }
}
