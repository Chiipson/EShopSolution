using EShopData.Common;
using EShopData.DTOs.Product;
using EShopData.Models;
using EShopData.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.Menus
{
    public class ProductBrowser
    {
        private readonly ConsoleHelper consoleHelper;
        private readonly ProductService productService;
        private readonly CategoryService categoryService;
        private readonly FilterMenu filterMenu;
        public ProductBrowser(
            ConsoleHelper consoleHelper,
            ProductService productService,
            CategoryService categoryService,
            FilterMenu filterMenu
            )
        {
            this.consoleHelper = consoleHelper;
            this.productService = productService;
            this.categoryService = categoryService;
            this.filterMenu = filterMenu;
        }

        public void ShowAllProducts(Action<ProductDetailsDto> ShowProductDetails)
        {
            var products = productService.GetProductList();

            ShowProducts(products, ShowProductDetails);
        }

        public void SearchMenu(Action<ProductDetailsDto> ShowProductDetails)
        {
            var exit = false;

            var menu = new List<MenuItem>
            {
                new("By name", ()=>SearchByName(ShowProductDetails)),
                new("By category", ()=>SearchByCategory(ShowProductDetails)),
                new("Back", ()=> exit=true)
            };

            while (!exit)
            {
                var choice = consoleHelper.ShowArrowMenu("Search options:", menu.Select(m => m.Name).ToArray());

                menu[choice].Action();
            }
        }

        public void ShowFilteredProducts(Action<ProductDetailsDto> ShowProductDetails)
        {
            var filterOptions = filterMenu.Show();

            if (filterOptions == null)
            {
                return;
            }

            var products = productService.GetFilteredProducts(filterOptions);

            ShowProducts(products, ShowProductDetails);
        }

        private void ShowProducts(List<ProductsNamesListDto> products, Action<ProductDetailsDto> ShowProductDetails)
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
        private void SearchByName(Action<ProductDetailsDto> ShowProductDetails)
        {
            Console.Clear();

            var partOfProductName = consoleHelper.GetString("Enter product name:");

            var products = productService.GetProductListByName(partOfProductName);

            ShowProducts(products, ShowProductDetails);
        }

        private void SearchByCategory(Action<ProductDetailsDto> ShowProductDetails)
        {
            var categories = categoryService.GetCategoryList();

            var choice = consoleHelper.ShowArrowMenu("Choose category:", categories.Select(c => c.Name).ToArray());

            var products = productService.GetProductListByCategory(categories[choice].Id);

            ShowProducts(products,ShowProductDetails);
        }

    }
}
