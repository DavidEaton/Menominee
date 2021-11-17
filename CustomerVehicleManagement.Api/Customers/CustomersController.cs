using CustomerVehicleManagement.Api.Organizations;
using CustomerVehicleManagement.Api.Persons;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
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
        public async Task<ActionResult<Customer>> UpdateCustomerAsync(long id, CustomerToWrite customerToWrite)
        {
            // VK: best not to use DtoHelpers. What happens if the incoming data is incorrect? How is this case handled?
            // Looks like this use case needs validation. Check out my PS course for how this can be done: https://app.pluralsight.com/library/courses/fluentvalidation-fundamentals/table-of-contents
            // You can skip the parts about FluentValidation and go straight to "Validating Input the DDD Way" module.
            // Let me know if you need a code with 30-day access to Pluralsight

            CustomerToRead customerFromRepository = await customerRepository.GetCustomerAsync(id);

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

                var organizationNameOrError = OrganizationName.Create(customerToWrite.OrganizationToWrite.Name);
                if (organizationNameOrError.IsFailure)
                    return BadRequest(organizationNameOrError.Error);

                organizationFromRepository.SetName(organizationNameOrError.Value);

                if (customerToWrite.OrganizationToWrite?.Address != null)
                    organizationFromRepository.SetAddress(Address.Create(customerToWrite.OrganizationToWrite.Address.AddressLine,
                                                                         customerToWrite.OrganizationToWrite.Address.City,
                                                                         customerToWrite.OrganizationToWrite.Address.State,
                                                                         customerToWrite.OrganizationToWrite.Address.PostalCode).Value);

                organizationFromRepository.SetNote(customerToWrite.OrganizationToWrite.Note);
                organizationFromRepository.SetContact(PersonToWrite.ConvertToEntity(customerToWrite.OrganizationToWrite.Contact));
                organizationFromRepository.SetPhones(PhoneToWrite.ConvertToEntities(customerToWrite.OrganizationToWrite.Phones));
                organizationFromRepository.SetEmails(EmailToWrite.ConvertToEntities(customerToWrite.OrganizationToWrite.Emails));

                organizationFromRepository.SetTrackingState(TrackingState.Modified);
                customerRepository.FixTrackingState();
            }

            if (customerFromRepository.EntityType == EntityType.Person)
            {
                Person personFromRepository = await personRepository.GetPersonEntityAsync(customerFromRepository.Person.Id);

                personFromRepository.SetName(PersonName.Create(
                                                customerToWrite.PersonToWrite.Name.LastName,
                                                customerToWrite.PersonToWrite.Name.FirstName,
                                                customerToWrite.PersonToWrite.Name.MiddleName).Value);
                personFromRepository.SetGender(customerToWrite.PersonToWrite.Gender);
                personFromRepository.SetAddress(AddressToWrite.ConvertToEntity(customerToWrite.PersonToWrite.Address));
                personFromRepository.SetBirthday(customerToWrite.PersonToWrite.Birthday);
                personFromRepository.SetDriversLicense(DriversLicenseToWrite.ConvertToEntity(customerToWrite.PersonToWrite.DriversLicense));
                personFromRepository.SetEmails(EmailToWrite.ConvertToEntities(customerToWrite.PersonToWrite.Emails));
                personFromRepository.SetPhones(PhoneToWrite.ConvertToEntities(customerToWrite.PersonToWrite.Phones));

                personFromRepository.SetTrackingState(TrackingState.Modified);
                customerRepository.FixTrackingState();
            }

            if (await customerRepository.SaveChangesAsync())
                return NoContent();

            return BadRequest($"Failed to update .");
        }

        // POST: api/Customer/
        [HttpPost]
        public async Task<ActionResult<CustomerToRead>> CreateCustomerAsync(CustomerToWrite customerToWrite)
        {
            // VK: here, the logic should be:
            // 1. Look at customerToWrite.EntityType and create a customer of the corresponding type (you can introduce a factory method for this)
            // 2. Save it to the DB

            var entityType = customerToWrite.EntityType;
            Customer customer = null;

            if (entityType == EntityType.Person)
                customer = CreatePersonCustomer(customerToWrite.PersonToWrite);

            if (entityType == EntityType.Organization)
                customer = AddOrganizationCustomer(customerToWrite.OrganizationToWrite);

            if (customer != null)
                await customerRepository.AddCustomerAsync(customer);

            if (!await customerRepository.SaveChangesAsync())
                return BadRequest($"Failed to add {customerToWrite}.");

            CustomerToRead customerFromRepository = await customerRepository.GetCustomerAsync(customer.Id);

            if (customerFromRepository == null)
                return BadRequest($"Failed to add {customerToWrite}.");

            return CreatedAtRoute("GetCustomerAsync",
                new { id = customerFromRepository.Id },
                customerFromRepository);
        }

        private static Customer AddOrganizationCustomer(OrganizationToWrite organizationToWrite)
        {
            var organizationNameOrError = OrganizationName.Create(organizationToWrite.Name);

            if (organizationNameOrError.IsSuccess)
            {
                var organization = new Organization(organizationNameOrError.Value);

                if (organizationToWrite.Contact != null)
                {
                    var contact = new Person(
                        PersonName.Create(
                            organizationToWrite.Contact.Name.LastName,
                            organizationToWrite.Contact.Name.LastName,
                            organizationToWrite.Contact.Name.MiddleName).Value,
                        organizationToWrite.Contact.Gender);

                    if (organizationToWrite?.Contact?.Address != null)
                        contact.SetAddress(Address.Create(
                            organizationToWrite.Contact.Address.AddressLine,
                            organizationToWrite.Contact.Address.City,
                            organizationToWrite.Contact.Address.State,
                            organizationToWrite.Contact.Address.PostalCode).Value);

                    contact.SetBirthday(organizationToWrite.Contact?.Birthday);

                    if (organizationToWrite.Contact.DriversLicense != null)
                    {
                        var dateTimeRange = DateTimeRange.Create(organizationToWrite.Contact.DriversLicense.Issued,
                                                              organizationToWrite.Contact.DriversLicense.Expiry).Value;

                        var driversLicense = DriversLicense.Create(organizationToWrite.Contact.DriversLicense.Number,
                                                                organizationToWrite.Contact.DriversLicense.State,
                                                                dateTimeRange).Value;

                        contact.SetDriversLicense(driversLicense);
                    }

                    if (organizationToWrite?.Contact?.Phones?.Count > 0)
                        foreach (var phone in organizationToWrite.Contact.Phones)
                            contact.AddPhone(new Phone(phone.Number, phone.PhoneType, phone.IsPrimary));

                    if (organizationToWrite?.Contact?.Emails?.Count > 0)
                        foreach (var email in organizationToWrite.Contact.Emails)
                            contact.AddEmail(new Domain.Entities.Email(email.Address, email.IsPrimary));

                    organization.SetContact(contact);
                    organization.SetNote(organizationToWrite.Note);
                }

                Customer customer = new(organization);

                return customer;
            }

            return null;
        }

        private static Customer CreatePersonCustomer(PersonToWrite personToWrite)
        {
            var person = new Person(
                PersonName.Create(
                    personToWrite.Name.LastName, personToWrite.Name.FirstName, personToWrite.Name.MiddleName).Value,
                personToWrite.Gender);

            if (personToWrite?.Address != null)
                person.SetAddress(Address.Create(personToWrite.Address.AddressLine,
                                                personToWrite.Address.City,
                                                personToWrite.Address.State,
                                                personToWrite.Address.PostalCode).Value);

            person.SetBirthday(personToWrite?.Birthday);

            if (personToWrite.DriversLicense != null)
            {
                var dateTimeRange = DateTimeRange.Create(personToWrite.DriversLicense.Issued,
                                                      personToWrite.DriversLicense.Expiry).Value;

                var driversLicense = DriversLicense.Create(personToWrite.DriversLicense.Number,
                                                        personToWrite.DriversLicense.State,
                                                        dateTimeRange).Value;

                person.SetDriversLicense(driversLicense);
            }

            if (personToWrite?.Phones?.Count > 0)
                foreach (var phone in personToWrite.Phones)
                    person.AddPhone(new Phone(phone.Number, phone.PhoneType, phone.IsPrimary));

            if (personToWrite?.Emails?.Count > 0)
                foreach (var email in personToWrite.Emails)
                    person.AddEmail(new Domain.Entities.Email(email.Address, email.IsPrimary));

            return new Customer(person);
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
