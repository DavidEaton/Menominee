using NUnit.Framework;
using SharedKernel.ValueObjects;
using System;

namespace CustomerVehicleManagement.Tests.ValueObjectTests
{
    public class AddressShould
    {
        [Test]
        public void CreateNewAddress()
        {
            var addressLine = "1234 Five Street";
            var city = "Gaylord";
            var state = "MI";
            var postalCode = "49735";

            var address = new Address(addressLine, city, state, postalCode);

            Assert.That(address, Is.Not.Null);
        }

        [Test]
        public void ThrowExceptionWithEmptyAddressDetails()
        {
            string addressLine = null;
            string city = "Gaylord";
            string state = "MI";
            string postalCode = "49735";

            var exception = Assert.Throws<ArgumentException>(
                () => { new Address(addressLine, city, state, postalCode); });

            Assert.That(exception.Message, Is.EqualTo(Address.AddressEmptyMessage));

            addressLine = "1234 Five Street";
            city = null;
            state = "MI";
            postalCode = "49735";

            exception = Assert.Throws<ArgumentException>(
                () => { new Address(addressLine, city, state, postalCode); });

            Assert.That(exception.Message, Is.EqualTo(Address.AddressEmptyMessage));

            addressLine = "1234 Five Street";
            city = "Gaylord";
            state = null;
            postalCode = "49735";

            exception = Assert.Throws<ArgumentException>(
                () => { new Address(addressLine, city, state, postalCode); });

            Assert.That(exception.Message, Is.EqualTo(Address.AddressEmptyMessage));

            addressLine = "1234 Five Street";
            city = "Gaylord";
            state = "MI";
            postalCode = null;

            exception = Assert.Throws<ArgumentException>(
                () => { new Address(addressLine, city, state, postalCode); });

            Assert.That(exception.Message, Is.EqualTo(Address.AddressEmptyMessage));
        }

        [Test]
        public void EquateTwoAddressInstancesHavingSameValues()
        {
            var address1 = CreateAddress();
            var address2 = CreateAddress();

            Assert.That(address1.Equals(address2));
        }

        [Test]
        public void NotEquateTwoAddressInstancesHavingDifferingValues()
        {
            var address1 = CreateAddress();
            var address2 = CreateAddress();

            address2 = address2.NewAddressLine("BR549");

            Assert.That(address1, Is.Not.EqualTo(address2));
        }


        [Test]
        public void ReturnNewAddressOnNewAddressLine()
        {
            var address = CreateAddress();

            address = address.NewAddressLine("5432 One Street");

            Assert.That(address.AddressLine, Is.EqualTo("5432 One Street"));
        }

        [Test]
        public void ReturnNewAddressOnNewCity()
        {
            var address = CreateAddress();

            address = address.NewCity("Oomapopalis");

            Assert.That(address.City, Is.EqualTo("Oomapopalis"));
        }

        [Test]
        public void ReturnNewAddressOnNewState()
        {
            var address = CreateAddress();

            address = address.NewState("ZA");

            Assert.That(address.State, Is.EqualTo("ZA"));
        }

        [Test]
        public void ReturnNewAddressOnNewPostalCode()
        {
            var address = CreateAddress();

            address = address.NewPostalCode("55555");

            Assert.That(address.PostalCode, Is.EqualTo("55555"));
        }

        internal static Address CreateAddress()
        {
            var addressLine = "1234 Five Street";
            var city = "Gaylord";
            var state = "MI";
            var postalCode = "49735";

            return new Address(addressLine, city, state, postalCode);
        }
    }
}
