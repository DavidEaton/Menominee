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
        public void UpdatePhoneNumberOnSetNumber()
        {
            var number = "989.627.9206";
            var phoneType = PhoneType.Home;
            var phone = Phone.Create(number, phoneType, true).Value;

            var newNumber = "555-555-5555";

            phone.Number.Should().Be(number);
            phone.SetNumber(newNumber);

            phone.Number.Should().Be(newNumber);
        }

        [Fact]
        public void UpdatePhoneTypeOnSetPhoneType()
        {
            var number = "989.627.9206";
            var phoneType = PhoneType.Home;
            var phone = Phone.Create(number, phoneType, true).Value;

            phone.PhoneType.Should().Be(PhoneType.Home);
            phone.SetPhoneType(PhoneType.Mobile);

            phone.PhoneType.Should().Be(PhoneType.Mobile);
        }

        [Fact]
        public void UpdatePhoneIsPrimaryOnSetIsPrimary()
        {
            var number = "989.627.9206";
            var phoneType = PhoneType.Home;
            var phone = Phone.Create(number, phoneType, true).Value;

            phone.IsPrimary.Should().BeTrue();
            phone.SetIsPrimary(false);

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

            phone.SetNumber("96279206");

            phone.ToString().Should().Be("96279206");

            phone.SetNumber("279206");

            phone.ToString().Should().Be("279206");

            phone.SetNumber("79206");

            phone.ToString().Should().Be("79206");

            phone.SetNumber("9206");

            phone.ToString().Should().Be("9206");

            phone.SetNumber("206");

            phone.ToString().Should().Be("206");

            phone.SetNumber("06");

            phone.ToString().Should().Be("06");

            phone.SetNumber("6");

            phone.ToString().Should().Be("6");
        }
    }
}
