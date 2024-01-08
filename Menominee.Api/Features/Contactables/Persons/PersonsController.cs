using Menominee.Shared.Models.Http;
using Menominee.Shared.Models.Persons;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.Features.Contactables.Persons
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
            var persons = await repository.GetListAsync();

            return persons is not null
                ? Ok(persons)
                : NotFound();
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<PersonToRead>>> GetAllAsync()
        {
            var persons = await repository.GetAllAsync();

            return persons is not null
                ? Ok(persons)
                : NotFound();
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<PersonToRead>> GetAsync(long id)
        {
            var person = await repository.GetAsync(id);

            return person is not null
                ? Ok(person)
                : NotFound();
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult> UpdateAsync(PersonToWrite personFromCaller)
        {
            var personFromRepository = await repository.GetEntityAsync(personFromCaller.Id);
            if (personFromRepository is null)
                return NotFound($"Could not find {personFromCaller.Name.FirstName} {personFromCaller.Name.LastName} to update");

            ContactDetailUpdaters.UpdatePerson(personFromCaller, personFromRepository);

            await repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<PostResponse>> AddAsync(PersonToWrite personToAdd)
        {
            var person = PersonHelper.ConvertWriteDtoToEntity(personToAdd);

            repository.Add(person);
            await repository.SaveChangesAsync();

            return Created(new Uri($"/api/PersonsController/{person.Id}",
                UriKind.Relative),
                new { person.Id });
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult> DeleteAsync(long id)
        {
            var personFromRepository = await repository.GetEntityAsync(id);

            if (personFromRepository == null)
                return NotFound($"Could not find Person in the database to delete with Id: {id}.");

            repository.Delete(personFromRepository);
            await repository.SaveChangesAsync();

            return NoContent();
        }
    }
}