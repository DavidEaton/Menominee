using AutoMapper;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Api.Profiles;
using Xunit;
using System.Threading.Tasks;
using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Api.Data.Repositories;
using FluentAssertions;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using CustomerVehicleManagement.Api.Data.Models;

namespace CustomerVehicleManagement.Api.IntegrationTests.Repositories
{
    public class OrganizationsRepositoryShould
    {
        [Fact]
        public async Task GetOrganizationsAsync()
        {
            // Arrange
            // Create an in-memory database
            var options = CreateContextOptions();

            // Due to the disconnected nature of ASP.NET Core,
            // our tests create and use unique contexts for each
            // request, like our production code does.
            // Add a Organization to the in-memory database
            using (var context = new AppDbContext(options))
            {
                context.Organizations.Add(CreateValidOrganization());
                context.SaveChanges();
            }
            // Read all Organizations from the in-memory database
            using (var context = new AppDbContext(options))
            {
                var repository = new OrganizationRepository(context);

                // Act
                var organizations = (List<OrganizationReadDto>)await repository.GetOrganizationsAsync();

                // Assert
                organizations.Count.Should().BeGreaterThan(0);
            }
        }

        [Fact]
        public async Task AddAndGetOrganizationAsync()
        {
            var options = CreateContextOptions();
            var id = 0;

            using (var context = new AppDbContext(options))
            {
                var organization = CreateValidOrganization();
                context.Organizations.Add(organization);
                context.SaveChanges();
                id = organization.Id;
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new OrganizationRepository(context);

                var organizationFromRepo = await repository.GetOrganizationAsync(id);

                organizationFromRepo.Should().NotBeNull();
            }
        }

        [Fact]
        public async Task GetOrganizationsListAsync()
        {
            var options = CreateContextOptions();

            using (var context = new AppDbContext(options))
            {
                context.Organizations.Add(CreateValidOrganization());
                context.SaveChanges();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new OrganizationRepository(context);

                var organizations = await repository.GetOrganizationsListAsync();

                organizations.Count().Should().BeGreaterThan(0);
            }
        }

        [Fact]
        public void SaveChangesAsync()
        {

        }

        [Fact]
        public void UpdateOrganizationAsync()
        {

        }

        [Fact]
        public void DeleteOrganization()
        {

        }

        [Fact]
        public void FixState()
        {

        }

        [Fact]
        public void ThrowExceptionWhenPassedNullContext()
        {
            Action action = () => new OrganizationRepository(null);

            action.Should().Throw<ArgumentNullException>();
        }

        private static DbContextOptions<AppDbContext> CreateContextOptions()
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase($"testdb{Guid.NewGuid()}")
                .Options;
        }

        private static Organization CreateValidOrganization()
        {
            var name = "jane's";
            var organization = new Organization(name);

            return organization;
        }
    }
}
