using PaymentsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentsAPI.Interfaces
{
    public interface IMessageSender
    {
        public void SendOrder(Order order);
        public void SendCardPayment(CardPayment cardPayment);
    }
}
