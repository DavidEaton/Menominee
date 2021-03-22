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
using CustomerVehicleManagement.Api.Data.Dtos;

namespace CustomerVehicleManagement.Api.IntegrationTests.Repositories
{
    public class OrganizationsRepositoryShould
    {
        private static IMapper mapper;

        public OrganizationsRepositoryShould()
        {
            if (mapper == null)
            {
                var mapperConfiguration = new MapperConfiguration(configuration =>
                {
                    configuration.AddProfile(new OrganizationProfile());
                    configuration.AddProfile(new PersonProfile());
                    configuration.AddProfile(new EmailProfile());
                    configuration.AddProfile(new PhoneProfile());
                });

                mapper = mapperConfiguration.CreateMapper();
            }
        }
        [Fact]
        public async Task GetOrganizationsAsync()
        {
            // Arrange
            // Create an in-memory database
            var options = CreateDbContextContextOptions();

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
                var repository = new OrganizationRepository(context, mapper);

                // Act
                var organizations = (List<OrganizationReadDto>)await repository.GetOrganizationsAsync();

                // Assert
                organizations.Count.Should().BeGreaterThan(0);
            }
        }

        [Fact]
        public async Task GetOrganizationAsync()
        {
            var options = CreateDbContextContextOptions();
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
                var repository = new OrganizationRepository(context, mapper);

                var organizationFromRepo = await repository.GetOrganizationAsync(id);

                organizationFromRepo.Should().NotBeNull();
            }
        }

        [Fact]
        public async Task GetOrganizationsListAsync()
        {
            var options = CreateDbContextContextOptions();

            using (var context = new AppDbContext(options))
            {
                context.Organizations.Add(CreateValidOrganization());
                context.SaveChanges();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new OrganizationRepository(context, mapper);

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
            Action action = () => new OrganizationRepository(null, mapper);

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ThrowExceptionWhenPassedNullMapper()
        {
            var options = CreateDbContextContextOptions();

            using (var context = new AppDbContext(options))
            {
                Action action = () => new PersonRepository(context, null);
                action.Should().Throw<ArgumentNullException>();
            }
        }


        private static DbContextOptions<AppDbContext> CreateDbContextContextOptions()
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
