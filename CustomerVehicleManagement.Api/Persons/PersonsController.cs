using AutoMapper;
using CustomerVehicleManagement.Api.Utilities;
using CustomerVehicleManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using CustomerVehicleManagement.Api.Emails;
using CustomerVehicleManagement.Api.Phones;

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
        //[ResponseCache(Duration = MaxCacheAge)]
        public async Task<ActionResult<IEnumerable<PersonInListDto>>> GetPersonsListAsync()
        {
            var persons = await repository.GetPersonsListAsync();
            return Ok(persons);
        }

        // GET: api/persons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonReadDto>>> GetPersonsAsync()
        {
            var persons = await repository.GetPersonsAsync();

            if (persons == null)
                return NotFound();

            return Ok(persons);
        }

        // GET: api/persons/1
        [HttpGet("{id:int}", Name = "GetPerson")]
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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var notFoundMessage = $"Could not find Person to update";

            /* Update Pattern in Controllers:
                1) Get domain entity from repository
                2) Update domain entity with data in data transfer object (DTO)
                3) Set entity's TrackingState to Modified
                4) FixTrackingState: moves entity state tracking back out of
                the object and into the context to track entity state in this
                disconnected applications. In other words, sych the EF Change
                Tracker with our disconnected entity's TrackingState
                5) Save changes
                6) return NoContent()
            */

            Person personFromRepository = await repository.GetPersonEntityAsync(id);
            if (personFromRepository == null)
                return NotFound(notFoundMessage);

            DtoHelpers.ConvertPersonUpdateDtoToDomainModel(personUpdateDto, personFromRepository, mapper);
            //personFromRepository.SetTrackingState(TrackingState.Modified);
            //repository.FixTrackingState();
            repository.UpdatePersonAsync(personUpdateDto);

            /* Returning the updated resource is acceptible like:
                 return Ok(personFromRepository);
               even preferred over returning NoContent if updated resource
               contains properties that are mutated by the data store
               (which they are not in this case).

               Instead, our app will:
                 return NoContent();
               ... and let the caller decide whether to get the updated resource,
               which is also acceptible. The HTTP specification (RFC 2616) has a
               number of recommendations that are applicable:
            HTTP status code 200 OK for a successful PUT of an update to an existing resource. No response body needed.
            HTTP status code 201 Created for a successful PUT of a new resource
            HTTP status code 409 Conflict for a PUT that is unsuccessful due to a 3rd-party modification
            HTTP status code 400 Bad Request for an unsuccessful PUT
            */

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

            await repository.AddPersonAsync(personCreateDto);

            if (await repository.SaveChangesAsync())
            {
                var personReadDto = new PersonReadDto
                {
                    Id = personCreateDto.Id,
                    Name = personCreateDto.Name.LastFirstMiddle,
                    Gender = personCreateDto.Gender,
                    Birthday = personCreateDto.Birthday,
                    DriversLicense = personCreateDto.DriversLicense,
                    Address = new Address(personCreateDto.Address.AddressLine,
                                          personCreateDto.Address.City,
                                          personCreateDto.Address.State,
                                          personCreateDto.Address.PostalCode),
                    Phones = mapper.Map<IList<PhoneReadDto>>(personCreateDto.Phones),
                    Emails = mapper.Map<IList<EmailReadDto>>(personCreateDto.Emails)
                };

                return CreatedAtRoute("GetPerson",
                    new { id = personReadDto.Id },
                    personReadDto);
            }

            return BadRequest($"Failed to add {personCreateDto.Name.FirstMiddleLast}.");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePersonAsync(int id)
        {   /* Delete Pattern in Controllers:
                1) Get domain entity from repository
                2) Call repository.DeletePerson(), which removes person from context
                3) Save changes
                4) return Ok()
            */
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