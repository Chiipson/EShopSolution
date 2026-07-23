using EShopData.DTOs;
using EShopData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.Services
{
    public class CartService
    {
        private readonly List<CartItem> cartItems = new();

        public void Add(AddToCartDto item)
        {
            var cartItem = cartItems.Find(ci=>ci.ProductId == item.ProductId);

            if (cartItem == null)
            {
                cartItems.Add(new CartItem()
                {
                    ProductId = item.ProductId,
                    ProductName = item.Name,
                    Quantity = 1,
                    Price = item.Price
                });
            }else
            {
                cartItem.IncreaseQuantity();
            }
        }
        public IReadOnlyList<CartItem> GetAll()
        {
            return cartItems;
        }
    }
}
