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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

/*
 * VK: I'll be outlining what I did using "VK" comments. I'll also put importance/priority to my edits, so you know how important that edit is:
 * Importance (Im.) 1: minor issue, borderline matter of taste. You can reverse the edit freely if you prefer the old way
 * Im.2: medium priority, something I'd generally recommend you do
 * Im.3: something very important
 */

namespace CustomerVehicleManagement.Tests.Integration.Tests
{
    /*
     * VK, Im.1: I don't really like this "Should" notation. A test must convey a story about the application in plain English,
     * and when we read a "should" test, it makes it seem as though the application should do what the test's name says,
     * e.g it should always add/read/delete inventory items. A better way would be this:
     * InventoryItemsControllerShould -> InventoryItemsController
     * AddInventoryItem -> CanAddInventoryItem
     * So the whole thing would read as "Inventory items controller can add inventory item". This way, you are emphasizing optionality.
     * I normally don't do even that, and just go for "[Controller]Tests" notation, which is the simplest option.
     *
     * Im.2: Don't use "Async" in test names, those are implementation details that don't have anything to do with the story the test tells
     * Im.1: Put underscores between words in test name
     */
    public class InventoryItemsControllerShould : IntegrationTestBase
    {
        private const int NoContent = 204;
        private const int Created = 201;

        // VK Im.2: Arrange and Act sections tend to have lots of duplicated code;
        // it's best to extract the repeating parts

        // Only test the controller’s observable behavior, as it’s perceived by its client
        [Fact]
        public async Task Add_an_inventory_item()
        {
            // Each test creates its own set of data
            AddProductCodes();

            // Arrange
            var productCode = GetProductCodeToRead();
            var manufacturer = GetManufacturerToRead();
            var itemNumber = "Item Number One";
            var description = "a description";
            InventoryItemToWrite itemToAdd = new()
            {
                Manufacturer = manufacturer,
                ItemNumber = itemNumber,
                Description = description,
                ProductCode = productCode,
                ItemType = InventoryItemType.Part,
                Part = CreateInventoryItemPartToWrite()
            };

            // Act
            var response = await InvokeControllerAdd(itemToAdd);

            // VK Im.1: You can assert against what the API returns in GetInventoryItem, but it's easier to just check
            // the object in the database. Moreover, you don't always have appropriate APIs to peer into the DB, so 
            // asserting against domain objects is often the only choice. Article on this topic: https://enterprisecraftsmanship.com/posts/how-to-assert-database-state/


            // Assert
            var result = response as CreatedResult;
            result.StatusCode.Should().Be(Created);
            dynamic value = JsonConvert.DeserializeObject<ExpandoObject>(JsonConvert.SerializeObject(result.Value));
            long id = value.Id;
            id.Should().BeGreaterThan(0);
            using (var context = Helpers.CreateTestContext())
            {
                var inventoryItem = await new InventoryItemRepository(context).GetItemEntity(id);

                inventoryItem.Manufacturer.Id.Should().Be(manufacturer.Id);
                inventoryItem.ItemNumber.Should().Be(itemNumber);
                inventoryItem.Description.Should().Be(description);
                inventoryItem.ProductCode.Id.Should().Be(productCode.Id);
                inventoryItem.ItemType.Should().Be(InventoryItemType.Part);
                inventoryItem.Part.Should().NotBeNull();
            }
        }

        // VK Im.2: Read-only operations aren't that important for integration testing because bugs in them usually don't lead to
        // currupted data. There's no complex logic in read operations, you can skip testing them
        [Fact]
        public async Task Get_inventory_items()
        {
            int count = 10;
            AddInventoryItems(count);

            IReadOnlyList<InventoryItemToReadInList> items = await InvokeControllerGetItemsInList();

            items.Count.Should().BeGreaterThanOrEqualTo(count);
        }

        [Fact]
        public async Task Delete_an_InventoryItem()
        {
            AddInventoryItems(10);
            var items = await InvokeControllerGetItemsInList();

            var response = await InvokeControllerDelete(items[0].Id);

            response.Should().BeOfType<NoContentResult>();
            var result = response as StatusCodeResult;
            result.StatusCode.Should().Be(NoContent);

            using (var context = Helpers.CreateTestContext())
            {
                var item = await new InventoryItemRepository(context).GetItem(items[0].Id);
                item.Should().BeNull();
            }
        }

        [Fact]
        public async Task Update_an_InventoryItem()
        {
            var description = "Description set by integration test";
            AddInventoryItems(10);
            var items = await InvokeControllerGetItemsInList();
            var itemToRead = await GetItem(items[0].Id);
            var itemToEdit = InventoryItemHelper.ConvertReadToWriteDto(itemToRead);
            itemToEdit.Description = description;

            var response = await InvokeControllerUpdate(itemToEdit) as StatusCodeResult;

            response.StatusCode.Should().Be(204);

            using (var context = Helpers.CreateTestContext())
            {
                var updatedItem = await new InventoryItemRepository(context).GetItem(itemToEdit.Id);
                updatedItem.Description.Should().Be(description);
            }
        }

        [Fact]
        public async Task Get_an_InventoryItem_by_Id()
        {
            AddInventoryItems(10);
            var items = await InvokeControllerGetItemsInList();

            var itemFromControllerResult = await InvokeControllerGet(items[0].Id);

            if (itemFromControllerResult.Result is OkObjectResult okResult && okResult.Value is InventoryItemToRead itemFromController)
            {
                itemFromController.Id.Should().Be(items[0].Id);

                using (var context = Helpers.CreateTestContext())
                {
                    var itemFromRepository = await new InventoryItemRepository(context).GetItem(items[0].Id);
                    itemFromRepository.Id.Should().Be(items[0].Id);
                }
            }
        }


        [Fact]
        public async Task Get_an_InventoryItem_by_Manufacturer_Id_and_Part_Number()
        {
            AddInventoryItems(10);
            var manufacturer = GetManufacturerToRead();
            var itemNumber = "Item Number One";

            var itemToReadResult = await InvokeControllerGet(
                id: -1,
                manufacturerId: manufacturer.Id,
                itemNumber: itemNumber);

            if (itemToReadResult.Result is OkObjectResult okResult && okResult.Value is InventoryItemToRead itemToRead)
                itemToRead.Manufacturer.Id.Should().Be(manufacturer.Id);
        }


        [Fact]
        public async Task Not_add_invalid_inventory_item_but_return_Status404NotFound()
        {
            InventoryItemToWrite itemToAdd = new();

            var response = await InvokeControllerAdd(itemToAdd);
            var result = (NotFoundObjectResult)response;

            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task Not_update_invalid_inventory_item_but_return_Status404NotFound()
        {
            long invalidItemId = -1;
            InventoryItemToWrite itemToEdit = new()
            {
                Id = invalidItemId,
                Description = "New description"
            };

            var response = await InvokeControllerUpdate(itemToEdit);
            var result = (NotFoundObjectResult)response;

            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task Not_delete_invalid_inventory_item_but_return_Status404NotFound()
        {
            long invalidItemId = -1;

            var response = await InvokeControllerDelete(invalidItemId);

            var result = (NotFoundObjectResult)response;
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task Not_get_invalid_inventory_item_but_return_NotFoundResult()
        {
            AddInventoryItems(10);
            long invalidItemId = 9999999;

            var response = await InvokeControllerGet(id: invalidItemId);

            var notFoundResult = response.Result as NotFoundResult;
            notFoundResult.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Not_get_inventory_item_by_invalid_manufacturer_Id_and_part_number_but_return_NotFoundResult()
        {
            long invalidManufacturerId = -1;
            string partNumber = "Invalid Part Number";

            var response = await InvokeControllerGet(id: 1, invalidManufacturerId, partNumber);

            var notFoundResult = response.Result as NotFoundResult;
            notFoundResult.Should().BeOfType<NotFoundResult>();

        }

        private static async Task<IActionResult> InvokeControllerUpdate(InventoryItemToWrite itemToEdit)
        {
            using (var context = Helpers.CreateTestContext())
            {
                var controller = new InventoryItemsController(
                    new InventoryItemRepository(context),
                    new ManufacturerRepository(context),
                    new ProductCodeRepository(context));

                var result =
                (ActionResult)await controller
                    .Update(itemToEdit.Id, itemToEdit);

                return result;
            }
        }

        private static async Task<IActionResult> InvokeControllerAdd(InventoryItemToWrite itemToAdd)
        {
            using (var context = Helpers.CreateTestContext())
            {
                var controller = new InventoryItemsController(
                    new InventoryItemRepository(context),
                    new ManufacturerRepository(context),
                    new ProductCodeRepository(context));

                var result = await controller.Add(itemToAdd);

                return result;
            }
        }

        private static async Task<IReadOnlyList<InventoryItemToReadInList>> InvokeControllerGetItemsInList()
        {
            using (var context = Helpers.CreateTestContext())
            {
                var controller = new InventoryItemsController(
                    new InventoryItemRepository(context),
                    new ManufacturerRepository(context),
                    new ProductCodeRepository(context));

                var manufacturer = await context.Manufacturers.FirstAsync();
                var actionResult = await controller.GetItemsInList(manufacturer.Id);
                var actionResultResult = (OkObjectResult)actionResult.Result;

                return (IReadOnlyList<InventoryItemToReadInList>)actionResultResult.Value;
            }
        }

        private static async Task<InventoryItemToRead> GetItem(long id)
        {
            using (var context = Helpers.CreateTestContext())
            {
                var repo = new InventoryItemRepository(context);
                return await repo.GetItem(id);
            }
        }

        private static async Task<ActionResult<InventoryItemToRead>> InvokeControllerGet(
            long id = 0,
            long manufacturerId = 0,
            string itemNumber = null)
        {
            using (var context = Helpers.CreateTestContext())
            {
                var controller = new InventoryItemsController(
                    new InventoryItemRepository(context),
                    new ManufacturerRepository(context),
                    new ProductCodeRepository(context));

                ActionResult<InventoryItemToRead> actionResult = null;

                if (id > 0)
                    actionResult = await controller.Get(id);

                if (manufacturerId > 0 && itemNumber is not null)
                    actionResult = await controller.Get(manufacturerId, itemNumber);

                var result = actionResult.Result is OkObjectResult okResult
                    && okResult.Value is InventoryItemToRead item
                    ? (ActionResult<InventoryItemToRead>)new OkObjectResult(item)
                    : new NotFoundResult();

                return result;
            }
        }

        private static async Task<IActionResult> InvokeControllerDelete(long id)
        {
            using (var context = Helpers.CreateTestContext())
            {
                var controller = new InventoryItemsController(
                    new InventoryItemRepository(context),
                    new ManufacturerRepository(context),
                    new ProductCodeRepository(context));

                var actionResult = await controller.Delete(id);

                return actionResult;
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

        private static ProductCodeToRead GetProductCodeToRead()
        {
            using var context = Helpers.CreateTestContext();

            return ProductCodeHelper.ConvertEntityToReadDto(
                context.ProductCodes.First());
        }

        private static ManufacturerToRead GetManufacturerToRead()
        {
            using var context = Helpers.CreateTestContext();

            return ManufacturerHelper.ConvertEntityToReadDto(
                context.Manufacturers.First());
        }
    }
}
