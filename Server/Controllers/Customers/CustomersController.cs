using MenomineePlayWASM.Shared.Entities.Customers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Server.Controllers.Customers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        //private readonly ApplicationDbContext context;

        //public CustomersController(ApplicationDbContext context)
        //{
        //    this.context = context;
        //}

        //// GET: api/<CustomersController>
        //[HttpGet]
        //public async Task<ActionResult<List<Customer>>> Get()
        //{
        //    return await context.Customers.ToListAsync();
        //}

        //// GET api/<CustomersController>/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Customer>> Get(int id)
        //{
        //    var Customer = await context.Customers.FirstOrDefaultAsync(x => x.Id == id);
        //    if (Customer == null)
        //    {
        //        return NotFound();
        //    }

        //    return Customer;
        //}

        //// POST api/<CustomersController>
        //[HttpPost]
        //public async Task<ActionResult<int>> Post(Customer Customer)
        //{
        //    context.Add(Customer);
        //    await context.SaveChangesAsync();
        //    return Customer.Id;
        //}

        //// PUT api/<CustomersController>/5
        ////[HttpPut("{id}")]
        //[HttpPut]
        //public async Task<ActionResult> Put(Customer Customer)
        //{
        //    context.Attach(Customer).State = EntityState.Modified;
        //    await context.SaveChangesAsync();
        //    return NoContent();
        //}

        //// DELETE api/<CustomersController>/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    //await context.Remove(id);
        //    return NoContent();
        //}
    }
}
