using CustomerVehicleManagement.Api.Organizations;
using CustomerVehicleManagement.Api.Persons;
using CustomerVehicleManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerVehicleManagement.Api.Utilities;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using AutoMapper;
using SharedKernel.ValueObjects;

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
        private readonly IMapper mapper;

        public CustomersController(ICustomerRepository customerRepository,
                                   IPersonRepository personRepository,
                                   IOrganizationRepository organizationRepository,
                                   IMapper mapper)
        {
            this.customerRepository = customerRepository ??
                throw new ArgumentNullException(nameof(customerRepository));
            this.personRepository = personRepository ??
                throw new ArgumentNullException(nameof(personRepository));
            this.organizationRepository = organizationRepository ??
                throw new ArgumentNullException(nameof(organizationRepository));
            this.mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
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
            CustomerReadDto customer = await customerRepository.GetCustomerAsync(id);

            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        // PUT: api/Customer/1
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Customer>> UpdateCustomerAsync(int id, CustomerUpdateDto customerUpdateDto)
        {
            /* Update Pattern in Controllers:
                1) Get domain entity from repository
                2) Update domain entity with data in data transfer object (DTO)
                3) Set entity's TrackingState to Modified
                4) FixTrackingState: moves entity state tracking back out of
                the object and into the context to track entity state in this
                disconnected applications. In other words, sych the EF Change
                Tracker with our disconnected entity's TrackingState
                5) Save changes
                6) return NoContent()
            */

            //1) Get domain entity from repository
            var fetchedCustomer = await customerRepository.GetCustomerAsync(id);

            if (fetchedCustomer == null || fetchedCustomer?.EntityType == EntityType.Organization)
                return NotFound($"Could not find Customer in the database to update.");

            // 2) Update domain entity with data in data transfer object(DTO)
            if (fetchedCustomer.EntityType == EntityType.Organization)
            {
                //1.a) Get domain Organization from repository
                var organizationFromRepository = await organizationRepository.GetOrganizationAsync(fetchedCustomer.Id);

                // 2.a) Update domain entity with data in data transfer object(DTO)
                //DtoHelpers.ConvertOrganizationUpdateDtoToDomainModel(customerUpdateDto.OrganizationUpdateDto, organizationFromRepository, mapper);

                // Update the objects ObjectState and sych the EF Change Tracker
                // 3) Set entity's TrackingState to Modified
                //fetchedCustomer.SetTrackingState(TrackingState.Modified);
                // 4) FixTrackingState: moves entity state tracking into the context
                customerRepository.FixTrackingState();
            }

            if (fetchedCustomer.EntityType == EntityType.Person)
            {
                //1.a) Get domain Person from repository
                Person personFromRepository = await personRepository.GetPersonEntityAsync(fetchedCustomer.Id);
                // 2.a) Update domain entity with data in data transfer object(DTO)
                DtoHelpers.ConvertPersonUpdateDtoToDomainModel(customerUpdateDto.PersonUpdateDto, personFromRepository, mapper);

                // Update the objects ObjectState and sych the EF Change Tracker
                // 3) Set entity's TrackingState to Modified
                //fetchedCustomer.SetTrackingState(TrackingState.Modified);
                // 4) FixTrackingState: moves entity state tracking into the context
                customerRepository.FixTrackingState();
            }

            //5) Save changes
            if (await customerRepository.SaveChangesAsync())
                // 6) return NoContent()
                return NoContent();

            return BadRequest($"Failed to update .");
        }

        // POST: api/Customer/
        [HttpPost]
        public async Task<ActionResult<CustomerReadDto>> CreateCustomerAsync(CustomerCreateDto customerCreateDto)
        {
            Customer customer = null;

            if (customerCreateDto.PersonCreateDto != null)
            {
                // 1. Map dto to domain entity
                var person = mapper.Map<Person>(customerCreateDto.PersonCreateDto);

                if (person != null)
                {
                    await personRepository.AddPersonAsync(person);

                    if (await personRepository.SaveChangesAsync())
                    {
                        customer = new(person);

                        if (customer != null)
                            await customerRepository.AddCustomerAsync(customer);

                        if (!await customerRepository.SaveChangesAsync())
                            return BadRequest($"Failed to add {customerCreateDto}.");
                    }
                }
            }

            if (customerCreateDto.OrganizationCreateDto != null)
            {
                var organizationNameOrError = OrganizationName.Create(customerCreateDto.OrganizationCreateDto.Name);
                if (organizationNameOrError.IsFailure)
                    return BadRequest(organizationNameOrError.Error);

                var organization = new Organization(organizationNameOrError.Value);

                organization.SetNotes(customerCreateDto.OrganizationCreateDto.Notes);
                organization.SetAddress(customerCreateDto.OrganizationCreateDto.Address);
                organization.SetContact(mapper.Map<Person>(customerCreateDto.OrganizationCreateDto.Contact));

                foreach (var phoneCreateDto in customerCreateDto.OrganizationCreateDto.Phones)
                    organization.AddPhone(new Phone(phoneCreateDto.Number, phoneCreateDto.PhoneType, phoneCreateDto.IsPrimary));

                foreach (var emailCreateDto in customerCreateDto.OrganizationCreateDto.Emails)
                    organization.AddEmail(new Email(emailCreateDto.Address, emailCreateDto.IsPrimary));

                if (organization != null)
                {
                    await organizationRepository.AddOrganizationAsync(organization);

                    if (await organizationRepository.SaveChangesAsync())
                    {
                        customer = new(organization);

                        if (customer != null)
                            await customerRepository.AddCustomerAsync(customer);

                        if (!await customerRepository.SaveChangesAsync())
                            return BadRequest($"Failed to add {customerCreateDto}.");
                    }
                }
            }

            if (customer != null)
            {
                // 4. Get ReadDto (with new Id) from database after save)
                CustomerReadDto customerFromRepository = await customerRepository.GetCustomerAsync(customer.Id);

                // 5. Return to consumer
                return CreatedAtRoute("GetCustomerAsync",
                    new { id = customerFromRepository.Id },
                    customerFromRepository);
            }

            return BadRequest($"Failed to add {customerCreateDto}.");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCustomerAsync(int id)
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
