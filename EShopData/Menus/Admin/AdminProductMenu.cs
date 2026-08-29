using EShopData.Common;
using EShopData.DTOs.Product;
using EShopData.Models;
using EShopData.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.Menus.Admin
{
    public class AdminProductMenu
    {
        private readonly ConsoleHelper consoleHelper;
        private readonly ProductBrowser productBrowser;
        private readonly ProductService productService;
        private readonly CategoryService categoryService;
        private readonly ProducerService producerService;
        private readonly TagService tagService;
        private readonly ConvertingHelper convertingHelper;

        public AdminProductMenu(
            ConsoleHelper consoleHelper,
            ProductBrowser productBrowser,
            ProductService productService,
            CategoryService categoryService,
            ProducerService producerService,
            TagService tagService,
            ConvertingHelper convertingHelper
            )
        {
            this.consoleHelper = consoleHelper;
            this.productBrowser = productBrowser;
            this.productService = productService;
            this.categoryService = categoryService;
            this.producerService = producerService;
            this.tagService = tagService;
            this.convertingHelper = convertingHelper;
        }

        public void Show()
        {
            var exit = false;

            var menu = new List<MenuItem>
            {
                new("Show all products", ()=> productBrowser.ShowAllProducts(ShowProductDetails)),
                new("Show filter products",()=> productBrowser.ShowFilteredProducts(ShowProductDetails)),
                new("Search product", () => productBrowser.SearchMenu(ShowProductDetails)),
                new("Add product", AddProduct),
                new("Back", ()=>{ exit = true; })
            };

            while (!exit)
            {
                var selected = consoleHelper.ShowArrowMenu("Product menu", menu.Select(m => m.Name).ToArray());

                menu[selected].Action();
            }
        }

        public void ShowProductDetails(ProductDetailsDto product)
        {
            var title =
                $"""
                Product

                Name: {product.Name}
                Producer: {product.ProducerName}
                Category: {product.CategoryName}
                Tags: {string.Join(",", product.Tags)}
                Price: {product.Price}
                Stock: {product.StockQuantity}

                Options:
                """;

            var exit = false;

            var options = new List<MenuItem>
            {
                new("Edit", EditProduct),
                new("Edit stock",DeleteProduct),
                new("Delete",DeleteProduct),
                new("Back", ()=> exit=true)
            };


            while (!exit)
            {
                var selected = consoleHelper.ShowArrowMenu(title, options.Select(m => m.Name).ToArray());

                options[selected].Action();
            }
        }

        public void AddProduct()
        {
            consoleHelper.PrintUserMessage("Product adding form", 2);
            Console.Clear();

            var name = consoleHelper.GetString("product name:");
            Console.Clear();

            var price = consoleHelper.GetNumber<decimal>("price:");
            Console.Clear();

            var stockQuantity = consoleHelper.GetNumber<int>("product quantity:");

            var categoriesIdName = categoryService.GetCategoryList();
            var selectedCategoryRow = consoleHelper.ShowArrowMenu("Chose category:", categoriesIdName.Select(c=>c.Name).ToArray());
            var categoryId = categoriesIdName[selectedCategoryRow].Id;

            var producersIdName = producerService.GetProducerList();
            var selectedProducerRow = consoleHelper.ShowArrowMenu("Chose producer:", producersIdName.Select(p => p.Name).ToArray());
            var producerId = producersIdName[selectedProducerRow].Id;

            var tagsIdName = tagService.GetTagList();
            var selectedTagsRow = consoleHelper.ShowCheckBoxMenu("Chose tags:","Enter", tagsIdName.Select(t => t.Name), new bool[tagsIdName.Count]);
            var tagIds = convertingHelper.GetIdsOfChosenOptions(selectedTagsRow, tagsIdName.Select(t => t.Id).ToArray());

            productService.AddProduct(new AddProductDto(
                name,
                price,
                stockQuantity,
                categoryId,
                producerId,
                tagIds
                ));

            consoleHelper.PrintUserMessage("Product successfully added", 2);
        }

        public void EditProduct()
        {

        }

        public void DeleteProduct()
        {
            
        }
    }
}
