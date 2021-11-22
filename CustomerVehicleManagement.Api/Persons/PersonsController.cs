using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace CustomerVehicleManagement.Api.Persons
{
    public class PersonsController : ApplicationController
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
        public async Task<ActionResult<IReadOnlyList<PersonToReadInList>>> GetPersonsListAsync()
        {
            var persons = await repository.GetPersonsListAsync();

            if (persons == null)
                return NotFound();

            return Ok(persons);
        }

        // GET: api/persons
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<PersonToRead>>> GetPersonsAsync()
        {
            var persons = await repository.GetPersonsAsync();

            if (persons == null)
                return NotFound();

            return Ok(persons);
        }

        // GET: api/persons/1
        [HttpGet("{id:long}", Name = "GetPersonAsync")]
        public async Task<ActionResult<PersonToRead>> GetPersonAsync(long id)
        {
            var person = await repository.GetPersonAsync(id);

            if (person == null)
                return NotFound();

            return Ok(person);
        }

        // PUT: api/persons/1
        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdatePersonAsync(long id, PersonToEdit personUpdateDto)
        {
            var notFoundMessage = $"Could not find Person to update";

            Person personFromRepository = await repository.GetPersonEntityAsync(id);
            if (personFromRepository == null)
                return NotFound(notFoundMessage);

            List<Phone> phones = new();
            List<Email> emails = new();

            personFromRepository.SetName(PersonName.Create(
                                            personUpdateDto.Name.LastName,
                                            personUpdateDto.Name.FirstName,
                                            personUpdateDto.Name.MiddleName).Value);

            personFromRepository.SetGender(personUpdateDto.Gender);

            if (personUpdateDto?.Address != null)
                personFromRepository.SetAddress(Address.Create(personUpdateDto.Address.AddressLine,
                                                               personUpdateDto.Address.City,
                                                               personUpdateDto.Address.State,
                                                               personUpdateDto.Address.PostalCode).Value);



            personFromRepository.SetBirthday(personUpdateDto.Birthday);


            if (personUpdateDto?.Phones.Count > 0)
                foreach (var phone in personFromRepository.Phones)
                {
                    phones.Add(Phone.Create(phone.Number, phone.PhoneType, phone.IsPrimary).Value);
                    personFromRepository.SetPhones(phones);
                }

            if (personUpdateDto?.Emails.Count > 0)
                foreach (var email in personFromRepository.Emails)
                {
                    emails.Add(Email.Create(email.Address, email.IsPrimary).Value);
                    personFromRepository.SetEmails(emails);
                }

            if (personUpdateDto?.DriversLicense != null)
            {
                personFromRepository.SetDriversLicense(DriversLicense.Create(personUpdateDto.DriversLicense.Number,
                    personUpdateDto.DriversLicense.State,
                    DateTimeRange.Create(
                    personUpdateDto.DriversLicense.Issued,
                    personUpdateDto.DriversLicense.Expiry).Value).Value);
            }

            personFromRepository.SetTrackingState(TrackingState.Modified);
            repository.FixTrackingState();

            repository.UpdatePersonAsync(personFromRepository);

            if (await repository.SaveChangesAsync())
                return NoContent();

            return BadRequest($"Failed to update {personUpdateDto.Name.LastFirstMiddle}.");
        }

        // POST: api/persons/
        [HttpPost]
        public async Task<ActionResult<PersonToRead>> CreatePersonAsync(PersonToAdd personCreateDto)
        {
            Address address = null;
            List<Phone> phones = new();
            List<Email> emails = new();
            DriversLicense driversLicense = null;

            var personName = PersonName.Create(personCreateDto.Name.LastName, personCreateDto.Name.FirstName, personCreateDto.Name.MiddleName).Value;

            if (personCreateDto?.Address != null)
                address = Address.Create(personCreateDto.Address.AddressLine, personCreateDto.Address.City, personCreateDto.Address.State, personCreateDto.Address.PostalCode).Value;

            if (personCreateDto?.Phones.Count > 0)
                foreach (var phone in personCreateDto.Phones)
                    phones.Add(Phone.Create(phone.Number, phone.PhoneType, phone.IsPrimary).Value);

            if (personCreateDto?.Emails.Count > 0)
                foreach (var email in personCreateDto.Emails)
                    emails.Add(Email.Create(email.Address, email.IsPrimary).Value);

            if (personCreateDto?.DriversLicense != null)
            {
                DateTimeRange dateTimeRange = DateTimeRange.Create(
                    personCreateDto.DriversLicense.Issued,
                    personCreateDto.DriversLicense.Expiry).Value;

                driversLicense = DriversLicense.Create(personCreateDto.DriversLicense.Number,
                    personCreateDto.DriversLicense.State,
                    dateTimeRange).Value;
            }

            Person person = new(personName, personCreateDto.Gender, address, emails, phones, personCreateDto.Birthday, driversLicense);

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