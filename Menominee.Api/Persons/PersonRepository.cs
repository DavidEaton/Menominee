using Menominee.Api.Data;
using Menominee.Domain.Entities;
using Menominee.Shared.Models.Persons;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Api.Persons
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

        public void Add(Person person)
        {
            if (person is not null)
                context.Attach(person);
        }

        public void Delete(Person person)
        {
            context.Remove(person);
        }
        public async Task<Person> GetEntityAsync(long id)
        {
            return await context.Persons
                .Include(person => person.Phones
                    .OrderByDescending(phone => phone.IsPrimary))
                .Include(person => person.Emails
                    .OrderByDescending(email => email.IsPrimary))
                .AsSplitQuery()
                .FirstOrDefaultAsync(person => person.Id == id);
        }

        public async Task<PersonToRead> GetAsync(long id)
        {
            var personFromContext = await GetEntityAsync(id);

            return personFromContext is not null
                ? PersonHelper.ConvertToReadDto(personFromContext)
                : null;
        }

        public async Task<IReadOnlyList<PersonToRead>> GetAllAsync()
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

        public async Task<IReadOnlyList<PersonToReadInList>> GetListAsync()
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
                        PersonHelper.ConvertToReadInListDto(person))
                .ToList();
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public void DeletePhone(Phone phone)
        {
            context.Entry(phone).State = EntityState.Deleted;
        }

        public void DeleteEmail(Email email)
        {
            context.Entry(email).State = EntityState.Deleted;
        }
    }
}