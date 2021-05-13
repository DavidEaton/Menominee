using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using CustomerVehicleManagement.Domain.Entities;
using System.Collections.Generic;
using AutoMapper;
using CustomerVehicleManagement.Api.Utilities;

namespace CustomerVehicleManagement.Api.Persons
{
    public class PersonRepository : IPersonRepository
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;

        public PersonRepository(
            AppDbContext context,
            IMapper mapper)
        {
            this.context = context ??
                throw new ArgumentNullException(nameof(context));

            this.mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public async Task AddPersonAsync(PersonCreateDto personCreateDto)
        {
            Person person = null;

            if (personCreateDto != null)
            {
                person = new Person(personCreateDto.Name, personCreateDto.Gender);

                if (personCreateDto.Birthday != null)
                    person.SetBirthday(personCreateDto.Birthday);

                if (personCreateDto.DriversLicense != null)
                    person.SetDriversLicense(personCreateDto.DriversLicense);

                if (personCreateDto.Address != null)
                    person.SetAddress(personCreateDto.Address);

                if (personCreateDto.Phones != null)
                    if (personCreateDto.Phones.Count > 0)
                        person.SetPhones(mapper.Map<IList<Phone>>(personCreateDto.Phones));

                if (personCreateDto.Emails != null)
                    if (personCreateDto.Emails.Count > 0)
                        person.SetEmails(mapper.Map<IList<Email>>(personCreateDto.Emails));
            }

            if (person != null)
                await context.AddAsync(person);
        }

        public void DeletePerson(Person person)
        {
            context.Remove(person);
        }
        public async Task<Person> GetPersonEntityAsync(int id)
        {
            // Prefer FindAsync() over Single() or First() for single objects (non-collections);
            // FindAsync() checks the Identity Map Cache before making a trip to the database.
            var personFromContext = await context.Persons
                .Include(person => person.Phones)
                .Include(person => person.Emails)
                .FirstOrDefaultAsync(person => person.Id == id);

            return personFromContext;
        }

        public async Task<PersonReadDto> GetPersonAsync(int id)
        {
            var personFromContext = await context.Persons
                .Include(person => person.Phones)
                .Include(person => person.Emails)
                .FirstOrDefaultAsync(person => person.Id == id);

            return mapper.Map<PersonReadDto>(personFromContext);
        }

        public async Task<IEnumerable<PersonReadDto>> GetPersonsAsync()
        {
            var personsFromContext = await context.Persons.ToArrayAsync();

            foreach (var person in personsFromContext)
            {
                // Mapping Phones works but Emails fail:
                // mapper.Map<IEnumerable<Email>>(person.Emails);
                // ...so instead of automapper for the emails, MapDomainEmailToReadDto()
                mapper.Map<IEnumerable<Phone>>(person.Phones);
            }

            IEnumerable<PersonReadDto> list = mapper.Map<IEnumerable<PersonReadDto>>(personsFromContext);

            foreach (var personReadDto in list)
            {
                personReadDto.Emails = ContactableHelpers.MapEmailReadDtoToReadDto(personReadDto.Emails);
            }

            return list;
        }

        public async Task<IEnumerable<PersonInListDto>> GetPersonsListAsync()
        {
            var personsFromContext = await context.Persons.ToArrayAsync();

            var personsList = new List<PersonInListDto>();

            foreach (var person in personsFromContext)
            {
                personsList.Add(DtoHelpers.CreatePersonsListDtoFromDomain(person));
            }

            return personsList;
        }

        public async Task<bool> SaveChangesAsync()
        {
            bool result = await context.SaveChangesAsync() > 0;
            return result;
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

        public async Task<int> GetPersonsTotalAsync()
        {
            return await context.Persons.CountAsync();
        }
    }
}
