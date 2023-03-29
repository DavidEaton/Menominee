using CustomerVehicleManagement.Api.Customers;
using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Tests.Unit.Helpers;
using FluentAssertions;
using Menominee.Common.Enums;
using System.Threading.Tasks;
using Xunit;
using static CustomerVehicleManagement.Api.IntegrationTests.Helpers.TestUtilities;
using static CustomerVehicleManagement.Tests.Utilities;

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

        [Fact]
        public async Task GetPersonCustomerAsync()
        {
            var options = CreateDbContextOptions();
            using (var context = new ApplicationDbContext(options))
            {
                var person = ContactableTestHelper.CreatePerson();
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
                var organization = ContactableTestHelper.CreateOrganization();
                await context.AddAsync(organization);

                if ((await context.SaveChangesAsync()) > 0)
                {
                    Customer customer = new(organization, CustomerType.Retail);
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
                var organization = ContactableTestHelper.CreateOrganization();
                await context.AddAsync(organization);

                Customer customer = new(organization, CustomerType.Retail);
                await context.AddAsync(customer);
                await context.SaveChangesAsync();

                var repository = new CustomerRepository(context);
                var customersFromRepo = await repository.GetCustomersInListAsync();

                customersFromRepo.Count.Should().BeGreaterThan(0);
            }
        }
    }

}