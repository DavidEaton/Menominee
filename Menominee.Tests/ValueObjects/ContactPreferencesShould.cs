using FluentAssertions;
using Menominee.Domain.ValueObjects;
using Xunit;

namespace Menominee.Tests.ValueObjects
{
    public class ContactPreferencesShould
    {
        [Fact]
        public void Create_ContactPreferences()
        {
            var contactPreferencesOrError = ContactPreferences.Create(false, false, false);

            contactPreferencesOrError.Value.Should().NotBeNull();
            contactPreferencesOrError.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void Return_New_ContactPreferences_On_NewAllowMail()
        {
            var contactPreferencesOrError = ContactPreferences.Create(false, false, false);
            var contactPreferences = contactPreferencesOrError.Value;

            contactPreferences.AllowMail.Should().BeFalse();

            contactPreferences = contactPreferences.NewAllowMail(true);

            contactPreferences.Should().NotBeNull();
            contactPreferences.AllowEmail.Should().BeFalse();
            contactPreferences.AllowMail.Should().BeTrue();
            contactPreferences.AllowSms.Should().BeFalse();
        }

        [Fact]
        public void Return_New_ContactPreferences_On_NewAllowEMail()
        {
            var contactPreferencesOrError = ContactPreferences.Create(false, false, false);
            var contactPreferences = contactPreferencesOrError.Value;

            contactPreferences.AllowEmail.Should().BeFalse();

            contactPreferences = contactPreferences.NewAllowEmail(true);

            contactPreferences.Should().NotBeNull();
            contactPreferences.AllowEmail.Should().BeTrue();
            contactPreferences.AllowMail.Should().BeFalse();
            contactPreferences.AllowSms.Should().BeFalse();
        }

        [Fact]
        public void Return_New_ContactPreferences_On_NewAllowSms()
        {
            var contactPreferencesOrError = ContactPreferences.Create(false, false, false);
            var contactPreferences = contactPreferencesOrError.Value;

            contactPreferences.AllowSms.Should().BeFalse();

            contactPreferences = contactPreferences.NewAllowSms(true);

            contactPreferences.Should().NotBeNull();
            contactPreferences.AllowEmail.Should().BeFalse();
            contactPreferences.AllowMail.Should().BeFalse();
            contactPreferences.AllowSms.Should().BeTrue();
        }

        [Fact]
        public void Equate_Two_Instances_Having_Same_Values()
        {
            var contactPreferencesOne = ContactPreferences.Create(false, false, false).Value;
            var contactPreferencesTwo = ContactPreferences.Create(false, false, false).Value;

            contactPreferencesOne.Should().BeEquivalentTo(contactPreferencesTwo);
        }

        [Fact]
        public void Not_Equate_Two_Instances_Having_Differing_Values()
        {
            var contactPreferencesOne = ContactPreferences.Create(false, false, false).Value;
            var contactPreferencesTwo = ContactPreferences.Create(true, true, true).Value;

            contactPreferencesOne.Should().NotBeSameAs(contactPreferencesTwo);
        }
    }
}
