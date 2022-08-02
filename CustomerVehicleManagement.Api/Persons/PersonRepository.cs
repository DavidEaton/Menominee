using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models.Persons;
using Menominee.Common.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Persons
{
    public class PersonRepository : IPersonRepository
    {
        private readonly ApplicationDbContext context;

        public PersonRepository(
            ApplicationDbContext context)
        {
            this.context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public async Task AddPersonAsync(Person person)
        {
            if (person != null)
                await context.AddAsync(person);
        }

        public void DeletePerson(Person person)
        {
            context.Remove(person);
        }
        public async Task<Person> GetPersonEntityAsync(long id)
        {
            Person personFromContext = await context.Persons
                .Include(person => person.Phones
                    .OrderByDescending(phone => phone.IsPrimary))
                .Include(person => person.Emails
                    .OrderByDescending(email => email.IsPrimary))
                .AsSplitQuery()
                .FirstOrDefaultAsync(person => person.Id == id);

            return personFromContext;
        }

        public async Task<PersonToRead> GetPersonAsync(long id)
        {
            Person personFromContext = await GetPersonEntityAsync(id);

            Guard.ForNull(personFromContext, "personFromContext");
            return PersonHelper.ConvertToReadDto(personFromContext);
        }

        public async Task<IReadOnlyList<PersonToRead>> GetPersonsAsync()
        {
            IReadOnlyList<Person> personsFromContext = await context.Persons
                .Include(person => person.Phones
                    .OrderByDescending(phone => phone.IsPrimary))
                .Include(person => person.Emails
                    .OrderByDescending(email => email.IsPrimary))
                .AsNoTracking()
                .AsSplitQuery()
                .ToArrayAsync();

            return personsFromContext
                .Select(person =>
                        PersonHelper.ConvertToReadDto(person))
                .ToList();
        }

        public async Task<IReadOnlyList<PersonToReadInList>> GetPersonsListAsync()
        {
            IReadOnlyList<Person> personsFromContext = await context.Persons
                .Include(person => person.Phones
                    .OrderByDescending(phone => phone.IsPrimary))
                .Include(person => person.Emails
                    .OrderByDescending(email => email.IsPrimary))
                .AsNoTracking()
                .AsSplitQuery()
                .ToArrayAsync();

            return personsFromContext
                .Select(person =>
                        PersonToReadInList.ConvertToDto(person))
                .ToList();
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public void FixTrackingState()
        {
            context.FixState();
        }

        public void UpdatePersonAsync(Person person)
        {
            // No code in this implementation.

            /* We're working on a contract (IPersonRepository), not an implementation.

               Always code a complete set of methods for the required funtionality and call them,
               even of they don't do anything in the current implementation.

               Controller has changed the entity to a modified state; executing save on the repo from
               the controller will write the changes to the database; therefore no update code is
               required in this Update method.
            */
        }

        public async Task<bool> PersonExistsAsync(long id)
        {
            return await context.Persons
                .AnyAsync(person => person.Id == id);
        }
    }
}
