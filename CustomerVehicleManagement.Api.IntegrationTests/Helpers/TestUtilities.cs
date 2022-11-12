using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CustomerVehicleManagement.Tests.Utilities;

namespace CustomerVehicleManagement.Api.IntegrationTests.Helpers
{
    public static class TestUtilities
    {
        public static void InitializeDbForTests(ApplicationDbContext db)
        {
            Task<bool> customersHasRows = db.Customers.AnyAsync();

            if (!customersHasRows.Result)
            {
                db.Customers.AddRange(SeedCustomers());
                db.Persons.AddRange(SeedPersons());
                db.Organizations.AddRange(SeedOrganizations());
                db.SaveChanges();
            }

        }

        private static List<Organization> SeedOrganizations()
        {
            return new List<Organization>()
            {
                Organization.Create(OrganizationName.Create("Koops, Inc.").Value, null, null).Value,
                Organization.Create(OrganizationName.Create("Loops, Intl.").Value, null, null).Value,
                Organization.Create(OrganizationName.Create("Noops Brothers").Value, null, null).Value,
            };
        }

        public static List<Person> SeedPersons()
        {
            return new List<Person>()
            {
                Person.Create(PersonName.Create("Smith", "Jane").Value, Gender.Female).Value,
                Person.Create(PersonName.Create("Jones", "Latisha").Value, Gender.Female).Value,
                Person.Create(PersonName.Create("Lee", "Wong").Value, Gender.Male).Value,
                Person.Create(PersonName.Create("Kelly", "Junice").Value, Gender.Female).Value,
            };
        }

        public static List<Customer> SeedCustomers()
        {
            return new List<Customer>()
            {
                new Customer(Person.Create(PersonName.Create("Smith", "Jane").Value, Gender.Female).Value, CustomerType.Retail),
                new Customer(Organization.Create(OrganizationName.Create("Moops & Co.").Value, null, null).Value, CustomerType.Retail)
            };
        }

        public static DbContextOptions<ApplicationDbContext> CreateDbContextOptions()
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase($"testdb{Guid.NewGuid()}")
                .Options;
        }

        public static long CreateAndSaveValidOrganizationId(DbContextOptions<ApplicationDbContext> options)
        {
            long id;
            using (var context = new ApplicationDbContext(options))
            {
                Organization organization = CreateTestOrganization();
                context.Organizations.Add(organization);
                context.SaveChanges();
                id = organization.Id;
            }

            return id;
        }
    }
}