using EShopData.Data;
using EShopData.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.Services
{
    public class ProducerService
    {
        private readonly EShopDbContext _context;

        public ProducerService(EShopDbContext context)
        {
            _context = context;
        }

        public List<IdNameDto> GetProducerList() =>
            _context.Producers
                .Select(c =>
                    new IdNameDto(
                        c.Id,
                        c.Name
                    ))
                .ToList();
    }
}
