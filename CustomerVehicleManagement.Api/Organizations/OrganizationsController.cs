using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Organizations
{
    public class OrganizationsController : ApplicationController
    {
        private readonly IOrganizationRepository repository;
        private readonly string BasePath = "/api/organizations/";

        public OrganizationsController(IOrganizationRepository repository)
        {
            this.repository = repository ??
                throw new ArgumentNullException(nameof(repository));
        }

        // api/organizations/list
        [Route("list")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrganizationToReadInList>>> GetOrganizationsListAsync()
        {
            var results = await repository.GetOrganizationsListAsync();
            return Ok(results);
        }

        // api/organizations
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrganizationToRead>>> GetOrganizationsAsync()
        {
            var result = await repository.GetOrganizationsAsync();
            return Ok(result);
        }

        // api/organizations/1
        [HttpGet("{id:long}", Name = "GetOrganizationAsync")]
        public async Task<ActionResult<OrganizationToRead>> GetOrganizationAsync(long id)
        {
            var result = await repository.GetOrganizationAsync(id);

            if (result == null)
                return NotFound();

            return result;
        }

        // api/organizations/1
        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateOrganizationAsync(long id, OrganizationToEdit organizationUpdateDto)
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
            // VK: here, the logic should be:
            // 1. Get the Organization entity (not DTO) from the DB
            // 3. Update the corresponding fields in the Organization
            // 4. Save back to the DB

            var notFoundMessage = $"Could not find Organization to update: {organizationUpdateDto.Name}";

            if (!await repository.OrganizationExistsAsync(id))
                return NotFound(notFoundMessage);

            DriversLicense driversLicense;
            List<Phone> phones = new();
            List<Email> emails = new();
            Address address = null;

            //1) Get domain entity from repository
            var organization = repository.GetOrganizationEntityAsync(id).Result;

            // 2) Update domain entity with data in data transfer object(DTO)
            var organizationNameOrError = OrganizationName.Create(organizationUpdateDto.Name);

            if (organizationNameOrError.IsSuccess)
                organization.SetName(organizationNameOrError.Value);

            if (organizationUpdateDto?.Address != null)
                organization.SetAddress(Address.Create(organizationUpdateDto.Address.AddressLine,
                                                                     organizationUpdateDto.Address.City,
                                                                     organizationUpdateDto.Address.State,
                                                                     organizationUpdateDto.Address.PostalCode).Value);
            organization.SetNote(organizationUpdateDto.Note);

            if (organizationUpdateDto?.Phones.Count > 0)
                foreach (var phone in organizationUpdateDto.Phones)
                {
                    phones.Add(Phone.Create(phone.Number, phone.PhoneType, phone.IsPrimary).Value);
                    organization.SetPhones(phones);
                }

            if (organizationUpdateDto?.Emails.Count > 0)
                foreach (var email in organizationUpdateDto.Emails)
                {
                    emails.Add(Email.Create(email.Address, email.IsPrimary).Value);
                    organization.SetEmails(emails);
                }

            if (organizationUpdateDto.Contact != null)
            {
                var contact = new Person(PersonName.Create(
                                            organizationUpdateDto.Contact.Name.LastName,
                                            organizationUpdateDto.Contact.Name.FirstName,
                                            organizationUpdateDto.Contact.Name.MiddleName).Value,
                                            organizationUpdateDto.Contact.Gender,
                                            address,
                                            null, null);

                if (organizationUpdateDto.Contact?.DriversLicense != null)
                {
                    DateTimeRange dateTimeRange = DateTimeRange.Create(
                        organizationUpdateDto.Contact.DriversLicense.Issued,
                        organizationUpdateDto.Contact.DriversLicense.Expiry).Value;

                    driversLicense = DriversLicense.Create(organizationUpdateDto.Contact.DriversLicense.Number,
                        organizationUpdateDto.Contact.DriversLicense.State,
                        dateTimeRange).Value;

                    contact.SetDriversLicense(driversLicense);
                }

                if (organizationUpdateDto?.Contact?.Address != null)
                    contact.SetAddress(Address.Create(organizationUpdateDto.Contact.Address.AddressLine,
                                                           organizationUpdateDto.Contact.Address.City,
                                                           organizationUpdateDto.Contact.Address.State,
                                                           organizationUpdateDto.Contact.Address.PostalCode).Value);

                contact.SetBirthday(organizationUpdateDto.Contact.Birthday);
                contact.SetPhones(phones);
                contact.SetEmails(emails);

                organization.SetContact(contact);
            }

            // Update the objects ObjectState and sych the EF Change Tracker
            // 3) Set entity's TrackingState to Modified
            organization.SetTrackingState(TrackingState.Modified);
            // 4) FixTrackingState: moves entity state tracking into the context
            repository.FixTrackingState();

            repository.UpdateOrganizationAsync(organization);

            /* Returning the updated resource is acceptible, for example:
                 return Ok(personFromRepository);
               even preferred over returning NoContent if updated resource
               contains properties that are mutated by the data store
               (which they are not in this case).

               Instead, our app will:
                 return NoContent();
               ... and let the caller decide whether to get the updated resource,
               which is also acceptible. The HTTP specification (RFC 2616) has a
               number of recommendations that are applicable:
            HTTP status code 200 OK for a successful PUT of an update to an existing resource. No response body needed.
            HTTP status code 201 Created for a successful PUT of a new resource
            HTTP status code 409 Conflict for a PUT that is unsuccessful due to a 3rd-party modification
            HTTP status code 400 Bad Request for an unsuccessful PUT
            */

            await repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> AddOrganizationAsync(OrganizationToAdd organizationToAdd)
        {
            /*
                Web API controllers don't have to check ModelState.IsValid if they have the
                [ApiController] attribute; most of our controllers inherit from ApplicationController,
                which has the [ApiController] attribute. With [ApiController] attribute applied,
                an automatic HTTP 400 response containing error details is returned when model
                state is invalid.*/

            /* Controller Pattern:
                1. Convert data contract/data transfer object (dto) to domain entity
                2. Add domain entity to repository
                3. Save changes on repository
                4. Return to consumer */

            // 1. Convert dto to domain entity
            Address organizationAddress = null;
            List<Phone> phones = new();
            List<Email> emails = new();

            // FluentValidation has already validated request; no need to validate Name again here
            var organizationName = OrganizationName.Create(organizationToAdd.Name).Value;

            if (organizationToAdd?.Address != null)
                organizationAddress = Address.Create(
                    organizationToAdd.Address.AddressLine,
                    organizationToAdd.Address.City,
                    organizationToAdd.Address.State,
                    organizationToAdd.Address.PostalCode).Value;

            if (organizationToAdd?.Phones?.Count > 0)
                // FluentValidation has already validated contactable collections
                foreach (var phone in organizationToAdd.Phones)
                    phones.Add(Phone.Create(phone.Number, phone.PhoneType, phone.IsPrimary).Value);

            if (organizationToAdd?.Emails?.Count > 0)
                foreach (var email in organizationToAdd.Emails)
                    emails.Add(Email.Create(email.Address, email.IsPrimary).Value);

            //Organization.Contact
            Person person = null;
            Address personAddress = null;
            DriversLicense driversLicense = null;

            if (organizationToAdd?.Contact != null)
            {
                if (organizationToAdd?.Contact?.Address != null)
                    personAddress = Address.Create(
                        organizationToAdd.Contact.Address.AddressLine,
                        organizationToAdd.Contact.Address.City,
                        organizationToAdd.Contact.Address.State,
                        organizationToAdd.Contact.Address.PostalCode).Value;

                if (organizationToAdd?.Contact?.DriversLicense != null)
                {
                    DateTimeRange dateTimeRange = DateTimeRange.Create(
                        organizationToAdd.Contact.DriversLicense.Issued,
                        organizationToAdd.Contact.DriversLicense.Expiry).Value;

                    driversLicense = DriversLicense.Create(organizationToAdd.Contact.DriversLicense.Number,
                        organizationToAdd.Contact.DriversLicense.State,
                        dateTimeRange).Value;
                }

                person = new Person(
                PersonName.Create(
                    organizationToAdd.Contact.Name.LastName,
                    organizationToAdd.Contact.Name.FirstName,
                    organizationToAdd.Contact.Name.MiddleName).Value,
                organizationToAdd.Contact.Gender,
                personAddress, emails, phones,
                organizationToAdd.Contact.Birthday,
                driversLicense);
            }

            var organization = new Organization(organizationName, organizationToAdd.Note, person, organizationAddress, emails, phones);

            // 2. Add domain entity to repository
            await repository.AddOrganizationAsync(organization);

            // 3. Save changes on repository
            await repository.SaveChangesAsync();

            // 4. Return new Id from database to consumer after save
            return Created(new Uri($"{BasePath}/{organization.Id}", UriKind.Relative), new { id = organization.Id });
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteOrganizationAsync(long id)
        {
            /* Delete Pattern in Controllers:
             1) Get domain entity from repository
             2) Call repository.Delete(), which removes entity from context
             3) Save changes
             4) return Ok()
         */
            var organizationFromRepository = await repository.GetOrganizationAsync(id);
            if (organizationFromRepository == null)
                return NotFound($"Could not find Organization in the database to delete with Id: {id}.");

            await repository.DeleteOrganizationAsync(id);

            await repository.SaveChangesAsync();

            return NoContent();
        }
    }
}
