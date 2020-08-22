using PaymentsAPI.Interfaces;
using PaymentsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentsAPI.Services
{
    public class DirectCardPaymentService : IDirectCardPaymentService
    {
        private readonly IRpcClient rpcClient;

        public DirectCardPaymentService(IRpcClient rpcClient)
        {
            this.rpcClient = rpcClient;
        }

        public async Task<string> ProcessPayment(CardPayment payment)
        {
            var authCode = await rpcClient.MakePaymentAsync(payment);
            Console.WriteLine($"Payment Sent {payment.Name}, {payment.CardNumber}, ${payment.Amount}, code: {authCode}");
            return authCode;
        }
    }
}
