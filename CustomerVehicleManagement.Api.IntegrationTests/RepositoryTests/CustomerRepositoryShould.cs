using CustomerVehicleManagement.Api.Customers;
using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities;
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
        //        var repository = new CustomerRepository(context, mapper);
        //        var personToWrite = new PersonToWrite(new PersonName("Moops", "Molly"), Gender.Female);
        //        var customerToWrite = new CustomerToWrite(personToWrite, null, CustomerType.Retail);
        //        var customer = new Customer(customerToWrite.PersonToWrite)

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

                var note = "notes are strings";

                var contact = Helper.CreatePerson();
                contact.SetEmails(Helper.CreateEmailList());
                contact.SetPhones(Helper.CreatePhoneList());

                organization.SetEmails(Helper.CreateEmailList());
                organization.SetPhones(Helper.CreatePhoneList());
                organization.SetNote(note);
                organization.SetContact(contact);

                Customer customer = new(organization);
                await context.AddAsync(customer);
                await context.SaveChangesAsync();

                var repository = new CustomerRepository(context);
                var customersFromRepo = await repository.GetCustomersInListAsync();

                customersFromRepo.Should().NotBeNull();
                customersFromRepo[0].Should().NotBeNull();
                customersFromRepo[0].EntityType.Should().Be(EntityType.Organization);
                customersFromRepo[0].Name.Should().NotBeNullOrEmpty();
                customersFromRepo[0].Note.Should().NotBeNullOrEmpty();
                customersFromRepo[0].PrimaryEmail.Should().NotBeNullOrEmpty();
                customersFromRepo[0].PrimaryPhone.Should().NotBeNullOrEmpty();
                customersFromRepo[0].PrimaryPhoneType.Should().NotBeNullOrEmpty();
                //customersFromRepo[0].ContactName.Should().NotBeNullOrEmpty();
                //customersFromRepo[0].ContactPrimaryPhone.Should().NotBeNullOrEmpty();
                //customersFromRepo[0].ContactPrimaryPhoneType.Should().NotBeNullOrEmpty();
            }
        }
    }

}