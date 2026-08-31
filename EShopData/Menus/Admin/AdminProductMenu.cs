using EShopData.Common;
using EShopData.DTOs.Product;
using EShopData.Models;
using EShopData.Services;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
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

        public void ShowProductDetails(ProductDetailsDto productInfo)
        {
            var exit = false;

            var title = GetProductInfoString(productInfo);

            var options = new List<MenuItem>
            {
                new("Edit", ()=>
                {
                    if(EditProduct(productInfo))
                    {
                        consoleHelper.PrintUserMessage("Product successfully edit", 2);
                        productInfo = productService.GetProductDetails(productInfo.id);
                        title = GetProductInfoString(productInfo);
                    }
                    else
                    {
                        consoleHelper.PrintUserMessage("Error while editing, operation is canceled", 2);
                    }
                }),
                new("Edit stock", () =>
                {
                    if(EditProductStock(productInfo.id,productInfo.StockQuantity))
                    {
                        consoleHelper.PrintUserMessage("Product stock successfully edit", 2);
                        productInfo = productService.GetProductDetails(productInfo.id);
                        title = GetProductInfoString(productInfo);
                    }
                    else
                    {
                        consoleHelper.PrintUserMessage("Error while editing stock, operation is canceled", 2);
                    }
                }),
                new("Delete", ()=>
                {
                    DeleteProduct(productInfo.id);
                    exit = true;
                }),
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

            var addingProduct = new AddProductDto();

            addingProduct.Name = consoleHelper.GetString("product name:");
            Console.Clear();

            addingProduct.Price = consoleHelper.GetNumber<decimal>("price:");
            Console.Clear();

            addingProduct.StockQuantity = consoleHelper.GetNumber<int>("product quantity:");

            var categoriesIdName = categoryService.GetCategoryList();
            var selectedCategoryRow = consoleHelper.ShowArrowMenu("Chose category:", categoriesIdName.Select(c => c.Name).ToArray());
            addingProduct.CategoryId = categoriesIdName[selectedCategoryRow].Id;

            var producersIdName = producerService.GetProducerList();
            var selectedProducerRow = consoleHelper.ShowArrowMenu("Chose producer:", producersIdName.Select(p => p.Name).ToArray());
            addingProduct.ProducerId = producersIdName[selectedProducerRow].Id;

            var tagsIdName = tagService.GetTagList();
            var selectedTagsRow = consoleHelper.ShowCheckBoxMenu("Chose tags:", "Enter", tagsIdName.Select(t => t.Name), new bool[tagsIdName.Count]);
            addingProduct.TagIds = convertingHelper.GetIdsOfChosenOptions(selectedTagsRow, tagsIdName.Select(t => t.Id).ToArray());

            productService.AddProduct(addingProduct);

            consoleHelper.PrintUserMessage("Product successfully added", 2);
        }

        public bool EditProduct(ProductDetailsDto productDetails)
        {
            var exit = false;

            var isEdited = false;

            var editProductInfo = new EditProductDto();

            var editMenu = new List<MenuItem>
            {
                new("Name", ()=>
                {
                Console.Clear();
                editProductInfo.Name = consoleHelper.GetString("new name:");
                }),
                new("Producer", ()=>
                {
                    var producersIdName = producerService.GetProducerList();
                    var selectedProducerRow = consoleHelper.ShowArrowMenu("Chose producer:", producersIdName.Select(p => p.Name).ToArray());
                    editProductInfo.ProducerId = producersIdName[selectedProducerRow].Id;
                }),
                new("Category", ()=>
                {
                    var categoriesIdName = categoryService.GetCategoryList();
                    var selectedCategoryRow = consoleHelper.ShowArrowMenu("Chose category:", categoriesIdName.Select(c => c.Name).ToArray());
                    editProductInfo.CategoryId =  categoriesIdName[selectedCategoryRow].Id;
                }),
                new("Tags", ()=>
                {
                    var tagsIdName = tagService.GetTagList();
                    var selectedTagsRow = consoleHelper.ShowCheckBoxMenu("Chose tags:", "Enter", tagsIdName.Select(t => t.Name), new bool[tagsIdName.Count]);
                    editProductInfo.TagIds = convertingHelper.GetIdsOfChosenOptions(selectedTagsRow, tagsIdName.Select(t => t.Id).ToArray());
                }),
                new("Price", ()=>
                {
                    Console.Clear();
                    editProductInfo.Price = consoleHelper.GetNumber<decimal>("new price:");
                }),
                new("Apply", ()=>
                {
                    isEdited = productService.UpdateProduct(productDetails.id,editProductInfo);
                    exit = true;
                }),
                new("Back", ()=>
                {
                    exit=true;
                })
            };

            while (!exit)
            {
                var selected = consoleHelper.ShowArrowMenu("Product editing:", editMenu.Select(m => m.Name).ToArray());

                editMenu[selected].Action();
            }

            return isEdited;
        }

        public bool EditProductStock(int productId, int previousQuantity)
        {
            var isSockEdit = false;

            var stockMenu = new List<MenuItem>
            {
                new("Set new quantity", ()=>
                {
                    Console.Clear();

                    isSockEdit = productService.SetQuantity(
                        productId,
                        consoleHelper.GetNumber<int>("New quantity:")
                        );
                }),
                new("Increase the amount of product",()=>
                {
                    Console.Clear();

                    isSockEdit = productService.SetQuantity(
                        productId,
                        previousQuantity + consoleHelper.GetNumber<int>("Amount to add:")
                        );
                }),
                new("Reduce the amount of product", ()=>
                {
                    Console.Clear();

                    isSockEdit = productService.SetQuantity(
                        productId,
                        previousQuantity - consoleHelper.GetNumber<int>("Amount to reduce:")
                        );
                }),
                new("Back",()=>{ })
            };

            Console.Clear();

            var selected = consoleHelper.ShowArrowMenu("Product editing:", stockMenu.Select(m => m.Name).ToArray());

            stockMenu[selected].Action();

            return isSockEdit;
        }

        public void DeleteProduct(int productId)
        {
            productService.DeleteProduct(productId);

            consoleHelper.PrintUserMessage("Product successfully deleted", 2);
        }

        private string GetProductInfoString(ProductDetailsDto product)
        {
            return
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
        }

    }
}
