using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Enums;
using SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Helper = CustomerVehicleManagement.Shared.TestUtilities.Utilities;

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
                new Organization(OrganizationName.Create("Koops, Inc.").Value),
                new Organization(OrganizationName.Create("Loops, Intl.").Value),
                new Organization(OrganizationName.Create("Noops Brothers").Value),
            };
        }

        public static List<Person> SeedPersons()
        {
            return new List<Person>()
            {
                new Person(PersonName.Create("Jones", "Latisha").Value, Gender.Female),
                new Person(PersonName.Create("Lee", "Wong").Value, Gender.Male),
                new Person(PersonName.Create("Kelly", "Junice").Value, Gender.Female),
            };
        }

        public static List<Customer> SeedCustomers()
        {
            return new List<Customer>()
            {
                new Customer(new Person(PersonName.Create("Smith", "Jane").Value, Gender.Female)),
                new Customer(new Organization(OrganizationName.Create("Moops & Co.").Value))
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
