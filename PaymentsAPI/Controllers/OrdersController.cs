using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using PaymentsAPI.Interfaces;
using PaymentsAPI.Models;

namespace PaymentsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> Create([FromBody] OrderCreateDto dto)
        {
            try
            {
                var result = await orderService.CreateOrder(dto);
                return Ok(result.OrderNumber);
            }
            catch (Exception)
            {
                return BadRequest("Unable to create order");
            }

        }
    }
}
