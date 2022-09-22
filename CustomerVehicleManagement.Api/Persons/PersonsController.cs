using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models.Persons;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Persons
{
    public class PersonsController : ApplicationController
    {
        private readonly IPersonRepository repository;
        private readonly string BasePath = "/api/persons/";

        public PersonsController(IPersonRepository repository)
        {
            this.repository = repository ??
                throw new ArgumentNullException(nameof(repository));
        }

        [Route("list")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<PersonToReadInList>>> GetPersonsListAsync()
        {
            var persons = await repository.GetPersonsListAsync();

            if (persons == null)
                return NotFound();

            return Ok(persons);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<PersonToRead>>> GetPersonsAsync()
        {
            var persons = await repository.GetPersonsAsync();

            if (persons == null)
                return NotFound();

            return Ok(persons);
        }

        [HttpGet("{id:long}", Name = "GetPersonAsync")]
        public async Task<ActionResult<PersonToRead>> GetPersonAsync(long id)
        {
            var person = await repository.GetPersonAsync(id);

            if (person == null)
                return NotFound();

            return Ok(person);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdatePersonAsync(long id, PersonToWrite person)
        {
            var notFoundMessage = $"Could not find {person.Name.FirstName} {person.Name.LastName} to update";

            if (!await repository.PersonExistsAsync(id))
                return NotFound(notFoundMessage);

            var personFromRepository = await repository.GetPersonEntityAsync(id);

            if (personFromRepository is null)
                return NotFound(notFoundMessage);

            var personNameOrError = PersonName.Create(
                person.Name.LastName,
                person.Name.FirstName,
                person.Name.MiddleName);

            if (personNameOrError.IsSuccess)
                personFromRepository.SetName(personNameOrError.Value);

            if (person?.Address is not null)
                personFromRepository.SetAddress(Address.Create(
                    person.Address.AddressLine,
                    person.Address.City,
                    person.Address.State,
                    person.Address.PostalCode).Value);

            if (person?.Address is null)
                personFromRepository.SetAddress(null);

            personFromRepository.SetGender(person.Gender);
            personFromRepository.SetBirthday(person.Birthday);

            List<Phone> phones = new();

            foreach (var phone in person?.Phones)
                phones.Add(Phone.Create(phone.Number, phone.PhoneType, phone.IsPrimary).Value);

            List<Email> emails = new();

            if (person?.Emails.Count > 0)
                foreach (var email in person.Emails)
                    emails.Add(Email.Create(email.Address, email.IsPrimary).Value);

            personFromRepository.SetEmails(emails);

            if (person?.DriversLicense is not null)
            {
                personFromRepository.SetDriversLicense(DriversLicense.Create(person.DriversLicense.Number,
                    person.DriversLicense.State,
                    DateTimeRange.Create(
                    person.DriversLicense.Issued,
                    person.DriversLicense.Expiry).Value).Value);
            }

            await repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> AddPersonAsync(PersonToWrite personToAdd)
        {
            Person person = PersonHelper.ConvertWriteDtoToEntity(personToAdd);

            await repository.AddPersonAsync(person);
            await repository.SaveChangesAsync();

            return Created(new Uri($"{BasePath}/{person.Id}",
                               UriKind.Relative),
                               new { id = person.Id });
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeletePersonAsync(long id)
        {
            var personFromRepository = await repository.GetPersonEntityAsync(id);

            if (personFromRepository == null)
                return NotFound($"Could not find Person in the database to delete with Id: {id}.");

            repository.DeletePerson(personFromRepository);
            await repository.SaveChangesAsync();

            return NoContent();
        }
    }
}