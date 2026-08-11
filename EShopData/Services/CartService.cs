using EShopData.Data;
using EShopData.DTOs;
using EShopData.Entities;
using EShopData.Models;
using EShopData.Security;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.Services
{
    public class CartService
    {
        private readonly EShopDbContext context;
        private readonly UserSession session;

        public CartService(EShopDbContext context, UserSession session)
        {
            this.context = context;
            this.session = session;
        }

        public void Add(int productId)
        {
            if (!session.IsLoggenIn())
            {
                throw new Exception();
            }

            var cart = context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefault(c => c.UserId == session.User.Id);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = session.User.Id,
                    CartItems = new List<CartItem>()
                };
                context.Carts.Add(cart);
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

            if (cartItem == null)
            {
                cart.CartItems.Add(new CartItem()
                {
                    ProductId = productId,
                    Quantity = 1
                });
            }
            else
            {
                cartItem.Quantity++;
            }

            context.SaveChanges();
        }

        public List<CartItemDetailsDto> GetCartItemsDetails()
        {
            var cart = context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefault(c => c.UserId == session.User.Id);

            if (cart == null)
            {
                return new List<CartItemDetailsDto>();
            }

            return cart.CartItems.Select(ci => new CartItemDetailsDto(
                ci.ProductId,
                ci.Product.Name,
                ci.Quantity,
                ci.Product.Price
                )).ToList();
        }

        public void Clear()
        {
            if (!session.IsLoggenIn())
            {
                throw new Exception();
            }

            var cart = context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefault(c => c.UserId == session.User.Id);

            if(cart == null)
            {
                return;
            }

            context.CartItems.RemoveRange(cart.CartItems);

            context.SaveChanges();
        }

        public void RemoveProduct(int productId)
        {
            if (!session.IsLoggenIn())
            {
                throw new Exception();
            }

            var cart = context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefault(c => c.UserId == session.User.Id);

            if (cart == null)
            {
                return;
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

            if( cartItem == null)
            {
                return;
            }

            context.CartItems.Remove(cartItem);
            
            context.SaveChanges();
        }
    }
}
