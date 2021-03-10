using Xunit;
using SharedKernel.ValueObjects;
using System;
using FluentAssertions;

namespace CustomerVehicleManagement.Tests.ValueObjectTests
{
    public class AddressShould
    {
        [Fact]
        public void CreateAddress()
        {
            var addressLine = "1234 Five Street";
            var city = "Gaylord";
            var state = "MI";
            var postalCode = "49735";

            var address = new Address(addressLine, city, state, postalCode);

            address.Should().NotBeNull();
        }

        [Fact]
        public void ThrowExceptionWithEmptyAddressLine()
        {
            string addressLine = null;
            var city = "Gaylord";
            var state = "MI";
            var postalCode = "49735";

            Action action = () => new Address(addressLine, city, state, postalCode);

            action.Should().Throw<ArgumentException>()
                           .WithMessage(Address.AddressEmptyMessage);
        }

        [Fact]
        public void ThrowExceptionWithEmptyCity()
        {
            var addressLine = "1234 Five Street";
            string city = null;
            var state = "MI";
            var postalCode = "49735";

            Action action = () => new Address(addressLine, city, state, postalCode);

            action.Should().Throw<ArgumentException>()
                           .WithMessage(Address.AddressEmptyMessage);
        }

        [Fact]
        public void ThrowExceptionWithEmptyState()
        {
            var addressLine = "1234 Five Street";
            var city = "Gaylord";
            string state = null;
            var postalCode = "49735";

            Action action = () => new Address(addressLine, city, state, postalCode);

            action.Should().Throw<ArgumentException>()
                           .WithMessage(Address.AddressEmptyMessage);
        }

        [Fact]
        public void ThrowExceptionWithEmptyPostalCode()
        {
            var addressLine = "1234 Five Street";
            var city = "Gaylord";
            var state = "MI";
            string postalCode = null;

            Action action = () => new Address(addressLine, city, state, postalCode);

            action.Should().Throw<ArgumentException>()
                           .WithMessage(Address.AddressEmptyMessage);
        }

        [Fact]
        public void EquateTwoAddressInstancesHavingSameValues()
        {
            var address1 = CreateValidAddress();
            var address2 = CreateValidAddress();

            address1.Should().BeEquivalentTo(address2);
        }

        [Fact]
        public void NotEquateTwoAddressInstancesHavingDifferingValues()
        {
            var address1 = CreateValidAddress();
            var address2 = CreateValidAddress();
            var newAddressLine = "54321";

            address2 = address2.NewAddressLine(newAddressLine);

            address1.Should().NotBeEquivalentTo(address2);
        }


        [Fact]
        public void ReturnNewAddressOnNewAddressLine()
        {
            var address = CreateValidAddress();
            var newAddressLine = "5432 One Street";

            address = address.NewAddressLine("5432 One Street");

            address.AddressLine.Should().Be(newAddressLine);
        }

        [Fact]
        public void ReturnNewAddressOnNewCity()
        {
            var address = CreateValidAddress();
            var newCity = "Oomapopalis";

            address = address.NewCity(newCity);

            address.City.Should().Be(newCity);
        }

        [Fact]
        public void ReturnNewAddressOnNewState()
        {
            var address = CreateValidAddress();
            var newState = "ZA";

            address = address.NewState("ZA");

            address.State.Should().Be(newState);
        }

        [Fact]
        public void ReturnNewAddressOnNewPostalCode()
        {
            var address = CreateValidAddress();
            var newPostalCode = "55555";

            address = address.NewPostalCode(newPostalCode);

            address.PostalCode.Should().Be(newPostalCode);
        }

        internal static Address CreateValidAddress()
        {
            var addressLine = "1234 Five Street";
            var city = "Gaylord";
            var state = "MI";
            var postalCode = "49735";

            return new Address(addressLine, city, state, postalCode);
        }
    }

}
