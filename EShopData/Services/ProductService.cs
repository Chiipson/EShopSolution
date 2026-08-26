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
        private readonly EShopDbContext context;

        public ProductService(EShopDbContext context)
        {
            this.context = context;
        }

        public List<ProductsNamesListDto> GetProductList() =>
            context.Products
                .Select(p =>
                    new ProductsNamesListDto(
                        p.Id,
                        p.Name
                    ))
                .ToList();

        public List<ProductsNamesListDto> GetProductListByName(string productName) =>
            context.Products
                .Where(p => EF.Functions.ILike(p.Name, $"%{productName}%"))
                .Select(p =>
                    new ProductsNamesListDto(
                        p.Id,
                        p.Name
                    ))
                .ToList();

        public List<ProductsNamesListDto> GetProductListByCategory(int categoryId) =>
            context.Products
                .Where(p => p.CategoryId == categoryId)
                .Select(p =>
                    new ProductsNamesListDto(
                        p.Id,
                        p.Name
                    ))
                .ToList();

        public ProductDetailsDto GetProductDetails(int id) =>
           context.Products
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
            var query = context.Products.AsQueryable();

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

        public void AddProduct(AddProductDto newProduct)
        {
            var tags = context.Tags.Where(t => newProduct.TagIds.Contains(t.Id)).ToList();

            var product = new Product
            {
                Name = newProduct.Name,
                CategoryId = newProduct.CategoryId,
                ProducerId = newProduct.ProducerId,
                Price = newProduct.Price,
                StockQuantity = newProduct.StockQuantity,
                Tags = tags
            };

            context.Products.Add(product);

            context.SaveChanges();
        }

        public void DeleteProduct(int productId)
        {
            var product = context.Products.Find(productId);

            if (product != null)
            {
                context.Products.Remove(product);

                context.SaveChanges();
            }
        }

        public void UpdateProduct(int productId, EditProductDto editProductDto)
        {
            var product = context.Products.Find(productId);

            if (product == null)
            {
                throw new InvalidOperationException("Product not found");
            }

            if (editProductDto.Name != null)
            {
                product.Name = editProductDto.Name;
            }

            if (editProductDto.Price.HasValue)
            {
                product.Price = editProductDto.Price.Value;
            }

            if (editProductDto.CategoryId.HasValue)
            {
                product.CategoryId = editProductDto.CategoryId.Value;
            }

            if (editProductDto.ProducerId.HasValue)
            {
                product.ProducerId = editProductDto.ProducerId.Value;
            }

            if(editProductDto.TagIds != null)
            {
                var tags = context.Tags.Where(t => editProductDto.TagIds.Contains(t.Id)).ToList();

                product.Tags = tags;
            }

            context.SaveChanges();
        }
    }
}
