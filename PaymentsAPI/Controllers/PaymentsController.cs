using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaymentsAPI.Interfaces;
using PaymentsAPI.Models;


namespace PaymentsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly ICardPaymentService paymentService;

        public PaymentsController(ICardPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }
        
        [HttpPost]
        public async Task<ActionResult> ProcessPayment([FromBody] CardPayment payment)
        {
            try
            {
                var result = await paymentService.ProcessPayment(payment);
                return Ok(result.VerificationCode);
            }
            catch (Exception)
            {
                return BadRequest("Unable to make payment");
            }
        }
    }
}
