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

        public IEnumerable<ListProductDto> GetAll() =>
            _context.Products.Select(p =>
                new ListProductDto(
                    p.Name,
                    p.Price,
                    p.Category.Name,
                    p.Producer.Name,
                    p.Tags.Select(t => t.Name)
                    ))
                .ToList();

    }
}
