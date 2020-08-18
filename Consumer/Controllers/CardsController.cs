using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consumer.Data;
using Consumer.Entities;
using Consumer.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Consumer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly IRepository<CardPayment> repository;

        public CardsController(IRepository<CardPayment> repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<CardPayment>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<CardPayment> GetAsync(Guid id)
        {
            return await repository.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] CardPayment payment)
        {
            try
            {
                var entity = await repository.AddAsync(payment);
                Console.WriteLine($"Payment added {payment}");
                return Ok(entity);
            }
            catch (Exception)
            {
                return BadRequest();
            }        
        }
    }
}
