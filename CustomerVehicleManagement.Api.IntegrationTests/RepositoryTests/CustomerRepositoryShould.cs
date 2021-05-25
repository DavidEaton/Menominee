using AutoMapper;
using CustomerVehicleManagement.Api.Customers;
using CustomerVehicleManagement.Api.Organizations;
using FluentAssertions;
using SharedKernel.Enums;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static CustomerVehicleManagement.Api.IntegrationTests.Helpers.Utilities;

namespace CustomerVehicleManagement.Api.IntegrationTests.Repositories
{
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
        public async Task AddCustomerAsync()
        {
            var options = CreateDbContextOptions();

            using (var context = new AppDbContext(options))
            {
                // Arrange
                var repository = new CustomerRepository(context, mapper);
                var organization = CreateAndSaveValidOrganization(options);
                var organizationReadDto = mapper.Map<OrganizationReadDto>(organization);
                var customerCreateDto = new CustomerCreateDto(null, organizationReadDto, CustomerType.Retail);

                // Act
                await repository.AddCustomerAsync(customerCreateDto);
                await repository.SaveChangesAsync();
                var Customers = await repository.GetCustomersAsync();

                // Assert
                Customers.Count().Should().BeGreaterThan(0);
            }
        }


        [Fact]
        public async Task GetCustomerAsync()
        {
            var options = CreateDbContextOptions();
            var id = 0;

            using (var context = new AppDbContext(options))
            {
                var customer = CreateValidOrganizationCustomer();
                context.Customers.Add(customer);
                context.SaveChanges();
                id = customer.Id;
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new CustomerRepository(context, mapper);

                var customerFromRepo = await repository.GetCustomerAsync(id);

                customerFromRepo.Should().NotBeNull();
            }
        }
    }
}
