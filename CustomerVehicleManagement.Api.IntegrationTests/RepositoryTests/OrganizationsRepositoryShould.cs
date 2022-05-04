using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Api.Organizations;
using CustomerVehicleManagement.Shared.Models.Organizations;
using FluentAssertions;
using Menominee.Common.ValueObjects;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static CustomerVehicleManagement.Api.IntegrationTests.Helpers.TestUtilities;
using Helper = CustomerVehicleManagement.Shared.TestUtilities.Utilities;

namespace CustomerVehicleManagement.Api.IntegrationTests.Repositories
{
    public class OrganizationsRepositoryShould
    {
        [Fact]
        public async Task AddOrganizationAsync()
        {
            // Due to the disconnected nature of web requests/responses,
            // our tests create and use unique contexts for each
            // request, like our production code does.
            var options = CreateDbContextOptions();
            using var context = new ApplicationDbContext(options);

            // Arrange
            var repository = new OrganizationRepository(context);
            var organization = Helper.CreateOrganization();

            // Act
            await repository.AddOrganizationAsync(organization);
            await repository.SaveChangesAsync();
            var organizations = await repository.GetOrganizationsAsync();

            // Assert
            organizations.Count.Should().Be(1);
        }

        [Fact]
        public async Task GetOrganizationsAsync()
        {
            var options = CreateDbContextOptions();
            using (var context = new ApplicationDbContext(options))
            {
                context.Organizations.Add(Helper.CreateOrganization());
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var repository = new OrganizationRepository(context);

                var organizations = await repository.GetOrganizationsAsync();

                organizations.Count.Should().BeGreaterOrEqualTo(1);
            }
        }

        [Fact]
        public async Task GetOrganizationsInListAsync()
        {
            var options = CreateDbContextOptions();
            using (var context = new ApplicationDbContext(options))
            {
                context.Organizations.Add(Helper.CreateOrganization());
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var repository = new OrganizationRepository(context);

                // Act
                var organizations = await repository.GetOrganizationsListAsync();

                // Assert
                organizations.Count.Should().BeGreaterOrEqualTo(1);
            }
        }

        [Fact]
        public async Task GetOrganizationAsync()
        {
            var options = CreateDbContextOptions();
            long id = CreateAndSaveValidOrganizationId(options);

            using (var context = new ApplicationDbContext(options))
            {
                var repository = new OrganizationRepository(context);

                var organizationFromRepository = await repository.GetOrganizationAsync(id);

                organizationFromRepository.Should().NotBeNull();
            }
        }

        [Fact]
        public async Task GetOrganizationEntityAsync()
        {
            var options = CreateDbContextOptions();
            long id = CreateAndSaveValidOrganizationId(options);

            using (var context = new ApplicationDbContext(options))
            {
                var repository = new OrganizationRepository(context);

                var organizationFromRepository = await repository.GetOrganizationEntityAsync(id);

                organizationFromRepository.Should().NotBeNull();
            }
        }

        [Fact]
        public async Task GetOrganizationAsyncIncludesCollections()
        {
            var note = "Some notes in the note field";
            var options = CreateDbContextOptions();
            long id = CreateAndSaveValidOrganizationId(options);

            using (var context = new ApplicationDbContext(options))
            {
                var repository = new OrganizationRepository(context);
                var organizationFromRepository = await repository.GetOrganizationEntityAsync(id);

                organizationFromRepository.SetNote(note);
                organizationFromRepository.SetAddress(Helper.CreateAddress());
                organizationFromRepository.SetContact(Helper.CreatePerson());
                organizationFromRepository.SetPhones(Helper.CreatePhoneList());
                organizationFromRepository.SetEmails(Helper.CreateEmailList());

                repository.UpdateOrganizationAsync(organizationFromRepository);

                await repository.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var repository = new OrganizationRepository(context);
                var organizationFromRepository = await repository.GetOrganizationAsync(id);

                organizationFromRepository.Note.Should().Be(note);
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
            long id = CreateAndSaveValidOrganizationId(options);

            using (var context = new ApplicationDbContext(options))
            {
                var repository = new OrganizationRepository(context);
                var organizationFromRepository = await repository.GetOrganizationEntityAsync(id);

                organizationFromRepository.SetContact(Helper.CreatePerson());

                repository.UpdateOrganizationAsync(organizationFromRepository);

                await repository.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var repository = new OrganizationRepository(context);
                var organizationFromRepository = await repository.GetOrganizationAsync(id);

                organizationFromRepository.Contact.Should().NotBeNull();
            };
        }

        [Fact]
        public async Task GetOrganizationAsyncIncludesContactCollections()
        {
            var options = CreateDbContextOptions();
            long id = CreateAndSaveValidOrganizationId(options);

            using (var context = new ApplicationDbContext(options))
            {
                var repository = new OrganizationRepository(context);
                var organizationFromRepository = await repository.GetOrganizationEntityAsync(id);

                organizationFromRepository.SetContact(Helper.CreatePerson());
                organizationFromRepository.Contact.SetEmails(Helper.CreateEmailList());
                organizationFromRepository.Contact.SetPhones(Helper.CreatePhoneList());

                repository.UpdateOrganizationAsync(organizationFromRepository);

                await repository.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var repository = new OrganizationRepository(context);
                var organizationFromRepository = await repository.GetOrganizationAsync(id);

                organizationFromRepository.Contact.Should().NotBeNull();
                organizationFromRepository.Contact.Phones.Count().Should().BeGreaterOrEqualTo(1);
                organizationFromRepository.Contact.Emails.Count().Should().BeGreaterOrEqualTo(1);
            };
        }

        [Fact]
        public async Task GetOrganizationsListAsync()
        {
            var options = CreateDbContextOptions();

            using (var context = new ApplicationDbContext(options))
            {
                context.Organizations.Add(Helper.CreateOrganization());
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var repository = new OrganizationRepository(context);
                var organizations = await repository.GetOrganizationsListAsync();

                organizations.Count().Should().BeGreaterOrEqualTo(1);
            }
        }

        [Fact]
        public async Task Return_True_When_OrganizationExistsAsync()
        {
            var options = CreateDbContextOptions();
            long id = CreateAndSaveValidOrganizationId(options);

            using (var context = new ApplicationDbContext(options))
            {
                var repository = new OrganizationRepository(context);
                var organizationExists = await repository.OrganizationExistsAsync(id);

                organizationExists.Should().Be(true);
            }
        }

        [Fact]
        public async Task Return_False_When_OrganizationExistsAsync()
        {
            var options = CreateDbContextOptions();
            long id = 99999;

            using (var context = new ApplicationDbContext(options))
            {
                var repository = new OrganizationRepository(context);
                var organizationExists = await repository.OrganizationExistsAsync(id);

                organizationExists.Should().Be(false);
            }
        }

        [Fact]
        public async Task UpdateOrganizationNameAsync()
        {
            var options = CreateDbContextOptions();
            var newName = "New Name";
            long id = CreateAndSaveValidOrganizationId(options);

            using (var context = new ApplicationDbContext(options))
            {
                var repository = new OrganizationRepository(context);
                var organizationFromRepo = await repository.GetOrganizationEntityAsync(id);

                organizationFromRepo.SetName(OrganizationName.Create(newName).Value);
                repository.UpdateOrganizationAsync(organizationFromRepo);

                await repository.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var repository = new OrganizationRepository(context);
                var organizationFromRepo = await repository.GetOrganizationAsync(id);

                organizationFromRepo.Name.Should().Be(newName);
            };
        }

        [Fact]
        public async Task UpdateOrganizationNotesAsync()
        {
            var options = CreateDbContextOptions();
            long id = 0;
            var someNotes = "Some notes";
            var newNotes = "New notes";

            using (var context = new ApplicationDbContext(options))
            {
                var organization = Helper.CreateOrganization();
                organization.SetNote(someNotes);
                context.Organizations.Add(organization);
                context.SaveChanges();
                id = organization.Id;
            }

            using (var context = new ApplicationDbContext(options))
            {
                var repository = new OrganizationRepository(context);
                var organizationFromRepo = await repository.GetOrganizationEntityAsync(id);
                organizationFromRepo.Note.Should().Be(someNotes);

                organizationFromRepo.SetNote(newNotes);
                repository.UpdateOrganizationAsync(organizationFromRepo);

                await repository.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var repository = new OrganizationRepository(context);
                var organizationFromRepo = await repository.GetOrganizationAsync(id);

                organizationFromRepo.Note.Should().Be(newNotes);
            };
        }

        [Fact]
        public async Task UpdateOrganizationAddressAsync()
        {
            var options = CreateDbContextOptions();
            long id = CreateAndSaveValidOrganizationId(options);

            using (var context = new ApplicationDbContext(options))
            {
                var repository = new OrganizationRepository(context);
                var organizationFromRepo = await repository.GetOrganizationEntityAsync(id);
                organizationFromRepo.Address.Should().BeNull();

                organizationFromRepo.SetAddress(Helper.CreateAddress());

                repository.UpdateOrganizationAsync(organizationFromRepo);

                await repository.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var repository = new OrganizationRepository(context);
                OrganizationToRead organizationFromRepo = await repository.GetOrganizationAsync(id);

                organizationFromRepo.Address.AddressLine.Should().Be("1234 Fifth Ave.");
            };
        }

        [Fact]
        public async Task UpdateOrganizationContactAsync()
        {
            var options = CreateDbContextOptions();
            long id = CreateAndSaveValidOrganizationId(options);

            using (var context = new ApplicationDbContext(options))
            {
                var repository = new OrganizationRepository(context);
                var organizationFromRepo = await repository.GetOrganizationEntityAsync(id);
                organizationFromRepo.Contact.Should().BeNull();

                organizationFromRepo.SetContact(Helper.CreatePerson());

                repository.UpdateOrganizationAsync(organizationFromRepo);

                await repository.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var repository = new OrganizationRepository(context);
                OrganizationToRead organizationFromRepo = await repository.GetOrganizationAsync(id);

                organizationFromRepo.Contact.Should().NotBeNull();
            };
        }

        [Fact]
        public async Task DeleteOrganizationAsync()
        {
            var options = CreateDbContextOptions();
            long id = CreateAndSaveValidOrganizationId(options);

            using (var context = new ApplicationDbContext(options))
            {
                var repository = new OrganizationRepository(context);
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
            Action action = () => new OrganizationRepository(null);

            action.Should().Throw<ArgumentNullException>();
        }

    }
}
