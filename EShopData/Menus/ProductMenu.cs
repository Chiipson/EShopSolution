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
        private readonly FilterMenu filterMenu;

        public ProductMenu(
            ProductService productService,
            ConsoleHelper consoleHelper,
            CartService cartService,
            CategoryService categoryService,
            FilterMenu filterMenu
            )
        {
            this.productService = productService;
            this.consoleHelper = consoleHelper;
            this.cartService = cartService;
            this.categoryService = categoryService;
            this.filterMenu = filterMenu;
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

        public void FilterMenu()
        {
            var filterOptions = filterMenu.Show();

            if (filterOptions == null)
            {
                return;
            }

            var products = productService.GetFilteredProducts(filterOptions);

            ShowProducts(products);
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
