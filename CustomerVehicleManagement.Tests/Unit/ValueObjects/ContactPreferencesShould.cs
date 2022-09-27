using FluentAssertions;
using Menominee.Common.ValueObjects;
using Xunit;

namespace CustomerVehicleManagement.Tests.Unit.ValueObjects
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
    }
}
