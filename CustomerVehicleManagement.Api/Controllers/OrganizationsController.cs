using AutoMapper;
using CustomerVehicleManagement.Api.Data.Interfaces;
using CustomerVehicleManagement.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Enums;
using System;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationsController : ControllerBase
    {
        private readonly IOrganizationRepository data;
        private readonly IMapper mapper;

        public OrganizationsController(IOrganizationRepository data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }

        // GET: api/Organizations
        [HttpGet]
        public async Task<ActionResult<Organization[]>> GetOrganizations()
        {
            try
            {
                return await data.GetOrganizationsAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        // GET: api/Organization/1
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Organization>> GetOrganization(int id)
        {
            try
            {
                var result = await data.GetOrganizationAsync(id);

                if (result == null)
                    return NotFound();

                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        // PUT: api/Organization/1
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Organization>> UpdateOrganization(int id, Organization model)
        {
            if (model == null)
                return BadRequest();

            try
            {
                var fetchedOrganization = await data.GetOrganizationAsync(id);
                if (fetchedOrganization == null)
                    return NotFound($"Could not find Organization in the database to updte: {model.Name}");

                //mapper.Map(model, fetchedOrganization);

                // Update the objects ObjectState and sych the EF Change Tracker
                fetchedOrganization.UpdateTrackingState(TrackingState.Modified);
                data.FixState();

                if (await data.SaveChangesAsync())
                    return fetchedOrganization;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

            return BadRequest($"Failed to update {model.Name}.");
        }

        // POST: api/Organization/
        [HttpPost]
        public async Task<ActionResult<Organization>> CreateOrganization(Organization model)
        {
            try
            {
                //var organization = mapper.Map<Organization>(model);
                data.AddOrganization(model);

                if (await data.SaveChangesAsync())
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
                var fetchedOrganization = await data.GetOrganizationAsync(id);
                if (fetchedOrganization == null)
                    return NotFound($"Could not find Organization in the database to delete with Id: {id}.");

                data.DeleteOrganization(fetchedOrganization);
                if (await data.SaveChangesAsync())
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
