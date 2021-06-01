using AutoMapper;
using CustomerVehicleManagement.Api.Emails;
using CustomerVehicleManagement.Api.Persons;
using CustomerVehicleManagement.Api.Phones;
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
        [HttpGet("{id:int}")]
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
            // Pattern:
            // Map dto to domain entity
            // Add domain entity to repository
            // Save changes on repository
            // Return saved domain entity with new Id from database
            // Map saved domain entity to read dto
            // Return to consumer

            // Map dto to domain entity
            var organizationNameOrError = OrganizationName.Create(organizationCreateDto.Name);
            if (organizationNameOrError.IsFailure)
                return BadRequest($"Failed to add {organizationCreateDto.Name}.");

            var organization = new Organization(organizationNameOrError.Value);
            if (organization.Address != null)
                organization.SetAddress(organization.Address);

            if (organization.Contact != null)
                organization.SetContact(new Person(organization.Contact.Name, organization.Contact.Gender));

            if (organization.Phones != null)
                organization.SetPhones(organization.Phones);

            if (organization.Emails != null)
                organization.SetEmails(organization.Emails);

            // Add domain entity to repository
            await repository.AddOrganizationAsync(organization);

            // Save changes on repository
            // Return saved domain entity with new Id from database
            if (await repository.SaveChangesAsync())
            {
                // Map saved domain entity to read dto
                var organizationReadDto = new OrganizationReadDto
                {
                    Id = organization.Id,
                    Name = organization.Name.Value,
                    //Contact = new PersonReadDto
                    //{
                    //    Id = organizationCreateDto.Contact.Id,
                    //    Name = organizationCreateDto.Contact.Name.LastFirstMiddleInitial,
                    //    Phones = mapper.Map<IReadOnlyList<PhoneReadDto>>(organizationCreateDto.Contact.Phones),
                    //    Emails = mapper.Map<IReadOnlyList<EmailReadDto>>(organizationCreateDto.Contact.Emails)
                    //},
                    Address = organization.Address,
                    Phones = mapper.Map<IList<PhoneReadDto>>(organization.Phones),
                    Emails = mapper.Map<IList<EmailReadDto>>(organization.Emails)
                };

                // Return to consumer
                return CreatedAtRoute("GetOrganization",
                new { id = organizationReadDto.Id },
                    organizationReadDto);
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
