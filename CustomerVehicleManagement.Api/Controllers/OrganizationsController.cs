using AutoMapper;
using CustomerVehicleManagement.Api.Data.Interfaces;
using CustomerVehicleManagement.Api.Data.Dtos;
using CustomerVehicleManagement.Api.Utilities;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Controllers
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

            DtoHelpers.ConvertOrganizationUpdateDtoToDomainModel(organizationUpdateDto, organizationFromRepository);
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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            repository.Create(organizationCreateDto);

            if (await repository.SaveChangesAsync())
            {
                var organizationReadDto = new OrganizationReadDto
                {
                    Id = organizationCreateDto.Id,
                    Name = organizationCreateDto.Name,
                    Contact = new PersonReadDto
                    {
                        Id = organizationCreateDto.Contact.Id,
                        Name = organizationCreateDto.Contact.Name.LastFirstMiddleInitial,
                        Phones = mapper.Map<IList<PhoneReadDto>>(organizationCreateDto.Phones)
                    },
                    AddressLine = organizationCreateDto.Address.AddressLine,
                    City = organizationCreateDto.Address.City,
                    State = organizationCreateDto.Address.State,
                    PostalCode = organizationCreateDto.Address.PostalCode
                };

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

            repository.Delete(organizationFromRepository);

            if (await repository.SaveChangesAsync())
                return NoContent();

            return BadRequest($"Failed to delete Organization with Id: {id}.");
        }
    }
}
