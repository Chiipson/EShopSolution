using EShopData.Common;
using EShopData.Entities;
using EShopData.Models;
using EShopData.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.Menus
{
    public class FilterMenu
    {
        private readonly ConsoleHelper consoleHelper;
        private readonly CategoryService categoryService;
        private readonly TagService tagService;
        private readonly ProducerService producerService;
        private readonly ConvertingHelper convertingHelper;

        public FilterMenu(
            ConsoleHelper consoleHelper,
            CategoryService categoryService,
            TagService tagService,
            ProducerService producerService,
            ConvertingHelper convertingHelper
            )
        {
            this.consoleHelper = consoleHelper;
            this.categoryService = categoryService;
            this.tagService = tagService;
            this.producerService = producerService;
            this.convertingHelper = convertingHelper;
        }

        public FilterOptions? Show()
        {
            var categoryIdsNames = categoryService.GetCategoryList();
            var tagIdsNames = tagService.GetTagList();
            var producerIdsNames = producerService.GetProducerList();

            var chosenCategories = new bool[categoryIdsNames.Count];
            var chosenTags = new bool[tagIdsNames.Count];
            var chosenProducers = new bool[producerIdsNames.Count];

            var filterOptions = new FilterOptions();

            var exit = false;

            var menu = new List<MenuItem>
            {
                new("Price", ()=>
                {
                    Console.Clear();

                    filterOptions.PriceLowerBound = consoleHelper.GetNumber<decimal>("Enter lower bound of price:");
                    filterOptions.PriceUpperBound = consoleHelper.GetNumber<decimal>("Enter upper bound of price:");
                }),
                new("Category", ()=>
                {
                    chosenCategories = consoleHelper.ShowCheckBoxMenu(
                        "Categories: ",
                        "Back",
                        categoryIdsNames.Select(c=>c.Name),
                        chosenCategories);
                }),
                new("Tag", ()=>
                {
                    chosenTags = consoleHelper.ShowCheckBoxMenu(
                        "Tags: ",
                        "Back",
                        tagIdsNames.Select(t=>t.Name),
                        chosenTags
                        );
                }),
                new("Producer\n", ()=>
                {
                    chosenProducers = consoleHelper.ShowCheckBoxMenu(
                        "Producer: ",
                        "Back",
                        producerIdsNames.Select(p=>p.Name),
                        chosenProducers);
                }),
                new("Apply filtration", ()=>
                {
                    filterOptions.TagIds = 
                        convertingHelper.GetIdsOfChosenOptions(chosenTags, tagIdsNames.Select(t=>t.Id).ToArray());
                    filterOptions.CategoryIds =
                        convertingHelper.GetIdsOfChosenOptions(chosenCategories, categoryIdsNames.Select(c=>c.Id).ToArray());
                    filterOptions.ProducerIds =
                        convertingHelper.GetIdsOfChosenOptions(chosenProducers, producerIdsNames.Select(p=>p.Id).ToArray());

                    exit = true;
                }),
                new("Back", ()=>
                {
                    filterOptions = null;
                    exit = true;
                })
            };

            while (!exit)
            {
                var chosen = consoleHelper.ShowArrowMenu("Filtration", menu.Select(m => m.Name).ToArray());

                menu[chosen].Action();
            }

            return filterOptions;
        }
    }
}
