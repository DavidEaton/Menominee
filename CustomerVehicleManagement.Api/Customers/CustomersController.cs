using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Api.Organizations;
using CustomerVehicleManagement.Api.Persons;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared;
using CustomerVehicleManagement.Shared.Helpers;
using CustomerVehicleManagement.Shared.Models;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace CustomerVehicleManagement.Api.Customers
{
    [Authorize(Policies.PaidUser)]
    public class CustomersController : ApplicationController
    {
        private readonly ICustomerRepository customerRepository;
        private readonly PersonsController personsController;
        private readonly OrganizationsController organizationsController;

        public CustomersController(ICustomerRepository customerRepository,
                                   PersonsController personsController,
                                   OrganizationsController organizationsController)
        {
            this.customerRepository = customerRepository ??
                throw new ArgumentNullException(nameof(customerRepository));
            this.personsController = personsController ??
                throw new ArgumentNullException(nameof(personsController));
            this.organizationsController = organizationsController ??
                throw new ArgumentNullException(nameof(organizationsController));
        }

        //// GET: api/customers/list
        [Route("list")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<CustomerToReadInList>>> GetCustomersListAsync()
        {
            var customers = await customerRepository.GetCustomersInListAsync();

            if (customers == null)
                return NotFound();

            return Ok(customers);
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<CustomerToRead>>> GetCustomersAsync()
        {
            var customers = await customerRepository.GetCustomersAsync();

            if (customers == null)
                return NotFound();

            return Ok(customers);
        }

        // GET: api/Customer/1
        [HttpGet("{id:long}", Name = "GetCustomerAsync")]
        public async Task<ActionResult<CustomerToRead>> GetCustomerAsync(long id)
        {
            CustomerToRead customer = await customerRepository.GetCustomerAsync(id);

            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        // PUT: api/Customer/1
        [HttpPut("{id:long}")]
        public async Task<ActionResult> UpdateCustomerAsync(long id, CustomerToWrite customerToWrite)
        {
            // VK: here, the logic should be:
            // 1. Get the customer entity (not DTO) from the DB
            // 2. Look at its type
            // 3. Update the corresponding fields in the customer depending on the type (i.e take the fields from the DTO needed for this specific customer type)
            // 4. Save back to the DB

            CustomerToRead customerFromRepository = await customerRepository.GetCustomerAsync(id);

            if (customerFromRepository == null || customerFromRepository?.EntityType == null)
                return NotFound($"Could not find Customer in the database to update.");

            if (customerFromRepository.EntityType == EntityType.Organization)
                await organizationsController.UpdateOrganizationAsync(customerFromRepository.Organization.Id, customerToWrite.Organization);

            if (customerFromRepository.EntityType == EntityType.Person)
                await personsController.UpdatePersonAsync(customerFromRepository.Person.Id, customerToWrite.Person);

            customerRepository.FixTrackingState();

            if (await customerRepository.SaveChangesAsync())
                return NoContent();

            return BadRequest($"Failed to update .");
        }

        // POST: api/Customer/
        [HttpPost]
        public async Task<ActionResult> AddCustomerAsync(CustomerToWrite customerToAdd)
        {
            Customer customer = null;

            if (customerToAdd.EntityType == EntityType.Person)
                customer = new(PersonHelper.CreateEntityFromWriteDto(customerToAdd.Person), customerToAdd.CustomerType);

            if (customerToAdd.EntityType == EntityType.Organization)
                customer = new(OrganizationHelper.CreateEntityFromWriteDto(customerToAdd.Organization), customerToAdd.CustomerType);

            await customerRepository.AddCustomerAsync(customer);

            if (!await customerRepository.SaveChangesAsync())
                return BadRequest($"Failed to add {customerToAdd}.");

            CustomerToRead customerFromRepository = await customerRepository.GetCustomerAsync(customer.Id);

            if (customerFromRepository == null)
                return BadRequest($"Failed to add {customerToAdd}.");

            return CreatedAtRoute("GetCustomerAsync",
                new { id = customerFromRepository.Id },
                customerFromRepository);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteCustomerAsync(long id)
        {
            var customerFromRepository = await customerRepository.GetCustomerAsync(id);

            if (customerFromRepository == null)
                return NotFound($"Could not find Customer in the database to delete with Id: {id}.");

            await customerRepository.DeleteCustomerAsync(id);

            if (await customerRepository.SaveChangesAsync())
                return NoContent();

            return BadRequest($"Failed to delete Customer with Id: {id}.");
        }
    }

}
