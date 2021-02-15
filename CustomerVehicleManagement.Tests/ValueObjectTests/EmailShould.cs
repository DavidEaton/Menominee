using NUnit.Framework;
using SharedKernel.ValueObjects;
using System;

namespace CustomerVehicleManagement.Tests.ValueObjectTests
{
    public class EmailShould
    {
        [Test]
        public void CreateNewEmail()
        {
            var email = CreateEmail();

            Assert.That(email, Is.Not.Null);
        }

        [Test]
        public void ThrowExceptionWithNullEmail()
        {
            string address = null;
            bool primary = true;

            var exception = Assert.Throws<ArgumentException>(
                () => { new Email(address, primary); });

            Assert.That(exception.Message, Is.EqualTo(Email.EmailInvalidMessage));
        }

        [Test]
        public void ThrowExceptionWithEmptyEmail()
        {
            string address = string.Empty;
            bool primary = true;

            var exception = Assert.Throws<ArgumentException>(
                () => { new Email(address, primary); });

            Assert.That(exception.Message, Is.EqualTo(Email.EmailInvalidMessage));
        }

        [Test]
        public void ThrowExceptionWithMalformattedEmail()
        {
            string address = "johnatdoedotcom";
            bool primary = true;

            var exception = Assert.Throws<ArgumentException>(
                () => { new Email(address, primary); });

            Assert.That(exception.Message, Is.EqualTo(Email.EmailErrorMessage));
        }

        [Test]
        public void EquateEmailInstancesHavingSameValues()
        {
            var address = "john@doe.com";
            var primary = true;

            var address1 = new Email(address, primary);
            var address2 = new Email(address, primary);

            Assert.That(address1.Equals(address2));
        }

        [Test]
        public void NotEquateEmailInstancesHavingDifferingValues()
        {
            var address = "john@doe.com";
            var primary = true;

            var address1 = new Email(address, primary);

            address = "jane@doe.com";
            primary = false;
            
            var address2 = new Email(address, primary);

            Assert.That(address1, Is.Not.EqualTo(address2));
        }

        [Test]
        public void ReturnNewAddresssOnNewAddress()
        {
            var email = CreateEmail();
            var newAddress = "new@address.com";

            email = email.NewAddress(newAddress);

            Assert.That(email.Address.Equals(newAddress));
        }

        [Test]
        public void ReturnNewAddresssOnNewPrimary()
        {
            var email = CreateEmail();
            var newPrimary = false;

            email = email.NewPrimary(newPrimary);

            Assert.That(email.Primary.Equals(newPrimary));
        }

        internal static Email CreateEmail()
        {
            var address = "john@doe.com";
            var primary = true;

            return new Email(address, primary);
        }
    }
}
