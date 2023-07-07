using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using CustomerVehicleManagement.Tests.Helpers.Fakers;
using FluentAssertions;
using Menominee.Common.Enums;
using Telerik.ReportViewer.BlazorNative.Tools;
using TestingHelperLibrary.Fakers;
using Xunit;

namespace CustomerVehicleManagement.Tests.Entities
{
    public class RepairOrderItemShould
    {
        [Fact]
        public void Create_RepairOrderItem()
        {
            // Arrange
            var manufacturer = new ManufacturerFaker(true).Generate();
            var partNumber = "123456";
            var description = "Test description";
            var saleCode = new SaleCodeFaker(true).Generate();
            var productCode = new ProductCodeFaker(true).Generate();
            var part = new RepairOrderItemPartFaker(true).Generate();
            var partType = PartType.Part;

            // Act
            var result = RepairOrderItem.Create(manufacturer, partNumber, description, saleCode, productCode, partType, part);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Manufacturer.Should().Be(manufacturer);
            result.Value.PartNumber.Should().Be(partNumber);
            result.Value.Description.Should().Be(description);
            result.Value.SaleCode.Should().Be(saleCode);
            result.Value.ProductCode.Should().Be(productCode);
            result.Value.PartType.Should().Be(partType);
        }

        [Fact]
        public void Return_Failure_On_Create_RepairOrderItem_With_Invalid_PartNumber()
        {
            var manufacturer = new ManufacturerFaker(true).Generate();
            var partNumber = ""; // Invalid part number
            var description = "Test description";
            var saleCode = new SaleCodeFaker(true).Generate();
            var productCode = new ProductCodeFaker(true).Generate();
            var part = new RepairOrderItemPartFaker(true).Generate();
            var partType = PartType.Part;

            var result = RepairOrderItem.Create(manufacturer, partNumber, description, saleCode, productCode, partType, part);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderItem.InvalidLengthMessage);
        }

        [Fact]
        public void Return_Failure_On_Create_RepairOrderItem_With_Invalid_Manufacturer()
        {
            Manufacturer manufacturer = null;
            var partNumber = ""; // Invalid part number
            var description = "Test description";
            var saleCode = new SaleCodeFaker(true).Generate();
            var productCode = new ProductCodeFaker(true).Generate();
            var part = new RepairOrderItemPartFaker(true).Generate();
            var partType = PartType.Part;

            var result = RepairOrderItem.Create(manufacturer, partNumber, description, saleCode, productCode, partType, part);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderItem.InvalidLengthMessage);
        }

        [Fact]
        public void SetManufacturer()
        {
            var manufacturer = new ManufacturerFaker(true).Generate();
            var repairOrderItem = new RepairOrderItemFaker().Generate();

            var result = repairOrderItem.SetManufacturer(manufacturer);

            result.IsSuccess.Should().BeTrue();
            repairOrderItem.Manufacturer.Should().Be(manufacturer);
        }

        [Fact]
        public void SetPartNumber()
        {
            var partNumber = "123456";
            var repairOrderItem = new RepairOrderItemFaker().Generate();

            var result = repairOrderItem.SetPartNumber(partNumber);

            result.IsSuccess.Should().BeTrue();
            repairOrderItem.PartNumber.Should().Be(partNumber);
        }

        [Fact]
        public void SetDescription()
        {
            var repairOrderItem = new RepairOrderItemFaker().Generate();
            var description = new string('a', RepairOrderItem.MaximumLength);

            var result = repairOrderItem.SetDescription(description);

            result.IsSuccess.Should().BeTrue();
            repairOrderItem.Description.Should().Be(description);
        }

        [Fact]
        public void SetSaleCode()
        {
            var saleCode = new SaleCodeFaker(true).Generate();
            var repairOrderItem = new RepairOrderItemFaker().Generate();

            var result = repairOrderItem.SetSaleCode(saleCode);

            result.IsSuccess.Should().BeTrue();
            repairOrderItem.SaleCode.Should().Be(saleCode);
        }

        [Fact]
        public void SetProductCode()
        {
            var productCode = new ProductCodeFaker(true).Generate();
            var repairOrderItem = new RepairOrderItemFaker().Generate();

            var result = repairOrderItem.SetProductCode(productCode);

            result.IsSuccess.Should().BeTrue();
            repairOrderItem.ProductCode.Should().Be(productCode);
        }

        [Fact]
        public void SetPartType()
        {
            var partType = PartType.Part;
            var repairOrderItem = new RepairOrderItemFaker().Generate();

            var result = repairOrderItem.SetPartType(partType);

            result.IsSuccess.Should().BeTrue();
            repairOrderItem.PartType.Should().Be(partType);
        }

        [Fact]
        public void SetPart()
        {
            var part = new RepairOrderItemPartFaker(true).Generate();
            var repairOrderItem = new RepairOrderItemFaker().Generate();

            var result = repairOrderItem.SetPart(part);

            result.IsSuccess.Should().BeTrue();
            repairOrderItem.Part.Should().Be(part);
        }

        [Fact]
        public void SetLabor()
        {
            var labor = new RepairOrderItemLaborFaker(true).Generate();
            var repairOrderItem = new RepairOrderItemFaker().Generate();

            var result = repairOrderItem.SetLabor(labor);

            result.IsSuccess.Should().BeTrue();
            repairOrderItem.Labor.Should().Be(labor);
        }

        [Fact]
        public void Return_Failure_On_Set_Null_Labor()
        {
            var repairOrderItem = new RepairOrderItemFaker().Generate();
            RepairOrderItemLabor labor = null;

            var result = repairOrderItem.SetLabor(labor);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderItem.RequiredMessage);
        }

        [Fact]
        public void Return_Failure_On_Set_Null_Part()
        {
            var repairOrderItem = new RepairOrderItemFaker().Generate();
            RepairOrderItemPart part = null;

            var result = repairOrderItem.SetPart(part);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderItem.RequiredMessage);
        }

        [Fact]
        public void Return_Failure_On_Set_Invalid_PartType()
        {
            var partType = (PartType)999; // Invalid PartType
            var repairOrderItem = new RepairOrderItemFaker().Generate();

            var result = repairOrderItem.SetPartType(partType);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderItem.RequiredMessage);
        }

        [Fact]
        public void Return_Failure_On_Set_Null_Manufacturer()
        {
            Manufacturer manufacturer = null;
            var repairOrderItem = new RepairOrderItemFaker().Generate();

            var result = repairOrderItem.SetManufacturer(manufacturer);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderItem.RequiredMessage);
        }

        [Fact]
        public void Return_Failure_On_Set_Invalid_PartNumber()
        {
            var partNumber = new string('a', RepairOrderItem.MaximumLength + 1);
            var repairOrderItem = new RepairOrderItemFaker().Generate();

            var result = repairOrderItem.SetPartNumber(partNumber);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderItem.InvalidLengthMessage);
        }

        [Fact]
        public void Return_Failure_On_Create_With_Null_SaleCode()
        {
            var manufacturer = new ManufacturerFaker(true).Generate();
            var partNumber = "123456";
            var description = "Test description";
            SaleCode saleCode = null;
            var productCode = new ProductCodeFaker(true).Generate();
            var part = new RepairOrderItemPartFaker(true).Generate();
            var partType = PartType.Part;

            var result = RepairOrderItem.Create(manufacturer, partNumber, description, saleCode, productCode, partType, part);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderItem.RequiredMessage);
        }

        [Fact]
        public void Return_Failure_On_Create_With_Null_ProductCode()
        {
            var manufacturer = new ManufacturerFaker(true).Generate();
            var partNumber = "123456";
            var description = "Test description";
            var saleCode = new SaleCodeFaker(true).Generate();
            ProductCode productCode = null;
            var part = new RepairOrderItemPartFaker(true).Generate();
            var partType = PartType.Part;

            var result = RepairOrderItem.Create(manufacturer, partNumber, description, saleCode, productCode, partType, part);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderItem.RequiredMessage);
        }

        [Fact]
        public void Return_Failure_On_Create_With_Invalid_PartType()
        {
            var manufacturer = new ManufacturerFaker(true).Generate();
            var partNumber = "123456";
            var description = "Test description";
            var saleCode = new SaleCodeFaker(true).Generate();
            var productCode = new ProductCodeFaker(true).Generate();
            var part = new RepairOrderItemPartFaker(true).Generate();
            var partType = (PartType)999; // Invalid PartType

            var result = RepairOrderItem.Create(manufacturer, partNumber, description, saleCode, productCode, partType, part);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderItem.RequiredMessage);
        }

        [Fact]
        public void Return_Failure_On_Create_With_Both_Part_And_Labor_Null()
        {
            var manufacturer = new ManufacturerFaker(true).Generate();
            var partNumber = "123456";
            var description = "Test description";
            var saleCode = new SaleCodeFaker(true).Generate();
            var productCode = new ProductCodeFaker(true).Generate();
            RepairOrderItemPart part = null;
            RepairOrderItemLabor labor = null;
            var partType = PartType.Part;

            var result = RepairOrderItem.Create(manufacturer, partNumber, description, saleCode, productCode, partType, part, labor);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderItem.RequiredMessage);
        }

        [Fact]
        public void Return_Failure_On_Create_With_Both_Part_And_Labor_Not_Null()
        {
            var manufacturer = new ManufacturerFaker(true).Generate();
            var partNumber = "123456";
            var description = "Test description";
            var saleCode = new SaleCodeFaker(true).Generate();
            var productCode = new ProductCodeFaker(true).Generate();
            var part = new RepairOrderItemPartFaker(true).Generate();
            var labor = new RepairOrderItemLaborFaker(true).Generate();
            var partType = PartType.Part;

            var result = RepairOrderItem.Create(manufacturer, partNumber, description, saleCode, productCode, partType, part, labor);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderItem.RequiredMessage);
        }

        [Fact]
        public void Return_Failure_On_Create_With_Invalid_Description()
        {
            var manufacturer = new ManufacturerFaker(true).Generate();
            var partNumber = "123456";
            var description = new string('a', RepairOrderItem.MaximumLength + 1); // Invalid description
            var saleCode = new SaleCodeFaker(true).Generate();
            var productCode = new ProductCodeFaker(true).Generate();
            var part = new RepairOrderItemPartFaker(true).Generate();
            var partType = PartType.Part;

            var result = RepairOrderItem.Create(manufacturer, partNumber, description, saleCode, productCode, partType, part);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderItem.InvalidLengthMessage);
        }

        [Fact]
        public void Return_Failure_On_SetDescription_With_Invalid_Description()
        {
            var description = new string('a', RepairOrderItem.MaximumLength + 1); // Invalid description
            var repairOrderItem = new RepairOrderItemFaker().Generate();

            var result = repairOrderItem.SetDescription(description);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderItem.InvalidLengthMessage);
        }

        [Fact]
        public void Return_Failure_On_SetSaleCode_With_Null_SaleCode()
        {
            SaleCode saleCode = null;
            var repairOrderItem = new RepairOrderItemFaker().Generate();

            var result = repairOrderItem.SetSaleCode(saleCode);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderItem.RequiredMessage);
        }

        [Fact]
        public void Return_Failure_On_SetProductCode_With_Null_ProductCode()
        {
            ProductCode productCode = null;
            var repairOrderItem = new RepairOrderItemFaker().Generate();

            var result = repairOrderItem.SetProductCode(productCode);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderItem.RequiredMessage);
        }

        [Fact]
        public void Return_Failure_On_SetPartType_With_Invalid_PartType()
        {
            var partType = (PartType)999; // Invalid PartType
            var repairOrderItem = new RepairOrderItemFaker().Generate();

            var result = repairOrderItem.SetPartType(partType);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderItem.RequiredMessage);
        }
    }
}
