using Microsoft.EntityFrameworkCore;
using CustomerVehicleManagement.Api.Data.Interfaces;
using System;
using System.Threading.Tasks;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Api.Data.Models;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;
using CustomerVehicleManagement.Api.Utilities;

namespace CustomerVehicleManagement.Api.Data.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;

        public PersonRepository(AppDbContext context, IMapper mapper)
        {
            this.context = context ??
                throw new ArgumentNullException(nameof(context));

            this.mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public void Create(PersonCreateDto personCreateDto)
        {
            Person person = null;

            if (personCreateDto != null)
            {
                person = new Person(personCreateDto.Name, personCreateDto.Gender);
                person.SetBirthday(personCreateDto.Birthday);
                person.SetDriversLicense(personCreateDto.DriversLicense);
                person.SetAddress(personCreateDto.Address);

                if (personCreateDto.Phones != null)
                    person.SetPhones(mapper.Map<IList<Phone>>(personCreateDto.Phones));

                if (personCreateDto.Emails != null)
                    person.SetEmails(mapper.Map<IList<Email>>(personCreateDto.Emails));
            }

            if (person != null)
                context.Add(person);
        }

        public void Delete(Person person)
        {
            context.Remove(person);
        }
        public async Task<Person> GetPersonEntityAsync(int id)
        {
            // Prefer Find() over Single() or First() for single objects (non-collections);
            // Find() checks the Identity Map Cache before making a trip to the database.
            var personFromContext = context.Persons.Find(id);

            return await Task.FromResult(personFromContext);
        }

        public async Task<PersonReadDto> GetPersonAsync(int id)
        {
            var personFromContext = context.Persons.Find(id);

            return await Task.FromResult(mapper.Map<PersonReadDto>(personFromContext));
        }

        public async Task<IEnumerable<PersonReadDto>> GetPersonsAsync()
        {
            var personsFromContext = await context.Persons.ToArrayAsync();

            foreach (var person in personsFromContext)
            {
                // Automapper may have a bug: mapping Phones works but Emails fail...
                // mapper.Map<IEnumerable<Email>>(person.Emails);
                // ...so instead of automapper for the emails, MapDomainEmailToReadDto()
                mapper.Map<IEnumerable<Phone>>(person.Phones);
            }

            IEnumerable<PersonReadDto> list = mapper.Map<IEnumerable<PersonReadDto>>(personsFromContext);

            foreach (var personReadDto in list)
            {
                personReadDto.Emails = ContactableHelpers.MapDomainEmailToReadDto(personReadDto.Emails);
            }

            return list;
        }

        public async Task<IEnumerable<PersonInListDto>> GetPersonsListAsync()
        {
            var personsFromContext = context.Persons.ToArray();

            var personsList = new List<PersonInListDto>();

            foreach (var person in personsFromContext)
            {
                personsList.Add(DtoHelpers.CreatePersonsListDtoFromDomain(person));
            }

            return await Task.FromResult(personsList);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public void FixTrackingState()
        {
            context.FixState();
        }

        public void UpdatePersonAsync(PersonUpdateDto person)
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
    }
}
