﻿using CustomerVehicleManagement.Api.Data;
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

                var organizationNameOrError = OrganizationName.Create(customerUpdateDto.OrganizationUpdateDto.Name);
                if (organizationNameOrError.IsFailure)
                    return BadRequest(organizationNameOrError.Error);

                organizationFromRepository.SetName(organizationNameOrError.Value);

                if (customerUpdateDto.OrganizationUpdateDto?.Address != null)
                    organizationFromRepository.SetAddress(Address.Create(customerUpdateDto.OrganizationUpdateDto.Address.AddressLine,
                                                                         customerUpdateDto.OrganizationUpdateDto.Address.City,
                                                                         customerUpdateDto.OrganizationUpdateDto.Address.State,
                                                                         customerUpdateDto.OrganizationUpdateDto.Address.PostalCode).Value);

                organizationFromRepository.SetNote(customerUpdateDto.OrganizationUpdateDto.Note);
                organizationFromRepository.SetContact(PersonToEdit.ConvertToEntity(customerUpdateDto.OrganizationUpdateDto.Contact));
                organizationFromRepository.SetPhones(PhoneToEdit.ConvertToEntities(customerUpdateDto.OrganizationUpdateDto.Phones));
                organizationFromRepository.SetEmails(EmailToEdit.ConvertToEntities(customerUpdateDto.OrganizationUpdateDto.Emails));

                organizationFromRepository.SetTrackingState(TrackingState.Modified);
                customerRepository.FixTrackingState();
            }

            if (customerFromRepository.EntityType == EntityType.Person)
            {
                Person personFromRepository = await personRepository.GetPersonEntityAsync(customerFromRepository.Person.Id);

                personFromRepository.SetName(PersonName.Create(
                                                customerUpdateDto.PersonUpdateDto.Name.LastName,
                                                customerUpdateDto.PersonUpdateDto.Name.FirstName,
                                                customerUpdateDto.PersonUpdateDto.Name.MiddleName).Value);
                personFromRepository.SetGender(customerUpdateDto.PersonUpdateDto.Gender);
                personFromRepository.SetAddress(customerUpdateDto.PersonUpdateDto.Address);
                personFromRepository.SetBirthday(customerUpdateDto.PersonUpdateDto.Birthday);
                personFromRepository.SetDriversLicense(DriversLicenseToEdit.ConvertToEntity(customerUpdateDto.PersonUpdateDto.DriversLicense));
                personFromRepository.SetEmails(EmailToEdit.ConvertToEntities(customerUpdateDto.PersonUpdateDto.Emails));
                personFromRepository.SetPhones(PhoneToEdit.ConvertToEntities(customerUpdateDto.PersonUpdateDto.Phones));

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
                var organization = new Organization(organizationNameOrError.Value);

                if (organizationAddDto.Contact != null)
                {
                    var contact = new Person(
                        PersonName.Create(
                            organizationAddDto.Contact.Name.LastName,
                            organizationAddDto.Contact.Name.LastName,
                            organizationAddDto.Contact.Name.MiddleName).Value,
                        organizationAddDto.Contact.Gender);

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

        private static Customer CreatePersonCustomer(PersonToAdd personAddDto)
        {
            var person = new Person(
                PersonName.Create(
                    personAddDto.Name.LastName, personAddDto.Name.FirstName, personAddDto.Name.MiddleName).Value,
                personAddDto.Gender);

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
                    person.AddPhone(new Phone(phone.Number, phone.PhoneType, phone.IsPrimary));

            if (personAddDto?.Emails?.Count > 0)
                foreach (var email in personAddDto.Emails)
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
