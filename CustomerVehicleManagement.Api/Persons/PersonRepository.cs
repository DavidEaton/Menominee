using AutoMapper;
using CustomerVehicleManagement.Api.Utilities;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Persons
{
    public class PersonRepository : IPersonRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public PersonRepository(
            ApplicationDbContext context,
            IMapper mapper)
        {
            this.context = context ??
                throw new ArgumentNullException(nameof(context));

            this.mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
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
        public async Task<Person> GetPersonEntityAsync(int id)
        {
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

        public async Task<IReadOnlyList<PersonReadDto>> GetPersonsAsync()
        {
            var personsFromContext = await context.Persons
                                                  .Include(person => person.Phones)
                                                  .Include(person => person.Emails)
                                                  .ToArrayAsync();

            foreach (var person in personsFromContext)
            {
                mapper.Map<IReadOnlyList<Phone>>(person.Phones);
            }

            IReadOnlyList<PersonReadDto> list = mapper.Map<IReadOnlyList<PersonReadDto>>(personsFromContext);

            foreach (var personReadDto in list)
            {

                var emailReadDtos = new List<EmailReadDto>();

                foreach (var email in personReadDto.Emails)
                {
                    emailReadDtos.Add(new EmailReadDto
                    {
                        Address = email.Address,
                        IsPrimary = email.IsPrimary
                    });
                }

                personReadDto.Emails = emailReadDtos;
            }


            return list;
        }

        public async Task<IReadOnlyList<PersonInListDto>> GetPersonsListAsync()
        {
            var personsFromContext = await context.Persons
                                                  .Include(person => person.Phones)
                                                  .Include(person => person.Emails)
                                                  .ToArrayAsync();

            var personsList = new List<PersonInListDto>();

            foreach (var person in personsFromContext)
            {
                personsList.Add(DtoHelpers.PersonToPersonInListDto(person));
            }

            return personsList;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await context.SaveChangesAsync()) > 0;
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
