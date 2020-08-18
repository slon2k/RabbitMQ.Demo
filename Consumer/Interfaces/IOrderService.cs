using Consumer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consumer.Interfaces
{
    public interface IOrderService
    {
        Task MakePaymentAsync(Order order);
    }
}
