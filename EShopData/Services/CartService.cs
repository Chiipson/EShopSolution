using EShopData.Data;
using EShopData.DTOs.Cart;
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

        public void Add(int productId, int amount)
        {
            if (session.IsLoggedIn())
            {
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
                        Quantity = amount
                    });
                }
                else
                {
                    cartItem.Quantity+= amount;
                }

                context.SaveChanges();
            }
            else
            {
                var item = session.GuestCart.Find(ci => ci.ProductId == productId);

                if (item == null)
                {
                    item = new GuestCartItem { ProductId = productId, Quantity = 1 };
                    session.GuestCart.Add(item);
                }
                else
                {
                    item.Quantity++;
                }
            }
        }

        public List<CartItemDetailsDto> GetCartItemsDetails()
        {
            if (!session.IsLoggedIn())
            {
                return session.GuestCart.Join(
                    context.Products,
                    item => item.ProductId,
                    product => product.Id,
                    (item, product) => new CartItemDetailsDto(
                        item.ProductId,
                        product.Name,
                        item.Quantity,
                        product.Price
                        )).ToList();
            }

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
            if (session.IsLoggedIn())
            {
                var cart = context.Carts
                    .Include(c => c.CartItems)
                    .FirstOrDefault(c => c.UserId == session.User.Id);

                if (cart == null)
                {
                    return;
                }

                context.CartItems.RemoveRange(cart.CartItems);

                context.SaveChanges();
            }
            else
            {
                session.GuestCart.Clear();
            }
        }

        public void RemoveProduct(int productId)
        {
            if (session.IsLoggedIn())
            {
                var cart = context.Carts
                    .Include(c => c.CartItems)
                    .FirstOrDefault(c => c.UserId == session.User.Id);

                if (cart == null)
                {
                    return;
                }

                var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

                if (cartItem == null)
                {
                    return;
                }

                context.CartItems.Remove(cartItem);

                context.SaveChanges();
            }
            else
            {
                var item = session.GuestCart.Find(ci => ci.ProductId == productId);

                if (item == null)
                {
                    return;
                }

                session.GuestCart.Remove(item);
            }
        }

        public void MergeUserAndGuestcarts()
        {
            foreach (var item in session.GuestCart)
            {
                Add(item.ProductId, item.Quantity);
            }

            session.GuestCart.Clear();
        }
    }
}
