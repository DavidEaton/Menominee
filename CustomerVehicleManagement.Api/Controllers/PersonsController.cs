using AutoMapper;
using CustomerVehicleManagement.Api.Data.Interfaces;
using CustomerVehicleManagement.Api.Data.Models;
using CustomerVehicleManagement.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace CustomerVehicleManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private const int MaxCacheAge = 300;
        private readonly IPersonRepository repository;
        private readonly IMapper mapper;

        public PersonsController(IPersonRepository repository, IMapper mapper)
        {
            this.repository = repository ??
                throw new ArgumentNullException(nameof(repository));

            this.mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        // GET: api/persons/list
        [Route("list")]
        [HttpGet]
        [ResponseCache(Duration = MaxCacheAge)]
        public async Task<ActionResult<IEnumerable<PersonListDto>>> GetPersonsList()
        {
            var persons = await repository.GetPersonsListAsync();
            return Ok(persons);
        }

        // GET: api/persons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonReadDto>>> GetPersons()
        {
            var persons = await repository.GetPersonsAsync();
            return Ok(persons);
        }

        // GET: api/persons/1
        [HttpGet("{id:int}", Name = "GetPerson")]
        public async Task<ActionResult<PersonReadDto>> GetPerson(int id)
        {
            var person = await repository.GetPersonAsync(id);

            if (person == null)
                return NotFound();

            return Ok(person);
        }

        // PUT: api/persons/1
        [HttpPut("{id:int}")]
        public async Task<ActionResult<PersonReadDto>> UpdatePerson(int id, PersonUpdateDto personUpdateDto)
        {
            if (!await repository.PersonExistsAsync(id))
                return NotFound();

            var personFromRepository = await repository.GetPersonAsync(id);
            if (personFromRepository == null)
                return NotFound($"Could not find Person to update: {personUpdateDto.Name.FirstMiddleLast}");

            // map the PersonUpdateDto back to the domain entity
            MapUpdateDtoToDomainModel(personUpdateDto, personFromRepository);

            // Update the objects ObjectState and sych the EF Change Tracker
            personFromRepository.UpdateTrackingState(TrackingState.Modified);
            repository.FixState();
            repository.UpdatePersonAsync(personFromRepository);

            if (await repository.SaveChangesAsync())
                return Ok(personFromRepository);

            /* Returning the updated resource is acceptible (as above),
               even preferred over returning NoContent if updated resource
               contains properties that are mutated by the data store.

               Our app will not:
            return NoContent();
               ... but rather return the updated resource.
            */

            return BadRequest($"Failed to update {personUpdateDto.Name.FirstMiddleLast}.");
        }

        private static void MapUpdateDtoToDomainModel(PersonUpdateDto personUpdateDto, Person personFromRepository)
        {
            personFromRepository.SetName(personUpdateDto.Name);
            personFromRepository.SetGender(personUpdateDto.Gender);
            personFromRepository.SetAddress(personUpdateDto.Address);
            personFromRepository.SetBirthday(personUpdateDto.Birthday);
            personFromRepository.SetDriversLicense(personUpdateDto.DriversLicense);
            personFromRepository.SetPhones(personUpdateDto.Phones);
        }

        // POST: api/persons/
        //[ValidateModelState]
        [HttpPost]
        public async Task<ActionResult<PersonReadDto>> CreatePerson(PersonCreateDto personCreateDto)
        {
            PersonReadDto personToReturn = await repository.SaveChangesAsync(personCreateDto);

            if (personToReturn != null)
            {
                return CreatedAtRoute("GetPerson",
                    new { id = personToReturn.Id },
                    personToReturn);

            }

            return BadRequest($"Failed to add {personCreateDto.Name.FirstMiddleLast}.");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            var personFromRepository = await repository.GetPersonAsync(id);

            if (personFromRepository == null)
                return NotFound($"Could not find Person in the database to delete with Id: {id}.");

            repository.DeletePerson(personFromRepository);

            if (await repository.SaveChangesAsync())
            {
                return Ok();
            }
            return BadRequest($"Failed to delete Person with Id: {id}.");
        }

        [HttpGet("total")]
        public async Task<IActionResult> GetPersonsTotalAsync()
        {
            var results = await repository.GetPersonsAsync();
            return Ok(new PersonsTotalOutputModel { PersonsTotal = results.Count() });
        }
    }
}