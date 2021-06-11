using AutoMapper;
using CustomerVehicleManagement.Api.Emails;
using CustomerVehicleManagement.Api.Organizations;
using CustomerVehicleManagement.Api.Persons;
using CustomerVehicleManagement.Api.Phones;
using FluentAssertions;
using SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static CustomerVehicleManagement.Api.IntegrationTests.Helpers.Utilities;

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
            var options = CreateDbContextOptions();

            using (var context = new AppDbContext(options))
            {
                // Arrange
                var repository = new OrganizationRepository(context, mapper);
                var organization = CreateValidOrganization();

                // Act
                await repository.AddOrganizationAsync(organization);
                await repository.SaveChangesAsync();
                var organizations = await repository.GetOrganizationsAsync();

                // Assert
                organizations.Count().Should().Be(1);
            }
        }

        [Fact]
        public async Task GetOrganizationsAsync()
        {
            // Arrange

            // Create an in-memory database
            var options = CreateDbContextOptions();

            // Due to the disconnected nature of web requests/responses,
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
                var organizations = await repository.GetOrganizationsAsync();

                // Assert
                organizations.Count.Should().Be(1);
            }
        }

        [Fact]
        public async Task GetOrganizationAsync()
        {
            var options = CreateDbContextOptions();
            var id = CreateAndSaveValidOrganizationId(options);

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
            var options = CreateDbContextOptions();
            var id = CreateAndSaveValidOrganizationId(options);

            using (var context = new AppDbContext(options))
            {
                var repository = new OrganizationRepository(context, mapper);

                var organizationFromRepository = await repository.GetOrganizationEntityAsync(id);

                organizationFromRepository.Should().NotBeNull();
            }
        }

        [Fact]
        public async Task GetOrganizationAsyncIncludesCollections()
        {
            var someNotes = "Some notes";
            var options = CreateDbContextOptions();
            var id = CreateAndSaveValidOrganizationId(options);

            using (var context = new AppDbContext(options))
            {
                var repository = new OrganizationRepository(context, mapper);
                var organizationFromRepository = await repository.GetOrganizationEntityAsync(id);

                organizationFromRepository.SetNotes(someNotes);
                organizationFromRepository.SetAddress(CreateValidAddress());
                organizationFromRepository.SetContact(CreateValidPerson());
                organizationFromRepository.SetPhones(CreateValidPhones());
                organizationFromRepository.SetEmails(CreateValidEmails());

                repository.UpdateOrganizationAsync(organizationFromRepository);

                await repository.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new OrganizationRepository(context, mapper);
                var organizationFromRepository = await repository.GetOrganizationAsync(id);

                organizationFromRepository.Notes.Should().Be(someNotes);
                organizationFromRepository.Address.Should().NotBeNull();
                organizationFromRepository.Phones.Count().Should().BeGreaterThan(0);
                organizationFromRepository.Emails.Count().Should().BeGreaterThan(0);
                organizationFromRepository.Contact.Should().NotBeNull();
            };
        }

        [Fact]
        public async Task GetOrganizationAsyncIncludesContact()
        {
            var options = CreateDbContextOptions();
            var id = CreateAndSaveValidOrganizationId(options);

            using (var context = new AppDbContext(options))
            {
                var repository = new OrganizationRepository(context, mapper);
                var organizationFromRepository = await repository.GetOrganizationEntityAsync(id);

                organizationFromRepository.SetContact(CreateValidPerson());

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
            var options = CreateDbContextOptions();
            var id = CreateAndSaveValidOrganizationId(options);

            using (var context = new AppDbContext(options))
            {
                var repository = new OrganizationRepository(context, mapper);
                var organizationFromRepository = await repository.GetOrganizationEntityAsync(id);

                organizationFromRepository.SetContact(CreateValidPerson());
                organizationFromRepository.Contact.SetEmails(CreateValidEmails());
                organizationFromRepository.Contact.SetPhones(CreateValidPhones());

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
            var options = CreateDbContextOptions();

            using (var context = new AppDbContext(options))
            {
                context.Organizations.Add(CreateValidOrganization());
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
            var options = CreateDbContextOptions();
            var id = CreateAndSaveValidOrganizationId(options);

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
            var options = CreateDbContextOptions();
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
            var options = CreateDbContextOptions();
            var newName = "New Name";
            var id = CreateAndSaveValidOrganizationId(options);

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
            var options = CreateDbContextOptions();
            var id = 0;
            var someNotes = "Some notes";
            var newNotes = "New notes";

            using (var context = new AppDbContext(options))
            {
                var organization = CreateValidOrganization();
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
            var options = CreateDbContextOptions();
            var id = CreateAndSaveValidOrganizationId(options);

            using (var context = new AppDbContext(options))
            {
                var repository = new OrganizationRepository(context, mapper);
                var organizationFromRepo = await repository.GetOrganizationEntityAsync(id);
                organizationFromRepo.Address.Should().BeNull();

                organizationFromRepo.SetAddress(CreateValidAddress());

                repository.UpdateOrganizationAsync(organizationFromRepo);

                await repository.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new OrganizationRepository(context, mapper);
                OrganizationReadDto organizationFromRepo = await repository.GetOrganizationAsync(id);

                organizationFromRepo.Address.AddressLine.Should().Be("1234 Fifth Ave.");
            };
        }

        [Fact]
        public async Task UpdateOrganizationContactAsync()
        {
            var options = CreateDbContextOptions();
            var id = CreateAndSaveValidOrganizationId(options);

            using (var context = new AppDbContext(options))
            {
                var repository = new OrganizationRepository(context, mapper);
                var organizationFromRepo = await repository.GetOrganizationEntityAsync(id);
                organizationFromRepo.Contact.Should().BeNull();

                organizationFromRepo.SetContact(CreateValidPerson());

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
            var options = CreateDbContextOptions();
            var id = CreateAndSaveValidOrganizationId(options);

            using (var context = new AppDbContext(options))
            {
                var repository = new OrganizationRepository(context, mapper);
                var organizationFromRepo = await repository.GetOrganizationEntityAsync(id);
                organizationFromRepo.Should().NotBeNull();

                await repository.DeleteOrganizationAsync(organizationFromRepo.Id);
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
            var options = CreateDbContextOptions();

            using (var context = new AppDbContext(options))
            {
                Action action = () => new OrganizationRepository(context, null);
                action.Should().Throw<ArgumentNullException>();
            }
        }
    }
}
