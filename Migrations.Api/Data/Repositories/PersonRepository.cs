using Microsoft.EntityFrameworkCore;
using Migrations.Api.Data.Interfaces;
using Migrations.Core.Entities;
using System;
using System.Threading.Tasks;

namespace Migrations.Api.Data.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly AppDbContext context;
        public PersonRepository()
        {
        }

        public PersonRepository(AppDbContext context)
        {
            this.context = context;
        }

        public void AddPerson(Person person)
        {
            context.Add(person);
        }

        public void DeletePerson(Person person)
        {
            context.Remove(person);
        }

        public async Task<Person> GetPersonAsync(int id)
        {
            return await context.Persons
                .Include(p => p.Address)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Person[]> GetPersonsAsync()
        {
            return await context.Persons
                // Tracking is not needed (and expensive) for this disconnected data collection
                .AsNoTracking()
                .Include(p => p.Address)
                .ToArrayAsync();
        }

        public async Task<bool> PersonExistsAsync(int id)
        {
            return await context.Persons
                .AnyAsync(person => person.Id == id);
        }

        public async Task<bool> SaveChangesAsync(Person person)
        {
            // Mark person EF tracking state = modified via dbContext:
            context.Persons
                .Update(person);

            return (await context.SaveChangesAsync()) > 0;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await context.SaveChangesAsync()) > 0;
        }

        public void FixState()
        {
            context.FixState();
        }

        public async Task<Person> UpdatePersonAsync(Person person)
        {
            if (person == null)
                throw new NullReferenceException("Person Id missing.");

            context.Entry(person).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await PersonExistsAsync(person.Id))
                    return null;// something that tells the controller to return NotFound();
                throw;
            }

            return null;
        }

    }
}
