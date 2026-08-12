using EShopData.Data;
using EShopData.Entities;
using EShopData.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.Services
{
    public class CheckoutService
    {
        private readonly EShopDbContext context;
        private readonly UserSession session;
        private readonly CartService cartService;

        public CheckoutService(EShopDbContext context, UserSession session, CartService cartService)
        {
            this.context = context;
            this.session = session;
            this.cartService = cartService;
        }

        public void Checkout()
        {
            if(!session.IsLoggedIn())
            {
                throw new InvalidOperationException("User isn't login");
            }

            var cartItems = cartService.GetCartItemsDetails();

            if (!cartItems.Any())
            {
                throw new InvalidOperationException("Cart is empty.");
            }

            var order = new Order
            {
                UserId = session.User.Id,
                CreatedAt = DateTime.UtcNow,
                OrderStatus = Enums.OrderStatus.Delivered,
                OrderItems = cartItems.Select(ci => new OrderItem
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.Price
                }).ToList()
            };

            context.Orders.Add(order);

            context.SaveChanges();

            cartService.Clear();
        }
    }
}
