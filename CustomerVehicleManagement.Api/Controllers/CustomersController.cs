using CustomerVehicleManagement.Api.Data.Interfaces;
using CustomerVehicleManagement.Api.Data.Models;
using CustomerVehicleManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace CustomerVehicleManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository data;
        private const int MaxCacheAge = 300;

        public CustomersController(ICustomerRepository data)
        {
            this.data = data;
        }

        // GET: api/Customers
        [HttpGet]
        [ResponseCache(Duration = MaxCacheAge)]
        public async Task<ActionResult<IEnumerable<CustomerReadDto>>> GetCustomers()
        {
            var results = await data.GetCustomersAsync();
            return Ok(results);
        }

        // GET: api/Customer/1
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var result = await data.GetCustomerAsync(id);

            if (result == null)
                return NotFound();

            return result;
        }

        // PUT: api/Customer/1
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Customer>> UpdateCustomer(int id, Customer model)
        {

            var fetchedCustomer = await data.GetCustomerAsync(id);
            if (fetchedCustomer == null)
                return NotFound($"Could not find Customer in the database to updte: {model.Entity}");

            //mapper.Map(model, fetchedCustomer);

            // Update the objects ObjectState and sych the EF Change Tracker
            fetchedCustomer.UpdateTrackingState(TrackingState.Modified);
            data.FixTrackingState();

            if (await data.SaveChangesAsync())
                return fetchedCustomer;


            return BadRequest($"Failed to update {model.Entity}.");
        }

        // POST: api/Customer/
        [HttpPost]
        public async Task<ActionResult<Customer>> CreateCustomer(Customer model)
        {
            //var customer = mapper.Map<Customer>(model);
            data.AddCustomer(model);

            if (await data.SaveChangesAsync())
            {
                //var location = linkGenerator.GetPathByAction("Get", "Customers", new { id = customer.Id });
                string location = $"/api/customers/{model.Id}";
                //return Created(location, mapper.Map<CustomerModel>(customer));
            }

            return BadRequest($"Failed to add {model.Entity}.");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var fetchedCustomer = await data.GetCustomerAsync(id);
            if (fetchedCustomer == null)
                return NotFound($"Could not find Customer in the database to delete with Id: {id}.");

            data.DeleteCustomer(fetchedCustomer);
            if (await data.SaveChangesAsync())
            {
                return Ok();
            }

            return BadRequest($"Failed to delete Customer with Id: {id}.");
        }
    }

}
