using CustomerVehicleManagement.Domain.Entities;
using FluentAssertions;
using SharedKernel.Enums;
using SharedKernel.ValueObjects;
using System;
using Xunit;

namespace CustomerVehicleManagement.UnitTests.EntityTests
{
    public class ContactableShould
    {
        [Fact]
        public void SetAddress()
        {
            var addressLine = "1234 Five Street";
            var city = "Gaylord";
            var state = "MI";
            var postalCode = "49735";
            var address = new Address(addressLine, city, state, postalCode);
            var organization = Helpers.CreateValidOrganization();

            organization.SetAddress(address);
            var customer = new Customer(organization);
            var janes = customer.Organization;

            customer.EntityType.Should().Be(EntityType.Organization);
            customer.EntityType.Should().BeOfType<EntityType>();
            janes.Address.AddressLine.Should().Be(addressLine);
            janes.Address.City.Should().Be(city);
            janes.Address.State.Should().Be(state);
            janes.Address.PostalCode.Should().Be(postalCode);
        }
    }
}
