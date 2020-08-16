using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentsAPI.Models
{
    public class PaymentDetails
    {
        public string Name { get; set; }
        public int VerificationCode { get; set; }
        public decimal Amount { get; set; }
    }
}
