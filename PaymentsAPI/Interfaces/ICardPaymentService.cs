using PaymentsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentsAPI.Interfaces
{
    public interface ICardPaymentService
    {
        public void ProcessPayment(CardPayment payment);
    }
}
