using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Api.Organizations;
using CustomerVehicleManagement.Api.Persons;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace CustomerVehicleManagement.Api.Customers
{
    public class CustomersController : ApplicationController
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
        public async Task<ActionResult<Customer>> UpdateCustomerAsync(long id, CustomerToEdit customerUpdateDto)
        {
            CustomerToRead customerFromRepository = await customerRepository.GetCustomerAsync(id);

            if (customerFromRepository == null || customerFromRepository?.EntityType == null)
                return NotFound($"Could not find Customer in the database to update.");

            DriversLicense driversLicense = null;
            List<Phone> phones = new();
            List<Email> emails = new();
            Address address = null;

            if (customerFromRepository.EntityType == EntityType.Organization)
            {
                Organization organizationFromRepository = await organizationRepository.GetOrganizationEntityAsync(customerFromRepository.Organization.Id);

                if (organizationFromRepository == null)
                    return NotFound($"Could not find Organization '{customerFromRepository.Organization.Name}' in the database to update.");

                var organizationNameOrError = OrganizationName.Create(customerUpdateDto.OrganizationUpdateDto.Name);
                if (organizationNameOrError.IsFailure)
                    return BadRequest(organizationNameOrError.Error);


                if (customerUpdateDto.OrganizationUpdateDto?.Address != null)
                    address = Address.Create(customerUpdateDto.OrganizationUpdateDto.Address.AddressLine,
                                                                         customerUpdateDto.OrganizationUpdateDto.Address.City,
                                                                         customerUpdateDto.OrganizationUpdateDto.Address.State,
                                                                         customerUpdateDto.OrganizationUpdateDto.Address.PostalCode).Value;

                if (customerUpdateDto.OrganizationUpdateDto?.Phones.Count > 0)
                    foreach (var phone in customerUpdateDto.OrganizationUpdateDto.Phones)
                        phones.Add(Phone.Create(phone.Number, phone.PhoneType, phone.IsPrimary).Value);

                if (customerUpdateDto.OrganizationUpdateDto?.Emails.Count > 0)
                    foreach (var email in customerUpdateDto.OrganizationUpdateDto.Emails)
                        emails.Add(Email.Create(email.Address, email.IsPrimary).Value);

                organizationFromRepository.SetName(organizationNameOrError.Value);
                organizationFromRepository.SetNote(customerUpdateDto.OrganizationUpdateDto.Note);
                organizationFromRepository.SetAddress(address);
                organizationFromRepository.SetEmails(emails);
                organizationFromRepository.SetPhones(phones);

                organizationFromRepository.SetTrackingState(TrackingState.Modified);
                customerRepository.FixTrackingState();
            }

            if (customerFromRepository.EntityType == EntityType.Person)
            {
                // VK: here, the logic should be:
                // 1. Get the customer entity (not DTO) from the DB
                // 2. Look at its type
                // 3. Update the corresponding fields in the customer depending on the type (i.e take the fields from the DTO needed for this specific customer type)
                // 4. Save back to the DB

                Person personFromRepository = await personRepository.GetPersonEntityAsync(customerFromRepository.Person.Id);

                personFromRepository.SetName(PersonName.Create(
                                                customerUpdateDto.PersonUpdateDto.Name.LastName,
                                                customerUpdateDto.PersonUpdateDto.Name.FirstName,
                                                customerUpdateDto.PersonUpdateDto.Name.MiddleName).Value);
                personFromRepository.SetGender(customerUpdateDto.PersonUpdateDto.Gender);
                personFromRepository.SetAddress(customerUpdateDto.PersonUpdateDto.Address);
                personFromRepository.SetBirthday(customerUpdateDto.PersonUpdateDto.Birthday);

                if (customerUpdateDto.PersonUpdateDto?.Phones.Count > 0)
                    foreach (var phone in customerUpdateDto.PersonUpdateDto.Phones)
                        phones.Add(Phone.Create(phone.Number, phone.PhoneType, phone.IsPrimary).Value);

                if (customerUpdateDto.PersonUpdateDto?.Emails.Count > 0)
                    foreach (var email in customerUpdateDto.PersonUpdateDto.Emails)
                        emails.Add(Email.Create(email.Address, email.IsPrimary).Value);

                if (customerUpdateDto.PersonUpdateDto?.DriversLicense != null)
                {
                    DateTimeRange dateTimeRange = DateTimeRange.Create(
                        customerUpdateDto.PersonUpdateDto.DriversLicense.Issued,
                        customerUpdateDto.PersonUpdateDto.DriversLicense.Expiry).Value;

                    driversLicense = DriversLicense.Create(customerUpdateDto.PersonUpdateDto.DriversLicense.Number,
                        customerUpdateDto.PersonUpdateDto.DriversLicense.State,
                        dateTimeRange).Value;

                    personFromRepository.SetDriversLicense(driversLicense);
                }

                personFromRepository.SetEmails(emails);
                personFromRepository.SetPhones(phones);

                personFromRepository.SetTrackingState(TrackingState.Modified);
                customerRepository.FixTrackingState();
            }

            if (await customerRepository.SaveChangesAsync())
                return NoContent();

            return BadRequest($"Failed to update .");
        }

        // POST: api/Customer/
        [HttpPost]
        public async Task<ActionResult<CustomerToRead>> CreateCustomerAsync(CustomerToAdd customerCreateDto)
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

            CustomerToRead customerFromRepository = await customerRepository.GetCustomerAsync(customer.Id);

            if (customerFromRepository == null)
                return BadRequest($"Failed to add {customerCreateDto}.");

            return CreatedAtRoute("GetCustomerAsync",
                new { id = customerFromRepository.Id },
                customerFromRepository);
        }

        private static Customer AddOrganizationCustomer(OrganizationToAdd organizationAddDto)
        {
            var organizationNameOrError = OrganizationName.Create(organizationAddDto.Name);

            if (organizationNameOrError.IsSuccess)
            {
                var organization = new Organization(organizationNameOrError.Value, null, null, null);

                if (organizationAddDto.Contact != null)
                {
                    var contact = new Person(
                        PersonName.Create(
                            organizationAddDto.Contact.Name.LastName,
                            organizationAddDto.Contact.Name.LastName,
                            organizationAddDto.Contact.Name.MiddleName).Value,
                        organizationAddDto.Contact.Gender, null, null, null, null, null);

                    if (organizationAddDto?.Contact?.Address != null)
                        contact.SetAddress(Address.Create(
                            organizationAddDto.Contact.Address.AddressLine,
                            organizationAddDto.Contact.Address.City,
                            organizationAddDto.Contact.Address.State,
                            organizationAddDto.Contact.Address.PostalCode).Value);

                    contact.SetBirthday(organizationAddDto.Contact?.Birthday);

                    if (organizationAddDto.Contact.DriversLicense != null)
                    {
                        var dateTimeRange = DateTimeRange.Create(organizationAddDto.Contact.DriversLicense.Issued,
                                                              organizationAddDto.Contact.DriversLicense.Expiry).Value;

                        var driversLicense = DriversLicense.Create(organizationAddDto.Contact.DriversLicense.Number,
                                                                organizationAddDto.Contact.DriversLicense.State,
                                                                dateTimeRange).Value;

                        contact.SetDriversLicense(driversLicense);
                    }

                    if (organizationAddDto?.Contact?.Phones?.Count > 0)
                        foreach (var phone in organizationAddDto.Contact.Phones)
                            contact.AddPhone(Phone.Create(phone.Number, phone.PhoneType, phone.IsPrimary).Value);

                    if (organizationAddDto?.Contact?.Emails?.Count > 0)
                        foreach (var email in organizationAddDto.Contact.Emails)
                            contact.AddEmail(Email.Create(email.Address, email.IsPrimary).Value);

                    organization.SetContact(contact);
                    organization.SetNote(organizationAddDto.Note);
                }

                Customer customer = new(organization);

                return customer;
            }

            return null;
        }

        private static Customer CreatePersonCustomer(PersonToAdd personAddDto)
        {
            var person = new Person(
                PersonName.Create(
                    personAddDto.Name.LastName, personAddDto.Name.FirstName, personAddDto.Name.MiddleName).Value,
                personAddDto.Gender, null, null, null, null, null);

            if (personAddDto?.Address != null)
                person.SetAddress(Address.Create(personAddDto.Address.AddressLine,
                                                personAddDto.Address.City,
                                                personAddDto.Address.State,
                                                personAddDto.Address.PostalCode).Value);

            person.SetBirthday(personAddDto?.Birthday);

            if (personAddDto.DriversLicense != null)
            {
                var dateTimeRange = DateTimeRange.Create(personAddDto.DriversLicense.Issued,
                                                      personAddDto.DriversLicense.Expiry).Value;

                var driversLicense = DriversLicense.Create(personAddDto.DriversLicense.Number,
                                                        personAddDto.DriversLicense.State,
                                                        dateTimeRange).Value;

                person.SetDriversLicense(driversLicense);
            }

            if (personAddDto?.Phones?.Count > 0)
                foreach (var phone in personAddDto.Phones)
                    person.AddPhone(Phone.Create(phone.Number, phone.PhoneType, phone.IsPrimary).Value);

            if (personAddDto?.Emails?.Count > 0)
                foreach (var email in personAddDto.Emails)
                    person.AddEmail(Email.Create(email.Address, email.IsPrimary).Value);

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
