using EShopData.Data;
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

        public IEnumerable<Product> GetAll()
        {
            return _context.Products.ToList();
        }
    }
}
