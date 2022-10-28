using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Api.Inventory;
using CustomerVehicleManagement.Api.Manufacturers;
using CustomerVehicleManagement.Api.ProductCodes;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Labor;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Part;
using CustomerVehicleManagement.Shared.Models.Manufacturers;
using CustomerVehicleManagement.Shared.Models.ProductCodes;
using FluentAssertions;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CustomerVehicleManagement.Tests.Integration.Tests
{
    public class InventoryItemsControllerShould : IntegrationTestBase
    {
        [Fact]
        public async Task AddInventoryItemAsync()
        {
            // Arrange
            var context = Helpers.CreateTestContext();
            var controller = new InventoryItemsController(
                new InventoryItemRepository(context),
                new ManufacturerRepository(context),
                new ProductCodeRepository(context));

            var manufacturersToRead =
                context.Manufacturers
                    .ToList()
                    .Select(manufacturer =>
                    ManufacturerHelper.ConvertEntityToReadDto(manufacturer));

            var manufacturer = manufacturersToRead.FirstOrDefault();

            InventoryItemToWrite itemToAdd = new()
            {
                Manufacturer = manufacturer,
                ItemNumber = "Item Number One",
                Description = "a description",
                ProductCode = ProductCodeHelper.ConvertEntityToReadDto(context.ProductCodes.FirstOrDefault()),
                ItemType = InventoryItemType.Part,
                Part = CreateInventoryItemPartToWrite()
            };

            // Act
            //var result = await controller.AddInventoryItemAsync(itemToAdd);
            //var inventoryItemToRead = await controller.GetInventoryItemAsync(result.Value.Id);

            // Assert
            //inventoryItemToRead.Should().BeOfType<ActionResult<InventoryItemToRead>>();
        }

        private static InventoryItemPartToWrite CreateInventoryItemPartToWrite()
        {
            return new()
            {
                List = InstallablePart.MaximumMoneyAmount,
                Cost = InstallablePart.MaximumMoneyAmount,
                Core = InstallablePart.MinimumMoneyAmount,
                Retail = InstallablePart.MinimumMoneyAmount,
                Fractional = false,
                TechAmount = new TechAmountToWrite
                {
                    PayType = ItemLaborType.Flat,
                    Amount = LaborAmount.MinimumAmount,
                    SkillLevel = SkillLevel.A
                }
            };
        }

        private static ManufacturerToRead CreateManufacturerToRead(ApplicationDbContext context)
        {
            ManufacturerToRead manufacturer =
            ManufacturerHelper.ConvertEntityToReadDto(
                context.Manufacturers.FirstOrDefault());
            ProductCodeToRead productCode = ProductCodeHelper.ConvertEntityToReadDto(context.ProductCodes.FirstOrDefault());
            InventoryItemPartToWrite part = CreateInventoryItemPartToWrite();

            return manufacturer;
        }

        //[Fact]
        //public async Task Return_NotFoundResult_On_GetPersonAsyncWithInvalidId()
        //{
        //    var result = await controller.GetPersonAsync(0);

        //    result.Result.Should().BeOfType<NotFoundResult>();
        //    result.Should().BeOfType<ActionResult<PersonToRead>>();
        //}
    }
}