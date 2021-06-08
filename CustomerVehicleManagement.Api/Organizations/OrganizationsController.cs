using AutoMapper;
using CustomerVehicleManagement.Domain.Entities;
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
        private const int MaxCacheAge = 300;
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
        [ResponseCache(Duration = MaxCacheAge)]
        public async Task<ActionResult<IEnumerable<OrganizationsInListDto>>> GetOrganizationsListAsync()
        {
            var results = await repository.GetOrganizationsListAsync();
            return Ok(results);
        }

        // api/organizations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrganizationReadDto>>> GetOrganizationsAsync()
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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var notFoundMessage = $"Could not find Organization to update: {organizationUpdateDto.Name}";

            if (!await repository.OrganizationExistsAsync(id))
                return NotFound(notFoundMessage);

            var organization = repository.GetOrganizationEntityAsync(id).Result;

            var organizationNameOrError = OrganizationName.Create(organizationUpdateDto.Name);

            if (organizationNameOrError.IsSuccess)
                organization.SetName(organizationNameOrError.Value);

            organization.SetAddress(organizationUpdateDto.Address);
            organization.SetNotes(organizationUpdateDto.Notes);

            SetCollections(organizationUpdateDto, organization);

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

            organization.SetTrackingState(TrackingState.Modified);
            repository.FixTrackingState();

            repository.UpdateOrganizationAsync(organization);

            if (await repository.SaveChangesAsync())
                return NoContent();

            return BadRequest($"Failed to update {organizationUpdateDto.Name}.");
        }

        private static void SetCollections(OrganizationUpdateDto organizationUpdateDto, Organization organization)
        {
            if (organizationUpdateDto.Phones != null)
            {
                organization.Phones.Clear();
                foreach (var phone in organizationUpdateDto.Phones)
                    organization.AddPhone(new Phone(phone.Number, phone.PhoneType, phone.IsPrimary));
            }

            if (organizationUpdateDto.Emails != null)
            {
                organization.Emails.Clear();
                foreach (var email in organizationUpdateDto.Emails)
                    organization.AddEmail(new Email(email.Address, email.IsPrimary));
            }
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

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // 1. Map dto to domain entity
            var organizationNameOrError = OrganizationName.Create(organizationCreateDto.Name);
            if (organizationNameOrError.IsFailure)
                return BadRequest(organizationNameOrError.Error);

            var organization = new Organization(organizationNameOrError.Value);
            organization.SetNotes(organizationCreateDto.Notes);

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
