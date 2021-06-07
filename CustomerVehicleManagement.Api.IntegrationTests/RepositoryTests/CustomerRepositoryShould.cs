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
        private static IMapper mapper;
        public CustomerRepositoryShould()
        {
            if (mapper == null)
            {
                var mapperConfiguration = new MapperConfiguration(configuration =>
                {
                    //configuration.AddProfile(new PersonProfile());
                    //configuration.AddProfile(new EmailProfile());
                    //configuration.AddProfile(new PhoneProfile());
                    configuration.AddProfile(new CustomerProfile());
                    configuration.AddProfile(new OrganizationProfile());
                });

                mapper = mapperConfiguration.CreateMapper();
            }
        }

        [Fact]
        public async Task AddAndSavePersonCustomerAsync()
        {
            var options = CreateDbContextOptions();

            using (var context = new AppDbContext(options))
            {
                // Arrange
                var repository = new CustomerRepository(context, mapper);
                var personCreateDto = new PersonCreateDto(new PersonName("Moops", "Molly"), Gender.Female);
                var customerCreateDto = new CustomerCreateDto(personCreateDto, null, CustomerType.Retail);

                // Act
                await repository.AddAndSaveCustomerAsync(customerCreateDto);
                await repository.SaveChangesAsync();
                var Customers = await repository.GetCustomersAsync();

                // Assert
                Customers.Count().Should().BeGreaterThan(0);
            }
        }

        [Fact]
        public async Task AddAndSaveOrganizationCustomerAsync()
        {
            var options = CreateDbContextOptions();

            using (var context = new AppDbContext(options))
            {
                var repository = new CustomerRepository(context, mapper);

                var organizationCreateDto = new OrganizationCreateDto("Jane's Automotive");
                var customerCreateDto = new CustomerCreateDto(null, organizationCreateDto, CustomerType.Retail);

                await repository.AddAndSaveCustomerAsync(customerCreateDto);
                await repository.SaveChangesAsync();
                var Customers = await repository.GetCustomersAsync();

                Customers.Count().Should().BeGreaterThan(0);
            }
        }

        [Fact]
        public async Task GetPersonCustomerAsync()
        {
            var options = CreateDbContextOptions();
            using (var context = new AppDbContext(options))
            {
                var person = CreateValidPerson();
                await context.AddAsync(person);

                if ((await context.SaveChangesAsync()) > 0)
                {
                    Customer customer = new(person);
                    if (customer != null)
                        await context.AddAsync(customer);
                    await context.SaveChangesAsync();
                    var repository = new CustomerRepository(context, mapper);
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
            using (var context = new AppDbContext(options))
            {
                var organization = CreateValidOrganization();
                await context.AddAsync(organization);

                if ((await context.SaveChangesAsync()) > 0)
                {
                    Customer customer = new(organization);
                    if (customer != null)
                        await context.AddAsync(customer);
                    await context.SaveChangesAsync();
                    var repository = new CustomerRepository(context, mapper);
                    var customerFromRepo = await repository.GetCustomerAsync(customer.Id);

                    customerFromRepo.Should().NotBeNull();
                    customerFromRepo.Id.Should().BeGreaterThan(0);
                }
            }
        }
    }
}