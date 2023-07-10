using Menominee.Domain.Entities.Inventory;
using FluentAssertions;
using Menominee.Common.Enums;
using Xunit;

namespace Menominee.Tests.Entities
{
    public class InventoryItemLaborShould
    {
        [Fact]
        public void Create_InventoryItemLabor()
        {
            // Arrange
            var laborAmount = LaborAmount.Create(ItemLaborType.Flat,
                LaborAmount.MinimumAmount)
                .Value;

            var techAmount = TechAmount.Create(ItemLaborType.Flat,
                LaborAmount.MinimumAmount, SkillLevel.A)
                .Value;

            // Act
            var resultOrError = InventoryItemLabor.Create(laborAmount, techAmount);

            // Assert
            resultOrError.Value.Should().BeOfType<InventoryItemLabor>();
            resultOrError.IsFailure.Should().BeFalse();
        }

        [Fact]
        public void Not_Create_InventoryItemLabor_With_Null_TechAmount()
        {
            var laborAmount = LaborAmount.Create(ItemLaborType.Flat,
                LaborAmount.MinimumAmount)
                .Value;

            var resultOrError = InventoryItemLabor.Create(laborAmount, null);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void Not_Create_InventoryItemLabor_With_Null_LaborAmount()
        {
            var techAmount = TechAmount.Create(ItemLaborType.Flat,
                           LaborAmount.MinimumAmount, SkillLevel.A)
                           .Value;

            var resultOrError = InventoryItemLabor.Create(null, techAmount);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void SetLaborAmount()
        {
            InventoryItemLabor labor = CreateInventoryItemLabor();
            var laborAmount = LaborAmount.Create(ItemLaborType.Flat,
                LaborAmount.MinimumAmount)
                .Value;

            var resultOrError = labor.SetLaborAmount(laborAmount);

            resultOrError.IsFailure.Should().BeFalse();
            labor.LaborAmount.Should().Be(laborAmount);
        }

        [Fact]
        public void Not_Set_Null_LaborAmount()
        {
            InventoryItemLabor labor = CreateInventoryItemLabor();

            var resultOrError = labor.SetLaborAmount(null);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void SetTechAmount()
        {
            InventoryItemLabor labor = CreateInventoryItemLabor();
            var techAmount = TechAmount.Create(ItemLaborType.Flat,
                LaborAmount.MinimumAmount, SkillLevel.A)
                .Value;

            var resultOrError = labor.SetTechAmount(techAmount);

            resultOrError.IsFailure.Should().BeFalse();
            labor.TechAmount.Should().Be(techAmount);
        }

        [Fact]
        public void Not_Set_Null_TechAmount()
        {
            InventoryItemLabor labor = CreateInventoryItemLabor();

            var resultOrError = labor.SetTechAmount(null);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        private static InventoryItemLabor CreateInventoryItemLabor()
        {
            var laborAmount = LaborAmount.Create(ItemLaborType.Flat,
                LaborAmount.MinimumAmount)
                .Value;

            var techAmount = TechAmount.Create(ItemLaborType.Flat,
                LaborAmount.MinimumAmount, SkillLevel.A)
                .Value;

            return InventoryItemLabor.Create(laborAmount, techAmount).Value;
        }
    }
}
