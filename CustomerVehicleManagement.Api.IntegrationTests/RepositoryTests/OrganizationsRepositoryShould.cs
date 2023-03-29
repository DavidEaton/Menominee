using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Api.Organizations;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models.Organizations;
using CustomerVehicleManagement.Tests.Unit.Helpers;
using FluentAssertions;
using Menominee.Common.ValueObjects;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static CustomerVehicleManagement.Api.IntegrationTests.Helpers.TestUtilities;

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
            var organization = ContactableTestHelper.CreateOrganization();

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
                context.Organizations.Add(ContactableTestHelper.CreateOrganization());
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
                context.Organizations.Add(ContactableTestHelper.CreateOrganization());
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var repository = new OrganizationRepository(context);

                var organizations = await repository.GetOrganizationsListAsync();

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
            var phonesCount = 5;
            var emailsCount = 5;

            using (var context = new ApplicationDbContext(options))
            {
                var repository = new OrganizationRepository(context);
                var organizationFromRepository = await repository.GetOrganizationEntityAsync(id);

                organizationFromRepository.SetNote(note);
                organizationFromRepository.SetAddress(ContactableTestHelper.CreateAddress());
                organizationFromRepository.SetContact(ContactableTestHelper.CreatePerson());
                //organizationFromRepository.SetPhones(CreatePhoneList());
                var emails = ContactableTestHelper.CreateEmails(emailsCount);
                foreach (var email in emails)
                    organizationFromRepository.AddEmail(Email.Create(email.Address, email.IsPrimary).Value);

                var phones = ContactableTestHelper.CreatePhones(phonesCount);
                foreach (var phone in phones)
                    organizationFromRepository.AddPhone(Phone.Create(phone.Number, phone.PhoneType, phone.IsPrimary).Value);

                await repository.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var repository = new OrganizationRepository(context);
                var organizationFromRepository = await repository.GetOrganizationAsync(id);

                organizationFromRepository.Notes.Should().Be(note);
                organizationFromRepository.Address.Should().NotBeNull();
                organizationFromRepository.Phones.Count().Should().Be(phonesCount);
                organizationFromRepository.Emails.Count().Should().Be(emailsCount);
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

                organizationFromRepository.SetContact(ContactableTestHelper.CreatePerson());

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
            var phoneCount = 5;
            var emailCount = 5;
            var options = CreateDbContextOptions();
            long id = CreateAndSaveValidOrganizationId(options);

            using (var context = new ApplicationDbContext(options))
            {
                var repository = new OrganizationRepository(context);
                var organizationFromRepository = await repository.GetOrganizationEntityAsync(id);

                organizationFromRepository.SetContact(ContactableTestHelper.CreatePersonWithPhonesAndEmails());

                var phones = ContactableTestHelper.CreatePhones(phoneCount);
                foreach (var phone in phones)
                    organizationFromRepository.AddPhone(Phone.Create(phone.Number, phone.PhoneType, phone.IsPrimary).Value);

                var emails = ContactableTestHelper.CreateEmails(emailCount);
                foreach (var email in emails)
                    organizationFromRepository.AddEmail(Email.Create(email.Address, email.IsPrimary).Value);

                await repository.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var repository = new OrganizationRepository(context);
                var organizationFromRepository = await repository.GetOrganizationAsync(id);

                organizationFromRepository.Phones.Count().Should().BeGreaterOrEqualTo(1);
                organizationFromRepository.Emails.Count().Should().BeGreaterOrEqualTo(1);
                organizationFromRepository.Contact.Should().NotBeNull();
                organizationFromRepository.Contact.Phones.Count().Should().BeGreaterOrEqualTo(phoneCount);
                organizationFromRepository.Contact.Emails.Count().Should().BeGreaterOrEqualTo(emailCount);
            };
        }

        [Fact]
        public async Task GetOrganizationsListAsync()
        {
            var options = CreateDbContextOptions();

            using (var context = new ApplicationDbContext(options))
            {
                context.Organizations.Add(ContactableTestHelper.CreateOrganization());
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
                var organization = ContactableTestHelper.CreateOrganization();
                organization.SetNote(someNotes);
                context.Organizations.Add(organization);
                context.SaveChanges();
                id = organization.Id;
            }

            using (var context = new ApplicationDbContext(options))
            {
                var repository = new OrganizationRepository(context);
                var organizationFromRepo = await repository.GetOrganizationEntityAsync(id);
                organizationFromRepo.Notes.Should().Be(someNotes);

                organizationFromRepo.SetNote(newNotes);

                await repository.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var repository = new OrganizationRepository(context);
                var organizationFromRepo = await repository.GetOrganizationAsync(id);

                organizationFromRepo.Notes.Should().Be(newNotes);
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

                organizationFromRepo.SetAddress(ContactableTestHelper.CreateAddress());

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

                organizationFromRepo.SetContact(ContactableTestHelper.CreatePerson());

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