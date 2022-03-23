using CustomerVehicleManagement.Api.Data;
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

namespace CustomerVehicleManagement.Api.Organizations
{
    [Authorize(Policies.PaidUser)]
    public class OrganizationsController : ApplicationController
    {
        private readonly IOrganizationRepository repository;
        private readonly string BasePath = "/api/organizations";
        private readonly PersonsController personsController;

        public OrganizationsController(IOrganizationRepository repository,
                                       PersonsController personsController)
        {
            this.repository = repository ??
                throw new ArgumentNullException(nameof(repository));
            this.personsController = personsController ??
                throw new ArgumentNullException(nameof(personsController));
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
        public async Task<IActionResult> UpdateOrganizationAsync(long id, OrganizationToWrite organizationToUpdate)
        {
            /* Update Pattern in Controllers:
                1) Get domain entity from repository
                2) Update domain entity with data in data transfer object (DTO)
                3) Set entity's TrackingState to Modified
                4) FixTrackingState: moves entity state tracking back out of
                the object and into the context to track entity state in this
                disconnected application. In other words, sych the EF Change
                Tracker with our disconnected entitys TrackingState.
                5) Save changes
                6) return NoContent()
            */
            // VK: here, the logic should be:
            // 1. Get the Organization entity (not DTO) from the DB
            // 3. Update the corresponding fields in the Organization
            // 4. Save back to the DB

            var notFoundMessage = $"Could not find Organization to update: {organizationToUpdate.Name}";

            if (!await repository.OrganizationExistsAsync(id))
                return NotFound(notFoundMessage);

            //1) Get domain entity from repository
            var organizationFromRepository = repository.GetOrganizationEntityAsync(id).Result;

            // 2) Update domain entity with data in data transfer object(DTO)
            var organizationNameOrError = OrganizationName.Create(organizationToUpdate.Name);

            if (organizationNameOrError.IsSuccess)
                organizationFromRepository.SetName(organizationNameOrError.Value);

            if (organizationToUpdate?.Address != null)
                organizationFromRepository.SetAddress(Address.Create(organizationToUpdate.Address.AddressLine,
                                                                     organizationToUpdate.Address.City,
                                                                     organizationToUpdate.Address.State,
                                                                     organizationToUpdate.Address.PostalCode).Value);
            organizationFromRepository.SetNote(organizationToUpdate.Note);

            List<Phone> phones = new();
            if (organizationToUpdate?.Phones.Count > 0)
                foreach (var phone in organizationToUpdate.Phones)
                    phones.Add(Phone.Create(phone.Number, phone.PhoneType, phone.IsPrimary).Value);
            organizationFromRepository.SetPhones(phones);

            List<Email> emails = new();
            if (organizationToUpdate?.Emails.Count > 0)
                foreach (var email in organizationToUpdate.Emails)
                    emails.Add(Email.Create(email.Address, email.IsPrimary).Value);
            organizationFromRepository.SetEmails(emails);

            if (organizationToUpdate?.Contact != null)
            {
                var contact = await personsController.UpdatePersonAsync(organizationFromRepository.Contact.Id, organizationToUpdate.Contact);
                organizationFromRepository.SetContact((Person)contact);
            }

            // Update the objects ObjectState and sych the EF Change Tracker
            // 3) Set entity's TrackingState to Modified
            organizationFromRepository.SetTrackingState(TrackingState.Modified);
            // 4) FixTrackingState: moves entity state tracking into the context
            repository.FixTrackingState();

            repository.UpdateOrganizationAsync(organizationFromRepository);

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
        public async Task<IActionResult> AddOrganizationAsync(OrganizationToWrite organizationToAdd)
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
            var organization = OrganizationHelper.CreateEntityFromWriteDto(organizationToAdd);

            // 2. Add domain entity to repository
            await repository.AddOrganizationAsync(organization);

            // 3. Save changes on repository
            await repository.SaveChangesAsync();

            // 4. Return new Id from database to consumer after save
            return Created(new Uri($"{BasePath}/{organization.Id}",
                               UriKind.Relative),
                               new { id = organization.Id });
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
