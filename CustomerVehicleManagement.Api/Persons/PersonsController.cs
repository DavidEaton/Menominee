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
        public async Task<IActionResult> UpdatePersonAsync(long id, PersonToWrite personToUpdate)
        {
            var notFoundMessage = $"Could not find Person to update";

            Person personFromRepository = await repository.GetPersonEntityAsync(id);
            if (personFromRepository == null)
                return NotFound(notFoundMessage);

            List<Phone> phones = new();
            List<Email> emails = new();

            personFromRepository.SetName(PersonName.Create(
                                            personToUpdate.Name.LastName,
                                            personToUpdate.Name.FirstName,
                                            personToUpdate.Name.MiddleName).Value);

            personFromRepository.SetGender(personToUpdate.Gender);

            if (personToUpdate?.Address != null)
                personFromRepository.SetAddress(Address.Create(personToUpdate.Address.AddressLine,
                                                               personToUpdate.Address.City,
                                                               personToUpdate.Address.State,
                                                               personToUpdate.Address.PostalCode).Value);



            personFromRepository.SetBirthday(personToUpdate.Birthday);


            if (personToUpdate?.Phones.Count > 0)
                foreach (var phone in personFromRepository.Phones)
                {
                    phones.Add(Phone.Create(phone.Number, phone.PhoneType, phone.IsPrimary).Value);
                    personFromRepository.SetPhones(phones);
                }

            if (personToUpdate?.Emails.Count > 0)
                foreach (var email in personFromRepository.Emails)
                {
                    emails.Add(Email.Create(email.Address, email.IsPrimary).Value);
                    personFromRepository.SetEmails(emails);
                }

            if (personToUpdate?.DriversLicense != null)
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

            if (await repository.SaveChangesAsync())
                return NoContent();

            return BadRequest($"Failed to update {personToUpdate.Name.LastFirstMiddle}.");
        }

        // POST: api/persons/
        [HttpPost]
        public async Task<ActionResult<PersonToRead>> AddPersonAsync(PersonToWrite personToAdd)
        {
            Address address = null;
            List<Phone> phones = new();
            List<Email> emails = new();
            DriversLicense driversLicense = null;

            var personName = PersonName.Create(personToAdd.Name.LastName, personToAdd.Name.FirstName, personToAdd.Name.MiddleName).Value;

            if (personToAdd?.Address != null)
                address = Address.Create(personToAdd.Address.AddressLine, personToAdd.Address.City, personToAdd.Address.State, personToAdd.Address.PostalCode).Value;

            if (personToAdd?.Phones.Count > 0)
                foreach (var phone in personToAdd.Phones)
                    phones.Add(Phone.Create(phone.Number, phone.PhoneType, phone.IsPrimary).Value);

            if (personToAdd?.Emails.Count > 0)
                foreach (var email in personToAdd.Emails)
                    emails.Add(Email.Create(email.Address, email.IsPrimary).Value);

            if (personToAdd?.DriversLicense != null)
            {
                DateTimeRange dateTimeRange = DateTimeRange.Create(
                    personToAdd.DriversLicense.Issued,
                    personToAdd.DriversLicense.Expiry).Value;

                driversLicense = DriversLicense.Create(personToAdd.DriversLicense.Number,
                    personToAdd.DriversLicense.State,
                    dateTimeRange).Value;
            }

            Person person = new(personName, personToAdd.Gender, address, emails, phones, personToAdd.Birthday, driversLicense);

            await repository.AddPersonAsync(person);

            if (await repository.SaveChangesAsync())
            {
                var personFromRepository = repository.GetPersonAsync(person.Id).Result;

                return CreatedAtRoute("GetPersonAsync",
                    new { id = person.Id },
                    personFromRepository);
            }

            return BadRequest($"Failed to add {personToAdd.Name}.");
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