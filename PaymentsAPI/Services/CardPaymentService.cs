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
        public async Task<PaymentDetails> ProcessPayment(CardPayment payment)
        {
            Thread.Sleep(1000);
            return new PaymentDetails()
            {
                Amount = payment.Amount,
                Name = payment.Name,
                VerificationCode = GenerateNumber()
            };
        }
        
        private static int GenerateNumber()
        {
            return Rand.Next(100000, 999999);
        }

        private static Random Rand = new Random();
    }
}
