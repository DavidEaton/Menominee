using AutoMapper;
using CustomerVehicleManagement.Api.Customers;
using CustomerVehicleManagement.Api.Organizations;
using CustomerVehicleManagement.Api.Persons;
using CustomerVehicleManagement.Domain.Entities;
using FluentAssertions;
using SharedKernel.Enums;
using SharedKernel.ValueObjects;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static CustomerVehicleManagement.Api.IntegrationTests.Helpers.Utilities;

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
        //        var repository = new CustomerRepository(context, mapper);
        //        var personCreateDto = new PersonCreateDto(new PersonName("Moops", "Molly"), Gender.Female);
        //        var customerCreateDto = new CustomerCreateDto(personCreateDto, null, CustomerType.Retail);
        //        var customer = new Customer(customerCreateDto.PersonCreateDto)

        //        // Act
        //        await repository.AddCustomerAsync(customerCreateDto);
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

        //        var organizationCreateDto = new OrganizationCreateDto("Jane's Automotive");
        //        var customerCreateDto = new CustomerCreateDto(null, organizationCreateDto, CustomerType.Retail);

        //        await repository.CustomerAsync(customerCreateDto);
        //        await repository.SaveChangesAsync();
        //        var Customers = await repository.GetCustomersAsync();

        //        Customers.Count().Should().BeGreaterThan(0);
        //    }
        //}

        [Fact]
        public async Task GetPersonCustomerAsync()
        {
            var options = CreateDbContextOptions();
            using (var context = new ApplicationDbContext(options, null, null, null, null))
            {
                var person = CreateValidPerson();
                await context.AddAsync(person);

                if ((await context.SaveChangesAsync()) > 0)
                {
                    Customer customer = new(person);
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
            using (var context = new ApplicationDbContext(options, null, null, null, null))
            {
                var organization = CreateValidOrganization();
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
    }
}