using AutoMapper;
using CustomerVehicleManagement.Api.Profiles;
using Xunit;
using System.Threading.Tasks;
using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Api.Data.Repositories;
using FluentAssertions;
using System;
using System.Linq;
using System.Collections.Generic;
using CustomerVehicleManagement.Api.Data.Dtos;
using SharedKernel.ValueObjects;
using Microsoft.EntityFrameworkCore;

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
        public async Task AddOrganizationAsync()
        {
            var options = Helpers.CreateDbContextOptions();

            using (var context = new AppDbContext(options))
            {
                // Arrange
                var repository = new OrganizationRepository(context, mapper);
                var organization = Helpers.CreateValidOrganization();

                // Act
                await repository.AddOrganizationAsync(mapper.Map<OrganizationCreateDto>(organization));
                await repository.SaveChangesAsync();
                var persons = await repository.GetOrganizationsAsync();

                // Assert
                persons.Count().Should().Be(1);
            }
        }

        [Fact]
        public async Task GetOrganizationsAsync()
        {
            // Arrange

            // Create an in-memory database
            var options = Helpers.CreateDbContextOptions();

            // Due to the disconnected nature of web requests/responses,
            // our tests create and use unique contexts for each
            // request, like our production code does.

            // Add a Organization to the in-memory database
            using (var context = new AppDbContext(options))
            {
                context.Organizations.Add(Helpers.CreateValidOrganization());
                context.SaveChanges();
            }
            // Read all Organizations from the in-memory database
            using (var context = new AppDbContext(options))
            {
                var repository = new OrganizationRepository(context, mapper);

                // Act
                var organizations = (List<OrganizationReadDto>)await repository.GetOrganizationsAsync();

                // Assert
                organizations.Count.Should().Be(1);
            }
        }

        [Fact]
        public async Task GetOrganizationAsync()
        {
            var options = Helpers.CreateDbContextOptions();
            var id = CreateAndSaveOrganization(options);

            using (var context = new AppDbContext(options))
            {
                var repository = new OrganizationRepository(context, mapper);

                var organizationFromRepository = await repository.GetOrganizationAsync(id);

                organizationFromRepository.Should().NotBeNull();
            }
        }

        [Fact]
        public async Task GetOrganizationEntityAsync()
        {
            var options = Helpers.CreateDbContextOptions();
            var id = CreateAndSaveOrganization(options);

            using (var context = new AppDbContext(options))
            {
                var repository = new OrganizationRepository(context, mapper);

                var organizationFromRepository = await repository.GetOrganizationEntityAsync(id);

                organizationFromRepository.Should().NotBeNull();
            }
        }

        [Fact]
        public async Task GetOrganizationAsyncIncludesCollection()
        {
            var someNotes = "Some notes";
            var options = Helpers.CreateDbContextOptions();
            var id = CreateAndSaveOrganization(options);

            using (var context = new AppDbContext(options))
            {
                var repository = new OrganizationRepository(context, mapper);
                var organizationFromRepository = await repository.GetOrganizationEntityAsync(id);

                organizationFromRepository.SetNotes(someNotes);
                organizationFromRepository.SetAddress(Helpers.CreateValidAddress());
                organizationFromRepository.SetContact(Helpers.CreateValidPerson());
                organizationFromRepository.SetPhones(Helpers.CreateValidPhones());
                organizationFromRepository.SetEmails(Helpers.CreateValidEmails());

                repository.UpdateOrganizationAsync(organizationFromRepository);

                await repository.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new OrganizationRepository(context, mapper);
                var organizationFromRepository = await repository.GetOrganizationAsync(id);

                organizationFromRepository.Notes.Should().Be(someNotes);
                organizationFromRepository.AddressLine.Should().NotBeNull();
                organizationFromRepository.City.Should().NotBeNull();
                organizationFromRepository.State.Should().NotBeNull();
                organizationFromRepository.PostalCode.Should().NotBeNull();
                organizationFromRepository.AddressFull.Should().NotBeNull();
                organizationFromRepository.Phones.Count().Should().BeGreaterThan(0);
                organizationFromRepository.Emails.Count().Should().BeGreaterThan(0);
                organizationFromRepository.Contact.Should().NotBeNull();
            };
        }

        [Fact]
        public async Task GetOrganizationAsyncIncludesContact()
        {
            var options = Helpers.CreateDbContextOptions();
            var id = CreateAndSaveOrganization(options);

            using (var context = new AppDbContext(options))
            {
                var repository = new OrganizationRepository(context, mapper);
                var organizationFromRepository = await repository.GetOrganizationEntityAsync(id);

                organizationFromRepository.SetContact(Helpers.CreateValidPerson());

                repository.UpdateOrganizationAsync(organizationFromRepository);

                await repository.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new OrganizationRepository(context, mapper);
                var organizationFromRepository = await repository.GetOrganizationAsync(id);

                organizationFromRepository.Contact.Should().NotBeNull();
            };
        }

        [Fact]
        public async Task GetOrganizationAsyncIncludesContactCollections()
        {
            var options = Helpers.CreateDbContextOptions();
            var id = CreateAndSaveOrganization(options);

            using (var context = new AppDbContext(options))
            {
                var repository = new OrganizationRepository(context, mapper);
                var organizationFromRepository = await repository.GetOrganizationEntityAsync(id);

                organizationFromRepository.SetContact(Helpers.CreateValidPerson());
                organizationFromRepository.Contact.SetEmails(Helpers.CreateValidEmails());
                organizationFromRepository.Contact.SetPhones(Helpers.CreateValidPhones());

                repository.UpdateOrganizationAsync(organizationFromRepository);

                await repository.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new OrganizationRepository(context, mapper);
                var organizationFromRepository = await repository.GetOrganizationAsync(id);

                organizationFromRepository.Contact.Should().NotBeNull();
                organizationFromRepository.Contact.Phones.Count().Should().BeGreaterThan(0);
                organizationFromRepository.Contact.Emails.Count().Should().BeGreaterThan(0);
            };
        }

        [Fact]
        public async Task GetOrganizationsListAsync()
        {
            var options = Helpers.CreateDbContextOptions();

            using (var context = new AppDbContext(options))
            {
                context.Organizations.Add(Helpers.CreateValidOrganization());
                context.SaveChanges();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new OrganizationRepository(context, mapper);
                var organizations = await repository.GetOrganizationsListAsync();

                organizations.Count().Should().Be(1);
            }
        }

        [Fact]
        public async Task Return_True_When_OrganizationExistsAsync()
        {
            var options = Helpers.CreateDbContextOptions();
            var id = CreateAndSaveOrganization(options);

            using (var context = new AppDbContext(options))
            {
                var repository = new OrganizationRepository(context, mapper);
                var organizationExists = await repository.OrganizationExistsAsync(id);

                organizationExists.Should().Be(true);
            }
        }

        [Fact]
        public async Task Return_False_When_OrganizationExistsAsync()
        {
            var options = Helpers.CreateDbContextOptions();
            var id = 99999;

            using (var context = new AppDbContext(options))
            {
                var repository = new OrganizationRepository(context, mapper);
                var organizationExists = await repository.OrganizationExistsAsync(id);

                organizationExists.Should().Be(false);
            }
        }

        [Fact]
        public async Task UpdateOrganizationNameAsync()
        {
            var options = Helpers.CreateDbContextOptions();
            var newName = "New Name";
            var id = CreateAndSaveOrganization(options);

            using (var context = new AppDbContext(options))
            {
                var repository = new OrganizationRepository(context, mapper);
                var organizationFromRepo = await repository.GetOrganizationEntityAsync(id);

                organizationFromRepo.SetName(OrganizationName.Create(newName).Value);
                repository.UpdateOrganizationAsync(organizationFromRepo);

                await repository.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new OrganizationRepository(context, mapper);
                var organizationFromRepo = await repository.GetOrganizationAsync(id);

                organizationFromRepo.Name.Should().Be(newName);
            };
        }

        [Fact]
        public async Task UpdateOrganizationNotesAsync()
        {
            var options = Helpers.CreateDbContextOptions();
            var id = 0;
            var someNotes = "Some notes";
            var newNotes = "New notes";

            using (var context = new AppDbContext(options))
            {
                var organization = Helpers.CreateValidOrganization();
                organization.SetNotes(someNotes);
                context.Organizations.Add(organization);
                context.SaveChanges();
                id = organization.Id;
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new OrganizationRepository(context, mapper);
                var organizationFromRepo = await repository.GetOrganizationEntityAsync(id);
                organizationFromRepo.Notes.Should().Be(someNotes);

                organizationFromRepo.SetNotes(newNotes);
                repository.UpdateOrganizationAsync(organizationFromRepo);

                await repository.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new OrganizationRepository(context, mapper);
                var organizationFromRepo = await repository.GetOrganizationAsync(id);

                organizationFromRepo.Notes.Should().Be(newNotes);
            };
        }

        [Fact]
        public async Task UpdateOrganizationAddressAsync()
        {
            var options = Helpers.CreateDbContextOptions();
            var id = CreateAndSaveOrganization(options);

            using (var context = new AppDbContext(options))
            {
                var repository = new OrganizationRepository(context, mapper);
                var organizationFromRepo = await repository.GetOrganizationEntityAsync(id);
                organizationFromRepo.Address.Should().BeNull();

                organizationFromRepo.SetAddress(Helpers.CreateValidAddress());

                repository.UpdateOrganizationAsync(organizationFromRepo);

                await repository.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new OrganizationRepository(context, mapper);
                OrganizationReadDto organizationFromRepo = await repository.GetOrganizationAsync(id);

                organizationFromRepo.AddressLine.Should().Be("1234 Fifth Ave.");
            };
        }

        [Fact]
        public async Task UpdateOrganizationContactAsync()
        {
            var options = Helpers.CreateDbContextOptions();
            var id = CreateAndSaveOrganization(options);

            using (var context = new AppDbContext(options))
            {
                var repository = new OrganizationRepository(context, mapper);
                var organizationFromRepo = await repository.GetOrganizationEntityAsync(id);
                organizationFromRepo.Contact.Should().BeNull();

                organizationFromRepo.SetContact(Helpers.CreateValidPerson());

                repository.UpdateOrganizationAsync(organizationFromRepo);

                await repository.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new OrganizationRepository(context, mapper);
                OrganizationReadDto organizationFromRepo = await repository.GetOrganizationAsync(id);

                organizationFromRepo.Contact.Should().NotBeNull();
            };
        }

        [Fact]
        public async Task DeleteOrganizationAsync()
        {
            var options = Helpers.CreateDbContextOptions();
            var id = CreateAndSaveOrganization(options);

            using (var context = new AppDbContext(options))
            {
                var repository = new OrganizationRepository(context, mapper);
                var organizationFromRepo = await repository.GetOrganizationEntityAsync(id);
                organizationFromRepo.Should().NotBeNull();

                repository.DeleteOrganization(organizationFromRepo);
                await repository.SaveChangesAsync();
                organizationFromRepo = await repository.GetOrganizationEntityAsync(id);

                organizationFromRepo.Should().BeNull();
            }
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
            var options = Helpers.CreateDbContextOptions();

            using (var context = new AppDbContext(options))
            {
                Action action = () => new OrganizationRepository(context, null);
                action.Should().Throw<ArgumentNullException>();
            }
        }


        private static int CreateAndSaveOrganization(DbContextOptions<AppDbContext> options)
        {
            int id;
            using (var context = new AppDbContext(options))
            {
                var organization = Helpers.CreateValidOrganization();
                context.Organizations.Add(organization);
                context.SaveChanges();
                id = organization.Id;
            }

            return id;
        }
    }
}
