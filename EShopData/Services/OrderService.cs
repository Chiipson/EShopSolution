using EShopData.Data;
using EShopData.DTOs;
using EShopData.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.Services
{
    public class OrderService
    {
        private readonly EShopDbContext context;
        private readonly UserSession session;
        public OrderService(EShopDbContext context, UserSession session)
        {
            this.context = context;
            this.session = session;
        }

        public List<OrderUiDto> GetOrderList()
        {
            if(!session.IsLoggedIn())
            {
                throw new InvalidOperationException("User isn't login");
            }

            return context.Orders.Where(o=> o.UserId == session.User.Id).Select(o => new OrderUiDto(
                o.Id,
                o.CreatedAt,
                o.OrderStatus
                )).ToList();
        }

        public List<OrderItemDetailsDto> GetOrderItemsDetails(int orderId)
        {
            if (!session.IsLoggedIn())
            {
                throw new InvalidOperationException("User isn't login");
            }

            return context.OrderItems.Where(oi => oi.OrderId == orderId).Select(oi => new OrderItemDetailsDto(
                oi.Product.Name,
                oi.UnitPrice,
                oi.Quantity
                )).ToList();
        }
    }
}
