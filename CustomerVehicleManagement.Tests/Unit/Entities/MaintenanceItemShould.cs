using CustomerVehicleManagement.Domain.Entities.Inventory;
using FluentAssertions;
using Menominee.Common.Enums;
using System;
using Xunit;

namespace CustomerVehicleManagement.Tests.Unit.Entities
{
    public class MaintenanceItemShould
    {
        [Fact]
        public void Create_MaintenanceItem()
        {
            // Arrange
            Manufacturer manufacturer = CreateManufacturer();
            ProductCode productCode = CreateProductCode();
            InventoryItemPart part = CreateInventoryItemPart();
            InventoryItem item = CreateInventoryItem(manufacturer, productCode, part);

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
            var resultOrError = MaintenanceItem.Create(-1, CreateInventoryItem());

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("valid");
        }

        [Fact]
        public void SetInventoryItem()
        {
            MaintenanceItem maintenanceItem = CreateMaintenanceItem();
            InventoryItem oldItem = maintenanceItem.InventoryItem;
            InventoryItem newItem = CreateInventoryItem();

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

        private InventoryItem CreateInventoryItem()
        {
            Manufacturer manufacturer = CreateManufacturer();
            ProductCode productCode = CreateProductCode();
            InventoryItemPart part = CreateInventoryItemPart();
            return CreateInventoryItem(manufacturer, productCode, part);
        }

        private MaintenanceItem CreateMaintenanceItem()
        {
            Manufacturer manufacturer = CreateManufacturer();
            ProductCode productCode = CreateProductCode();
            InventoryItemPart part = CreateInventoryItemPart();
            InventoryItem item = CreateInventoryItem(manufacturer, productCode, part);

            return MaintenanceItem.Create(1, item).Value;
        }

        private InventoryItem CreateInventoryItem(Manufacturer manufacturer, ProductCode productCode, InventoryItemPart part)
        {
            return InventoryItem.Create(
                manufacturer,
                "001",
                "a description",
                productCode,
                InventoryItemType.Part,
                part: part).Value;
        }

        private static InventoryItemPart CreateInventoryItemPart()
        {
            return InventoryItemPart.Create(
                10, 1, 1, 15,
                TechAmount.Create(ItemLaborType.Flat, TechAmount.MinimumAmount + 1, SkillLevel.A).Value,
                fractional: false).Value;
        }

        private static ProductCode CreateProductCode()
        {
            return new ProductCode()
            {
                Name = "A Product",
                Code = "P1"
            };
        }

        private static Manufacturer CreateManufacturer()
        {
            return Manufacturer.Create("Manufacturer One", "M1", "V1").Value;
        }
    }
}
