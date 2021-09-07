using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using Microsoft.AspNetCore.Mvc;
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

            PersonDtoHelper.PersonUpdateDtoToEntity(personUpdateDto, personFromRepository);

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

            Person person = CreateNewPerson(personCreateDto);

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

        private static Person CreateNewPerson(PersonCreateDto personCreateDto)
        {
            var person = new Person(personCreateDto.Name, personCreateDto.Gender);

            person.SetAddress(personCreateDto?.Address);
            person.SetBirthday(personCreateDto?.Birthday);

            if (personCreateDto.DriversLicense != null)
            {
                var dateTimeRange = new DateTimeRange(personCreateDto.DriversLicense.Issued,
                                                      personCreateDto.DriversLicense.Expiry);

                var driversLicense = new DriversLicense(personCreateDto.DriversLicense.Number,
                                                        personCreateDto.DriversLicense.State,
                                                        dateTimeRange);

                person.SetDriversLicense(driversLicense);
            }

            if (personCreateDto?.Phones?.Count > 0)
                foreach (var phone in personCreateDto.Phones)
                    person.AddPhone(new Phone(phone.Number, phone.PhoneType, phone.IsPrimary));

            if (personCreateDto?.Emails?.Count > 0)
                foreach (var email in personCreateDto.Emails)
                    person.AddEmail(new Domain.Entities.Email(email.Address, email.IsPrimary));

            return person;
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
    }
}