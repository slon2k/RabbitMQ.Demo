using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentsAPI.Models
{
    public class OrderCreateDto
    {
        public decimal Amount { get; set; }
        public string CompanyName { get; set; }
    }
}
