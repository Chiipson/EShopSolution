using EShopData.Data;
using EShopData.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.Services
{
    public class TagService
    {
        private readonly EShopDbContext _context;

        public TagService(EShopDbContext context)
        {
            _context = context;
        }

        public List<IdNameDto> GetTagList() =>
            _context.Tags
                .Select(c =>
                    new IdNameDto(
                        c.Id,
                        c.Name
                    ))
                .ToList();
    }
}
