using CustomerVehicleManagement.Domain.Entities;
using FluentAssertions;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
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
            var state = State.MI;
            var postalCode = "49735";
            var addressOrError = Address.Create(addressLine, city, state, postalCode);
            var organization = Helpers.CreateValidOrganization();

            organization.SetAddress(addressOrError.Value);
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
