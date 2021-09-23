using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Enums;
using SharedKernel.ValueObjects;
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
        private readonly IPersonRepository repository;

        public PersonsController(IPersonRepository repository)
        {
            this.repository = repository ??
                throw new ArgumentNullException(nameof(repository));
        }

        // GET: api/persons/list
        [Route("list")]
        [HttpGet]
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
        [HttpGet("{id:long}", Name = "GetPersonAsync")]
        public async Task<ActionResult<PersonReadDto>> GetPersonAsync(long id)
        {
            var person = await repository.GetPersonAsync(id);

            if (person == null)
                return NotFound();

            return Ok(person);
        }

        // PUT: api/persons/1
        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdatePersonAsync(long id, PersonUpdateDto personUpdateDto)
        {
            var notFoundMessage = $"Could not find Person to update";

            Person personFromRepository = await repository.GetPersonEntityAsync(id);
            if (personFromRepository == null)
                return NotFound(notFoundMessage);

            personFromRepository.SetName(PersonName.Create(
                                            personUpdateDto.Name.LastName,
                                            personUpdateDto.Name.FirstName,
                                            personUpdateDto.Name.MiddleName).Value);

            personFromRepository.SetGender(personUpdateDto.Gender);
            personFromRepository.SetAddress(personUpdateDto.Address);
            personFromRepository.SetBirthday(personUpdateDto.Birthday);
            personFromRepository.SetDriversLicense(DriversLicenseUpdateDto.ConvertToEntity(personUpdateDto.DriversLicense));
            personFromRepository.SetEmails(EmailUpdateDto.ConvertToEntities(personUpdateDto.Emails));
            personFromRepository.SetPhones(PhoneUpdateDto.ConvertToEntities(personUpdateDto.Phones));

            personFromRepository.SetTrackingState(TrackingState.Modified);
            repository.FixTrackingState();

            repository.UpdatePersonAsync(personFromRepository);

            if (await repository.SaveChangesAsync())
                return NoContent();

            return BadRequest($"Failed to update {personUpdateDto.Name.LastFirstMiddle}.");
        }

        // POST: api/persons/
        [HttpPost]
        public async Task<ActionResult<PersonReadDto>> CreatePersonAsync(PersonAddDto personCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Person person = PersonAddDto.ConvertToEntity(personCreateDto);

            await repository.AddPersonAsync(person);

            if (await repository.SaveChangesAsync())
            {
                var personFromRepository = repository.GetPersonAsync(person.Id).Result;

                return CreatedAtRoute("GetPersonAsync",
                    new { id = person.Id },
                    personFromRepository);
            }

            return BadRequest($"Failed to add {personCreateDto.Name}.");
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeletePersonAsync(long id)
        {
            var personFromRepository = await repository.GetPersonEntityAsync(id);

            if (personFromRepository == null)
                return NotFound($"Could not find Person in the database to delete with Id: {id}.");

            repository.DeletePerson(personFromRepository);

            if (await repository.SaveChangesAsync())
                return NoContent();

            return BadRequest($"Failed to delete Person with Id: {id}.");
        }
    }
}