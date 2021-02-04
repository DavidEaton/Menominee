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

        /// <summary>
        /// This test is for value object phone, not entity type phone
        /// </summary>
        //[Test]
        //public void EquateTwoPhoneInstancesHavingSameValues()
        //{
        //    var number = "989.627.9206";

        //    var phone1 = new Phone(number, PhoneType.Home);
        //    var phone2 = new Phone(number, PhoneType.Home);

        //    Assert.That(phone1.Equals(phone2));
        //}

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

    }
}
