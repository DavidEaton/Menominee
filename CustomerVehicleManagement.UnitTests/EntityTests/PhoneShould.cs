using CustomerVehicleManagement.Domain.Entities;
using FluentAssertions;
using Menominee.Common.Enums;
using Xunit;

namespace CustomerVehicleManagement.UnitTests.EntityTests
{
    public class PhoneShould
    {
        [Fact]
        public void Create_Phone()
        {
            // Arrange
            var number = "989.627.9206";
            var phoneType = PhoneType.Home;

            // Act
            var phone = Phone.Create(number, phoneType, true).Value;

            // Assert
            phone.Should().NotBeNull();
        }

        [Fact]
        public void Return_IsFailure_Result_On_Create_With_Null_Number()
        {
            string number = null;
            var phoneType = PhoneType.Home;

            var result = Phone.Create(number, phoneType, true);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Phone.EmptyMessage);
        }

        [Fact]
        public void Return_IsFailure_Result_On_Create_With_Empty_Number()
        {
            string number = string.Empty;
            var phoneType = PhoneType.Home;

            var result = Phone.Create(number, phoneType, true);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Phone.EmptyMessage);
        }

        [Fact]
        public void Return_IsFailure_Result_On_Create_With_Invalid_Number()
        {
            var number = "989.627.9206?";
            var phoneType = PhoneType.Home;

            var result = Phone.Create(number, phoneType, true);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Phone.InvalidMessage);
        }

        [Fact]
        public void Return_IsFailure_Result_On_Create_With_Invalid_PhoneType()
        {
            var number = "989.627.9206";
            var phoneType = (PhoneType)99;

            var result = Phone.Create(number, phoneType, true);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Phone.PhoneTypeInvalidMessage);
        }

        [Fact]
        public void EquateTwoPhoneInstancesHavingSameValuesOnEqualsByProperty()
        {
            var number = "989.627.9206";

            var phone1 = Phone.Create(number, PhoneType.Home, true).Value;
            var phone2 = Phone.Create(number, PhoneType.Home, true).Value;

            Phone.EqualsByProperty(phone1, phone2).Should().BeTrue();
        }

        [Fact]
        public void NotEquateTwoPhoneInstancesHavingDifferentValuesOnEqualsByProperty()
        {
            var number = "989.627.9206";
            var phone1 = Phone.Create(number, PhoneType.Home, true).Value;
            var phone2 = Phone.Create(number, PhoneType.Home, false).Value;
            Phone.EqualsByProperty(phone1, phone2).Should().BeFalse();
        }

        [Fact]
        public void NotEquateTwoPhoneInstancesHavingDifferingValues()
        {
            var number = "989.627.9206";
            var newNumber = "555-555-5555";
            var phone1 = Phone.Create(number, PhoneType.Home, true).Value;
            var phone2 = Phone.Create(number, PhoneType.Home, false).Value;

            phone2 = phone2.NewNumber(newNumber);

            Phone.EqualsByProperty(phone1, phone2).Should().BeFalse();
        }

        [Fact]
        public void ReturnNewPhoneOnNewNumber()
        {
            var number = "989.627.9206";
            var newNumber = "555-555-5555";
            var phoneType = PhoneType.Home;
            var phone = Phone.Create(number, phoneType, true).Value;

            phone = phone.NewNumber(newNumber);

            phone.Number.Should().Be(newNumber);
        }

        [Fact]
        public void ReturnNewPhoneOnNewPhoneType()
        {
            var number = "989.627.9206";
            var phoneType = PhoneType.Home;
            var phone = Phone.Create(number, phoneType, true).Value;

            phone = phone.NewPhoneType(PhoneType.Mobile);

            phone.PhoneType.Should().Be(PhoneType.Mobile);
        }

        [Fact]
        public void ReturnNewPhoneOnNewPrimary()
        {
            var number = "989.627.9206";
            var phoneType = PhoneType.Home;
            var phone = Phone.Create(number, phoneType, true).Value;

            phone.IsPrimary.Should().BeTrue();
            phone = phone.NewPrimary(false);

            phone.IsPrimary.Should().BeFalse();
        }

        [Fact]
        public void ReturnFormattedTenDigitPhoneNumberOnToString()
        {
            var number = "9896279206";
            var numberFormatted = "(989) 627-9206";
            var phoneType = PhoneType.Home;

            var phone = Phone.Create(number, phoneType, true).Value;

            phone.ToString().Should().Be(numberFormatted);
        }

        [Fact]
        public void ReturnFormattedSevenDigitPhoneNumberOnToString()
        {
            var number = "6279206";
            var numberFormatted = "627-9206";
            var phoneType = PhoneType.Home;

            var phone = Phone.Create(number, phoneType, true).Value;

            phone.ToString().Should().Be(numberFormatted);
        }

        [Fact]
        public void ReturnNonFormattedPhoneNumberOnToStringForNonStandardLengthNumbers()
        {
            var number = "896279206";
            var phoneType = PhoneType.Home;

            var phone = Phone.Create(number, phoneType, true).Value;

            phone.ToString().Should().Be("896279206");

            phone = phone.NewNumber("96279206");

            phone.ToString().Should().Be("96279206");

            phone = phone.NewNumber("279206");

            phone.ToString().Should().Be("279206");

            phone = phone.NewNumber("79206");

            phone.ToString().Should().Be("79206");

            phone = phone.NewNumber("9206");

            phone.ToString().Should().Be("9206");

            phone = phone.NewNumber("206");

            phone.ToString().Should().Be("206");

            phone = phone.NewNumber("06");

            phone.ToString().Should().Be("06");

            phone = phone.NewNumber("6");

            phone.ToString().Should().Be("6");
        }
    }
}
