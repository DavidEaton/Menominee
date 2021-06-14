using AutoMapper;
using CustomerVehicleManagement.Api.Utilities;
using CustomerVehicleManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace CustomerVehicleManagement.Api.Persons
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
        public async Task<ActionResult<IReadOnlyList<PersonInListDto>>> GetPersonsListAsync()
        {
            var persons = await repository.GetPersonsListAsync();

            if (persons == null)
                return NotFound();

            return Ok(persons);
        }

        // GET: api/persons
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<PersonReadDto>>> GetPersonsAsync()
        {
            var persons = await repository.GetPersonsAsync();

            if (persons == null)
                return NotFound();

            return Ok(persons);
        }

        // GET: api/persons/1
        [HttpGet("{id:int}", Name = "GetPersonAsync")]
        public async Task<ActionResult<PersonReadDto>> GetPersonAsync(int id)
        {
            var person = await repository.GetPersonAsync(id);

            if (person == null)
                return NotFound();

            return Ok(person);
        }

        // PUT: api/persons/1
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdatePersonAsync(int id, PersonUpdateDto personUpdateDto)
        {
            var notFoundMessage = $"Could not find Person to update";

            Person personFromRepository = await repository.GetPersonEntityAsync(id);
             if (personFromRepository == null)
                return NotFound(notFoundMessage);

            DtoHelpers.PersonUpdateDtoToPerson(personUpdateDto, personFromRepository);

            repository.UpdatePersonAsync(personUpdateDto);

            if (await repository.SaveChangesAsync())
                return NoContent();

            return BadRequest($"Failed to update {personUpdateDto.Name.FirstMiddleLast}.");
        }

        // POST: api/persons/
        [HttpPost]
        public async Task<ActionResult<PersonReadDto>> CreatePersonAsync(PersonCreateDto personCreateDto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var person = mapper.Map<Person>(personCreateDto);

            await repository.AddPersonAsync(person);

            if (await repository.SaveChangesAsync())
            {
                var personFromRepository = repository.GetPersonAsync(person.Id).Result;

                return CreatedAtRoute("GetPersonAsync",
                    new { id = person.Id },
                    personFromRepository);
            }

            return BadRequest($"Failed to add {personCreateDto.Name.FirstMiddleLast}.");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePersonAsync(int id)
        {
            var personFromRepository = await repository.GetPersonEntityAsync(id);

            if (personFromRepository == null)
                return NotFound($"Could not find Person in the database to delete with Id: {id}.");

            repository.DeletePerson(personFromRepository);

            if (await repository.SaveChangesAsync())
                return NoContent();

            return BadRequest($"Failed to delete Person with Id: {id}.");
        }

        [HttpGet("total")]
        public async Task<IActionResult> GetPersonsTotalAsync()
        {
            var results = await repository.GetPersonsTotalAsync();
            return Ok(results);
        }
    }
}