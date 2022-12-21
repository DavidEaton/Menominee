using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Tests.Unit.Helpers;
using FluentAssertions;
using Xunit;

namespace CustomerVehicleManagement.Tests.Unit.Entities
{
    public class MaintenanceItemShould
    {
        [Fact]
        public void Create_MaintenanceItem()
        {
            // Arrange
            InventoryItem item = InventoryItemTestHelper.CreateInventoryItem();

            // Act
            var resultOrError = MaintenanceItem.Create(1, item);

            // Assert
            resultOrError.Value.Should().BeOfType<MaintenanceItem>();
            resultOrError.IsFailure.Should().BeFalse();
        }

        [Fact]
        public void Not_Create_MaintenanceItem_With_Null_Item()
        {
            var resultOrError = MaintenanceItem.Create(1, null);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void Not_Create_MaintenanceItem_With_Invalid_DisplayOrder()
        {
            var resultOrError = MaintenanceItem.Create(-1, InventoryItemTestHelper.CreateInventoryItem());

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("valid");
        }

        [Fact]
        public void SetInventoryItem()
        {
            MaintenanceItem maintenanceItem = CreateMaintenanceItem();
            InventoryItem oldItem = maintenanceItem.InventoryItem;
            InventoryItem newItem = InventoryItemTestHelper.CreateInventoryItem();

            var resultOrError = maintenanceItem.SetInventoryItem(newItem);

            resultOrError.Value.Should().Be(newItem);
            maintenanceItem.InventoryItem.Should().Be(newItem);
            maintenanceItem.InventoryItem.Should().NotBe(oldItem);
        }

        [Fact]
        public void Not_Set_Null_InventoryItem()
        {
            MaintenanceItem maintenanceItem = CreateMaintenanceItem();

            var resultOrError = maintenanceItem.SetInventoryItem(null);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void SetDisplayOrder()
        {
            MaintenanceItem maintenanceItem = CreateMaintenanceItem();
            int oldDisplayOrder = maintenanceItem.DisplayOrder;
            int newDisplayOrder = oldDisplayOrder++;

            var resultOrError = maintenanceItem.SetDisplayOrder(newDisplayOrder);

            resultOrError.Value.Should().Be(newDisplayOrder);
            maintenanceItem.DisplayOrder.Should().Be(newDisplayOrder);
            maintenanceItem.DisplayOrder.Should().NotBe(oldDisplayOrder);
        }

        [Fact]
        public void Not_Set_Invalid_DisplayOrder()
        {
            MaintenanceItem maintenanceItem = CreateMaintenanceItem();
            int invalidDisplayOrder = -1;

            var resultOrError = maintenanceItem.SetDisplayOrder(invalidDisplayOrder);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("valid");
        }

        private static MaintenanceItem CreateMaintenanceItem()
        {
            return MaintenanceItem.Create(1, InventoryItemTestHelper.CreateInventoryItem()).Value;
        }
    }
}
