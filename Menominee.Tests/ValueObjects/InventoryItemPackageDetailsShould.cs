using Menominee.Domain.Entities.Inventory;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace Menominee.Tests.ValueObjects
{
    public class InventoryItemPackageDetailsShould
    {
        [Fact]
        public void Create_InventoryItemPackageDetails()
        {
            // Arrange
            // Act
            var resultOrError = InventoryItemPackageDetails.Create(InventoryItemPackageDetails.MinimumValue + .1, true, true, true);

            // Assert
            resultOrError.Value.Should().BeOfType<InventoryItemPackageDetails>();
            resultOrError.IsFailure.Should().BeFalse();
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Create_InventoryItemPackageDetails_With_Invalid_Quantity(double invalidValue)
        {
            var resultOrError = InventoryItemPackageDetails.Create(invalidValue, true, true, true);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Fact]
        public void Return_New_On_NewQuantity()
        {
            var package = InventoryItemPackageDetails.Create(InventoryItemPackageDetails.MinimumValue + .1, true, true, true).Value;
            double quantity = 10;
            package.Quantity.Should().NotBe(quantity);

            var resultOrError = package.NewQuantity(quantity);

            resultOrError.IsFailure.Should().BeFalse();
            resultOrError.Value.Quantity.Should().Be(quantity);
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set__Invalid_Quantity(double invalidValue)
        {
            var package = InventoryItemPackageDetails.Create(InventoryItemPackageDetails.MinimumValue + .1, true, true, true).Value;
            double quantity = 10;
            package.Quantity.Should().NotBe(quantity);

            var resultOrError = package.NewQuantity(invalidValue);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Fact]
        public void Equate_Two_Instances_Having_Same_Values()
        {
            var packageOne = InventoryItemPackageDetails.Create(InventoryItemPackageDetails.MinimumValue + .1, true, true, true).Value;
            var packageTwo = InventoryItemPackageDetails.Create(InventoryItemPackageDetails.MinimumValue + .1, true, true, true).Value;

            packageOne.Should().Be(packageTwo);
        }

        [Fact]
        public void Not_Equate_Two_Instances_Having_Differing_Values()
        {
            var packageOne = InventoryItemPackageDetails.Create(InventoryItemPackageDetails.MinimumValue + .1, true, true, true).Value;
            var packageTwo = InventoryItemPackageDetails.Create(InventoryItemPackageDetails.MinimumValue + 1, true, true, true).Value;

            packageOne.Should().NotBe(packageTwo);
        }

        [Fact]
        public void Return_New_On_NewPartAmountIsAdditional()
        {
            var package = InventoryItemPackageDetails.Create(InventoryItemPackageDetails.MinimumValue + .1, true, true, true).Value;
            bool partAmountIsAdditionalquantity = true;
            package.PartAmountIsAdditional.Should().Be(partAmountIsAdditionalquantity);

            var resultOrError = package.NewPartAmountIsAdditional(!partAmountIsAdditionalquantity);

            resultOrError.IsFailure.Should().BeFalse();
            resultOrError.Value.PartAmountIsAdditional.Should().Be(!partAmountIsAdditionalquantity);
        }

        [Fact]
        public void Return_New_On_NewLaborAmountIsAdditional()
        {
            var package = InventoryItemPackageDetails.Create(InventoryItemPackageDetails.MinimumValue + .1, true, true, true).Value;
            bool laborAmountIsAdditionalquantity = true;
            package.LaborAmountIsAdditional.Should().Be(laborAmountIsAdditionalquantity);

            var resultOrError = package.NewLaborAmountIsAdditional(!laborAmountIsAdditionalquantity);

            resultOrError.IsFailure.Should().BeFalse();
            resultOrError.Value.LaborAmountIsAdditional.Should().Be(!laborAmountIsAdditionalquantity);
        }

        [Fact]
        public void Return_New_On_NewExciseFeeIsAdditional()
        {
            var package = InventoryItemPackageDetails.Create(InventoryItemPackageDetails.MinimumValue + .1, true, true, true).Value;
            bool exciseFeeIsAdditionalquantity = true;
            package.ExciseFeeIsAdditional.Should().Be(exciseFeeIsAdditionalquantity);

            var resultOrError = package.NewExciseFeeIsAdditional(!exciseFeeIsAdditionalquantity);

            resultOrError.IsFailure.Should().BeFalse();
            resultOrError.Value.ExciseFeeIsAdditional.Should().Be(!exciseFeeIsAdditionalquantity);
        }

        internal class TestData
        {
            public static IEnumerable<object[]> Data
            {
                get
                {
                    yield return new object[] { InventoryItemPackageDetails.MinimumValue - .01 };
                    yield return new object[] { InventoryItemPackageDetails.MaximumValue + .01 };
                }
            }
        }
    }
}
