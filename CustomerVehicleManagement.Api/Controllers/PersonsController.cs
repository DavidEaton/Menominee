using AutoMapper;
using CustomerVehicleManagement.Api.Data.Interfaces;
using CustomerVehicleManagement.Api.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace CustomerVehicleManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly IPersonRepository repository;

        public PersonsController(IPersonRepository repository)
        {
            this.repository = repository ?? 
                throw new ArgumentNullException(nameof(repository));
        }

        // GET: api/persons/list
        [Route("list")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonListDto>>> GetPersonsList()
        {
            try
            {
                var results = await repository.GetPersonsListAsync();
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        // GET: api/persons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonReadDto>>> GetPersons()
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

        // GET: api/persons/1
        [HttpGet("{id:int}")]
        public async Task<ActionResult<PersonReadDto>> GetPerson(int id)
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

        // PUT: api/persons/1
        [HttpPut("{id:int}")]
        public async Task<ActionResult<PersonReadDto>> UpdatePerson(int id, PersonUpdateDto model)
        {
            if (model == null)
                return BadRequest();

            if (id != model.Id)
                return BadRequest();

            try
            {
                var personFromDatabase = await repository.GetPersonAsync(id);
                if (personFromDatabase == null)
                    return NotFound($"Could not find Person in the database to update: {model.Name.FirstMiddleLast}");

                //mapper.Map(model, personFromDatabase);

                personFromDatabase.Name = model.Name.LastFirstMiddle;
                personFromDatabase.Address = model.Address;
                personFromDatabase.Birthday = model.Birthday;
                personFromDatabase.DriversLicense = model.DriversLicense;
                personFromDatabase.Gender = model.Gender;
                personFromDatabase.Phones = model.Phones;

                // Update the objects ObjectState and sych the EF Change Tracker
                //personFromDatabase.UpdateState(TrackingState.Modified);
                //repository.FixState();

                if (await repository.SaveChangesAsync())
                    return Ok(personFromDatabase);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

            return BadRequest($"Failed to update {model.Name.FirstMiddleLast}.");
        }

        // POST: api/persons/
        //[ValidateModelState]
        [HttpPost]
        public async Task<ActionResult<PersonReadDto>> AddPerson(PersonCreateDto model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                repository.AddPerson(model);

                if (await repository.SaveChangesAsync())
                {
                    PersonReadDto personFromDatabase = await repository.GetPersonAsync(model.Id);

                    //var location = linkGenerator.GetPathByAction("Get", "Persons", new { id = person.Id });
                    //string location = $"/api/persons/{model.Id}";
                    //return Created(location, model);

                    return CreatedAtRoute("DefaultApi", new { id = personFromDatabase.Id }, personFromDatabase);
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