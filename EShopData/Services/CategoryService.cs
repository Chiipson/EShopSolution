using EShopData.Data;
using EShopData.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.Services
{
    public class CategoryService
    {
        private readonly EShopDbContext _context;

        public CategoryService(EShopDbContext context)
        {
            _context = context;
        }

        public Category GetById(int id) => _context.Categories.Find(id);

        public Category GetByName(string name) => _context.Categories.Single(c => c.Name == name);

        public IEnumerable<Category> GetAll() => _context.Categories.ToList();
    }
}
