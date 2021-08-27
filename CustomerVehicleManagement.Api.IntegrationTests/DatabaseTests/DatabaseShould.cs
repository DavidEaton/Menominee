using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using SharedKernel.Enums;
using SharedKernel.ValueObjects;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xunit;

namespace CustomerVehicleManagement.Api.IntegrationTests.DatabaseTests
{
    [Category("Database")]
    public class DatabaseShould : IDisposable
    {
        private const string Connection = "Server=localhost;Database=MenomineeTest;Trusted_Connection=True;";

        [Fact]
        public void InsertPersonIntoDatabase()
        {
            // Arrange
            ApplicationDbContext context = CreateTestContext();

            var firstName = "Jane";
            var lastName = "Doe";

            // Act
            var name = PersonName.Create(lastName, firstName).Value;
            var person = new Person(name, Gender.Female);
            var efDefaultId = person.Id;

            context.Persons.Add(person);
            context.SaveChanges();

            // Assert
            person.Id.Should().NotBe(efDefaultId);
            person.Name.LastName.Should().Be(lastName);
            person.Name.FirstName.Should().Be(firstName);

            Dispose();
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task GetPersonFromDatabase()
        {
            var id = 0;
            ApplicationDbContext context = CreateTestContext();
            var firstName = "Tasha";
            var lastName = "Yar";
            var name = PersonName.Create(lastName, firstName).Value;
            var person = new Person(name, Gender.Female);

            context.Persons.Add(person);
            context.SaveChanges();

            id = person.Id;

            person = await context.Persons
                .FirstOrDefaultAsync(p => p.Id == id);

            person.Name.LastName.Should().Be(lastName);
            person.Name.FirstName.Should().Be(firstName);

            Dispose();
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task UpdatePersonInDatabase()
        {
            var id = 0;
            ApplicationDbContext context = CreateTestContext();

            var firstName = "Tasha";
            var lastName = "Yar";
            var name = PersonName.Create(lastName, firstName).Value;
            var person = new Person(name, Gender.Female);

            context.Persons.Add(person);
            context.SaveChanges();

            id = person.Id;

            // Clear person from memory
            person = null;

            // Get new person from mdatabase
            person = await context.Persons
                .FirstOrDefaultAsync(p => p.Id == id);

            person.Name.LastName.Should().Be(lastName);
            person.Name.FirstName.Should().Be(firstName);

            var nameNew = PersonName.Create("Smith", firstName).Value;
            person.SetName(nameNew);

            context.Update(person);
            context.SaveChanges();

            // Clear person from memory
            person = null;

            // Get updated person from mdatabase
            person = await context.Persons
                .FirstOrDefaultAsync(p => p.Id == id);

            person.Name.Should().Be(nameNew);

            Dispose();
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task DeletePersonFromDatabase()
        {
            var id = 0;
            ApplicationDbContext context = CreateTestContext();

            var firstName = "Dianna";
            var lastName = "Troy";
            var name = PersonName.Create(lastName, firstName).Value;
            var person = new Person(name, Gender.Female);

            context.Persons.Add(person);
            context.SaveChanges();

            id = person.Id;

            person = await context.Persons
                .FirstOrDefaultAsync(p => p.Id == id);

            // Confirm that the person was created in the database
            person.Name.LastName.Should().Be(lastName);
            person.Name.FirstName.Should().Be(firstName);

            // Delete person
            context.Persons.Remove(person);
            context.SaveChanges();

            // Try to get person again
            person = await context.Persons
                .FirstOrDefaultAsync(p => p.Id == id);

            // Confirm that the person was deleted from the database
            person.Should().BeNull();

            Dispose();
            GC.SuppressFinalize(this);
        }
        private static ApplicationDbContext CreateTestContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(Connection, null);
            var mockConfiguration = new Mock<IConfiguration>();
            var mockLogger = new Mock<ILogger<ApplicationDbContext>>();
            var mockEnvironment = new Mock<IHostEnvironment>();
            mockEnvironment
                   .Setup(e => e.EnvironmentName)
                   .Returns("Hosting:Testing");
            var context = new ApplicationDbContext(Connection);

            // Set test database to known state
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            return context;
        }

        public void Dispose()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(Connection);
            var mockConfiguration = new Mock<IConfiguration>();
            var mockLogger = new Mock<ILogger<ApplicationDbContext>>();
            var mockEnvironment = new Mock<IHostEnvironment>();
            mockEnvironment
                   .Setup(e => e.EnvironmentName)
                   .Returns("Hosting:UnitTestEnvironment");
            var context = new ApplicationDbContext(Connection);
            context.Database.EnsureDeleted();
            GC.SuppressFinalize(this);
        }

    }
}