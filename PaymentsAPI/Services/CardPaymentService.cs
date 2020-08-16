using PaymentsAPI.Interfaces;
using PaymentsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentsAPI.Services
{
    public class CardPaymentService : ICardPaymentService
    {
        private readonly IMessageSender messageSender;

        public CardPaymentService(IMessageSender messageSender)
        {
            this.messageSender = messageSender;
        }

        public void ProcessPayment(CardPayment payment)
        {
            messageSender.SendCardPayment(payment);
            Console.WriteLine($"Payment Sent {payment.Name}, {payment.CardNumber}, ${payment.Amount}");
        }
    }
}
