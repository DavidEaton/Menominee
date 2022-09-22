using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Api.Persons;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using CustomerVehicleManagement.Shared.Models.Organizations;
using Menominee.Common.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Address = Menominee.Common.ValueObjects.Address;
using Phone = CustomerVehicleManagement.Domain.Entities.Phone;

namespace CustomerVehicleManagement.Api.Organizations
{
    public class OrganizationsController : ApplicationController
    {
        private readonly IOrganizationRepository repository;
        private readonly IPersonRepository personsRepository;
        private readonly PersonsController personsController;
        private readonly string BasePath = "/api/organizations";

        public OrganizationsController(IOrganizationRepository repository,
                                       PersonsController personsController,
                                       IPersonRepository personsRepository)
        {
            this.repository = repository ??
                throw new ArgumentNullException(nameof(repository));
            this.personsController = personsController ??
                throw new ArgumentNullException(nameof(personsController));
            this.personsRepository = personsRepository;
        }

        // api/organizations/list
        [Route("list")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrganizationToReadInList>>> GetOrganizationsListAsync()
        {
            var organizations = await repository.GetOrganizationsListAsync();

            if (organizations is null)
                return NotFound();

            return Ok(organizations);
        }

        // api/organizations
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrganizationToRead>>> GetOrganizationsAsync()
        {
            var organizations = await repository.GetOrganizationsAsync();

            if (organizations is null)
                return NotFound();

            return Ok(organizations);
        }

        // api/organizations/1
        [HttpGet("{id:long}", Name = "GetOrganizationAsync")]
        public async Task<ActionResult<OrganizationToRead>> GetOrganizationAsync(long id)
        {
            var organization = await repository.GetOrganizationAsync(id);

            if (organization is null)
                return NotFound();

            return organization;
        }

        // api/organizations/1
        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateOrganizationAsync(long id, OrganizationToWrite organizationFromCaller)
        {
            /* Update Pattern in Controllers:
                1) Get domain entity from repository
                2) Update domain entity with data in data transfer object (DTO)
                3) Save changes
                4) return NoContent()
            */
            // VK: here, the logic should be:
            // 1. Get the Organization entity (not DTO) from the DB
            // 3. Update the corresponding fields in the Organization
            // 4. Save back to the DB

            var notFoundMessage = $"Could not find Organization to update: {organizationFromCaller.Name}";

            if (!await repository.OrganizationExistsAsync(id))
                return NotFound(notFoundMessage);

            //1) Get domain entity from repository
            Organization organizationFromRepository = await repository.GetOrganizationEntityAsync(id);

            if (organizationFromRepository is null)
                return NotFound(notFoundMessage);

            // 2) Update domain entity with data in data transfer object(DTO)
            var organizationNameOrError = OrganizationName.Create(organizationFromCaller.Name);

            if (organizationNameOrError.IsSuccess)
                organizationFromRepository.SetName(organizationNameOrError.Value);

            if (organizationFromCaller?.Address != null)
                organizationFromRepository.SetAddress(
                    Address.Create(
                        organizationFromCaller.Address.AddressLine,
                        organizationFromCaller.Address.City,
                        organizationFromCaller.Address.State,
                        organizationFromCaller.Address.PostalCode).Value);

            // Client may send an empty or null Address VALUE OBJECT, signifying REMOVAL
            if (organizationFromCaller?.Address is null)
                organizationFromRepository.SetAddress(null);

            organizationFromRepository.SetNote(organizationFromCaller.Note);

            // Client may send an empty or null phones collection of ENTITIES, signifying
            // NO CHANGE TO COLLECTION
            foreach (var phone in organizationFromCaller?.Phones)
            {
                if (phone.Id == 0)
                    organizationFromRepository.AddPhone(Phone.Create(phone.Number, phone.PhoneType, phone.IsPrimary).Value);

                if (phone.Id != 0)
                {
                    var contextPhone = organizationFromRepository.Phones.FirstOrDefault(contextPhone => contextPhone.Id == phone.Id);
                    contextPhone.SetNumber(phone.Number);
                    contextPhone.SetIsPrimary(phone.IsPrimary);
                    contextPhone.SetPhoneType(phone.PhoneType);
                }

                if (phone.Id != 0)
                    organizationFromRepository.RemovePhone(
                        organizationFromRepository.Phones.FirstOrDefault(
                            contextPhone =>
                            contextPhone.Id == phone.Id));
            }

            // Client may send an empty or null emails collection, signifying
            // NO CHANGE TO COLLECTION
            List<Email> emails = new();
            if (organizationFromCaller?.Emails.Count > 0)
            {
                emails.AddRange(organizationFromCaller.Emails
                    .Select(email =>
                            Email.Create(email.Address,
                                         email.IsPrimary).Value));
            }

            // Contact
            if (organizationFromCaller?.Contact != null)
            {
                var result = await personsController.UpdatePersonAsync(
                                    organizationFromRepository.Contact.Id,
                                    organizationFromCaller.Contact);

                var person = await personsRepository.GetPersonEntityAsync(organizationFromRepository.Contact.Id);

                if (person != null)
                    organizationFromRepository.SetContact(person);
            }

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
            var organization = OrganizationHelper.ConvertWriteDtoToEntity(organizationToAdd);

            // 2. Add domain entity to repository
            await repository.AddOrganizationAsync(organization);

            // 3. Save changes on repository
            await repository.SaveChangesAsync();

            // 4. Return new id and route to new resource after save
            return Created(new Uri($"{BasePath}/{organization.Id}",
                               UriKind.Relative),
                               new { organization.Id });
        }


        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteOrganizationAsync(long id)
        {
            /* Delete Pattern in Controllers:
             1) Get domain entity from repository
             2) Call repository.Delete(), which removes entity from context
             3) Save changes
             4) return NoContent()
            */
            var organizationFromRepository = await repository.GetOrganizationEntityAsync(id);

            if (organizationFromRepository is null)
                return NotFound($"Could not find Organization in the database to delete with Id: {id}.");

            repository.DeleteOrganization(organizationFromRepository);

            await repository.SaveChangesAsync();

            return NoContent();
        }
    }
}