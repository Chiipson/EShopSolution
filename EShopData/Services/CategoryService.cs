using EShopData.Data;
using EShopData.DTOs.Category;
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

        public List<CategoryNamesDto> GetCategoryList() => 
            _context.Categories
                .Select(c=> 
                    new CategoryNamesDto(
                        c.Id,
                        c.Name
                    ))
                .ToList();
    }
}
