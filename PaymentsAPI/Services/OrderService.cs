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
        private readonly IMessageSender messageSender;

        public OrderService(IMessageSender messageSender)
        {
            this.messageSender = messageSender;
        }
        
        public void MakePayment(Order order)
        {
            messageSender.SendOrder(order);
            Console.WriteLine($"Purchase Order Sent, {order.CompanyName}, {order.OrderNumber}, ${order.Amount}");
        }
    }
}
