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
        private readonly string BasePath = "/api/persons/";

        public PersonsController(IPersonRepository repository, ILogger<PersonsController> logger) : base(logger)
        {
            this.repository = repository ??
                throw new ArgumentNullException(nameof(repository));
        }

        [Route("list")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<PersonToReadInList>>> GetPersonsListAsync()
        {
            var persons = await repository.GetPersonsListAsync();

            return persons is not null
                ? Ok(persons)
                : NotFound();
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<PersonToRead>>> GetPersonsAsync()
        {
            var persons = await repository.GetPersonsAsync();

            return persons is not null
                ? Ok(persons)
                : NotFound();
        }

        [HttpGet("{id:long}", Name = "GetPersonAsync")]
        public async Task<ActionResult<PersonToRead>> GetPersonAsync(long id)
        {
            var person = await repository.GetPersonAsync(id);

            return person is not null
                ? Ok(person)
                : NotFound();
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdatePersonAsync(long id, PersonToWrite personDto)
        {
            var notFoundMessage = $"Could not find {personDto.Name.FirstName} {personDto.Name.LastName} to update";

            var personFromRepository = await repository.GetPersonEntityAsync(id);
            if (personFromRepository is null)
                return NotFound(notFoundMessage);

            if (NamesAreNotEqual(personFromRepository.Name, personDto.Name))
            {
                var result = UpdateName(personDto, personFromRepository);
                if (result.IsFailure)
                    return NotFound(result.Error);
            }

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

        private Result<PersonName> UpdateName(PersonToWrite personDto, Person personFromRepository)
        {
            Result<PersonName> result;

            if (personDto?.Name == null)
            {
                result = Result.Failure<PersonName>("Name information is missing.");
            }
            else
            {
                var lastName = personDto.Name.LastName ?? string.Empty;
                var firstName = personDto.Name.FirstName ?? string.Empty;
                var middleName = personDto.Name.MiddleName ?? string.Empty;

                result = PersonName.Create(lastName, firstName, middleName);
            }

            return personFromRepository.SetName(result.Value);
        }

        private static bool NamesAreNotEqual(PersonName name, PersonNameToWrite nameDto) =>
            !string.Equals(name.FirstName, nameDto?.FirstName) ||
            !string.Equals(name.MiddleName, nameDto?.MiddleName) ||
            !string.Equals(name.LastName, nameDto?.LastName);

        [HttpPost]
        public async Task<ActionResult> AddPersonAsync(PersonToWrite personToAdd)
        {
            var person = PersonHelper.ConvertWriteDtoToEntity(personToAdd);

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
            {
                return NotFound($"Could not find Person in the database to delete with Id: {id}.");
            }

            repository.DeletePerson(personFromRepository);
            await repository.SaveChangesAsync();

            return NoContent();
        }
    }
}