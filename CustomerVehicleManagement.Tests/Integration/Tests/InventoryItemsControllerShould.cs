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
using NuGet.Protocol;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
            IEnumerable<ManufacturerToRead> manufacturersToRead;
            ProductCodeToRead productCode;
            InventoryItemToRead inventoryItemToRead;
            string itemNumber = "Item Number One";
            string description = "a description";

            using (var context = Helpers.CreateTestContext())
            {
                var controller = new InventoryItemsController(
                    new InventoryItemRepository(context),
                    new ManufacturerRepository(context),
                    new ProductCodeRepository(context));

                manufacturersToRead =
                    context.Manufacturers
                        .ToList()
                        .Select(manufacturer =>
                        ManufacturerHelper.ConvertEntityToReadDto(manufacturer));

                productCode = ProductCodeHelper.ConvertEntityToReadDto(context.ProductCodes.FirstOrDefault());
            }

            var manufacturer = manufacturersToRead.FirstOrDefault();

            InventoryItemToWrite itemToAdd = new()
            {
                Manufacturer = manufacturer,
                ItemNumber = itemNumber,
                Description = description,
                ProductCode = productCode,
                ItemType = InventoryItemType.Part.ToString(),
                Part = CreateInventoryItemPartToWrite()
            };

            // Act
            using (var context = Helpers.CreateTestContext())
            {
                var controller = new InventoryItemsController(
                    new InventoryItemRepository(context),
                    new ManufacturerRepository(context),
                    new ProductCodeRepository(context));

                var createdResult = await controller.AddInventoryItemAsync(itemToAdd);
                var createdResultResponse = JsonSerializer.Deserialize<CreatedResultResponse>(createdResult.ToJson());

                var actionResult = await controller.GetInventoryItemAsync(createdResultResponse.Value.Id);
                var actionResultResult = (OkObjectResult)actionResult.Result;
                inventoryItemToRead = (InventoryItemToRead)actionResultResult.Value;
            }

            // Assert
            inventoryItemToRead.Should().BeOfType<InventoryItemToRead>();
            inventoryItemToRead.Manufacturer.Id.Should().Be(manufacturer.Id);
            inventoryItemToRead.ItemNumber.Should().Be(itemNumber);
            inventoryItemToRead.Description.Should().Be(description);
            inventoryItemToRead.ProductCode.Id.Should().Be(productCode.Id);
            inventoryItemToRead.ItemType.Should().Be(InventoryItemType.Part);
            inventoryItemToRead.Part.Should().NotBeNull();
        }

        [Fact]
        public async Task GetInventoryItemsAsync()
        {
            var context = Helpers.CreateTestContext();
            var controller = new InventoryItemsController(
                new InventoryItemRepository(context),
                new ManufacturerRepository(context),
                new ProductCodeRepository(context));
            await CreateInventoryItems();

            var actionResult = await controller.GetInventoryItemsListAsync();
            var actionResultResult = (OkObjectResult)actionResult.Result;
            var inventoryItemsToRead = (IReadOnlyList<InventoryItemToReadInList>)actionResultResult.Value;

            inventoryItemsToRead.Should().BeOfType<List<InventoryItemToReadInList>>();
            inventoryItemsToRead.Count.Should().BeGreaterThanOrEqualTo(1);
        }

        //[Fact]
        //public async Task UpdateInventoryItemAsync()
        //{
        //    IEnumerable<ManufacturerToRead> manufacturersToRead;
        //    ProductCodeToRead productCode;
        //    InventoryItemToRead inventoryItemToRead;
        //    InventoryItemPartToWrite part = CreateInventoryItemPartToWrite();
        //    InventoryItemPartToWrite partForUpdate;
        //    string itemNumber = "Item Number One";
        //    string description = "a description";
        //    string itemNumberForUpdate = "Item Number TWO";
        //    string descriptionForUpdate = "a SECOND description";

        //    using (var context = Helpers.CreateTestContext())
        //    {
        //        var controller = new InventoryItemsController(
        //            new InventoryItemRepository(context),
        //            new ManufacturerRepository(context),
        //            new ProductCodeRepository(context));

        //        manufacturersToRead =
        //            context.Manufacturers
        //                .ToList()
        //                .Select(manufacturer =>
        //                ManufacturerHelper.ConvertEntityToReadDto(manufacturer));

        //        productCode = ProductCodeHelper.ConvertEntityToReadDto(context.ProductCodes.FirstOrDefault());
        //    }

        //    var manufacturer = manufacturersToRead.FirstOrDefault();

        //    InventoryItemToWrite itemToAdd = new()
        //    {
        //        Manufacturer = manufacturer,
        //        ItemNumber = itemNumber,
        //        Description = description,
        //        ProductCode = productCode,
        //        ItemType = InventoryItemType.Part,
        //        Part = part
        //    };

        //    using (var context = Helpers.CreateTestContext())
        //    {
        //        var controller = new InventoryItemsController(
        //            new InventoryItemRepository(context),
        //            new ManufacturerRepository(context),
        //            new ProductCodeRepository(context));


        //        var createdResult = await controller.AddInventoryItemAsync(itemToAdd);
        //        var createdResultResponse = JsonSerializer.Deserialize<CreatedResultResponse>(createdResult.ToJson());

        //        var actionResult = await controller.GetInventoryItemAsync(createdResultResponse.Value.Id);
        //        var actionResultResult = (OkObjectResult)actionResult.Result;
        //        inventoryItemToRead = (InventoryItemToRead)actionResultResult.Value;
        //        partForUpdate = new()
        //        {
        //            Core = inventoryItemToRead.Part.Core,
        //            Cost = inventoryItemToRead.Part.Cost,
        //            Fractional = inventoryItemToRead.Part.Fractional,
        //            Id = inventoryItemToRead.Part.Id,
        //            LineCode = inventoryItemToRead.Part.LineCode,
        //            SubLineCode = inventoryItemToRead.Part.SubLineCode,
        //            List = inventoryItemToRead.Part.List,
        //            Retail = inventoryItemToRead.Part.Retail,
        //            TechAmount = new TechAmountToWrite()
        //            {
        //                PayType = inventoryItemToRead.Part.TechAmount.PayType,
        //                SkillLevel = inventoryItemToRead.Part.TechAmount.SkillLevel,
        //                Amount = inventoryItemToRead.Part.TechAmount.Amount
        //            }
        //        };
        //    }

        //    inventoryItemToRead.Should().BeOfType<InventoryItemToRead>();
        //    inventoryItemToRead.ProductCode.Id.Should().Be(productCode.Id);

        //    InventoryItemToWrite itemToUpdate = new()
        //    {
        //        Manufacturer = manufacturer,
        //        ItemNumber = itemNumberForUpdate,
        //        Description = descriptionForUpdate,
        //        ProductCode = productCode,
        //        ItemType = InventoryItemType.Part,
        //        Part = partForUpdate
        //    };

        //    using (var context = Helpers.CreateTestContext())
        //    {
        //        var controller = new InventoryItemsController(
        //            new InventoryItemRepository(context),
        //            new ManufacturerRepository(context),
        //            new ProductCodeRepository(context));

        //        var updatedResult = await controller.UpdateInventoryItemAsync(inventoryItemToRead.Id, itemToUpdate);
        //        var updatedResultResponse = JsonSerializer.Deserialize<CreatedResultResponse>(updatedResult.ToJson());

        //        var actionResult = await controller.GetInventoryItemAsync(updatedResultResponse.Value.Id);
        //        var actionResultResult = (OkObjectResult)actionResult.Result;
        //        inventoryItemToRead = (InventoryItemToRead)actionResultResult.Value;
        //    }

        //}

        [Fact]
        public async Task DeleteInventoryItemAsync()
        {
            IEnumerable<ManufacturerToRead> manufacturersToRead;
            ProductCodeToRead productCode;
            InventoryItemToRead inventoryItemToRead;
            string itemNumber = "Item Number One";
            string description = "a description";
            long id = 0L;
            NotFoundResult notFoundResultResult;

            using (var context = Helpers.CreateTestContext())
            {
                var controller = new InventoryItemsController(
                    new InventoryItemRepository(context),
                    new ManufacturerRepository(context),
                    new ProductCodeRepository(context));

                manufacturersToRead =
                    context.Manufacturers
                        .ToList()
                        .Select(manufacturer =>
                        ManufacturerHelper.ConvertEntityToReadDto(manufacturer));

                productCode = ProductCodeHelper.ConvertEntityToReadDto(context.ProductCodes.FirstOrDefault());
            }

            var manufacturer = manufacturersToRead.FirstOrDefault();

            InventoryItemToWrite itemToAdd = new()
            {
                Manufacturer = manufacturer,
                ItemNumber = itemNumber,
                Description = description,
                ProductCode = productCode,
                ItemType = InventoryItemType.Part.ToString(),
                Part = CreateInventoryItemPartToWrite()
            };

            using (var context = Helpers.CreateTestContext())
            {
                var controller = new InventoryItemsController(
                    new InventoryItemRepository(context),
                    new ManufacturerRepository(context),
                    new ProductCodeRepository(context));

                var createdResult = await controller.AddInventoryItemAsync(itemToAdd);
                var createdResultResponse = JsonSerializer.Deserialize<CreatedResultResponse>(createdResult.ToJson());
                id = createdResultResponse.Value.Id;
                var actionResult = await controller.GetInventoryItemAsync(createdResultResponse.Value.Id);
                var actionResultResult = (OkObjectResult)actionResult.Result;
                inventoryItemToRead = (InventoryItemToRead)actionResultResult.Value;
            }

            inventoryItemToRead.Should().BeOfType<InventoryItemToRead>();
            inventoryItemToRead.Manufacturer.Id.Should().Be(manufacturer.Id);

            using (var context = Helpers.CreateTestContext())
            {
                var controller = new InventoryItemsController(
                    new InventoryItemRepository(context),
                    new ManufacturerRepository(context),
                    new ProductCodeRepository(context));

                var createdResult = await controller.DeleteInventoryItemAsync(id);
                var createdResultResponse = JsonSerializer.Deserialize<CreatedResultResponse>(createdResult.ToJson());

                var actionResult = await controller.GetInventoryItemAsync(id);
                notFoundResultResult = (NotFoundResult)actionResult.Result;
            }

            notFoundResultResult.Should().BeOfType<NotFoundResult>();
        }

        private static async Task CreateInventoryItems()
        {
            IEnumerable<ManufacturerToRead> manufacturersToRead;
            ProductCodeToRead productCode;
            string itemNumber = "Item Number One";
            string description = "a description";
            InventoryItemToWrite itemToAdd;
            InventoryItemToWrite anotherItemToAdd;

            using (var context = Helpers.CreateTestContext())
            {
                var controller = new InventoryItemsController(
                    new InventoryItemRepository(context),
                    new ManufacturerRepository(context),
                    new ProductCodeRepository(context));

                manufacturersToRead =
                    context.Manufacturers
                        .ToList()
                        .Select(manufacturer =>
                        ManufacturerHelper.ConvertEntityToReadDto(manufacturer));

                productCode = ProductCodeHelper.ConvertEntityToReadDto(context.ProductCodes.FirstOrDefault());

                var manufacturer = manufacturersToRead.FirstOrDefault();

                itemToAdd = new()
                {
                    Manufacturer = manufacturer,
                    ItemNumber = itemNumber,
                    Description = description,
                    ProductCode = productCode,
                    ItemType = InventoryItemType.Part.ToString(),
                    Part = CreateInventoryItemPartToWrite()
                };

                anotherItemToAdd = new()
                {
                    Manufacturer = manufacturer,
                    ItemNumber = itemNumber,
                    Description = description,
                    ProductCode = productCode,
                    ItemType = InventoryItemType.Part.ToString(),
                    Part = CreateInventoryItemPartToWrite()
                };
            }

            using (var context = Helpers.CreateTestContext())
            {
                var controller = new InventoryItemsController(
                    new InventoryItemRepository(context),
                    new ManufacturerRepository(context),
                    new ProductCodeRepository(context));

                await controller.AddInventoryItemAsync(itemToAdd);
            }

            using (var context = Helpers.CreateTestContext())
            {
                var controller = new InventoryItemsController(
                    new InventoryItemRepository(context),
                    new ManufacturerRepository(context),
                    new ProductCodeRepository(context));

                await controller.AddInventoryItemAsync(anotherItemToAdd);
            }
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
    }
}
