using CustomerVehicleManagement.Domain.Entities;
using FluentAssertions;
using Menominee.Common.Enums;
using System;
using Xunit;

namespace CustomerVehicleManagement.UnitTests.EntityTests
{
    public class PhoneShould
    {
        [Fact]
        public void CreatePhone()
        {
            // Arrange
            var number = "989.627.9206";
            var phoneType = PhoneType.Home;

            // Act
            var phone = new Phone(number, phoneType, true);

            // Assert
            phone.Should().NotBeNull();
        }

        [Fact]
        public void NotCreatePhoneWithEmptyNumber()
        {
            string number = null;
            var phoneType = PhoneType.Home;

            Action action = () => new Phone(number, phoneType, true);

            action.Should().Throw<ArgumentNullException>()
                           .WithMessage($"Value cannot be null. (Parameter '{Phone.PhoneEmptyMessage}')");
        }

        [Fact]
        public void EquateTwoPhoneInstancesHavingSameValuesOnEqualsByProperty()
        {
            var number = "989.627.9206";

            var phone1 = new Phone(number, PhoneType.Home, true);
            var phone2 = new Phone(number, PhoneType.Home, true);

            Phone.EqualsByProperty(phone1, phone2).Should().BeTrue();
        }

        [Fact]
        public void NotEquateTwoPhoneInstancesHavingDifferentValuesOnEqualsByProperty()
        {
            var number = "989.627.9206";
            var phone1 = new Phone(number, PhoneType.Home, true);
            var phone2 = new Phone(number, PhoneType.Home, false);

            Phone.EqualsByProperty(phone1, phone2).Should().BeFalse();
        }

        [Fact]
        public void NotEquateTwoPhoneInstancesHavingDifferingValues()
        {
            var number = "989.627.9206";
            var newNumber = "555-555-5555";
            var phone1 = new Phone(number, PhoneType.Home, true);
            var phone2 = new Phone(number, PhoneType.Home, false);

            phone2 = phone2.NewNumber(newNumber);

            Phone.EqualsByProperty(phone1, phone2).Should().BeFalse();
        }

        [Fact]
        public void ReturnNewPhoneOnNewNumber()
        {
            var number = "989.627.9206";
            var newNumber = "555-555-5555";
            var phoneType = PhoneType.Home;
            var phone = new Phone(number, phoneType, true);

            phone = phone.NewNumber(newNumber);

            phone.Number.Should().Be(newNumber);
        }

        [Fact]
        public void ReturnNewPhoneOnNewPhoneType()
        {
            var number = "989.627.9206";
            var phoneType = PhoneType.Home;
            var phone = new Phone(number, phoneType, true);

            phone = phone.NewPhoneType(PhoneType.Mobile);

            phone.PhoneType.Should().Be(PhoneType.Mobile);
        }

        [Fact]
        public void ReturnNewPhoneOnNewPrimary()
        {
            var number = "989.627.9206";
            var phoneType = PhoneType.Home;
            var phone = new Phone(number, phoneType, true);

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

            var phone = new Phone(number, phoneType, true);

            phone.ToString().Should().Be(numberFormatted);
        }

        [Fact]
        public void ReturnFormattedSevenDigitPhoneNumberOnToString()
        {
            var number = "6279206";
            var numberFormatted = "627-9206";
            var phoneType = PhoneType.Home;

            var phone = new Phone(number, phoneType, true);

            phone.ToString().Should().Be(numberFormatted);
        }

        [Fact]
        public void ReturnNonFormattedPhoneNumberOnToStringForNonStandardLengthNumbers()
        {
            var number = "896279206";
            var phoneType = PhoneType.Home;

            var phone = new Phone(number, phoneType, true);

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

        [Fact]
        public void RemoveNonNumericCharactersAndFormatOnToString()
        {
            var number = "989.627.9206?";
            var numberFormatted = "(989) 627-9206";
            var phoneType = PhoneType.Home;

            var phone = new Phone(number, phoneType, true);

            phone.ToString().Should().Be(numberFormatted);
        }
    }
}
