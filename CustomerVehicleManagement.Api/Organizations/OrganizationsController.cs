using AutoMapper;
using CustomerVehicleManagement.Api.Utilities;
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

            var organizationFromRepository = await repository.GetOrganizationEntityAsync(id);
            if (organizationFromRepository == null)
                return NotFound(notFoundMessage);

            DtoHelpers.ConvertOrganizationUpdateDtoToDomainModel(organizationUpdateDto, organizationFromRepository, mapper);
            organizationFromRepository.SetTrackingState(TrackingState.Modified);
            repository.FixTrackingState();
            repository.UpdateOrganizationAsync(organizationFromRepository);

            if (await repository.SaveChangesAsync())
                return NoContent();

            return BadRequest($"Failed to update {organizationUpdateDto.Name}.");
        }

        [HttpPost]
        public async Task<ActionResult<OrganizationReadDto>> CreateOrganizationAsync(OrganizationCreateDto organizationCreateDto)
        {
            /* Controller Pattern:
               1. Map dto to domain entity
               2. Add domain entity to repository
               3. Save changes on repository
               4. Fetch PersonReadDto with new Id from database (from saved domain entity: person.Id)
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

            //if (organizationCreateDto.Contact != null)
            //    organization.SetContact(new Person(organizationCreateDto.Contact.Name, organizationCreateDto.Contact.Gender));

            if (organizationCreateDto.Phones != null)
            {
                var phones = new List<Phone>();
                foreach (var phone in organizationCreateDto.Phones)
                    phones.Add(new Phone(phone.Number, phone.PhoneType, phone.IsPrimary));

                organization.SetPhones(phones);
            }

            //if (organizationCreateDto.Emails != null)
            //    organization.SetEmails(organization.Emails);


            // 2. Add domain entity to repository
            await repository.AddOrganizationAsync(organization);


            // 3. Save changes on repository
            // 4. Fetch saved (with new Id) from database
            if (await repository.SaveChangesAsync())
            {
                OrganizationReadDto result = repository.GetOrganizationAsync(organization.Id).Result;
                // 5. Return to consumer
                return CreatedAtRoute("GetOrganizationAsync",
                new { id = result.Id },
                    result);
            }

            return BadRequest($"Failed to add {organizationCreateDto.Name}.");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOrganizationAsync(int id)
        {
            var organizationFromRepository = await repository.GetOrganizationEntityAsync(id);

            if (organizationFromRepository == null)
                return NotFound($"Could not find Organization in the database to delete with Id: {id}.");

            repository.DeleteOrganization(organizationFromRepository);

            if (await repository.SaveChangesAsync())
                return NoContent();

            return BadRequest($"Failed to delete Organization with Id: {id}.");
        }
    }
}
