using Microsoft.EntityFrameworkCore;
using CustomerVehicleManagement.Api.Data.Interfaces;
using System;
using System.Threading.Tasks;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Api.Data.Models;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;
using SharedKernel.Enums;

namespace CustomerVehicleManagement.Api.Data.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;
        public PersonRepository()
        {
        }

        public PersonRepository(AppDbContext context, IMapper mapper)
        {
            this.context = context ??
                throw new ArgumentNullException(nameof(context));

            this.mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public void AddPerson(PersonCreateDto person)
        {
            context.Add(mapper.Map<Person>(person));
        }

        public void DeletePerson(PersonReadDto person)
        {
            context.Remove(person);
        }

        public async Task<PersonReadDto> GetPersonAsync(int id)
        {
            var personFromContext = await context.Persons
                                .AsNoTracking()
                                .SingleOrDefaultAsync(person => person.Id == id);

            return mapper.Map<PersonReadDto>(personFromContext);

        }

        public async Task<IEnumerable<PersonReadDto>> GetPersonsAsync()
        {
            var personsFromContext = await context.Persons
                                .Include(person => person.Phones)
                                .AsNoTracking()
                                .ToArrayAsync();

            return mapper.Map<IEnumerable<PersonReadDto>>(personsFromContext);
        }

        public async Task<IEnumerable<PersonListDto>> GetPersonsListAsync()
        {
            IQueryable<Person> personsFromContext = context.Persons
                                                .Include(person => person.Phones)
                                                .AsNoTracking();

            var personsList = mapper.Map<IEnumerable<PersonListDto>>(personsFromContext);

            foreach (var person in personsFromContext)
            {
                if (person.Phones.Count > 0)
                {
                    // Find the corresponding person in the list and set its Phone
                    personsList.FirstOrDefault(p => p.Id == person.Id).Phone = person.Phones.FirstOrDefault(phone => phone.Primary == true).ToString();
                }
            }

            return await Task.FromResult(personsList);
        }

        public async Task<bool> PersonExistsAsync(int id)
        {
            return await context.Persons
                .AnyAsync(person => person.Id == id);
        }

        public async Task<bool> SaveChangesAsync(PersonCreateDto personToCreate)
        {
            var person = mapper.Map<Person>(personToCreate);
            // Mark person EF tracking state = modified via context:
            context.Persons
                .Update(person);

            var result = await context.SaveChangesAsync();
            personToCreate.Id = person.Id;
            return result > 0;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public void FixState()
        {
            context.FixState();
        }

        public async Task<PersonReadDto> UpdatePersonAsync(PersonUpdateDto person)
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
                    return null;
                throw;
            }

            return null;
        }
    }
}
