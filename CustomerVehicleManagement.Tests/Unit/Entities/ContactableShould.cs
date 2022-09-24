using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models.Organizations;
using FluentAssertions;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using Xunit;

namespace CustomerVehicleManagement.Tests.Unit.Entities
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
            var organization = OrganizationHelper.CreateTestOrganization();

            organization.SetAddress(addressOrError.Value);
            var customerOrError = new Customer(organization, CustomerType.Retail);
            var janes = customerOrError.Organization;

            customerOrError.Should().BeOfType<Customer>();
            customerOrError.EntityType.Should().Be(EntityType.Organization);
            janes.Address.AddressLine.Should().Be(addressLine);
            janes.Address.City.Should().Be(city);
            janes.Address.State.Should().Be(state);
            janes.Address.PostalCode.Should().Be(postalCode);
        }
    }
}
