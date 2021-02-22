using Microsoft.EntityFrameworkCore;
using CustomerVehicleManagement.Api.Data.Interfaces;
using System;
using System.Threading.Tasks;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Api.Data.Models;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;
using SharedKernel.ValueObjects;
using CustomerVehicleManagement.Api.Utilities;

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

        public void DeletePerson(Person person)
        {
            context.Remove(person);
        }

        public async Task<Person> GetPersonAsync(int id)
        {
            // Prefer Find() over Single() or First() for single objects (non-collections);
            // Find() checks the Identity Map Cache before making a trip to the database.
            var personFromContext = context.Persons.Find(id);

            return await Task.FromResult(personFromContext);

        }

        public async Task<IEnumerable<PersonReadDto>> GetPersonsAsync()
        {
            var personsFromContext = await context.Persons.ToArrayAsync();

            return mapper.Map<IEnumerable<PersonReadDto>>(personsFromContext);
        }

        public async Task<IEnumerable<PersonListDto>> GetPersonsListAsync()
        {
            var personsFromContext = context.Persons.ToArray();

            var personsList = new List<PersonListDto>();

            foreach (var person in personsFromContext)
            {
                personsList.Add(DtoHelpers.CreatePersonsListDtoFromDomain(person));
            }

            return await Task.FromResult(personsList);
        }

        public async Task<PersonReadDto> SaveChangesAsync(PersonCreateDto personToCreate)
        {
            var person = new Person(
                new PersonName(personToCreate.Name.LastName
                              ,personToCreate.Name.FirstName
                              ,personToCreate.Name.MiddleName)
                              ,personToCreate.Gender
                              ,personToCreate.Birthday
                              ,personToCreate.Address);

            List<Phone> phones = CreatePhones(personToCreate);

            person.SetPhones(phones);

            context.Add(person);

            var result = await context.SaveChangesAsync();

            if (result > 0)
            {
                if (person?.Id > 0)
                    return mapper.Map<PersonReadDto>(person);
            }

            return null;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public void FixState()
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

        public async Task<bool> PersonExistsAsync(int id)
        {
            return await context.Persons
                .AnyAsync(person => person.Id == id);
        }

        private static List<Phone> CreatePhones(PersonCreateDto personToCreate)
        {
            var phones = new List<Phone>();
            Phone newPhone;

            foreach (var phone in personToCreate.Phones)
            {
                newPhone = new Phone(phone.Number, phone.PhoneType, phone.Primary);
                phones.Add(newPhone);
            }

            return phones;
        }
    }
}
