using CustomerVehicleManagement.Api.Customers;
using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using FluentAssertions;
using Menominee.Common.Enums;
using System.Threading.Tasks;
using Xunit;
using static CustomerVehicleManagement.Api.IntegrationTests.Helpers.TestUtilities;
using Helper = CustomerVehicleManagement.Shared.TestUtilities.Utilities;

namespace CustomerVehicleManagement.Api.IntegrationTests.Repositories
{
    /// <summary>
    /// Repository Tests UseInMemoryDatabase($"testdb{Guid.NewGuid()}").
    /// </summary>
    public class CustomerRepositoryShould
    {

        public CustomerRepositoryShould()
        {

        }

        //[Fact]
        //public async Task AddPersonCustomerAsync()
        //{
        //    var options = CreateDbContextOptions();

        //    using (var context = new ApplicationDbContext(options))
        //    {
        //        // Arrange
        //        var repository = new CustomerRepository(context);
        //        var personToWrite = new PersonToWrite()
        //        {
        //            Name = new PersonNameToWrite()
        //            {
        //                LastName = "Moops",
        //                FirstName = "Molly"
        //            },
        //            Gender = Gender.Female
        //        };

        //        var customerToWrite = new CustomerToWrite()
        //        {
        //            CustomerType = CustomerType.Retail,
        //            EntityType = EntityType.Person,
        //            Person = personToWrite
        //        };

        //        var customer = new Customer()

        //        // Act
        //        await repository.AddCustomerAsync(customerToWrite);
        //        await repository.SaveChangesAsync();
        //        var Customers = await repository.GetCustomersAsync();

        //        // Assert
        //        Customers.Count().Should().BeGreaterThan(0);
        //    }
        //}

        //[Fact]
        //public async Task AddOrganizationCustomerAsync()
        //{
        //    var options = CreateDbContextOptions();

        //    using (var context = new ApplicationDbContext(options))
        //    {
        //        var repository = new CustomerRepository(context, mapper);

        //        var organizationToWrite = new OrganizationToWrite("Jane's Automotive");
        //        var customerToWrite = new CustomerToWrite(null, organizationToWrite, CustomerType.Retail);

        //        await repository.CustomerAsync(customerToWrite);
        //        await repository.SaveChangesAsync();
        //        var Customers = await repository.GetCustomersAsync();

        //        Customers.Count().Should().BeGreaterThan(0);
        //    }
        //}

        [Fact]
        public async Task GetPersonCustomerAsync()
        {
            var options = CreateDbContextOptions();
            using (var context = new ApplicationDbContext(options))
            {
                var person = Helper.CreatePerson();
                await context.AddAsync(person);

                if ((await context.SaveChangesAsync()) > 0)
                {
                    Customer customer = new(person, CustomerType.Retail);
                    if (customer != null)
                        await context.AddAsync(customer);
                    await context.SaveChangesAsync();
                    var repository = new CustomerRepository(context);
                    var customerFromRepo = await repository.GetCustomerAsync(customer.Id);

                    customerFromRepo.Should().NotBeNull();
                    customerFromRepo.Id.Should().BeGreaterThan(0);
                }
            }
        }

        [Fact]
        public async Task GetOrganizationCustomerAsync()
        {
            var options = CreateDbContextOptions();
            using (var context = new ApplicationDbContext(options))
            {
                var organization = Helper.CreateOrganization();
                await context.AddAsync(organization);

                if ((await context.SaveChangesAsync()) > 0)
                {
                    Customer customer = new(organization);
                    if (customer != null)
                        await context.AddAsync(customer);
                    await context.SaveChangesAsync();
                    var repository = new CustomerRepository(context);
                    var customerFromRepo = await repository.GetCustomerAsync(customer.Id);

                    customerFromRepo.Should().NotBeNull();
                    customerFromRepo.Id.Should().BeGreaterThan(0);
                }
            }
        }

        [Fact]
        public async Task GetCustomersInListAsync()
        {
            var options = CreateDbContextOptions();
            using (var context = new ApplicationDbContext(options))
            {
                var organization = Helper.CreateOrganization();
                await context.AddAsync(organization);

                Customer customer = new(organization);
                await context.AddAsync(customer);
                await context.SaveChangesAsync();

                var repository = new CustomerRepository(context);
                var customersFromRepo = await repository.GetCustomersInListAsync();

                customersFromRepo.Count.Should().BeGreaterThan(0);
            }
        }
    }

}