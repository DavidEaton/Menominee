using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Api.Organizations;
using CustomerVehicleManagement.Api.Persons;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models.Customers;
using CustomerVehicleManagement.Shared.Models.Organizations;
using CustomerVehicleManagement.Shared.Models.Persons;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Customers
{
    public class CustomersController : ApplicationController
    {
        private readonly ICustomerRepository customerRepository;
        private readonly PersonsController personsController;
        private readonly OrganizationsController organizationsController;
        private readonly string BasePath = "/api/customers/";

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

        // GET: api/customers/list
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
        public async Task<IActionResult> UpdateCustomerAsync(long id, CustomerToWrite customerToWrite)
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

            await customerRepository.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Customer/
        [HttpPost]
        public async Task<IActionResult> AddCustomerAsync(CustomerToWrite customerToAdd)
        {
            Customer customer = null;

            if (customerToAdd.EntityType == EntityType.Person)
                customer = new(PersonHelper.ConvertWriteDtoToEntity(customerToAdd.Person), customerToAdd.CustomerType);

            if (customerToAdd.EntityType == EntityType.Organization)
                customer = new(OrganizationHelper.ConvertWriteDtoToEntity(customerToAdd.Organization), customerToAdd.CustomerType);

            await customerRepository.AddCustomerAsync(customer);

            await customerRepository.SaveChangesAsync();

            return Created(new Uri($"{BasePath}/{customer.Id}", UriKind.Relative), new { id = customer.Id });
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteCustomerAsync(long id)
        {
            var notFoundMessage = $"Could not find Customer in the database to delete with Id: {id}.";

            if (!await customerRepository.CustomerExistsAsync(id))
                return NotFound(notFoundMessage);

            var customerFromRepository = await customerRepository.GetCustomerEntityAsync(id);
            if (customerFromRepository is null)
                return NotFound(notFoundMessage);

            customerRepository.DeleteCustomer(customerFromRepository);
            await customerRepository.SaveChangesAsync();

            return NoContent();
        }
    }

}
