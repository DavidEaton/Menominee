using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using Microsoft.EntityFrameworkCore;
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
                new Organization(OrganizationName.Create("Koops, Inc.").Value, null, null),
                new Organization(OrganizationName.Create("Loops, Intl.").Value, null, null),
                new Organization(OrganizationName.Create("Noops Brothers").Value, null, null),
            };
        }

        public static List<Person> SeedPersons()
        {
            return new List<Person>()
            {
                new Person(PersonName.Create("Smith", "Jane").Value, Gender.Female, null, null, null),
                new Person(PersonName.Create("Jones", "Latisha").Value, Gender.Female, null, null, null),
                new Person(PersonName.Create("Lee", "Wong").Value, Gender.Male, null, null, null),
                new Person(PersonName.Create("Kelly", "Junice").Value, Gender.Female, null, null, null),
            };
        }

        public static List<Customer> SeedCustomers()
        {
            return new List<Customer>()
            {
                new Customer(new Person(PersonName.Create("Smith", "Jane").Value, Gender.Female, null, null, null), CustomerType.Retail),
                new Customer(new Organization(OrganizationName.Create("Moops & Co.").Value, null, null), CustomerType.Retail)
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
                customer = Helper.CreateOrganizationCustomer();
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
                Organization organization = Helper.CreateOrganization();
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
                organization = Helper.CreateOrganization();
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
                person = Helper.CreatePersonWithEmails();
                person.SetPhones(Helper.CreatePhoneList());
                context.Persons.Add(person);
                context.SaveChanges();
                id = person.Id;
            }
        }
    }
}
