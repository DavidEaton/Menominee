using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Helper = CustomerVehicleManagement.Shared.TestUtilities.Utilities;

namespace CustomerVehicleManagement.Api.IntegrationTests.Helpers
{
    public static class Utilities
    {
        public static void InitializeDbForTests(ApplicationDbContext db)
        {
            Task<bool> personsHasRows = db.Persons.AnyAsync();

            if (!personsHasRows.Result)
            {
                db.Persons.AddRange(GetSeedPersons());
                db.SaveChanges();
            }

        }

        public static void ReinitializeDbForTests(ApplicationDbContext db)
        {
            db.Persons.RemoveRange(db.Persons);
            InitializeDbForTests(db);
        }

        public static List<Person> GetSeedPersons()
        {
            return new List<Person>()
            {
                new Person(PersonName.Create("Smith", "Jane").Value, Gender.Female),
                new Person(PersonName.Create("Jones", "Latisha").Value, Gender.Female),
                new Person(PersonName.Create("Lee", "Wong").Value, Gender.Male),
                new Person(PersonName.Create("Kelly", "Junice").Value, Gender.Female),
            };
        }

        public static DbContextOptions<ApplicationDbContext> CreateDbContextOptions()
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase($"testdb{Guid.NewGuid()}")
                .Options;
        }


        public static void CreateAndSaveValidOrganizationCustomer(DbContextOptions<ApplicationDbContext> options, out Customer customer, out long id)
        {
            // Create a new Person with Emails and Phones, and save
            using (var context = new ApplicationDbContext(options))
            {
                customer = Helper.CreateValidOrganizationCustomer();
                context.Customers.Add(customer);
                context.SaveChanges();
                id = customer.Id;
            }
        }

        public static long CreateAndSaveValidOrganizationId(DbContextOptions<ApplicationDbContext> options)
        {
            long id;
            using (var context = new ApplicationDbContext(options))
            {
                Organization organization = Helper.CreateValidOrganization();
                context.Organizations.Add(organization);
                context.SaveChanges();
                id = organization.Id;
            }

            return id;
        }

        public static Organization CreateAndSaveValidOrganization(DbContextOptions<ApplicationDbContext> options)
        {
            Organization organization;
            using (var context = new ApplicationDbContext(options))
            {
                organization = Helper.CreateValidOrganization();
                context.Organizations.Add(organization);
                context.SaveChanges();
            }

            return organization;
        }


        public static void CreateAndSavePersonGraph(DbContextOptions<ApplicationDbContext> options, out Person person, out long id)
        {
            // Create a new Person with Emails and Phones, and save
            using (var context = new ApplicationDbContext(options))
            {
                person = Helper.CreateValidPersonWithEmails();
                person.SetPhones(Helper.CreateValidPhones());
                context.Persons.Add(person);
                context.SaveChanges();
                id = person.Id;
            }
        }

    }
}
