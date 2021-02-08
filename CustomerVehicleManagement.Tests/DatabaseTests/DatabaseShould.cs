using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SharedKernel.Enums;
using SharedKernel.ValueObjects;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Tests.DatabaseTests
{
    [Category("Database")]
    public class DatabaseShould
    {
        const string CONNECTION = "Server=localhost;Database=MenomineeTest;Trusted_Connection=True;";

        [SetUp]
        public void Setup()
        {
        }

        [TearDown]
        public void TearDown()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(CONNECTION);
            var mockConfiguration = new Mock<IConfiguration>();
            var mockLogger = new Mock<ILogger<AppDbContext>>();
            var mockEnvironment = new Mock<IHostEnvironment>();
            mockEnvironment
                   .Setup(e => e.EnvironmentName)
                   .Returns("Hosting:UnitTestEnvironment");
            var context = new AppDbContext(CONNECTION, true);
            context.Database.EnsureDeleted();

        }

        [Test]
        public void InsertPersonIntoDatabase()
        {
            // Arrange
            AppDbContext context = CreateTestContext();

            var firstName = "Kivas";
            var lastName = "Fajo";

            // Act
            var name = new PersonName(lastName, firstName);
            var person = new Person(name, Gender.Female);
            var efDefaultId = person.Id;

            context.Persons.Add(person);
            context.SaveChanges();

            // Assert
            Assert.AreNotEqual(efDefaultId, person.Id);
            Assert.AreEqual(lastName, person.Name.LastName);
            Assert.AreEqual(firstName, person.Name.FirstName);
        }

        [Test]
        public async Task GetPersonFromDatabase()
        {
            int id = 0;
            AppDbContext context = CreateTestContext();
            var firstName = "Tasha";
            var lastName = "Yar";
            var name = new PersonName(lastName, firstName);
            var person = new Person(name, Gender.Female);

            context.Persons.Add(person);
            context.SaveChanges();

            id = person.Id;

            person = await context.Persons
                .FirstOrDefaultAsync(p => p.Id == id);

            Assert.AreEqual(lastName, person.Name.LastName);
            Assert.AreEqual(firstName, person.Name.FirstName);
        }

        [Test]
        public async Task UpdatePersonInDatabase()
        {
            int id = 0;
            AppDbContext context = CreateTestContext();

            var firstName = "Tasha";
            var lastName = "Yar";
            var name = new PersonName(lastName, firstName);
            var person = new Person(name, Gender.Female);

            context.Persons.Add(person);
            context.SaveChanges();

            id = person.Id;

            // Clear person from memory
            person = null;

            // Get new person from mdatabase
            person = await context.Persons
                .FirstOrDefaultAsync(p => p.Id == id);

            Assert.AreEqual(firstName, person.Name.FirstName);
            Assert.AreEqual(lastName, person.Name.LastName);

            var nameNew = new PersonName("Smith", firstName);
            person.SetName(nameNew);

            context.Update(person);
            context.SaveChanges();

            // Clear person from memory
            person = null;

            // Get updated person from mdatabase
            person = await context.Persons
                .FirstOrDefaultAsync(p => p.Id == id);

            Assert.AreEqual("Smith", person.Name.LastName);
        }

        [Test]
        public async Task DeletePersonFromDatabase()
        {
            int id = 0;
            AppDbContext context = CreateTestContext();

            var firstName = "Dianna";
            var lastName = "Troy";
            var name = new PersonName(lastName, firstName);
            var person = new Person(name, Gender.Female);

            context.Persons.Add(person);
            context.SaveChanges();

            id = person.Id;

            person = await context.Persons
                .FirstOrDefaultAsync(p => p.Id == id);

            // Confirm that the person was created in the database
            Assert.AreEqual(firstName, person.Name.FirstName);
            Assert.AreEqual(lastName, person.Name.LastName);

            // Delete person
            context.Persons.Remove(person);
            context.SaveChanges();

            // Try to get person again
            person = await context.Persons
                .FirstOrDefaultAsync(p => p.Id == id);

            // Confirm that the person was deleted from the database
            Assert.AreEqual(person, null);
        }
        private static AppDbContext CreateTestContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(CONNECTION);
            var mockConfiguration = new Mock<IConfiguration>();
            var mockLogger = new Mock<ILogger<AppDbContext>>();
            var mockEnvironment = new Mock<IHostEnvironment>();
            mockEnvironment
                   .Setup(e => e.EnvironmentName)
                   .Returns("Hosting:UnitTestEnvironment");
            var context = new AppDbContext(CONNECTION, true);

            // Test database in known state
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            return context;
        }
    }
}
