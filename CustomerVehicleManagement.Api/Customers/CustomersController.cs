using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Api.Organizations;
using CustomerVehicleManagement.Api.Persons;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared;
using CustomerVehicleManagement.Shared.Helpers;
using CustomerVehicleManagement.Shared.Models;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
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

                var organizationNameOrError = OrganizationName.Create(customerToWrite.Organization.Name);
                if (organizationNameOrError.IsFailure)
                    return BadRequest(organizationNameOrError.Error);


                if (customerToWrite.Organization?.Address != null)
                    address = Address.Create(customerToWrite.Organization.Address.AddressLine,
                                                                         customerToWrite.Organization.Address.City,
                                                                         customerToWrite.Organization.Address.State,
                                                                         customerToWrite.Organization.Address.PostalCode).Value;

                if (customerToWrite.Organization?.Phones.Count > 0)
                    foreach (var phone in customerToWrite.Organization.Phones)
                        phones.Add(Phone.Create(phone.Number, phone.PhoneType, phone.IsPrimary).Value);

                if (customerToWrite.Organization?.Emails.Count > 0)
                    foreach (var email in customerToWrite.Organization.Emails)
                        emails.Add(Email.Create(email.Address, email.IsPrimary).Value);

                organizationFromRepository.SetName(organizationNameOrError.Value);
                organizationFromRepository.SetNote(customerToWrite.Organization.Note);
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
                                                customerToWrite.Person.Name.LastName,
                                                customerToWrite.Person.Name.FirstName,
                                                customerToWrite.Person.Name.MiddleName).Value);
                personFromRepository.SetGender(customerToWrite.Person.Gender);
                personFromRepository.SetBirthday(customerToWrite.Person.Birthday);

                if (customerToWrite.Person?.Address != null)
                    personFromRepository.SetAddress(Address.Create(customerToWrite.Person.Address.AddressLine,
                                                                   customerToWrite.Person.Address.City,
                                                                   customerToWrite.Person.Address.State,
                                                                   customerToWrite.Person.Address.PostalCode).Value);
                if (customerToWrite.Person?.Phones.Count > 0)
                    foreach (var phone in customerToWrite.Person.Phones)
                        phones.Add(Phone.Create(phone.Number, phone.PhoneType, phone.IsPrimary).Value);

                if (customerToWrite.Person?.Emails.Count > 0)
                    foreach (var email in customerToWrite.Person.Emails)
                        emails.Add(Email.Create(email.Address, email.IsPrimary).Value);

                if (customerToWrite.Person?.DriversLicense != null)
                {
                    DateTimeRange dateTimeRange = DateTimeRange.Create(
                        customerToWrite.Person.DriversLicense.Issued,
                        customerToWrite.Person.DriversLicense.Expiry).Value;

                    driversLicense = DriversLicense.Create(customerToWrite.Person.DriversLicense.Number,
                        customerToWrite.Person.DriversLicense.State,
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
        public async Task<ActionResult<CustomerToRead>> AddCustomerAsync(CustomerToWrite customerToAdd)
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
