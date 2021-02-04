using Microsoft.EntityFrameworkCore;
using CustomerVehicleManagement.Api.Data.Interfaces;
using System;
using System.Threading.Tasks;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Api.Data.Models;
using System.Linq;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Api.Data.Repositories
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

        public void AddPerson(PersonCreateDto person)
        {
            context.Add(new Person(person.Name, person.Gender, person.Birthday, person.Address, person.Phones, person.DriversLicense));
        }

        public void DeletePerson(PersonReadDto person)
        {
            context.Remove(person);
        }

        public async Task<PersonReadDto> GetPersonAsync(int id)
        {
            return await context.Persons
                                .Select(person =>
                                    new PersonReadDto()
                                    {
                                        Id = person.Id,
                                        Name = person.Name.LastFirstMiddle,
                                        Gender = person.Gender,
                                        Birthday = person.Birthday,
                                        DriversLicense = person.DriversLicense,
                                        Address = person.Address,
                                        Phones = person.Phones
                                    })
                                .AsNoTracking()
                                .SingleOrDefaultAsync(person => person.Id == id);
        }

        public async Task<IEnumerable<PersonReadDto>> GetPersonsAsync()
        {
            return await context.Persons
                                .Include(person => person.Phones)
                                .Select(person =>
                                    new PersonReadDto()
                                    {
                                        Id = person.Id,
                                        Name = person.Name.LastFirstMiddle,
                                        Gender = person.Gender,
                                        Birthday = person.Birthday,
                                        DriversLicense = person.DriversLicense,
                                        Address = person.Address,
                                        Phones = person.Phones
                                    })
                                .AsNoTracking()
                                .ToArrayAsync();
        }

        public async Task<IEnumerable<PersonListDto>> GetPersonsListAsync()
        {
            List<PersonListDto> personsList = new List<PersonListDto>();
            IQueryable<Person> persons = context.Persons
                                                .Include(person => person.Phones)
                                                .AsNoTracking();

            foreach (var person in persons)
            {

                PersonListDto personInList = new PersonListDto()
                {
                    Name = person.Name.LastFirstMiddle,
                    Id = person.Id,
                    Address = person?.Address?.AddressFull
                };

                if (person.Phones.Count > 0)
                    personInList.Phone = person.Phones.FirstOrDefault(phone => phone.Primary == true).ToString();

                personsList.Add(personInList);
            }

            return await Task.FromResult(personsList);
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

            return await context.SaveChangesAsync() > 0;
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
