using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Api.Persons;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static CustomerVehicleManagement.Api.IntegrationTests.Helpers.Utilities;


namespace CustomerVehicleManagement.Api.IntegrationTests.Repositories
{
    public class PersonsRepositoryShould
    {
        [Fact]
        public async Task GetPersonsAsync()
        {
            // Arrange
            // Create an in-memory database
            var options = CreateDbContextOptions();

            // Due to the disconnected nature of ASP.NET Core,
            // our tests create and use unique contexts for each
            // request, like our production code does.

            // Add a Person to the in-memory database
            using (var context = new ApplicationDbContext(options))
            {
                context.Persons.Add(CreateValidPerson());
                context.SaveChanges();
            }

            // Read all Persons from the in-memory database
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new PersonRepository(context);

                // Act
                var persons = (List<PersonReadDto>)await repository.GetPersonsAsync();

                // Assert
                persons.Count.Should().BeGreaterThan(0);
            }
        }

        [Fact]
        public async Task AddPersonAsync()
        {
            var options = CreateDbContextOptions();

            using (var context = new ApplicationDbContext(options))
            {
                var repository = new PersonRepository(context);

                // Act
                await repository.AddPersonAsync(CreateValidPerson());
                await repository.SaveChangesAsync();
                var persons = await repository.GetPersonsAsync();

                // Assert
                persons.Count().Should().BeGreaterThan(0);
            }
        }

        [Fact]
        public async Task GetPersonAsync()
        {
            var options = CreateDbContextOptions();
            var id = 0;

            using (var context = new ApplicationDbContext(options))
            {
                var person = CreateValidPerson();
                context.Persons.Add(person);
                context.SaveChanges();
                id = person.Id;
            }

            using (var context = new ApplicationDbContext(options))
            {
                var repository = new PersonRepository(context);

                var personFromRepo = await repository.GetPersonAsync(id);

                personFromRepo.Should().NotBeNull();
            }
        }

        [Fact]
        public async Task GetPersonAsyncIncludesPhones()
        {
            var options = CreateDbContextOptions();
            var id = 0;

            using (var context = new ApplicationDbContext(options))
            {
                var person = CreateValidPersonWithPhones();
                context.Persons.Add(person);
                context.SaveChanges();
                id = person.Id;
            }

            using (var context = new ApplicationDbContext(options))
            {
                var repository = new PersonRepository(context);
                var personFromRepo = await repository.GetPersonAsync(id);

                personFromRepo.Should().NotBeNull();
                personFromRepo.Phones.Should().NotBeNull();
                personFromRepo.Phones.Count().Should().BeGreaterThan(0);
            }
        }

        [Fact]
        public async Task GetPersonAsyncIncludesEmails()
        {
            var options = CreateDbContextOptions();
            var id = 0;

            using (var context = new ApplicationDbContext(options))
            {
                var person = CreateValidPersonWithEmails();
                context.Persons.Add(person);
                context.SaveChanges();
                id = person.Id;
            }

            using (var context = new ApplicationDbContext(options))
            {
                var repository = new PersonRepository(context);
                var personFromRepo = await repository.GetPersonAsync(id);

                personFromRepo.Should().NotBeNull();
                personFromRepo.Emails.Should().NotBeNull();
                personFromRepo.Emails.Count().Should().BeGreaterThan(0);
            }
        }

        [Fact]
        public async Task GetPersonsListAsync()
        {
            var options = CreateDbContextOptions();

            using (var context = new ApplicationDbContext(options))
            {
                context.Persons.Add(CreateValidPerson());
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var repository = new PersonRepository(context);

                var persons = await repository.GetPersonsListAsync();

                persons.Count().Should().BeGreaterThan(0);
            }
        }

        [Fact]
        public async Task UpdatePersonAsync()
        {
            var options = CreateDbContextOptions();
            Person person = CreateValidPerson();
            Person personFromRepository;
            var id = 0;

            // Create Person and save
            using (var context = new ApplicationDbContext(options))
            {
                context.Persons.Add(person);
                context.SaveChanges();
                id = person.Id;
            }

            // Get the saved Person, set their Birthday and save again
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new PersonRepository(context);
                personFromRepository = await repository.GetPersonEntityAsync(id);

                var personUpdateDto = new PersonUpdateDto
                {
                    Name = personFromRepository.Name,
                    Gender = personFromRepository.Gender,
                    Birthday = DateTime.Today.AddYears(-20)
                };

                PersonDtoHelper.PersonUpdateDtoToEntity(personUpdateDto, personFromRepository);

                repository.UpdatePersonAsync(personUpdateDto);

                await repository.SaveChangesAsync();
            }

            // Get the updated Person and Assert
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new PersonRepository(context);
                personFromRepository = await repository.GetPersonEntityAsync(id);

                personFromRepository.Birthday.Should().BeCloseTo(DateTime.Today.AddYears(-20), new TimeSpan(1, 0,0,0));
            }
        }

        [Fact(Skip = "Awaiting Implimentation")]
        [Trait("Category", "Experimental")]
        public void UpdatePersonGraph()
        {
            //var options = CreateDbContextOptions();
            //Person person;
            //Person personFromRepository;
            //PersonReadDto personFromRepositoryUpdated;
            //var id = 0;
            //var number = "(555) 555-5555";
            //var phoneType = PhoneType.Mobile;
            //var isPrimary = false;
            //var address = "a@b.c";

            //CreateAndSavePersonGraph(options, out person, out id);

            //// Get the Person from the database
            //using (var context = new ApplicationDbContext(options))
            //{
            //    var repository = new PersonRepository(context);
            //    personFromRepository = await repository.GetPersonEntityAsync(id);

            //    // Create an Update Dto with some changes for the Person
            //    var personUpdateDto = new PersonUpdateDto
            //    {
            //        Gender = personFromRepository.Gender,
            //        Name = personFromRepository.Name,
            //        Birthday = DateTime.Today.AddYears(-20),
            //        Phones = mapper.Map<List<PhoneUpdateDto>>(personFromRepository.Phones),
            //        Emails = mapper.Map<List<EmailUpdateDto>>(personFromRepository.Emails)
            //    };

            //    var phone = new PhoneUpdateDto(number, phoneType, isPrimary);

            //    var email = new EmailUpdateDto
            //    {
            //        Address = address,
            //        IsPrimary = isPrimary
            //    };

            //    personFromRepository.SetTrackingState(TrackingState.Modified);
            //    personUpdateDto.Phones.Add(phone);
            //    personUpdateDto.Emails.Add(email);

            //    // ACT
            //    mapper.Map<Person>(personUpdateDto);

            //    repository.FixTrackingState();


            //    repository.UpdatePersonAsync(personUpdateDto);

            //    await repository.SaveChangesAsync();
            //}

            //// Get the updated Person and Assert
            //using (var context = new ApplicationDbContext(options))
            //{
            //    var repository = new PersonRepository(context);
            //    personFromRepositoryUpdated = await repository.GetPersonAsync(id);

            //    personFromRepositoryUpdated.Birthday.Should().BeCloseTo(DateTime.Today.AddYears(-20));
            //    personFromRepositoryUpdated.Phones.Should().Contain(number);
            //}
        }


        [Fact]
        public async Task DeletePerson()
        {
            var options = CreateDbContextOptions();
            var id = 0;

            using (var context = new ApplicationDbContext(options))
            {
                var person = CreateValidPerson();
                context.Persons.Add(person);
                context.SaveChanges();
                id = person.Id;
            }

            using (var context = new ApplicationDbContext(options))
            {
                var repository = new PersonRepository(context);
                var personFromRepo = await repository.GetPersonEntityAsync(id);

                personFromRepo.Should().NotBeNull();

                // ACT
                repository.DeletePerson(personFromRepo);
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
            Action action = () => new PersonRepository(null);

            action.Should().Throw<ArgumentNullException>();
        }


        [Fact]
        public async Task ReturnTrueIfPersonExistsAsync()
        {
            var options = CreateDbContextOptions();
            var id = 0;

            using (var context = new ApplicationDbContext(options))
            {
                var person = CreateValidPerson();
                context.Persons.Add(person);
                context.SaveChanges();
                id = person.Id;
            }

            using (var context = new ApplicationDbContext(options))
            {
                var repository = new PersonRepository(context);
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
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;
            var person = CreateValidPerson();

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();
                context.Persons.Add(person);
                context.SaveChanges();

                Person savedPerson = context.Persons.FirstOrDefault(x => x.Id == person.Id);

                savedPerson.Should().NotBeNull();
            }
        }

    }
}
