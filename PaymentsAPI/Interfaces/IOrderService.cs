﻿using PaymentsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentsAPI.Interfaces
{
    public interface IOrderService
    {
        public void MakePayment(Order order);
    }
}
