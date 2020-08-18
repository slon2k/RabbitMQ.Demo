using Consumer.Entities;
using Consumer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consumer.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> repository;

        public OrderService(IRepository<Order> repository)
        {
            this.repository = repository;
        }

        public async Task MakePaymentAsync(Order order)
        {
            try
            {
                await repository.AddAsync(order);
                Console.WriteLine($"Purchase Order Received, {order.CompanyName}, {order.OrderNumber}, ${order.Amount}");
            }
            catch (Exception)
            {
                Console.WriteLine($"Unable to make payment, {order.CompanyName}, {order.OrderNumber}, ${order.Amount}");
            }          
        }
    }
}
