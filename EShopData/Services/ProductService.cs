using EShopData.Data;
using EShopData.DTOs.Product;
using EShopData.Entities;
using EShopData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.Services
{
    public class ProductService
    {
        private readonly EShopDbContext _context;

        public ProductService(EShopDbContext context)
        {
            _context = context;
        }

        public List<ProductsNamesListDto> GetProductList() =>
            _context.Products
                .Select(p =>
                    new ProductsNamesListDto(
                        p.Id,
                        p.Name
                    ))
                .ToList();

        public List<ProductsNamesListDto> GetProductListByName(string productName) =>
            _context.Products
                .Where(p => EF.Functions.ILike(p.Name, $"%{productName}%"))
                .Select(p =>
                    new ProductsNamesListDto(
                        p.Id,
                        p.Name
                    ))
                .ToList();

        public List<ProductsNamesListDto> GetProductListByCategory(int categoryId) =>
            _context.Products
                .Where(p => p.CategoryId == categoryId)
                .Select(p =>
                    new ProductsNamesListDto(
                        p.Id,
                        p.Name
                    ))
                .ToList();

        public ProductDetailsDto GetProductDetails(int id) =>
           _context.Products
                .Where(p => p.Id == id)
                .Select(p =>
                    new ProductDetailsDto(
                        p.Id,
                        p.Name,
                        p.Price,
                        p.StockQuantity,
                        p.Category.Name,
                        p.Producer.Name,
                        p.Tags.Select(t => t.Name)
                    ))
                .First();

        public List<ProductsNamesListDto> GetFilteredProducts(FilterOptions filterOptions)
        {
            var query = _context.Products.AsQueryable();

            if (filterOptions.PriceUpperBound != null)
            {
                query = query.Where(p => p.Price <= filterOptions.PriceUpperBound);
            }

            if (filterOptions.PriceLowerBound != null)
            {
                query = query.Where(p => p.Price >= filterOptions.PriceLowerBound);
            }

            if (filterOptions.CategoryIds.Count > 0)
            {
                query = query.Where(p => filterOptions.CategoryIds.Contains(p.CategoryId));
            }

            if (filterOptions.ProducerIds.Count > 0)
            {
                query = query.Where(p => filterOptions.ProducerIds.Contains(p.ProducerId));
            }

            if (filterOptions.TagIds.Count > 0)
            {
                query = query.Where(p => p.Tags.Any(t => filterOptions.TagIds.Contains(t.Id)));
            }

            return query.Select(p =>
                    new ProductsNamesListDto(
                        p.Id,
                        p.Name
                    ))
                .ToList();
        }
    }
}
