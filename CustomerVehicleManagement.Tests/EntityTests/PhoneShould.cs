using CustomerVehicleManagement.Domain.Entities;
using NUnit.Framework;
using SharedKernel.Enums;
using System;

namespace CustomerVehicleManagement.Tests.EntityTests
{
    public class PhoneShould
    {
        [Test]
        public void CreateNewPhone()
        {
            // Arrange
            var number = "989.627.9206";
            var phoneType = PhoneType.Home;

            // Act
            var phone = new Phone(number, phoneType, true);

            // Assert
            Assert.That(phone, Is.Not.Null);
        }

        [Test]
        public void NotCreateNewPhoneWithEmptyNumber()
        {
            string number = null;
            var phoneType = PhoneType.Home;

            var exception = Assert.Throws<ArgumentException>(
                () => { new Phone(number, phoneType, true); });

            Assert.That(exception.Message, Is.EqualTo(Phone.PhoneEmptyMessage));
        }

        [Test]
        public void EquateTwoPhoneInstancesHavingSameValuesOnEqualsByProperty()
        {
            var number = "989.627.9206";

            var phone1 = new Phone(number, PhoneType.Home, true);
            var phone2 = new Phone(number, PhoneType.Home, true);

            Assert.That(Phone.EqualsByProperty(phone1, phone2));
        }

        [Test]
        public void NotEquateTwoPhoneInstancesHavingSameValuesOnEqualsByProperty()
        {
            var number = "989.627.9206";

            var phone1 = new Phone(number, PhoneType.Home, true);
            var phone2 = new Phone(number, PhoneType.Home, false);

            Assert.That(!Phone.EqualsByProperty(phone1, phone2));
        }

        [Test]
        public void NotEquateTwoPhoneInstancesHavingDifferingValues()
        {
            var number = "989.627.9206";

            var phone1 = new Phone(number, PhoneType.Home, true);
            var phone2 = new Phone(number, PhoneType.Home, false);

            phone2 = phone2.NewNumber("555.555.5555");

            Assert.That(phone1, Is.Not.EqualTo(phone2));
        }

        [Test]
        public void ReturnNewPhoneOnNewNumber()
        {
            var number = "989.627.9206";
            var phoneType = PhoneType.Home;

            var phone = new Phone(number, phoneType, true);

            phone = phone.NewNumber("555-555-5555");

            Assert.That(phone.Number, Is.EqualTo("555-555-5555"));
        }

        [Test]
        public void ReturnNewPhoneOnNewPhoneType()
        {
            var number = "989.627.9206";
            var phoneType = PhoneType.Home;

            var phone = new Phone(number, phoneType, true);

            phone = phone.NewPhoneType(PhoneType.Mobile);

            Assert.That(phone.PhoneType, Is.EqualTo(PhoneType.Mobile));
        }

        [Test]
        public void ReturnNewPhoneOnNewPrimary()
        {
            var number = "989.627.9206";
            var phoneType = PhoneType.Home;

            var phone = new Phone(number, phoneType, true);

            Assert.That(phone.Primary, Is.EqualTo(true));

            phone = phone.NewPrimary(false);

            Assert.That(phone.Primary, Is.EqualTo(false));
        }

        [Test]
        public void ReturnFormattedTenDigitPhoneNumberOnToString()
        {
            var number = "9896279206";
            var phoneType = PhoneType.Home;

            var phone = new Phone(number, phoneType, true);

            Assert.That(phone.ToString(), Is.EqualTo("(989) 627-9206"));
        }

        [Test]
        public void ReturnFormattedSevenDigitPhoneNumberOnToString()
        {
            var number = "6279206";
            var phoneType = PhoneType.Home;

            var phone = new Phone(number, phoneType, true);

            Assert.That(phone.ToString(), Is.EqualTo("627-9206"));
        }

        [Test]
        public void ReturnNonFormattedOtherDigitPhoneNumberOnToString()
        {
            var number = "896279206";
            var phoneType = PhoneType.Home;

            var phone = new Phone(number, phoneType, true);

            Assert.That(phone.ToString(), Is.EqualTo("896279206"));

            phone = phone.NewNumber("96279206");

            Assert.That(phone.ToString(), Is.EqualTo("96279206"));

            phone = phone.NewNumber("279206");

            Assert.That(phone.ToString(), Is.EqualTo("279206"));

            phone = phone.NewNumber("79206");

            Assert.That(phone.ToString(), Is.EqualTo("79206"));

            phone = phone.NewNumber("9206");

            Assert.That(phone.ToString(), Is.EqualTo("9206"));

            phone = phone.NewNumber("206");

            Assert.That(phone.ToString(), Is.EqualTo("206"));

            phone = phone.NewNumber("06");

            Assert.That(phone.ToString(), Is.EqualTo("06"));

            phone = phone.NewNumber("6");

            Assert.That(phone.ToString(), Is.EqualTo("6"));
        }

        [Test]
        public void RemoveNonNumericCharactersAndFormatOnToString()
        {
            var number = "989.627.9206?";
            var phoneType = PhoneType.Home;

            var phone = new Phone(number, phoneType, true);

            Assert.That(phone.ToString(), Is.EqualTo("(989) 627-9206"));
        }
    }
}
