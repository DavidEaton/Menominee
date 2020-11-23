using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Migrations.Api.Data.Interfaces;
using Migrations.Core.Entities;
using SharedKernel.Enums;
using System;
using System.Threading.Tasks;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace Migrations.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository data;
        private readonly IMapper mapper;

        public CustomersController(ICustomerRepository data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<Customer[]>> GetCustomers()
        {
            try
            {
                return await data.GetCustomersAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        // GET: api/Customer/1
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            try
            {
                var result = await data.GetCustomerAsync(id);

                if (result == null)
                    return NotFound();

                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        // PUT: api/Customer/1
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Customer>> UpdateCustomer(int id, Customer model)
        {
            if (model == null)
                return BadRequest();

            try
            {
                var fetchedCustomer = await data.GetCustomerAsync(id);
                if (fetchedCustomer == null)
                    return NotFound($"Could not find Customer in the database to updte: {model.Entity}");

                //mapper.Map(model, fetchedCustomer);

                // Update the objects ObjectState and sych the EF Change Tracker
                fetchedCustomer.UpdateState(TrackingState.Modified);
                data.FixState();

                if (await data.SaveChangesAsync())
                    return fetchedCustomer;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

            return BadRequest($"Failed to update {model.Entity}.");
        }

        // POST: api/Customer/
        [HttpPost]
        public async Task<ActionResult<Customer>> AddCustomer(Customer model)
        {
            try
            {
                //var customer = mapper.Map<Customer>(model);
                data.AddCustomer(model);

                if (await data.SaveChangesAsync())
                {
                    //var location = linkGenerator.GetPathByAction("Get", "Customers", new { id = customer.Id });
                    string location = $"/api/customers/{model.Id}";
                    //return Created(location, mapper.Map<CustomerModel>(customer));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

            return BadRequest($"Failed to add {model.Entity}.");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                var fetchedCustomer = await data.GetCustomerAsync(id);
                if (fetchedCustomer == null)
                    return NotFound($"Could not find Customer in the database to delete with Id: {id}.");

                data.DeleteCustomer(fetchedCustomer);
                if (await data.SaveChangesAsync())
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

            return BadRequest($"Failed to delete Customer with Id: {id}.");
        }
    }

}
