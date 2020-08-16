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
    public class CardsController : ControllerBase
    {
        private readonly ICardPaymentService paymentService;

        public CardsController(ICardPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }
        
        [HttpPost]
        public ActionResult ProcessPayment([FromBody] CardPayment payment)
        {
            try
            {
                paymentService.ProcessPayment(payment);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest("Unable to make payment");
            }
        }
    }
}
