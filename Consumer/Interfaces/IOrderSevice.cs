using Consumer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consumer.Interfaces
{
    public interface IOrderSevice
    {
        Task MakePaymentAsync(Order order);
    }
}
