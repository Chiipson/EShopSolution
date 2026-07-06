using EShopData.Entities;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;

namespace EShopData.Data.Seed
{
    public static class DbSeder
    {
        public static void Seed(EShopDbContext context)
        {
            var categories = new List<Category>()
            {
                    new() { Name = "Home plants" },
                    new() { Name = "Garden plants" },
                    new() { Name = "Tools" },
                    new() { Name = "Pots" },
                    new() { Name = "Solid" },
            };

            var producers = new List<Producer>()
            {
                    new() { Name = "Garden to Home" },
                    new() { Name = "Spot for pot" },
                    new() { Name = "All company" },
            };

            var tags = new List<Tag>()
            {
                    new() { Name = "Small" },
                    new() { Name = "Medium" },
                    new() { Name = "Big" },
            };

            if (!context.Categories.Any())
            {
                context.Categories.AddRange(categories);
            }

            if (!context.Producers.Any())
            {
                context.Producers.AddRange(producers);
            }

            if (!context.Tags.Any())
            {
                context.Tags.AddRange(tags);
            }

            if (!context.Products.Any())
            {
                context.Products.AddRange(
                    new Product()
                    {
                        Name = "Round pot",
                        Price = 3.14m,
                        StockQuantity = 100,
                        Category = categories[3],
                        Producer = producers[2],
                        Tags = [tags[0]]
                    },
                    new Product()
                    {
                        Name = "rose bush",
                        Price = 4m,
                        StockQuantity = 140,
                        Category = categories[1],
                        Producer = producers[0],
                        Tags = [tags[2]]
                    }
                    );
            }

            context.SaveChanges();
        }
    }
}
