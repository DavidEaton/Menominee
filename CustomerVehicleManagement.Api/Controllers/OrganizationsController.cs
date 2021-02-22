using AutoMapper;
using CustomerVehicleManagement.Api.Data.Interfaces;
using CustomerVehicleManagement.Api.Data.Models;
using CustomerVehicleManagement.Domain.Entities;
using Microsoft.AspNetCore.Http;
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

        public OrganizationsController(IOrganizationRepository repository, IMapper mapper)
        {
            this.repository = repository ??
                throw new ArgumentNullException(nameof(repository));
        }

        // api/organizations/list
        [Route("list")]
        [HttpGet]
        [ResponseCache(Duration = MaxCacheAge)]
        public async Task<ActionResult<IEnumerable<OrganizationsListDto>>> GetOrganizationsList()
        {
            var results = await repository.GetOrganizationsListAsync();
            return Ok(results);
        }

        // api/organizations
        [HttpGet]
        public async Task<ActionResult<Organization[]>> GetOrganizations()
        {
            try
            {
                return await repository.GetOrganizationsAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        // api/organizations/1
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Organization>> GetOrganization(int id)
        {
            try
            {
                var result = await repository.GetOrganizationAsync(id);

                if (result == null)
                    return NotFound();

                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        // api/organizations/1
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Organization>> UpdateOrganization(int id, Organization model)
        {
            if (model == null)
                return BadRequest();

            try
            {
                var fetchedOrganization = await repository.GetOrganizationAsync(id);
                if (fetchedOrganization == null)
                    return NotFound($"Could not find Organization in the database to updte: {model.Name}");

                //mapper.Map(model, fetchedOrganization);

                // Update the objects ObjectState and sych the EF Change Tracker
                fetchedOrganization.UpdateTrackingState(TrackingState.Modified);
                repository.FixState();

                if (await repository.SaveChangesAsync())
                    return fetchedOrganization;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

            return BadRequest($"Failed to update {model.Name}.");
        }

        [HttpPost]
        public async Task<ActionResult<Organization>> CreateOrganization(Organization model)
        {
            try
            {
                //var organization = mapper.Map<Organization>(model);
                repository.AddOrganization(model);

                if (await repository.SaveChangesAsync())
                {
                    //var location = linkGenerator.GetPathByAction("Get", "Organizations", new { id = organization.Id });
                    string location = $"/api/organizations/{model.Id}";
                    //return Created(location, mapper.Map<OrganizationModel>(organization));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

            return BadRequest($"Failed to add {model.Name}.");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOrganization(int id)
        {
            try
            {
                var fetchedOrganization = await repository.GetOrganizationAsync(id);
                if (fetchedOrganization == null)
                    return NotFound($"Could not find Organization in the database to delete with Id: {id}.");

                repository.DeleteOrganization(fetchedOrganization);
                if (await repository.SaveChangesAsync())
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

            return BadRequest($"Failed to delete Organization with Id: {id}.");
        }
    }

}
