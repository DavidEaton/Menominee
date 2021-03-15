using AutoMapper;
using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Api.Data.Models;
using CustomerVehicleManagement.Api.Data.Repositories;
using CustomerVehicleManagement.Api.Profiles;
using CustomerVehicleManagement.Api.Utilities;
using CustomerVehicleManagement.Domain.Entities;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Enums;
using SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CustomerVehicleManagement.Api.IntegrationTests.Repositories
{
    public class PersonsRepositoryShould
    {
        private static IMapper mapper;
        public PersonsRepositoryShould()
        {
            if (mapper == null)
            {
                var mapperConfiguration = new MapperConfiguration(configuration =>
                {
                    configuration.AddProfile(new PersonProfile());
                    configuration.AddProfile(new EmailProfile());
                    configuration.AddProfile(new PhoneProfile());
                });

                mapper = mapperConfiguration.CreateMapper();
            }
        }

        [Fact]
        public async Task GetPersonsAsync()
        {
            // Arrange
            // Create an in-memory database
            var options = CreateContextOptions();

            // Due to the disconnected nature of ASP.NET Core,
            // our tests create and use unique contexts for each
            // request, like our production code does.
            // Add a Person to the in-memory database
            using (var context = new AppDbContext(options))
            {
                context.Persons.Add(CreateValidPerson());
                context.SaveChanges();
            }
            // Read all Persons from the in-memory database
            using (var context = new AppDbContext(options))
            {
                var repository = new PersonRepository(context, mapper);

                // Act
                var persons = (List<PersonReadDto>)await repository.GetPersonsAsync();

                // Assert
                persons.Count.Should().BeGreaterThan(0);
            }
        }

        [Fact]
        public async Task CreatePersonAsync()
        {
            var options = CreateContextOptions();

            using (var context = new AppDbContext(options))
            {
                var repository = new PersonRepository(context, mapper);

                // Act
                await repository.CreatePersonAsync(mapper.Map<PersonCreateDto>(CreateValidPerson()));
                await repository.SaveChangesAsync();
                var persons = await repository.GetPersonsAsync();

                // Assert
                persons.Count().Should().BeGreaterThan(0);
            }
        }

        [Fact]
        public async Task GetPersonAsync()
        {
            var options = CreateContextOptions();
            var id = 0;

            using (var context = new AppDbContext(options))
            {
                var person = CreateValidPerson();
                context.Persons.Add(person);
                context.SaveChanges();
                id = person.Id;
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new PersonRepository(context, mapper);

                var personFromRepo = await repository.GetPersonAsync(id);

                personFromRepo.Should().NotBeNull();
            }
        }

        [Fact]
        public async Task GetPersonAsyncIncludesPhones()
        {
            var options = CreateContextOptions();
            var id = 0;

            using (var context = new AppDbContext(options))
            {
                var person = CreateValidPersonWithPhones();
                context.Persons.Add(person);
                context.SaveChanges();
                id = person.Id;
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new PersonRepository(context, mapper);
                var personFromRepo = await repository.GetPersonAsync(id);

                personFromRepo.Should().NotBeNull();
                personFromRepo.Phones.Should().NotBeNull();
                personFromRepo.Phones.Count().Should().BeGreaterThan(0);
            }
        }

        [Fact]
        public async Task GetPersonAsyncIncludesEmails()
        {
            var options = CreateContextOptions();
            var id = 0;

            using (var context = new AppDbContext(options))
            {
                var person = CreateValidPersonWithEmails();
                context.Persons.Add(person);
                context.SaveChanges();
                id = person.Id;
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new PersonRepository(context, mapper);
                var personFromRepo = await repository.GetPersonAsync(id);

                personFromRepo.Should().NotBeNull();
                personFromRepo.Emails.Should().NotBeNull();
                personFromRepo.Emails.Count().Should().BeGreaterThan(0);
            }
        }

        [Fact]
        public async Task GetPersonsListAsync()
        {
            var options = CreateContextOptions();

            using (var context = new AppDbContext(options))
            {
                context.Persons.Add(CreateValidPerson());
                context.SaveChanges();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new PersonRepository(context, mapper);

                var persons = await repository.GetPersonsListAsync();

                persons.Count().Should().BeGreaterThan(0);
            }
        }

        [Fact]
        public async Task UpdatePersonAsync()
        {
            var options = CreateContextOptions();
            Person person;
            Person personFromRepository;
            var id = 0;

            // Create Person and save
            using (var context = new AppDbContext(options))
            {
                person = CreateValidPerson();
                context.Persons.Add(person);
                context.SaveChanges();
                id = person.Id;
            }

            // Get the saved Person, set their Birthday and save
            using (var context = new AppDbContext(options))
            {
                var repository = new PersonRepository(context, mapper);
                personFromRepository = await repository.GetPersonEntityAsync(id);

                var personUpdateDto = new PersonUpdateDto
                {
                    Name = personFromRepository.Name,
                    Gender = personFromRepository.Gender,
                    Birthday = DateTime.Today.AddYears(-20)
                };

                DtoHelpers.ConvertPersonUpdateDtoToDomainModel(personUpdateDto, personFromRepository, mapper);

                repository.UpdatePersonAsync(personUpdateDto);

                await repository.SaveChangesAsync();
            }

            // Get the updated Person and Assert
            using (var context = new AppDbContext(options))
            {
                var repository = new PersonRepository(context, mapper);
                personFromRepository = await repository.GetPersonEntityAsync(id);

                personFromRepository.Birthday.Should().BeCloseTo(DateTime.Today.AddYears(-20));
            }
        }

        [Fact(Skip = "Awaiting Implimentation")]
        [Trait("Category", "Experimental")]
        public async Task UpdatePersonGraphAsync()
        {
            var options = CreateContextOptions();
            Person person;
            Person personFromRepository;
            PersonReadDto personFromRepositoryUpdated;
            var id = 0;
            var number = "(555) 555-5555";
            var phoneType = PhoneType.Mobile;
            var isPrimary = false;
            var address = "a@b.c";

            CreateAndSavePersonGraph(options, out person, out id);

            // Get the Person from the database
            using (var context = new AppDbContext(options))
            {
                var repository = new PersonRepository(context, mapper);
                personFromRepository = await repository.GetPersonEntityAsync(id);

                // Create an Update Dto with some changes for the Person
                var personUpdateDto = new PersonUpdateDto
                {
                    Id = id,
                    Gender = personFromRepository.Gender,
                    Name = personFromRepository.Name,
                    Birthday = DateTime.Today.AddYears(-20),
                    Phones = mapper.Map<List<PhoneUpdateDto>>(personFromRepository.Phones),
                    Emails = mapper.Map<List<EmailUpdateDto>>(personFromRepository.Emails)
                };

                var phone = new PhoneUpdateDto
                {
                    Number = number,
                    IsPrimary = isPrimary,
                    PhoneType = phoneType
                };

                var email = new EmailUpdateDto
                {
                    Address = address,
                    IsPrimary = isPrimary
                };

                personFromRepository.SetTrackingState(TrackingState.Modified);
                personUpdateDto.Phones.Add(phone);
                personUpdateDto.Emails.Add(email);

                // ACT
                //DtoHelpers.ConvertPersonUpdateDtoToDomainModel(personUpdateDto, personFromRepository, mapper);

                mapper.Map<Person>(personUpdateDto);

                repository.FixTrackingState();


                repository.UpdatePersonAsync(personUpdateDto);

                await repository.SaveChangesAsync();
            }

            // Get the updated Person and Assert
            using (var context = new AppDbContext(options))
            {
                var repository = new PersonRepository(context, mapper);
                personFromRepositoryUpdated = await repository.GetPersonAsync(id);

                personFromRepositoryUpdated.Birthday.Should().BeCloseTo(DateTime.Today.AddYears(-20));
                personFromRepositoryUpdated.Phones.Should().Contain(number);
            }



        }


        [Fact]
        public async Task DeletePerson()
        {
            var options = CreateContextOptions();
            var id = 0;

            using (var context = new AppDbContext(options))
            {
                var person = CreateValidPerson();
                context.Persons.Add(person);
                context.SaveChanges();
                id = person.Id;
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new PersonRepository(context, mapper);
                var personFromRepo = await repository.GetPersonEntityAsync(id);

                personFromRepo.Should().NotBeNull();

                // ACT
                repository.Delete(personFromRepo);
                await repository.SaveChangesAsync();

                context.Persons.Count().Should().Be(0);
            }
        }

        [Fact]
        public void FixState()
        {

        }

        [Fact]
        public void ThrowExceptionWhenPassedNullContext()
        {
            Action action = () => new PersonRepository(null, mapper);

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ThrowExceptionWhenPassedNullMapper()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase($"testdb{Guid.NewGuid()}")
                .Options;

            using (var context = new AppDbContext(options))
            {
                Action action = () => new PersonRepository(context, null);
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task ReturnTrueIfPersonExistsAsync()
        {
            var options = CreateContextOptions();
            var id = 0;

            using (var context = new AppDbContext(options))
            {
                var person = CreateValidPerson();
                context.Persons.Add(person);
                context.SaveChanges();
                id = person.Id;
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new PersonRepository(context, mapper);
                var personExists = await repository.PersonExistsAsync(id);

                personExists.Should().Be(true);
            }
        }

        // CATEGORY
        [Fact(Skip = "SQLite example")]
        [Trait("Category", "Experimental")]
        public void SqliteExampleTest()
        {
            var connectionStringBuilder =
                new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionStringBuilder.ToString());
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connection)
                .Options;
            var person = CreateValidPerson();
            using (var context = new AppDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();
                context.Persons.Add(person);
                context.SaveChanges();

                Person savedPerson = context.Persons.FirstOrDefault(x => x.Id == person.Id);

                savedPerson.Should().NotBeNull();
            }
        }

        private static DbContextOptions<AppDbContext> CreateContextOptions()
        {
            // Create an in-memory database
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase($"testdb{Guid.NewGuid()}")
                .Options;
        }

        private static Person CreateValidPerson()
        {
            var firstName = "Jane";
            var lastName = "Doe";
            var name = new PersonName(lastName, firstName);
            var person = new Person(name, Gender.Female);

            return person;
        }

        private static Person CreateValidPersonWithPhones()
        {
            var person = CreateValidPerson();
            person.SetPhones(CreateValidPhones());
            return person;
        }

        private static Person CreateValidPersonWithEmails()
        {
            var person = CreateValidPerson();
            person.SetEmails(CreateValidEmails());
            return person;
        }

        private static void CreateAndSavePersonGraph(DbContextOptions<AppDbContext> options, out Person person, out int id)
        {
            // Create a new Person with Emails and Phones, and save
            using (var context = new AppDbContext(options))
            {
                person = CreateValidPersonWithEmails();
                person.SetPhones(CreateValidPhones());
                context.Persons.Add(person);
                context.SaveChanges();
                id = person.Id;
            }
        }
        private static IList<Phone> CreateValidPhones()
        {
            IList<Phone> phones = new List<Phone>();

            var number1 = "(555) 987-6543";
            var phoneType1 = PhoneType.Mobile;
            var isPrimary1 = true;
            var phone1 = new Phone(number1, phoneType1, isPrimary1);

            var number2 = "(555) 123-4567";
            var phoneType2 = PhoneType.Mobile;
            var isPrimary2 = false;
            var phone2 = new Phone(number2, phoneType2, isPrimary2);

            phones.Add(phone1);
            phones.Add(phone2);

            return phones;
        }

        private static IList<Email> CreateValidEmails()
        {
            IList<Email> Emails = new List<Email>();

            var address1 = "a@b.c";
            var isPrimary1 = true;
            var Email1 = new Email(address1, isPrimary1);

            var address2 = "d@e.f";
            var isPrimary2 = false;
            var Email2 = new Email(address2, isPrimary2);

            Emails.Add(Email1);
            Emails.Add(Email2);

            return Emails;
        }
    }
}
