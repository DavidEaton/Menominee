using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared;
using CustomerVehicleManagement.Shared.Models.Persons;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Persons
{
    [Authorize(Policies.PaidUser)]
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
        public async Task<IActionResult> UpdatePersonAsync(long id, PersonToWrite personToUpdate)
        {
            var notFoundMessage = $"Could not find Person to update: {personToUpdate.Name.FirstMiddleLast}";

            if (!await repository.PersonExistsAsync(id))
                return NotFound(notFoundMessage);

            var personFromRepository = await repository.GetPersonEntityAsync(id);

            if (personFromRepository is null)
                return NotFound(notFoundMessage);

            var personNameOrError = PersonName.Create(personToUpdate.Name.LastName,
                                                      personToUpdate.Name.FirstName,
                                                      personToUpdate.Name.MiddleName);

            if (personNameOrError.IsSuccess)
                personFromRepository.SetName(personNameOrError.Value);

            if (personToUpdate?.Address is not null)
                personFromRepository.SetAddress(Address.Create(personToUpdate.Address.AddressLine,
                                                               personToUpdate.Address.City,
                                                               personToUpdate.Address.State,
                                                               personToUpdate.Address.PostalCode).Value);

            if (personToUpdate?.Address is null)
                personFromRepository.SetAddress(null);

            personFromRepository.SetGender(personToUpdate.Gender);
            personFromRepository.SetBirthday(personToUpdate.Birthday);

            List<Phone> phones = new();

            foreach (var phone in personToUpdate?.Phones)
                phones.Add(Phone.Create(phone.Number, phone.PhoneType, phone.IsPrimary).Value);

            if (personToUpdate?.Phones is null || personToUpdate?.Phones.Count == 0)
                phones = null;

            personFromRepository.SetPhones(phones);


            List<Email> emails = new();

            if (personToUpdate?.Emails.Count > 0)
                foreach (var email in personToUpdate.Emails)
                {
                    emails.Add(Email.Create(email.Address, email.IsPrimary).Value);
                    personFromRepository.SetEmails(emails);
                }

            if (personToUpdate?.DriversLicense is not null)
            {
                personFromRepository.SetDriversLicense(DriversLicense.Create(personToUpdate.DriversLicense.Number,
                    personToUpdate.DriversLicense.State,
                    DateTimeRange.Create(
                    personToUpdate.DriversLicense.Issued,
                    personToUpdate.DriversLicense.Expiry).Value).Value);
            }

            personFromRepository.SetTrackingState(TrackingState.Modified);
            repository.FixTrackingState();

            repository.UpdatePersonAsync(personFromRepository);

            await repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> AddPersonAsync(PersonToWrite personToAdd)
        {
            Person person = PersonHelper.CreateEntityFromWriteDto(personToAdd);

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