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
    // VK: only use auto mappers when mapping from domain classes to DTOs, not the other way around
    // because DTO -> Domain requires making a decision as to whether the data in the DTO is correct.
    // This decision should be explicit; it is validation
    // More on this: https://enterprisecraftsmanship.com/posts/on-automappers/
    // And even for the Domain -> DTO conversion, you are usually better of just doing the mapping manually
    // It's more straightforward and at the same time requires the same amount of code
    // (for auto mapper, this code resides in the configurations; for the manual approach -- in the controller, where it should be)

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
        public async Task<ActionResult<IEnumerable<CustomerReadDto>>> GetCustomersAsync()
        {
            var customers = await customerRepository.GetCustomersAsync();

            if (customers == null)
                return NotFound();

            return Ok(customers);
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
            // VK: best not to use DtoHelpers. What happens if the incoming data is incorrect? How is this case handled?
            // Looks like this use case needs validation. Check out my PS course for how this can be done: https://app.pluralsight.com/library/courses/fluentvalidation-fundamentals/table-of-contents
            // You can skip the parts about FluentValidation and go straight to "Validating Input the DDD Way" module.
            // Let me know if you need a code with 30-day access to Pluralsight

            CustomerReadDto customerFromRepository = await customerRepository.GetCustomerAsync(id);

            if (customerFromRepository == null || customerFromRepository?.EntityType == null)
                return NotFound($"Could not find Customer in the database to update.");

            // VK: here, the logic should be:
            // 1. Get the customer entity (not DTO) from the DB
            // 2. Look at its type
            // 3. Update the corresponding fields in the customer depending on the type (i.e take the fields from the DTO needed for this specific customer type)
            // 4. Save back to the DB

            if (customerFromRepository.EntityType == EntityType.Organization)
            {
                Organization organizationFromRepository = await organizationRepository.GetOrganizationEntityAsync(customerFromRepository.Organization.Id);

                var organizationNameOrError = OrganizationName.Create(customerUpdateDto.OrganizationUpdateDto.Name);
                if (organizationNameOrError.IsFailure)
                    return BadRequest(organizationNameOrError.Error);

                organizationFromRepository.SetName(organizationNameOrError.Value);
                organizationFromRepository.SetAddress(customerUpdateDto.OrganizationUpdateDto.Address);
                organizationFromRepository.SetNote(customerUpdateDto.OrganizationUpdateDto.Note);
                DtoHelpers.PersonUpdateDtoToPerson(customerUpdateDto.OrganizationUpdateDto.Contact, organizationFromRepository.Contact);
                organizationFromRepository.SetPhones(DtoHelpers.PhonesUpdateDtoToPhones(customerUpdateDto.OrganizationUpdateDto.Phones));
                organizationFromRepository.SetEmails(DtoHelpers.EmailsUpdateDtoToEmails(customerUpdateDto.OrganizationUpdateDto.Emails));

                organizationFromRepository.SetTrackingState(TrackingState.Modified);
                customerRepository.FixTrackingState();
            }

            if (customerFromRepository.EntityType == EntityType.Person)
            {
                Person personFromRepository = await personRepository.GetPersonEntityAsync(customerFromRepository.Person.Id);
                DtoHelpers.PersonUpdateDtoToPerson(customerUpdateDto.PersonUpdateDto, personFromRepository);

                personFromRepository.SetTrackingState(TrackingState.Modified);
                customerRepository.FixTrackingState();
            }

            if (await customerRepository.SaveChangesAsync())
                return NoContent();

            return BadRequest($"Failed to update .");
        }

        // POST: api/Customer/
        [HttpPost]
        public async Task<ActionResult<CustomerReadDto>> CreateCustomerAsync(CustomerCreateDto customerCreateDto)
        {
            // VK: here, the logic should be:
            // 1. Look at customerCreateDto.EntityType and create a customer of the corresponding type (you can introduce a factory method for this)
            // 2. Save it to the DB

            Customer customer = null;

            if (customerCreateDto.PersonCreateDto != null)
            {
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

                organization.SetNote(customerCreateDto.OrganizationCreateDto.Note);
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
                CustomerReadDto customerFromRepository = await customerRepository.GetCustomerAsync(customer.Id);

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
