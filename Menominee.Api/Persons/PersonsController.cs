using CSharpFunctionalExtensions;
using Menominee.Api.Common;
using Menominee.Common.ValueObjects;
using Menominee.Domain.Entities;
using Menominee.Shared.Models.Persons;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Api.Persons
{
    public class PersonsController : BaseApplicationController<PersonsController>
    {
        private readonly IPersonRepository repository;

        public PersonsController(IPersonRepository repository, ILogger<PersonsController> logger) : base(logger)
        {
            this.repository = repository ??
                throw new ArgumentNullException(nameof(repository));
        }

        [Route("list")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<PersonToReadInList>>> GetListAsync()
        {
            var persons = await repository.GetPersonsListAsync();

            return persons is not null
                ? Ok(persons)
                : NotFound();
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<PersonToRead>>> GetAsync()
        {
            var persons = await repository.GetPersonsAsync();

            return persons is not null
                ? Ok(persons)
                : NotFound();
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<PersonToRead>> GetAsync(long id)
        {
            var person = await repository.GetPersonAsync(id);

            return person is not null
                ? Ok(person)
                : NotFound();
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult> UpdateAsync(long id, PersonToWrite personDto)
        {
            var notFoundMessage = $"Could not find {personDto.Name.FirstName} {personDto.Name.LastName} to update";

            var personFromRepository = await repository.GetPersonEntityAsync(id);
            if (personFromRepository is null)
                return NotFound(notFoundMessage);

            UpdateName(personDto, personFromRepository);

            var contactDetails = ContactDetailsFactory
                .Create(personDto.Phones.ToList(), personDto.Emails.ToList(), personDto.Address).Value;

            var phonesToDelete = personFromRepository.Phones
                .Where(phone => !contactDetails.Phones.Any(phoneToKeep => phoneToKeep.Id == phone.Id))
                .ToList();

            if (phonesToDelete.Any())
            {
                phonesToDelete.ForEach(phone => repository.DeletePhone(phone));
            }

            var emailsToDelete = personFromRepository.Emails
                .Where(email => !contactDetails.Emails.Any(emailToKeep => emailToKeep.Id == email.Id))
                .ToList();

            if (emailsToDelete.Any())
            {
                emailsToDelete.ForEach(email => repository.DeleteEmail(email));
            }

            personFromRepository.UpdateContactDetails(contactDetails);

            personFromRepository.SetGender(personDto.Gender);
            personFromRepository.SetBirthday(personDto.Birthday);

            if (personDto?.DriversLicense is not null)
            {
                personFromRepository.SetDriversLicense(DriversLicense.Create(personDto.DriversLicense.Number,
                    personDto.DriversLicense.State,
                    DateTimeRange.Create(
                    personDto.DriversLicense.Issued,
                    personDto.DriversLicense.Expiry).Value).Value);
            }

            await repository.SaveChangesAsync();

            return NoContent();
        }

        private static void UpdateName(PersonToWrite personDto, Person personFromRepository)
        {
            // Data Transfer Objects have been validated in ASP.NET request pipeline
            if (NamesAreNotEqual(personFromRepository.Name, personDto.Name))
            {
                personFromRepository.SetName(PersonName.Create(
                    personDto.Name.LastName,
                    personDto.Name.FirstName,
                    personDto.Name.MiddleName)
                    .Value);
            }
        }

        private static bool NamesAreNotEqual(PersonName name, PersonNameToWrite nameDto) =>
            !string.Equals(name.FirstName, nameDto?.FirstName) ||
            !string.Equals(name.MiddleName, nameDto?.MiddleName) ||
            !string.Equals(name.LastName, nameDto?.LastName);

        [HttpPost]
        public async Task<ActionResult> AddAsync(PersonToWrite personToAdd)
        {
            var person = PersonHelper.ConvertWriteDtoToEntity(personToAdd);

            await repository.AddPersonAsync(person);
            await repository.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetAsync),
                new { id = person.Id },
                new { person.Id });
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult> DeleteAsync(long id)
        {
            var personFromRepository = await repository.GetPersonEntityAsync(id);

            if (personFromRepository == null)
            {
                return NotFound($"Could not find Person in the database to delete with Id: {id}.");
            }

            repository.DeletePerson(personFromRepository);
            await repository.SaveChangesAsync();

            return NoContent();
        }
    }
}