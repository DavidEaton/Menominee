using CustomerVehicleManagement.Domain.Entities;
using FluentAssertions;
using System;
using Xunit;

namespace CustomerVehicleManagement.UnitTests.EntityTests
{
    public class EmailShould
    {
        [Fact]
        public void CreateEmail()
        {
            var address = "john@doe.com";
            var primary = true;

            var email = new Email(address, primary);

            email.Should().BeOfType<Email>();
        }

        [Fact]
        public void ThrowExceptionWithNullAddress()
        {
            string address = null;
            var primary = true;

            Action action = () => new Email(address, primary);
            
            action.Should().Throw<ArgumentException>()
                           .WithMessage($"{Email.EmailEmptyMessage} (Parameter 'address')")
                           .And
                           .ParamName.Should().Be("address");

        }

        [Fact]
        public void ThrowExceptionWithEmptyAddress()
        {
            var address = string.Empty;
            var primary = true;

            Action action = () => new Email(address, primary);

            action.Should().Throw<ArgumentException>()
                           .WithMessage($"{Email.EmailEmptyMessage} (Parameter 'address')")
                           .And
                           .ParamName.Should().Be("address");
        }

        [Fact]
        public void ThrowExceptionWithMalformattedAddress()
        {
            var address = "johnatdoedotcom";
            var primary = true;

            Action action = () => new Email(address, primary);

            action.Should().Throw<ArgumentException>()
                           .And
                           .Message.Should().Be(Email.EmailErrorMessage);
        }

        [Fact]
        public void EquateEmailInstancesHavingSameValues()
        {
            var address = "john@doe.com";
            var primary = true;

            var address1 = new Email(address, primary);
            var address2 = new Email(address, primary);

            address1.Should().BeEquivalentTo(address2);
        }

        [Fact]
        public void NotEquateEmailInstancesHavingDifferingValues()
        {
            var address = "john@doe.com";
            var primary = true;
            var email1 = new Email(address, primary);
            var newAddress = "jane@doe.com";
            primary = false;

            var email2 = new Email(newAddress, primary);

            email1.Should().NotBeEquivalentTo(email2);
        }

        [Fact]
        public void ReturnNewEmailOnNewAddress()
        {
            var email = CreatePrimaryEmail();
            var newAddress = "new@address.com";

            email = email.NewAddress(newAddress);

            email.Address.Should().Be(newAddress);
        }

        [Fact]
        public void ReturnNewEmailOnNewPrimary()
        {
            var email = CreatePrimaryEmail();
            var newPrimary = false;

            email = email.NewPrimary(newPrimary);

            email.IsPrimary.Should().Be(newPrimary);
        }

        internal static Email CreatePrimaryEmail()
        {
            var address = "john@doe.com";
            var primary = true;

            return new Email(address, primary);
        }
    }
}