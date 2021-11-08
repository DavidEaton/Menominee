﻿using CustomerVehicleManagement.Api.Customers;
using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities;
using FluentAssertions;
using SharedKernel.Enums;
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

        [Fact]
        public async Task GetPersonCustomerAsync()
        {
            var options = CreateDbContextOptions();
            using (var context = new ApplicationDbContext(options))
            {
                var person = Helper.CreateValidPerson();
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
                var organization = Helper.CreateValidOrganization();
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
                var organization = Helper.CreateValidOrganization();
                await context.AddAsync(organization);

                var note = "notes are strings";

                var contact = Helper.CreateValidPerson();
                contact.SetEmails(Helper.CreateValidEmails());
                contact.SetPhones(Helper.CreateValidPhones());

                organization.SetEmails(Helper.CreateValidEmails());
                organization.SetPhones(Helper.CreateValidPhones());
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
                customersFromRepo[0].ContactName.Should().NotBeNullOrEmpty();
                customersFromRepo[0].ContactPrimaryPhone.Should().NotBeNullOrEmpty();
                customersFromRepo[0].ContactPrimaryPhoneType.Should().NotBeNullOrEmpty();
            }
        }
    }

}