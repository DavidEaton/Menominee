using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Api.Persons;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static CustomerVehicleManagement.Api.IntegrationTests.Helpers.TestUtilities;
using Helper = CustomerVehicleManagement.Shared.TestUtilities.Utilities;

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
                context.Persons.Add(Helper.CreatePerson());
                context.SaveChanges();
            }

            // Read all Persons from the in-memory database
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new PersonRepository(context);

                // Act
                var persons = (List<PersonToRead>)await repository.GetPersonsAsync();

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
                await repository.AddPersonAsync(Helper.CreatePerson());
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
            long id = 0;

            using (var context = new ApplicationDbContext(options))
            {
                var person = Helper.CreatePerson();
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
            long id = 0;

            using (var context = new ApplicationDbContext(options))
            {
                var person = Helper.CreatePersonWithPhones();
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
            long id = 0;

            using (var context = new ApplicationDbContext(options))
            {
                var person = Helper.CreatePersonWithEmails();
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
                context.Persons.Add(Helper.CreatePerson());
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
        public async Task DeletePerson()
        {
            var options = CreateDbContextOptions();
            long id = 0;

            using (var context = new ApplicationDbContext(options))
            {
                var person = Helper.CreatePerson();
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
            long id = 0;

            using (var context = new ApplicationDbContext(options))
            {
                var person = Helper.CreatePerson();
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
    }
}
