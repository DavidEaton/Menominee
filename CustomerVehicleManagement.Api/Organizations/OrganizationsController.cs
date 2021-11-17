using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Organizations
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationsController : ControllerBase
    {
        private readonly IOrganizationRepository repository;

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
        public async Task<IActionResult> UpdateOrganizationAsync(long id, OrganizationToWrite organizationToWrite)
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

            var notFoundMessage = $"Could not find Organization to update: {organizationToWrite.Name}";

            if (!await repository.OrganizationExistsAsync(id))
                return NotFound(notFoundMessage);

            //1) Get domain entity from repository
            var organization = repository.GetOrganizationEntityAsync(id).Result;

            // 2) Update domain entity with data in data transfer object(DTO)
            var organizationNameOrError = OrganizationName.Create(organizationToWrite.Name);

            if (organizationNameOrError.IsSuccess)
                organization.SetName(organizationNameOrError.Value);

            if (organizationToWrite?.Address != null)
                organization.SetAddress(Address.Create(organizationToWrite.Address.AddressLine,
                                                                     organizationToWrite.Address.City,
                                                                     organizationToWrite.Address.State,
                                                                     organizationToWrite.Address.PostalCode).Value);
            organization.SetNote(organizationToWrite.Note);

            if (organizationToWrite.Phones.Count > 0)
                organization.SetPhones(PhoneToWrite.ConvertToEntities(organizationToWrite.Phones));

            if (organizationToWrite.Emails.Count > 0)
                organization.SetEmails(EmailToWrite.ConvertToEntities(organizationToWrite.Emails));

            if (organizationToWrite.Contact != null)
            {
                var contact = new Person(PersonName.Create(
                                            organizationToWrite.Contact.Name.LastName,
                                            organizationToWrite.Contact.Name.FirstName,
                                            organizationToWrite.Contact.Name.MiddleName).Value,
                                        organizationToWrite.Contact.Gender);

                contact.SetAddress(AddressToWrite.ConvertToEntity(organizationToWrite.Contact.Address));
                contact.SetBirthday(organizationToWrite.Contact.Birthday);
                contact.SetDriversLicense(DriversLicenseToWrite.ConvertToEntity(organizationToWrite.Contact.DriversLicense));
                contact.SetPhones(PhoneToWrite.ConvertToEntities(organizationToWrite.Contact.Phones));
                contact.SetEmails(EmailToWrite.ConvertToEntities(organizationToWrite.Contact.Emails));

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

            if (await repository.SaveChangesAsync())
                return NoContent();

            return BadRequest($"Failed to update {organizationToWrite.Name}.");
        }

        [HttpPost]
        public async Task<ActionResult<OrganizationToRead>> AddOrganizationAsync(OrganizationToWrite organizationToWrite)
        {
            /*
                Web API controllers don't have to check ModelState.IsValid if they have the
                [ApiController] attribute. In that case, an automatic HTTP 400 response containing
                error details is returned when model state is invalid.*/

            /* Controller Pattern:
                1. Convert data transfer object (dto) to domain entity
                2. Add domain entity to repository
                3. Save changes on repository
                4. Get ReadDto (with new Id) from database after save)
                5. Return to consumer */

            // 1. Convert dto to domain entity
            var organizationNameOrError = OrganizationName.Create(organizationToWrite.Name);
            if (organizationNameOrError.IsFailure)
                return BadRequest(organizationNameOrError.Error);

            var organization = new Organization(organizationNameOrError.Value);

            organization.SetNote(organizationToWrite.Note);

            if (organizationToWrite?.Address != null)
                organization.SetAddress(Address.Create(organizationToWrite.Address.AddressLine,
                                                organizationToWrite.Address.City,
                                                organizationToWrite.Address.State,
                                                organizationToWrite.Address.PostalCode).Value);

            if (organizationToWrite?.Phones != null)
                organization.SetPhones(organizationToWrite?.Phones
                                                          .Select(phone => PhoneToWrite.ConvertToEntity(phone))
                                                          .ToList());

            if (organizationToWrite?.Emails != null)
                organization.SetEmails(organizationToWrite?.Emails
                                                          .Select(email => EmailToWrite.ConvertToEntity(email))
                                                          .ToList());

            organization.SetContact(PersonToWrite.ConvertToEntity(organizationToWrite?.Contact));

            // 2. Add domain entity to repository
            await repository.AddOrganizationAsync(organization);

            // 3. Save changes on repository
            if (await repository.SaveChangesAsync())
            {
                // 4. Get ReadDto (with new Id) from database after save)
                OrganizationToRead result = repository.GetOrganizationAsync(organization.Id).Result;
                // 5. Return to consumer
                return CreatedAtRoute("GetOrganizationAsync",
                new { id = result.Id },
                    result);
            }

            return BadRequest($"Failed to add {organizationToWrite.Name}.");
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

            if (await repository.SaveChangesAsync())
                return NoContent();

            return BadRequest($"Failed to delete Organization with Id: {id}.");
        }
    }
}
