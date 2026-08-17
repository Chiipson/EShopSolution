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

        public FilterMenu(
            ConsoleHelper consoleHelper,
            CategoryService categoryService,
            TagService tagService,
            ProducerService producerService
            )
        {
            this.consoleHelper = consoleHelper;
            this.categoryService = categoryService;
            this.tagService = tagService;
            this.producerService = producerService;
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

                    filterOptions.PriceLowerBound = consoleHelper.GetDecimal("Enter lower bound of price:");
                    filterOptions.PriceUpperBound = consoleHelper.GetDecimal("Enter upper bound of price:");
                }),
                new("Category", ()=>
                {
                    chosenCategories = ShowCheckBoxMenu(
                        "Categories: ",
                        categoryIdsNames.Select(c=>c.Name),
                        chosenCategories);
                }),
                new("Tag", ()=>
                {
                    chosenTags = ShowCheckBoxMenu(
                        "Tags: ",
                        tagIdsNames.Select(t=>t.Name),
                        chosenTags);
                }),
                new("Producer", ()=>
                {
                    chosenProducers = ShowCheckBoxMenu(
                        "Producer: ",
                        producerIdsNames.Select(p=>p.Name),
                        chosenProducers);
                }),
                new("Apply filtration", ()=>
                {
                    filterOptions.TagIds = 
                        GetIdsOfChosenOptions(chosenTags, tagIdsNames.Select(t=>t.Id).ToArray());
                    filterOptions.CategoryIds = 
                        GetIdsOfChosenOptions(chosenCategories, categoryIdsNames.Select(c=>c.Id).ToArray());
                    filterOptions.ProducerIds = 
                        GetIdsOfChosenOptions(chosenProducers, producerIdsNames.Select(p=>p.Id).ToArray());

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

        private bool[] ShowCheckBoxMenu(string title, IEnumerable<string> options, bool[] chosenOptions)
        {
            var exit = false;

            while (!exit)
            {
                var chosen = consoleHelper
                    .ShowArrowMenu(
                        title,
                        options
                            .Select((option, index) => $"{(chosenOptions[index] ? "[x]" : "[ ]")} {option}")
                            .Append("Back")
                            .ToArray()
                        );

                if (chosen < options.Count())
                {
                    chosenOptions[chosen] = !chosenOptions[chosen];
                }
                else
                {
                    exit = true;
                }
            }

            return chosenOptions;
        }

        private List<int> GetIdsOfChosenOptions(bool[] chosenOptions, int[] allIds)
        {
            var ids = new List<int>();

            for (int i = 0; i < chosenOptions.Length; i++)
            {
                if (chosenOptions[i])
                {
                    ids.Add(allIds[i]);
                }
            }

            return ids;
        }
    }
}
