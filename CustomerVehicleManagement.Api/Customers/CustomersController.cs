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
using SharedKernel.ValueObjects;
using CustomerVehicleManagement.Shared.Models;

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
        //[Route("list")]
        //[HttpGet]
        //public async Task<ActionResult<IReadOnlyList<CustomerInListDto>>> GetCustomersListAsync()
        //{
        //    var customers = await customerRepository.GetCustomersInListAsync();

        //    if (customers == null)
        //        return NotFound();

        //    return Ok(customers);
        //}

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

            var entityType = customerCreateDto.EntityType;
            Customer customer = null;

            if (entityType == EntityType.Person)
                customer = CreatePersonCustomer(customerCreateDto);

            if (entityType == EntityType.Organization)
                customer = CreateOrganizationCustomer(customerCreateDto);

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

        private static Customer CreateOrganizationCustomer(CustomerCreateDto customerCreateDto)
        {
            var organizationNameOrError = OrganizationName.Create(customerCreateDto.OrganizationCreateDto.Name);

            if (organizationNameOrError.IsSuccess)
            {
                Organization organization = new(organizationNameOrError.Value);

                if (customerCreateDto.OrganizationCreateDto.Contact != null)
                {
                    Person contact = new(customerCreateDto.OrganizationCreateDto.Contact.Name,
                                         customerCreateDto.OrganizationCreateDto.Contact.Gender);

                    contact.SetAddress(customerCreateDto.OrganizationCreateDto.Contact?.Address);
                    contact.SetBirthday(customerCreateDto.OrganizationCreateDto.Contact?.Birthday);
                    contact.SetDriversLicense(customerCreateDto.OrganizationCreateDto.Contact?.DriversLicense);

                    if (customerCreateDto.OrganizationCreateDto?.Contact?.Phones?.Count > 0)
                        foreach (var phone in customerCreateDto.OrganizationCreateDto.Contact.Phones)
                            contact.AddPhone(new Phone(phone.Number, phone.PhoneType, phone.IsPrimary));

                    if (customerCreateDto.OrganizationCreateDto?.Contact?.Emails?.Count > 0)
                        foreach (var email in customerCreateDto.OrganizationCreateDto.Contact.Emails)
                            contact.AddEmail(new Email(email.Address, email.IsPrimary));

                    organization.SetContact(contact);
                    organization.SetNote(customerCreateDto.OrganizationCreateDto.Note);
                }

                Customer customer = new(organization);

                return customer;
            }

            return null;
        }

        private static Customer CreatePersonCustomer(CustomerCreateDto customerCreateDto)
        {
            var person = new Person(customerCreateDto.PersonCreateDto.Name, customerCreateDto.PersonCreateDto.Gender);

            person.SetAddress(customerCreateDto.PersonCreateDto?.Address);
            person.SetBirthday(customerCreateDto.PersonCreateDto?.Birthday);
            person.SetDriversLicense(customerCreateDto.PersonCreateDto?.DriversLicense);

            if (customerCreateDto.PersonCreateDto?.Phones?.Count > 0)
                foreach (var phone in customerCreateDto.PersonCreateDto.Phones)
                    person.AddPhone(new Phone(phone.Number, phone.PhoneType, phone.IsPrimary));

            if (customerCreateDto.PersonCreateDto?.Emails?.Count > 0)
                foreach (var email in customerCreateDto.PersonCreateDto.Emails)
                    person.AddEmail(new Email(email.Address, email.IsPrimary));

            Customer customer = new(person);

            return customer;
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
