using CustomerVehicleManagement.Domain.Entities;
using FluentAssertions;
using System;
using Xunit;

namespace CustomerVehicleManagement.UnitTests.EntityTests
{
    public class SaleCodeShopSuppliesShould
    {
        [Fact]
        public void Create_SaleCodeShopSupplies()
        {
            // Arrange
            var percentage = SaleCodeShopSupplies.MinimumValue;
            var minimumJobAmount = SaleCodeShopSupplies.MinimumValue;
            var minimumCharge = SaleCodeShopSupplies.MinimumValue;
            var maximumCharge = SaleCodeShopSupplies.MinimumValue;
            var includeParts = true;
            var includeLabor = true;

            // Act
            var suppliesOrError = SaleCodeShopSupplies.Create(
                percentage, minimumJobAmount, minimumCharge, maximumCharge, includeParts, includeLabor)
                ;

            // Assert
            suppliesOrError.Value.Should().BeOfType<SaleCodeShopSupplies>();
            suppliesOrError.IsSuccess.Should().BeTrue();
            suppliesOrError.Value.Percentage.Should().Be(percentage);
            suppliesOrError.Value.MinimumJobAmount.Should().Be(minimumJobAmount);
            suppliesOrError.Value.MaximumCharge.Should().Be(maximumCharge);
            suppliesOrError.Value.IncludeParts.Should().Be(includeParts);
            suppliesOrError.Value.IncludeLabor.Should().Be(includeLabor);
        }

        [Fact]
        public void Not_Create_SaleCodeShopSupplies_With_Invalid_Percentage()
        {
            var invalidPercentage = SaleCodeShopSupplies.MinimumValue - .01;
            var minimumJobAmount = SaleCodeShopSupplies.MinimumValue;
            var minimumCharge = SaleCodeShopSupplies.MinimumValue;
            var maximumCharge = SaleCodeShopSupplies.MinimumValue;
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
            var percentage = SaleCodeShopSupplies.MinimumValue;
            var invalidMinimumJobAmount = SaleCodeShopSupplies.MinimumValue - .01;
            var minimumCharge = SaleCodeShopSupplies.MinimumValue;
            var maximumCharge = SaleCodeShopSupplies.MinimumValue;
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
            var percentage = SaleCodeShopSupplies.MinimumValue;
            var minimumJobAmount = SaleCodeShopSupplies.MinimumValue;
            var invalidMinimumCharge = SaleCodeShopSupplies.MinimumValue - .01;
            var maximumCharge = SaleCodeShopSupplies.MinimumValue;
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
            var percentage = SaleCodeShopSupplies.MinimumValue;
            var minimumJobAmount = SaleCodeShopSupplies.MinimumValue;
            var minimumCharge = SaleCodeShopSupplies.MinimumValue;
            var invalidMaximumCharge = SaleCodeShopSupplies.MinimumValue - .01;
            var includeParts = true;
            var includeLabor = true;

            var supplies = SaleCodeShopSupplies.Create(
                percentage, minimumJobAmount, minimumCharge, invalidMaximumCharge, includeParts, includeLabor);

            supplies.IsFailure.Should().BeTrue();
            supplies.Error.Should().NotBeNull();
        }

        [Fact]
        public void SetPercentage()
        {
            var supplies = CreateSaleCodeShopSupplies();
            var newPercentage = SaleCodeShopSupplies.MinimumValue + .01;

            supplies.SetPercentage(newPercentage);

            supplies.Percentage.Should().Be(newPercentage);
        }

        [Fact]
        public void SetMinimumJobAmount()
        {
            var supplies = CreateSaleCodeShopSupplies();
            var newMinimumJobAmount = SaleCodeShopSupplies.MinimumValue + .01;

            supplies.SetMinimumJobAmount(newMinimumJobAmount);

            supplies.MinimumJobAmount.Should().Be(newMinimumJobAmount);
        }

        [Fact]
        public void SetMinimumCharge()
        {
            var supplies = CreateSaleCodeShopSupplies();
            var newMinimumCharge = SaleCodeShopSupplies.MinimumValue + .01;

            supplies.SetMinimumCharge(newMinimumCharge);

            supplies.MinimumCharge.Should().Be(newMinimumCharge);
        }

        [Fact]
        public void SetMaximumCharge()
        {
            var supplies = CreateSaleCodeShopSupplies();
            var newMaximumCharge = SaleCodeShopSupplies.MinimumValue + .01;

            supplies.SetMaximumCharge(newMaximumCharge);

            supplies.MaximumCharge.Should().Be(newMaximumCharge);
        }

        [Fact]
        public void SetIncludeParts()
        {
            var supplies = CreateSaleCodeShopSupplies();
            supplies.IncludeParts.Should().Be(supplies.IncludeParts);
            var notIncludeParts = !supplies.IncludeParts;

            supplies.SetIncludeParts(notIncludeParts);

            supplies.IncludeParts.Should().Be(notIncludeParts);
        }

        [Fact]
        public void SetIncludeLabor()
        {
            var supplies = CreateSaleCodeShopSupplies();
            supplies.IncludeLabor.Should().Be(supplies.IncludeLabor);
            var notIncludeLabor = !supplies.IncludeLabor;

            supplies.SetIncludeLabor(notIncludeLabor);

            supplies.IncludeLabor.Should().Be(notIncludeLabor);
        }

        [Fact]
        public void Not_Set_Invalid_Percentage()
        {
            var supplies = CreateSaleCodeShopSupplies();
            var invalidPercentage = SaleCodeShopSupplies.MinimumValue - .01;

            Assert.Throws<ArgumentOutOfRangeException>(() => supplies.SetPercentage(invalidPercentage));
        }

        [Fact]
        public void Not_Set_Invalid_MinimumJobAmount()
        {
            var supplies = CreateSaleCodeShopSupplies();
            var invalidMinimumJobAmount = SaleCodeShopSupplies.MinimumValue - .01;

            Assert.Throws<ArgumentOutOfRangeException>(() => supplies.SetMinimumJobAmount(invalidMinimumJobAmount));
        }

        [Fact]
        public void Not_Set_Invalid_MinimumCharge()
        {
            var supplies = CreateSaleCodeShopSupplies();
            var invalidMinimumCharge = SaleCodeShopSupplies.MinimumValue - .01;

            Assert.Throws<ArgumentOutOfRangeException>(() => supplies.SetMinimumCharge(invalidMinimumCharge));
        }

        [Fact]
        public void Not_Set_Invalid_MaximumCharge()
        {
            var supplies = CreateSaleCodeShopSupplies();
            var invalidMaximumCharge = SaleCodeShopSupplies.MinimumValue - .01;

            Assert.Throws<ArgumentOutOfRangeException>(() => supplies.SetMaximumCharge(invalidMaximumCharge));
        }

        private SaleCodeShopSupplies CreateSaleCodeShopSupplies()
        {
            var percentage = SaleCodeShopSupplies.MinimumValue;
            var minimumJobAmount = SaleCodeShopSupplies.MinimumValue;
            var minimumCharge = SaleCodeShopSupplies.MinimumValue;
            var maximumCharge = SaleCodeShopSupplies.MinimumValue;
            var includeParts = true;
            var includeLabor = true;

            return SaleCodeShopSupplies.Create(
                percentage, minimumJobAmount, minimumCharge, maximumCharge, includeParts, includeLabor)
                .Value;
        }
    }
}
