using CustomerVehicleManagement.Api.Organizations;
using CustomerVehicleManagement.Api.Persons;
using CustomerVehicleManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using SharedKernel.ValueObjects;
using CustomerVehicleManagement.Shared.Models;
using CustomerVehicleManagement.Api.Phones;
using CustomerVehicleManagement.Api.Email;

namespace CustomerVehicleManagement.Api.Customers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IPersonRepository personRepository;
        private readonly IOrganizationRepository organizationRepository;

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

        //// GET: api/customers/list
        [Route("list")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<CustomerInListDto>>> GetCustomersListAsync()
        {
            var customers = await customerRepository.GetCustomersInListAsync();

            if (customers == null)
                return NotFound();

            return Ok(customers);
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<CustomerReadDto>>> GetCustomersAsync()
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
                PersonDtoHelper.PersonUpdateDtoToEntity(customerUpdateDto.OrganizationUpdateDto.Contact, organizationFromRepository.Contact);
                organizationFromRepository.SetPhones(PhonesDtoHelper.UpdateDtosToEntities(customerUpdateDto.OrganizationUpdateDto.Phones));
                organizationFromRepository.SetEmails(EmailDtoHelper.UpdateDtosToEntities(customerUpdateDto.OrganizationUpdateDto.Emails));

                organizationFromRepository.SetTrackingState(TrackingState.Modified);
                customerRepository.FixTrackingState();
            }

            if (customerFromRepository.EntityType == EntityType.Person)
            {
                Person personFromRepository = await personRepository.GetPersonEntityAsync(customerFromRepository.Person.Id);
                PersonDtoHelper.PersonUpdateDtoToEntity(customerUpdateDto.PersonUpdateDto, personFromRepository);

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

            var entityType = customerCreateDto.EntityType;
            Customer customer = null;

            if (entityType == EntityType.Person)
                customer = CreatePersonCustomer(customerCreateDto.PersonCreateDto);

            if (entityType == EntityType.Organization)
                customer = AddOrganizationCustomer(customerCreateDto.OrganizationCreateDto);

            if (customer != null)
                await customerRepository.AddCustomerAsync(customer);

            if (!await customerRepository.SaveChangesAsync())
                return BadRequest($"Failed to add {customerCreateDto}.");

            CustomerReadDto customerFromRepository = await customerRepository.GetCustomerAsync(customer.Id);

            if (customerFromRepository == null)
                return BadRequest($"Failed to add {customerCreateDto}.");

            return CreatedAtRoute("GetCustomerAsync",
                new { id = customerFromRepository.Id },
                customerFromRepository);
        }

        private static Customer AddOrganizationCustomer(OrganizationAddDto organizationAddDto)
        {
            var organizationNameOrError = OrganizationName.Create(organizationAddDto.Name);

            if (organizationNameOrError.IsSuccess)
            {
                var organization = new Organization(organizationNameOrError.Value);

                if (organizationAddDto.Contact != null)
                {
                    var contact = new Person(organizationAddDto.Contact.Name,
                                             organizationAddDto.Contact.Gender);

                    contact.SetAddress(organizationAddDto.Contact?.Address);
                    contact.SetBirthday(organizationAddDto.Contact?.Birthday);

                    if (organizationAddDto.Contact.DriversLicense != null)
                    {
                        var dateTimeRange = new DateTimeRange(organizationAddDto.Contact.DriversLicense.Issued,
                                                              organizationAddDto.Contact.DriversLicense.Expiry);

                        var driversLicense = new DriversLicense(organizationAddDto.Contact.DriversLicense.Number,
                                                                organizationAddDto.Contact.DriversLicense.State,
                                                                dateTimeRange);

                        contact.SetDriversLicense(driversLicense);
                    }

                    if (organizationAddDto?.Contact?.Phones?.Count > 0)
                        foreach (var phone in organizationAddDto.Contact.Phones)
                            contact.AddPhone(new Phone(phone.Number, phone.PhoneType, phone.IsPrimary));

                    if (organizationAddDto?.Contact?.Emails?.Count > 0)
                        foreach (var email in organizationAddDto.Contact.Emails)
                            contact.AddEmail(new Domain.Entities.Email(email.Address, email.IsPrimary));

                    organization.SetContact(contact);
                    organization.SetNote(organizationAddDto.Note);
                }

                Customer customer = new(organization);

                return customer;
            }

            return null;
        }

        private static Customer CreatePersonCustomer(PersonCreateDto personCreateDto)
        {
            var person = new Person(personCreateDto.Name, personCreateDto.Gender);

            person.SetAddress(personCreateDto?.Address);
            person.SetBirthday(personCreateDto?.Birthday);

            if (personCreateDto.DriversLicense != null)
            {
                var dateTimeRange = new DateTimeRange(personCreateDto.DriversLicense.Issued,
                                                      personCreateDto.DriversLicense.Expiry);

                var driversLicense = new DriversLicense(personCreateDto.DriversLicense.Number,
                                                        personCreateDto.DriversLicense.State,
                                                        dateTimeRange);

                person.SetDriversLicense(driversLicense);
            }

            if (personCreateDto?.Phones?.Count > 0)
                foreach (var phone in personCreateDto.Phones)
                    person.AddPhone(new Phone(phone.Number, phone.PhoneType, phone.IsPrimary));

            if (personCreateDto?.Emails?.Count > 0)
                foreach (var email in personCreateDto.Emails)
                    person.AddEmail(new Domain.Entities.Email(email.Address, email.IsPrimary));

            return new Customer(person);
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
