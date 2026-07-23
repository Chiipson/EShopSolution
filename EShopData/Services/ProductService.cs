using EShopData.Data;
using EShopData.DTOs;
using EShopData.Entities;
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
            _context.Products.Select(p =>
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
                    p.Category.Name,
                    p.Producer.Name,
                    p.Tags.Select(t => t.Name)
                    ))
            .First();
    }
}
