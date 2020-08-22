using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentsAPI.Interfaces;
using PaymentsAPI.Models;

namespace PaymentsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectCardPaymentController : ControllerBase
    {
        private readonly IDirectCardPaymentService directCardPaymentService;

        public DirectCardPaymentController(IDirectCardPaymentService directCardPaymentService)
        {
            this.directCardPaymentService = directCardPaymentService;
        }

        [HttpPost]
        public async Task<ActionResult> ProcessPayment([FromBody] CardPayment payment)
        {
            try
            {
                var code = await directCardPaymentService.ProcessPayment(payment);
                return Ok(code);
            }
            catch (Exception)
            {
                return BadRequest("Unable to make payment");
            }
        }
    }
}
