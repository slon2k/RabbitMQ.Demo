using Consumer.Entities;
using Consumer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consumer.Services
{
    public class CardPaymentService : ICardPaymentService
    {
        private readonly IRepository<CardPayment> repository;

        public CardPaymentService(IRepository<CardPayment> repository)
        {
            this.repository = repository;
        }

        public async Task MakePayment(CardPayment cardPayment)
        {
            try
            {
                var payment = await repository.AddAsync(cardPayment);
                Console.WriteLine($"Card Payment Received {payment.Name}, {payment.CardNumber}, ${payment.Amount}");

            }
            catch (Exception)
            {
                Console.WriteLine($"Unable to make payment, {cardPayment.Name}, ${cardPayment.Amount}");
            }
        }
    }
}
