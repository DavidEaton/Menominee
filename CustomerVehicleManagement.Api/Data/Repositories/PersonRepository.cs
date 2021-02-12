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
using SharedKernel.ValueObjects;

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
                                .Include(person => person.Phones)
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
            var personsFromContext = context.Persons
                                                .Include(person => person.Phones)
                                                .AsNoTracking()
                                                .ToArray();

            var personsList = new List<PersonListDto>();

            foreach (var person in personsFromContext)
            {
                personsList.Add(new PersonListDto()
                {
                    AddressLine = person?.Address?.AddressLine,
                    City = person?.Address?.City,
                    Id = person.Id,
                    Name = person.Name.LastFirstMiddle,
                    PostalCode = person?.Address?.PostalCode,
                    State = person?.Address?.State,
                    Phone = (person.Phones.Count > 0) ? person.Phones.FirstOrDefault(phone => phone.Primary == true).ToString() : null,
                    PhoneType = (person.Phones.Count > 0) ? person.Phones.FirstOrDefault(phone => phone.Primary == true).PhoneType.ToString() : null
                });
            }

            return await Task.FromResult(personsList);
        }

        public async Task<PersonReadDto> SaveChangesAsync(PersonCreateDto personToCreate)
        {
            var person = new Person(
                new PersonName(personToCreate.Name.LastName, personToCreate.Name.FirstName, personToCreate.Name.MiddleName)
                , personToCreate.Gender
                , personToCreate.Birthday
                , personToCreate.Address);

            List<Phone> phones = MapPhones(personToCreate);

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

        public async Task<bool> PersonExistsAsync(int id)
        {
            return await context.Persons
                .AnyAsync(person => person.Id == id);
        }

        private static List<Phone> MapPhones(PersonCreateDto personToCreate)
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
