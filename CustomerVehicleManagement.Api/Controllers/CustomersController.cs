using CustomerVehicleManagement.Api.Data.Interfaces;
using CustomerVehicleManagement.Api.Data.Dtos;
using CustomerVehicleManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace CustomerVehicleManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository repository;
        private const int MaxCacheAge = 300;

        public CustomersController(ICustomerRepository repository)
        {
            this.repository = repository ??
                throw new ArgumentNullException(nameof(repository));
        }

        // GET: api/Customers
        [HttpGet]
        [ResponseCache(Duration = MaxCacheAge)]
        public async Task<ActionResult<IEnumerable<CustomerReadDto>>> GetCustomersAsync()
        {
            var results = await repository.GetCustomersAsync();
            return Ok(results);
        }

        // GET: api/Customer/1
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Customer>> GetCustomerAsync(int id)
        {
            var result = await repository.GetCustomerAsync(id);

            if (result == null)
                return NotFound();

            return result;
        }

        // PUT: api/Customer/1
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Customer>> UpdateCustomerAsync(int id, Customer model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var fetchedCustomer = await repository.GetCustomerAsync(id);
            if (fetchedCustomer == null)
                return NotFound($"Could not find Customer in the database to updte: {model.Entity}");

            //mapper.Map(model, fetchedCustomer);

            // Update the objects ObjectState and sych the EF Change Tracker
            fetchedCustomer.SetTrackingState(TrackingState.Modified);
            repository.FixTrackingState();

            if (await repository.SaveChangesAsync())
                return fetchedCustomer;


            return BadRequest($"Failed to update {model.Entity}.");
        }

        // POST: api/Customer/
        [HttpPost]
        public async Task<ActionResult<Customer>> CreateCustomerAsync(Customer model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //var customer = mapper.Map<Customer>(model);
            repository.AddCustomerAsync(model);

            if (await repository.SaveChangesAsync())
            {
                //var location = linkGenerator.GetPathByAction("Get", "Customers", new { id = customer.Id });
                string location = $"/api/customers/{model.Id}";
                //return Created(location, mapper.Map<CustomerModel>(customer));
            }

            return BadRequest($"Failed to add {model.Entity}.");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCustomerAsync(int id)
        {
            var fetchedCustomer = await repository.GetCustomerAsync(id);
            if (fetchedCustomer == null)
                return NotFound($"Could not find Customer in the database to delete with Id: {id}.");

            repository.DeleteCustomer(fetchedCustomer);
            if (await repository.SaveChangesAsync())
            {
                return Ok();
            }

            return BadRequest($"Failed to delete Customer with Id: {id}.");
        }
    }

}
