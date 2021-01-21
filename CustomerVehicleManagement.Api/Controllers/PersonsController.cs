using AutoMapper;
using CustomerVehicleManagement.Api.Data.Interfaces;
using CustomerVehicleManagement.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Enums;
using System;
using System.Threading.Tasks;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace CustomerVehicleManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly IPersonRepository repository;
        private readonly IMapper mapper;

        public PersonsController(IPersonRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        // GET: api/Persons
        [HttpGet]
        public async Task<ActionResult<Person[]>> GetPersons()
        {
            try
            {
                var results = await repository.GetPersonsAsync();
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        // GET: api/Person/1
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Person>> GetPerson(int id)
        {
            try
            {
                var result = await repository.GetPersonAsync(id);

                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        // PUT: api/Person/1
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Person>> UpdatePerson(int id, Person model)
        {
            if (model == null)
                return BadRequest();

            try
            {
                var fetchedPerson = await repository.GetPersonAsync(id);
                if (fetchedPerson == null)
                    return NotFound($"Could not find Person in the database to update: {model.Name.FirstMiddleLast}");

                //mapper.Map(model, fetchedPerson);

                // Update the objects ObjectState and sych the EF Change Tracker
                fetchedPerson.UpdateState(TrackingState.Modified);
                repository.FixState();

                if (await repository.SaveChangesAsync())
                    return Ok(fetchedPerson);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

            return BadRequest($"Failed to update {model.Name.FirstMiddleLast}.");
        }

        // POST: api/Person/
        [HttpPost]
        public async Task<ActionResult<Person>> AddPerson(Person model)
        {
            try
            {
                repository.AddPerson(model);

                if (await repository.SaveChangesAsync())
                {
                    //var location = linkGenerator.GetPathByAction("Get", "Persons", new { id = person.Id });
                    string location = $"/api/persons/{model.Id}";
                    return Created(location, model);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

            return BadRequest($"Failed to add {model.Name.FirstMiddleLast}.");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            try
            {
                var fetchedPerson = await repository.GetPersonAsync(id);
                if (fetchedPerson == null)
                    return NotFound($"Could not find Person in the database to delete with Id: {id}.");

                repository.DeletePerson(fetchedPerson);
                if (await repository.SaveChangesAsync())
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

            return BadRequest($"Failed to delete Person with Id: {id}.");
        }
    }

}