using AutoMapper;
using CustomerVehicleManagement.Api.Utilities;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Enums;
using SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Organizations
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationsController : ControllerBase
    {
        private readonly IOrganizationRepository repository;
        private readonly IMapper mapper;

        public OrganizationsController(IOrganizationRepository repository, IMapper mapper)
        {
            this.repository = repository ??
                throw new ArgumentNullException(nameof(repository));

            this.mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        // api/organizations/list
        [Route("list")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrganizationInListDto>>> GetOrganizationsListAsync()
        {
            var results = await repository.GetOrganizationsListAsync();
            return Ok(results);
        }

        // api/organizations
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrganizationReadDto>>> GetOrganizationsAsync()
        {
            var result = await repository.GetOrganizationsAsync();
            return Ok(result);
        }

        // api/organizations/1
        [HttpGet("{id:int}", Name = "GetOrganizationAsync")]
        public async Task<ActionResult<OrganizationReadDto>> GetOrganizationAsync(int id)
        {
            var result = await repository.GetOrganizationAsync(id);

            if (result == null)
                return NotFound();

            return result;
        }

        // api/organizations/1
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateOrganizationAsync(int id, OrganizationUpdateDto organizationUpdateDto)
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

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var notFoundMessage = $"Could not find Organization to update: {organizationUpdateDto.Name}";

            if (!await repository.OrganizationExistsAsync(id))
                return NotFound(notFoundMessage);

            //1) Get domain entity from repository
            var organization = repository.GetOrganizationEntityAsync(id).Result;

            // 2) Update domain entity with data in data transfer object(DTO)
            var organizationNameOrError = OrganizationName.Create(organizationUpdateDto.Name);

            if (organizationNameOrError.IsSuccess)
                organization.SetName(organizationNameOrError.Value);

            organization.SetAddress(organizationUpdateDto.Address);
            organization.SetNote(organizationUpdateDto.Notes);

            organization.SetPhones(DtoHelpers.PhonesUpdateDtoToPhones(organizationUpdateDto.Phones));
            organization.SetEmails(DtoHelpers.EmailsUpdateDtoToEmails(organizationUpdateDto.Emails));

            if (organizationUpdateDto.Contact != null)
            {
                var contact = new Person(organizationUpdateDto.Contact.Name, organizationUpdateDto.Contact.Gender);

                contact.SetAddress(organizationUpdateDto.Contact.Address);
                contact.SetBirthday(organizationUpdateDto.Contact.Birthday);
                contact.SetDriversLicense(organizationUpdateDto.Contact.DriversLicense);
                contact.SetPhones(mapper.Map<IList<Phone>>(organizationUpdateDto.Contact.Phones));
                contact.SetEmails(mapper.Map<IList<Email>>(organizationUpdateDto.Contact.Emails));

                organization.SetContact(contact);
            }

            // TODO: Discover why the next two operations break unit tests
            // and our Controller update pattern:

            //personFromRepository.SetTrackingState(TrackingState.Modified);
            //repository.FixTrackingState();

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

            return BadRequest($"Failed to update {organizationUpdateDto.Name}.");
        }

        [HttpPost]
        public async Task<ActionResult<OrganizationReadDto>> CreateOrganizationAsync(OrganizationCreateDto organizationCreateDto)
        {
            /* Controller Pattern:
                1. Map data transfer object (dto) to domain entity
                2. Add domain entity to repository
                3. Save changes on repository
                4. Get ReadDto (with new Id) from database after save)
                5. Return to consumer */

            /*
                Web API controllers don't have to check ModelState.IsValid if they have the
                [ApiController] attribute. In that case, an automatic HTTP 400 response containing
                error details is returned when model state is invalid.

                However, excluding the ModelState check code breaks tests. Comment the ModelState
                check code (if (!ModelState.IsValid)...) to break tests: */

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // 1. Map dto to domain entity
            var organizationNameOrError = OrganizationName.Create(organizationCreateDto.Name);
            if (organizationNameOrError.IsFailure)
                return BadRequest(organizationNameOrError.Error);

            var organization = new Organization(organizationNameOrError.Value);
            organization.SetNote(organizationCreateDto.Notes);

            if (organizationCreateDto.Address != null)
                organization.SetAddress(organizationCreateDto.Address);

            MapOrganizationContact(organizationCreateDto, organization);

            MapOrganizationPhones(organizationCreateDto, organization);

            MapOrganizationEmails(organizationCreateDto, organization);

            // 2. Add domain entity to repository
            await repository.AddOrganizationAsync(organization);

            // 3. Save changes on repository
            if (await repository.SaveChangesAsync())
            {
                // 4. Get ReadDto (with new Id) from database after save)
                OrganizationReadDto result = repository.GetOrganizationAsync(organization.Id).Result;
                // 5. Return to consumer
                return CreatedAtRoute("GetOrganizationAsync",
                new { id = result.Id },
                    result);
            }

            return BadRequest($"Failed to add {organizationCreateDto.Name}.");
        }

        private static void MapOrganizationEmails(OrganizationCreateDto organizationCreateDto, Organization organization)
        {
            if (organizationCreateDto.Emails != null)
            {
                var emails = new List<Email>();
                foreach (var email in organizationCreateDto.Emails)
                    emails.Add(new Email(email.Address, email.IsPrimary));

                organization.SetEmails(emails);
            }
        }

        private static void MapOrganizationPhones(OrganizationCreateDto organizationCreateDto, Organization organization)
        {
            if (organizationCreateDto.Phones != null)
            {
                var phones = new List<Phone>();
                foreach (var phone in organizationCreateDto.Phones)
                    phones.Add(new Phone(phone.Number, phone.PhoneType, phone.IsPrimary));

                organization.SetPhones(phones);
            }
        }

        private static void MapOrganizationContact(OrganizationCreateDto organizationCreateDto, Organization organization)
        {
            if (organizationCreateDto.Contact != null)
            {
                organization.SetContact(new Person(organizationCreateDto.Contact.Name, organizationCreateDto.Contact.Gender));

                if (organizationCreateDto.Contact.Birthday != null)
                    organization.Contact.SetBirthday(organizationCreateDto.Contact.Birthday);

                if (organizationCreateDto.Contact.DriversLicense != null)
                    organization.Contact.SetDriversLicense(organizationCreateDto.Contact.DriversLicense);

                if (organizationCreateDto.Contact.Address != null)
                    organization.Contact.SetAddress(organizationCreateDto.Contact.Address);

                if (organizationCreateDto.Contact.Phones != null)
                {
                    var phones = new List<Phone>();
                    foreach (var phone in organizationCreateDto.Contact.Phones)
                        phones.Add(new Phone(phone.Number, phone.PhoneType, phone.IsPrimary));

                    organization.Contact.SetPhones(phones);
                }

                if (organizationCreateDto.Contact.Emails != null)
                {
                    var emails = new List<Email>();
                    foreach (var email in organizationCreateDto.Contact.Emails)
                        emails.Add(new Email(email.Address, email.IsPrimary));

                    organization.Contact.SetEmails(emails);
                }
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOrganizationAsync(int id)
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
