using CustomerVehicleManagement.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace CustomerVehicleManagement.UnitTests.EntityTests
{
    public class SaleCodeShopSuppliesShould
    {
        [Fact]
        public void Create_SaleCodeShopSupplies()
        {
            // Arrange
            var percentage = .25;
            var minimumJobAmount = 10.0;
            var minimumCharge = 5.0;
            var maximumCharge = 99999.9;
            var includeParts = true;
            var includeLabor = true;

            // Act
            var supplies = SaleCodeShopSupplies.Create(
                percentage, minimumJobAmount, minimumCharge, maximumCharge, includeParts, includeLabor)
                .Value;

            // Assert
            supplies.Should().BeOfType<SaleCodeShopSupplies>();
            supplies.Percentage.Should().Be(percentage);
            supplies.MinimumJobAmount.Should().Be(minimumJobAmount);
            supplies.MaximumCharge.Should().Be(maximumCharge);
            supplies.IncludeParts.Should().Be(includeParts);
            supplies.IncludeLabor.Should().Be(includeLabor);
        }

        [Fact]
        public void Not_Create_SaleCodeShopSupplies_With_Invalid_Percentage()
        {
            var invalidPercentage = SaleCodeShopSupplies.MinimumValue - .01;
            var minimumJobAmount = 10.0;
            var minimumCharge = 5.0;
            var maximumCharge = 99999.9;
            var includeParts = true;
            var includeLabor = true;

            var supplies = SaleCodeShopSupplies.Create(
                invalidPercentage, minimumJobAmount, minimumCharge, maximumCharge, includeParts, includeLabor);

            supplies.IsFailure.Should().BeTrue();
            supplies.Error.Should().NotBeNull();
        }

        [Fact]
        public void Not_Create_SaleCodeShopSupplies_With_Invalid_MinimumJobAmount()
        {
            var percentage = .25;
            var invalidMinimumJobAmount = SaleCodeShopSupplies.MinimumValue - .01;
            var minimumCharge = 5.0;
            var maximumCharge = 99999.9;
            var includeParts = true;
            var includeLabor = true;

            var supplies = SaleCodeShopSupplies.Create(
                percentage, invalidMinimumJobAmount, minimumCharge, maximumCharge, includeParts, includeLabor);

            supplies.IsFailure.Should().BeTrue();
            supplies.Error.Should().NotBeNull();
        }

        [Fact]
        public void Not_Create_SaleCodeShopSupplies_With_Invalid_MinimumCharge()
        {
            var percentage = .25;
            var minimumJobAmount = 10.0;
            var invalidMinimumCharge = SaleCodeShopSupplies.MinimumValue - .01;
            var maximumCharge = 99999.9;
            var includeParts = true;
            var includeLabor = true;

            var supplies = SaleCodeShopSupplies.Create(
                percentage, minimumJobAmount, invalidMinimumCharge, maximumCharge, includeParts, includeLabor);

            supplies.IsFailure.Should().BeTrue();
            supplies.Error.Should().NotBeNull();
        }

        [Fact]
        public void Not_Create_SaleCodeShopSupplies_With_Invalid_MaximumCharge()
        {
            var percentage = .25;
            var minimumJobAmount = 10.0;
            var minimumCharge = 5.0;
            var invalidMaximumCharge = SaleCodeShopSupplies.MinimumValue - .01;
            var includeParts = true;
            var includeLabor = true;

            var supplies = SaleCodeShopSupplies.Create(
                percentage, minimumJobAmount, minimumCharge, invalidMaximumCharge, includeParts, includeLabor);

            supplies.IsFailure.Should().BeTrue();
            supplies.Error.Should().NotBeNull();
        }


    }
}
