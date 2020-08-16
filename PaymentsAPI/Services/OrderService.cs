using Microsoft.AspNetCore.Authentication;
using PaymentsAPI.Interfaces;
using PaymentsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentsAPI.Services
{
    public class OrderService : IOrderService
    {
        public async Task<Order> CreateOrder(OrderCreateDto order)
        {
            Thread.Sleep(1000);
            return new Order()
            {
                Amount = order.Amount,
                CompanyName = order.CompanyName,
                OrderNumber = GenerateNumber()
            };
        }

        private static string GenerateNumber()
        {
            return $"PO-{Rand.Next(10000, 99999)}";
        }

        private static Random Rand = new Random();
    }
}
