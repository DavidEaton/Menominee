using CustomerVehicleManagement.Api.Organizations;
using CustomerVehicleManagement.Api.Persons;
using CustomerVehicleManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Enums;
using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace CustomerVehicleManagement.Api.Customers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IPersonRepository personRepository;
        private readonly IOrganizationRepository organizationRepository;
        private const int MaxCacheAge = 300;

        public CustomersController(ICustomerRepository customerRepository,
                                   IPersonRepository personRepository,
                                   IOrganizationRepository organizationRepository)
        {
            this.customerRepository = customerRepository ??
                throw new ArgumentNullException(nameof(customerRepository));
            this.personRepository = personRepository ??
                throw new ArgumentNullException(nameof(personRepository));
            this.organizationRepository = organizationRepository ??
                throw new ArgumentNullException(nameof(organizationRepository));
        }

        // GET: api/Customers
        [HttpGet]
        [ResponseCache(Duration = MaxCacheAge)]
        public async Task<ActionResult<IEnumerable<CustomerReadDto>>> GetCustomersAsync()
        {
            var results = await customerRepository.GetCustomersAsync();
            return Ok(results);
        }

        // GET: api/Customer/1
        [HttpGet("{id:int}", Name = "GetCustomerAsync")]
        public async Task<ActionResult<CustomerReadDto>> GetCustomerAsync(int id)
        {
            var customer = await customerRepository.GetCustomerAsync(id);

            if (customer == null)
                return NotFound();

            CustomerReadDto customerReadDto = new()
            {
                Id = customer.Id,
                CustomerType = customer.CustomerType,
                EntityType = customer.EntityType
            };

            if (customer.EntityType == EntityType.Organization)
            {
                await customerRepository.GetCustomerOrganizationEntity(customer);
                customerReadDto.Address = ((Organization)customer.Entity).Address;
                customerReadDto.Name = ((Organization)customer.Entity).Name.Value;
                //customerReadDto.Phones = ((Organization)customer.Entity).Phones;
            }

            if (customer.EntityType == EntityType.Person)
            {
                await customerRepository.GetCustomerPersonEntity(customer);
                customerReadDto.Address = ((Person)customer.Entity).Address;
                customerReadDto.Name = ((Person)customer.Entity).Name.LastFirstMiddle;
            }

            return customerReadDto;
        }

        // PUT: api/Customer/1
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Customer>> UpdateCustomerAsync(int id, CustomerUpdateDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var fetchedCustomer = await customerRepository.GetCustomerAsync(id);
            if (fetchedCustomer == null)
                return NotFound($"Could not find Customer in the database to updte: ");

            //mapper.Map(model, fetchedCustomer);

            // Update the objects ObjectState and sych the EF Change Tracker
            fetchedCustomer.SetTrackingState(TrackingState.Modified);
            customerRepository.FixTrackingState();

            if (await customerRepository.SaveChangesAsync())
                return fetchedCustomer;


            return BadRequest($"Failed to update .");
        }

        // POST: api/Customer/
        [HttpPost]
        public async Task<ActionResult<CustomerReadDto>> CreateCustomerAsync(CustomerCreateDto customerCreateDto)
        {
            Customer customer = await customerRepository.AddAndSaveCustomerAsync(customerCreateDto);

            if (customer != null)
            {
                string name = string.Empty;

                if (customer.EntityType == EntityType.Organization)
                    name = customerCreateDto.OrganizationCreateDto.Name;

                if (customer.EntityType == EntityType.Person)
                    name = customerCreateDto.PersonCreateDto.Name.LastFirstMiddle;

                var customerReadDto = new CustomerReadDto
                {
                    Id = customer.Id,
                    EntityType = customer.EntityType,
                    Name = name
                };

                return CreatedAtRoute("GetCustomerAsync",
                    new { id = customerReadDto.Id },
                    customerReadDto);
            }

            return BadRequest($"Failed to add {customerCreateDto}.");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCustomerAsync(int id)
        {
            var fetchedCustomer = await customerRepository.GetCustomerAsync(id);
            if (fetchedCustomer == null)
                return NotFound($"Could not find Customer in the database to delete with Id: {id}.");

            customerRepository.DeleteCustomer(fetchedCustomer);
            if (await customerRepository.SaveChangesAsync())
            {
                return Ok();
            }

            return BadRequest($"Failed to delete Customer with Id: {id}.");
        }
    }

}
