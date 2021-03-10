using AutoMapper;
using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Api.Data.Models;
using CustomerVehicleManagement.Api.Data.Repositories;
using CustomerVehicleManagement.Api.Profiles;
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
        public async Task AddAndGetPersonAsync()
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
        public void SaveChangesAsync()
        {

        }

        [Fact]
        public void UpdatePersonAsync()
        {

        }

        [Fact]
        public void DeletePerson()
        {

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
        public void SqliteTest()
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
    }
}
