using EShopData.Common;
using EShopData.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.Menus
{
    public class OrderMenu
    {
        private readonly OrderService orderService;
        private readonly ConsoleHelper consoleHelper;

        public OrderMenu(OrderService orderService, ConsoleHelper consoleHelper)
        {
            this.orderService = orderService;
            this.consoleHelper = consoleHelper;
        }

        public void ShowOrderHistory()
        {
            var exit = false;

            while (!exit)
            {
                var orders = orderService.GetOrderList();

                var choice = consoleHelper.ShowArrowMenu(
                    "Order history",
                    orders.Select(o => $"Order{o.Id}\tData: {o.CreatedAt}\t{o.OrderStatus}")
                    .Append("Back")
                    .ToArray()
                    );

                if(choice<orders.Count)
                {
                    ShowOrderDetails(orders[choice].Id);
                }
                else
                {
                    exit = true;
                }
            }
        }

        public void ShowOrderDetails(int orderId)
        {
            var orderItems = orderService.GetOrderItemsDetails(orderId);

            decimal orderPrice = 0;
            var orderItemsOutput = new StringBuilder("Products:\n\n");

            foreach (var orderItem in orderItems)
            {
                orderPrice += orderItem.UnitPrice * orderItem.Quantity;

                orderItemsOutput.AppendLine($"{orderItem.Productname}\t{orderItem.UnitPrice}\t" +
                    $"x{orderItem.Quantity}\t{orderItem.UnitPrice*orderItem.Quantity}");
            }

            orderItemsOutput.Append($"\nOrder price: {orderPrice}");

            consoleHelper.ShowArrowMenu(orderItemsOutput.ToString(), ["back"]);
        }
    }
}
